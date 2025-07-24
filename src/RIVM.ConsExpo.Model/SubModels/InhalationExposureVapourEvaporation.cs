//#define UseDefaultMassTransfer // otherwise, the MassTransferCoefficient in the evaporation scenario will be input
#define testNewEvaporationFormulation // redefinition of the evaporation scenario

using DotNumerics.ODE;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Computations;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using RIVM.ConsExpo.Model.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using RIVM.ConsExpo.DTO.Extensions;
using RIVM.ConsExpo.DTO.Helpers;
using System.Diagnostics;

namespace RIVM.ConsExpo.Model.Submodels
{
    /// <summary>
    /// In this model, the substance evaporates from surface. This surface may be of constant size or increasing at a constant rate.
    /// </summary>
    /// <remarks>This class inherits InhalationExposureInstantaniousReleaseBase, because it will use an analytic solution after the release duration has elapsed.</remarks>
    internal class InhalationExposureVapourEvaporation : InhalationExposureInstantaniousReleaseBase, IInhalationExposureSubmodel
    {
#warning Tech Dept: this constant is not independent of the number of time steps in InhalationSimulation.CalculateTimeSeries().
        private const int DefaultNumberOfTimeSteps = 100;

        private int _numberOfTimeSteps;

        // These two fields are used to store a helper solution with a high time resolution, for determining a peak interval.
        private TimeInterval _peakInterval;

        private InhalationExposureVapourEvaporation _helperEvaporation;

        private const int NumberOfEquations = 3; //The product and the air + an integration of air concentration running along.
        private const int IndexOfTimeSeries = 0;
        private const int IndexOfInstantAirConcentrationSeries = 1;
        private const int IndexOfAmountInProductSeries = 2;
        private const int IndexOfIntegratedAirConcentrationSeries = 3;

        private const InhalationExposureSubmodelTypes type = InhalationExposureSubmodelTypes.VapourEvaporation;

        public InhalationExposureSubmodelTypes Type => type;

        public InhalationExposureVapourEvaporation(ScenarioModel scenario)
            : base(scenario, type, false)
        { }

        public override bool IsTimeDependent => true;

        public override Duration ApplicableExposureDuration
        {
            get
            {
                if (scenario.InhalationExposure.ReEntry)
                {
                    return scenario.InhalationExposure.EmissionDurationReEntry;
                }

                return scenario.InhalationExposure.ExposureDuration;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the model [supports peak air concentration].
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports peak air concentration]; otherwise, <c>false</c>.
        /// </value>
        public override bool SupportsPeakAirConcentration => true;

        public override bool SupportsMeanDayConcentration
        {
            get
            {
                if (scenario.InhalationExposure.ReEntry)
                {
                    return false;
                }
                return true;
            }
        }

        public override bool SupportsExternalDayDose
        {
            get
            {
                if (scenario.InhalationExposure.ReEntry)
                {
                    return false;
                }
                return true;
            }
        }

        public override bool SupportsInternalDayDose
        {
            get
            {
                if (scenario.InhalationExposure.ReEntry)
                {
                    return false;
                }
                return true;
            }
        }

        public override Time DefaultTimeMax => ApplicableExposureDuration.AsTime();

        /// <summary>
        /// The unit in which the numerical solution is expressed.
        /// </summary>
        /// <remarks>Should be consistent with the units in which the input parameters to the model are expressed.</remarks>
        private readonly TimeUnits _unitOfSolution = TimeUnits.Minute;

        /// <summary>
        /// The amount of substance released.
        /// </summary>
        public override double? AmountOfSubstance => AmountOfSubstanceByProductAmount;

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.InhalationExposureProductAmount,
                DTO.Models.ModelParameters.InhalationExposureWeightFractionSubstance,
                DTO.Models.ModelParameters.InhalationExposureRoomVolume,
                DTO.Models.ModelParameters.InhalationExposureVentilationRate,
                DTO.Models.ModelParameters.InhalationExposureVapourPressure,
                DTO.Models.ModelParameters.InhalationExposureApplicationTemperature,
                DTO.Models.ModelParameters.AssessmentMolecularWeight,
                DTO.Models.ModelParameters.InhalationExposureMassTransferCoefficient,
                DTO.Models.ModelParameters.InhalationExposureReleaseArea
            };

            if (scenario.InhalationExposure.ReEntry)
            {
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureEmissionDurationReEntry);
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureDailyDuration);
            }
            else
            {
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureExposureDuration);
            }

            switch (route.ReleaseAreaType)
            {
                case InhalationExposureReleaseAreaTypes.Constant:
                    if (scenario.InhalationExposure.ReEntry == false)
                    {
                        modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureEmissionDurationEvaporation);
                    }
                    break;

                case InhalationExposureReleaseAreaTypes.Increasing:
                    modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureApplicationDuration);
                    break;

                default:
                    throw new NotSupportedException(string.Format("Unsupported release area type '{0}'", route.ReleaseAreaType.ToString()));
            }

            if (!route.PureForm)
            {
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureMolecularWeightMatrix);
                if (route.ProductInDilution)
                {
                    modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureDilution);
                }
            }

            return modelParameters;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (scenario.InhalationExposure.ReEntry)
            {
                RequireEmissionDurationReEntry(validationResults);
                validationResults.AddRange(ValidateDurationAndFrequency(route.EmissionDurationReEntry, scenario.Frequency));
            }
            else
            {
                RequireExposureDuration(validationResults);
                validationResults.AddRange(ValidateDurationAndFrequency(route.ExposureDuration, scenario.Frequency));
            }

            RequireProductAmount(validationResults);
            RequireWeightFractionSubstance(validationResults);
            RequireRoomVolume(validationResults);
            RequireVentilationRate(validationResults);
            RequireApplicationTemperature(validationResults);
            RequireVapourPressure(validationResults);
            RequireMolecularWeight(validationResults, scenario.Assessment.Substance);
            RequireMassTransferCoefficient(validationResults);
            RequireReleaseArea(validationResults);

            switch (route.ReleaseAreaType)
            {
                case InhalationExposureReleaseAreaTypes.Constant:
                    if (!scenario.InhalationExposure.ReEntry)
                    {
                        RequireEmissionDurationEvaporation(validationResults);
                    }
                    break;

                case InhalationExposureReleaseAreaTypes.Increasing:
                    RequireApplicationDuration(validationResults);
                    break;

                default:
                    throw new NotSupportedException($"Unsupported release area type '{route.ReleaseAreaType}'.");
            }

            if (!route.PureForm)
            {
                RequireMolecularWeightMatrix(validationResults);
                if (route.ProductInDilution)
                {
                    RequireDilution(validationResults);
                }
            }

            if (!route.PureForm && !route.ProductInDilution && route.WeightFractionSubstance.HasValue && route.WeightFractionSubstance.AsFraction() == 1)
            {
                string productInDilutionLabel = ModelHelpers.GetDisplayName<InhalationExposureModel>(p => p.ProductInDilution);
                string pureFormLabel = ModelHelpers.GetDisplayName<InhalationExposureModel>(p => p.PureForm);
                string weightFractionSubstanceLabel = ModelHelpers.GetDisplayName<InhalationExposureModel>(p => p.WeightFractionSubstance);
                validationResults.Add($"A weight fraction of 1 is inconsistent with the product not being the substance in pure form and the product not being used in a diluted form. Either select the either option '{pureFormLabel}', or '{productInDilutionLabel}', or adjust the '{weightFractionSubstanceLabel}' to a value less than 1.");
            }
            return validationResults;
        }

        private const double R = PhysicalConstants.GasConstant; // gas constant in Pa*m3/mol/K
        private double Q;
        private double ApGram; // total product amount
        private double a; // active substance
        private double vp;
        private double m;
        private double T;
        private InhalationExposureReleaseAreaTypes releaseAreaType;
        private double releaseDuration;
        private bool pureForm;
        private bool productInDilution;
        private double? Mr;   // molecular weight matrix [g/mol]
        private double Kt;    // mass transfer coefficient [m/min]
        private double dS;    // release area growth per minute
        private double Stot;  // total surface area of the applied product [m2]
        private double Vr;    // room volume [m3]

        protected override void ParseScenario()
        {
            // This method is called from the base class, but it is a bit of a hack to use the base class InhalationExposureInstantaniousReleaseBase in this submodel.
            // The base submodel can calculate the instantaneous release, but this class uses it to calculate the air concentration after the release is stopped.
            // The air concentration at the time the release stops, is used to start the base submodel.
            // However, since initialization must be done differently, using calculated values, the ParseScenario does not apply here.
        }

        /// <summary>
        /// Parses the scenario and maps all relevant parameters to the values used in the calculation.
        /// </summary>
        protected void Initialize()
        {
            var inhalationExposure = scenario.InhalationExposure;

            //These are the parameters needed for the analytic model that is used after the release duration has ended.

#warning Tech. Dept: Unfortunately, the analytic model uses different units. for now, keep it like that.
            T0 = 0.0;
            V = inhalationExposure.RoomVolume.InCubicMetres();
            q = inhalationExposure.VentilationRate.InTimesPerSecond();
            wf = inhalationExposure.WeightFractionSubstance.AsFraction();
            A = inhalationExposure.ProductAmount.InMilligram() * wf;
            limitConcentrationToSaturatedAirConcentration = true;
            vapourPressure = scenario.InhalationExposure.VapourPressure;
            molecularWeight = scenario.Assessment.Substance.MolecularWeight.InGramPerMol();
            applicationTemperature = scenario.InhalationExposure.ApplicationTemperature.InKelvin();

            //These are the parameters in de numerical model used, up to release duration.
            Vr = inhalationExposure.RoomVolume.InCubicMetres();
            Q = Vr * inhalationExposure.VentilationRate.InTimesPerMinute();
            ApGram = inhalationExposure.ProductAmount.InGram();
            pureForm = inhalationExposure.PureForm;
            if (pureForm)
            {
                Mr = null;
                productInDilution = false;
            }
            else
            {
                productInDilution = inhalationExposure.ProductInDilution;
                Mr = inhalationExposure.MolecularWeightMatrix.InGramPerMol();
                if (productInDilution)
                {
                    wf = wf / inhalationExposure.Dilution.InTimes();
                }
            }
            a = ApGram * wf;
            vp = inhalationExposure.VapourPressure.InPascal();
            Kt = inhalationExposure.MassTransferCoefficient.InMetresPerMinute();
            m = scenario.Assessment.Substance.MolecularWeight.InGramPerMol();

            T = inhalationExposure.ApplicationTemperature.InKelvin();
            releaseAreaType = inhalationExposure.ReleaseAreaType;
            Stot = inhalationExposure.ReleaseArea.InSquareMetre();
            switch (releaseAreaType)
            {
                case InhalationExposureReleaseAreaTypes.Constant:
                    releaseDuration = inhalationExposure.ReEntry
                        ? inhalationExposure.EmissionDurationReEntry.InMinutes()
                        : inhalationExposure.EmissionDurationEvaporation.InMinutes();
                    dS = 0;
                    break;

                case InhalationExposureReleaseAreaTypes.Increasing:
                    releaseDuration = inhalationExposure.ApplicationDuration.InMinutes();
                    dS = Stot / releaseDuration;
                    break;

                default:
                    throw new NotSupportedException(string.Format("Unsupported release area type '{0}'", releaseAreaType.ToString()));
            }
        }

        /// <summary>
        /// The numerical solution from t = 0 to tRelease.
        /// </summary>
        private double[,] solutionUpToRelease;

        /// <summary>
        /// The solution of the model.
        /// </summary>
        private double[,] solutionComplete;

        public override AirConcentration InstantaneousAirConcentration(Time time)
        {
            double airConcentration;

#warning To Do: Test for limiting conditions that are not supported by integration of the ODE's.
            if (time.InMinutes() <= 0.0)
            {
                airConcentration = 0.0;
            }
            else
            {
                if (!solutionPrepared)
                {
                    throw new ApplicationException("A time series must be prepared to make sure the solution has been integrated up to the correct end time.");
                }

                double totalTime = ApplicableExposureDuration.InMinutes();
                int timestep = Convert.ToInt32((double)_numberOfTimeSteps * time.InMinutes() / totalTime);

                airConcentration = MassToAirConcentration(solutionComplete[timestep, IndexOfInstantAirConcentrationSeries]);
            }

            return AirConcentration.NewFromGramPerCubicMetre(airConcentration);
        }

        /// <summary>
        /// Prepares the time series. Typically needed for numerical solution methods.
        /// </summary>
        /// <param name="timeMax">The maximum time.</param>
        public override void PrepareTimeSeries(Time timeMax)
        {
            PrepareTimeSeries(timeMax, DefaultNumberOfTimeSteps);
        }

        /// <summary>
        /// Prepares the time series. Typically needed for numerical solution methods.
        /// </summary>
        /// <param name="timeMax">The maximum time.</param>
        /// <param name="numberOfTimeSteps">The number of interpolation points.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        public void PrepareTimeSeries(Time timeMax, int numberOfTimeSteps)
        {
            this._numberOfTimeSteps = numberOfTimeSteps;

            double productAmountAtRelease;

            Initialize();

            switch (releaseAreaType)
            {
                case InhalationExposureReleaseAreaTypes.Constant:
                    productAmountAtRelease = a;
                    break;

                case InhalationExposureReleaseAreaTypes.Increasing:
                    productAmountAtRelease = 0;
                    break;

                default:
                    throw new NotSupportedException(string.Format("Unsupported release area type '{0}'", releaseAreaType.ToString()));
            }

            if (releaseAreaType == InhalationExposureReleaseAreaTypes.Increasing || timeMax.InMinutes() <= releaseDuration)
            {
                solutionComplete = Solve(timeMax.InMinutes(), productAmountAtRelease, numberOfTimeSteps);
            }
            else
            {
                double airConcentrationAtRelease;
                double integratedAirConcentrationAtRelease;

                // The solution is calculated for two intervals:
                // a) t=0 to tRelease
                // b) tRelease to tMax.
                // The first part is done numerically. The second part is done with the analytic model for instantaneous release,
                // as if the air concentration at tRelease is the result of instantaneous release.
                // We need 100 time intervals (0 - 100, so 101 points). Therefore, the number of time steps before and after tRelease must be calculated.
                int numberOfTimeStepsUpToRelease = (int)Math.Floor(releaseDuration / timeMax.InMinutes() * numberOfTimeSteps);
                if (numberOfTimeStepsUpToRelease > 0)
                {
                    solutionUpToRelease = Solve(numberOfTimeStepsUpToRelease / (double)numberOfTimeSteps * timeMax.InMinutes(), productAmountAtRelease, numberOfTimeStepsUpToRelease);
                }
                else
                {
                    // Only the first point, at t=0, of the graph plotting points falls before the end of release. There is no interval for integration.
                    solutionUpToRelease = new double[1, NumberOfEquations + 1];
                    for (int i = 0; i < NumberOfEquations + 1; i++)
                    {
                        solutionUpToRelease[0, i] = 0.0;
                    }
                }

                if (releaseDuration > 0)
                { // Additionally, the tRelease may fall in between two time steps. E.g. if tMax = 100 minutes and tRelease = 17.5 minutes.
                  // Therefore, a bit of a hack is applied by solving twice. Once to the nice time points at each minute,
                  // and once to get the air concentration to use for the start of the interval after tRelease.
                    var solutionForReleaseDuration = Solve(releaseDuration, productAmountAtRelease, 1);

                    airConcentrationAtRelease = solutionForReleaseDuration[1, IndexOfInstantAirConcentrationSeries];
                    productAmountAtRelease = solutionForReleaseDuration[1, IndexOfAmountInProductSeries];
                    integratedAirConcentrationAtRelease = solutionForReleaseDuration[1, IndexOfIntegratedAirConcentrationSeries];
                }
                else
                {
                    airConcentrationAtRelease = 0.0;
                    integratedAirConcentrationAtRelease = 0.0;
                }

                solutionComplete = new double[numberOfTimeSteps + 1, NumberOfEquations + 1];

                int numberOfTimeStepsAfterRelease = numberOfTimeSteps - numberOfTimeStepsUpToRelease;
                SolveAnalytically(releaseDuration, timeMax, airConcentrationAtRelease, integratedAirConcentrationAtRelease, numberOfTimeSteps, numberOfTimeStepsAfterRelease);

                for (int i = 0; i <= numberOfTimeStepsUpToRelease; i++)
                {
                    for (int j = 0; j < NumberOfEquations + 1; j++) // time is first dimension.
                    {
                        solutionComplete[i, j] = solutionUpToRelease[i, j];
                    }
                }
            }

            solutionPrepared = true;
        }

        /// <summary>
        /// Use the analytic Instantaneous release model to calculate the air concentration after evaporation has stopped.
        /// </summary>
        /// <param name="tReleaseMinutes">The release duration in minutes.</param>
        /// <param name="tMax">The end time of the simulation.</param>
        /// <param name="airConcentrationAtRelease">The air concentration at tRelease.</param>
        /// <param name="integratedAirConcentrationAtRelease">The integrated air concentration at release.</param>
        /// <param name="numberOfInterpolationPoints">The number of interpolation points.</param>
        /// <param name="numberOfTimeStepsAfterRelease">The number of time steps after release.</param>
        private void SolveAnalytically(double tReleaseMinutes, Time tMax, double airConcentrationAtRelease, double integratedAirConcentrationAtRelease, int numberOfInterpolationPoints, int numberOfTimeStepsAfterRelease)
        {
            // Calculate the total amount of product in the air, to use as a faked release amount for instantaneous release.
            A = airConcentrationAtRelease * V;

            for (int step = (numberOfInterpolationPoints - numberOfTimeStepsAfterRelease + 1); step <= numberOfInterpolationPoints; step++)
            {
                Time tIn = new Time(tMax.InMinutes() * step / numberOfInterpolationPoints - tReleaseMinutes, TimeUnits.Minute);

                double airConcentration = base.InstantaneousAirConcentration(tIn).InMilligramPerCubicMetre();
                double integratedAirConcentration = integratedAirConcentrationAtRelease + base.MeanAirConcentration(tIn).InMilligramPerCubicMetre() * tIn.InMinutes();
                //Debug.WriteLine(String.Format("Analytic solution at time {0} is {1}", tIn.InMinutes(), airConcentration.InMilligramPerCubicMetre()));

                // Apply an offset to tRelease, as the analytical part is the tail solution for the period after release stops, but the analytical models assumes the calculation is started at t=0.
                solutionComplete[step, IndexOfTimeSeries] = tReleaseMinutes + tIn.InMinutes();
                solutionComplete[step, IndexOfInstantAirConcentrationSeries] = airConcentration;
                solutionComplete[step, IndexOfIntegratedAirConcentrationSeries] = integratedAirConcentration;
            }
        }

        /// <summary>
        /// Value indicating whether integration has been completed.
        /// </summary>
        private bool solutionPrepared = false;

        public override AirConcentration MeanAirConcentration()
        {
            return MeanAirConcentration(ApplicableExposureDuration.AsTime(), false);
        }

        public override AirConcentration MeanAirConcentration(Time time)
        {
            return MeanAirConcentration(time, true);
        }

        public override AirConcentration PeakAirConcentration(Time time)
        {
            _peakInterval = this.PeakInterval(time);

            double peakAirConcentration = PeakAirConcentration(_peakInterval, _helperEvaporation.MeanAirConcentration(_peakInterval.StartTime).Value.Value, _helperEvaporation.MeanAirConcentration(_peakInterval.EndTime).Value.Value);

            return new AirConcentration
            {
                Value = peakAirConcentration,
                Unit = DensityUnits.MilligramPerCubicMetre
            };
        }

        public override TimeInterval PeakInterval(Time intervalLength)
        {
            bool peakIsWholeExposureInterval;
            TimeInterval initialBracketTimeInterval;

            if (ApplicableExposureDuration.AsTime() <= intervalLength)
            {
                peakIsWholeExposureInterval = true;
                // The peak interval is simply the whole exposure interval.
                _peakInterval = new TimeInterval(0, ApplicableExposureDuration.InMinutes(), TimeUnits.Minute);
                initialBracketTimeInterval = _peakInterval;
                // Do not return yet, as we must also prepare the helper solution to be able to integrate in air concentration later on.
            }
            else
            {
                peakIsWholeExposureInterval = false;
                // The initial bracket is a wide interval that is guaranteed to contain de peak interval.
                initialBracketTimeInterval = InitialBracket(solutionComplete, ApplicableExposureDuration.AsTime(), intervalLength, _numberOfTimeSteps, IndexOfInstantAirConcentrationSeries);
            }

            // Take a large number of points for a good interpolation.
            const int stepsPerMinute = 6;
            int numberOfTimeStepsInInitialBracket = (int)Math.Round(stepsPerMinute * initialBracketTimeInterval.EndTime.InMinutes());

            // Create a new model instance to recalculate the solution over the peak interval, with a much better resolution.
            _helperEvaporation = new InhalationExposureVapourEvaporation(scenario);
            _helperEvaporation.PrepareTimeSeries(initialBracketTimeInterval.EndTime, numberOfTimeStepsInInitialBracket);
            double[,] helperEvaporationSolution = _helperEvaporation.solutionComplete;

            if (peakIsWholeExposureInterval) return _peakInterval;

            if (FindPeakInterval(intervalLength, numberOfTimeStepsInInitialBracket,
                    initialBracketTimeInterval, helperEvaporationSolution, IndexOfTimeSeries,
                    IndexOfInstantAirConcentrationSeries, _unitOfSolution, out _peakInterval))
            {
                return _peakInterval;
            }

#warning To Do: handle more user friendly.
            throw new ApplicationException("Could not find a peak interval");
        }

        public override AirConcentration MeanAirConcentrationPeak()
        {
            Debug.Assert(scenario.InhalationExposure.ReEntry, "MeanAirConcentrationPeak can only be calculated for a scenario with re-entry.");
            return PeakAirConcentration(scenario.InhalationExposure.DailyDuration.AsTimePerDay());
        }

        private AirConcentration MeanAirConcentration(Time time, bool requirePreparedSolution)
        {
            double meanAirConcentration;

#warning To Do: Test for limiting conditions that are not supported by integration of the ODE's.
            if (time.InMinutes() <= 0.0)
            {
                meanAirConcentration = 0.0;
            }
            else
            {
                if (!solutionPrepared)
                {
                    if (requirePreparedSolution)
                    {
                        throw new ApplicationException("A time series must be prepared to make sure the solution has been integrated up to the correct end time.");
                    }

                    PrepareTimeSeries(time);
                }

                // The highest time in the solution. Can be either the complete exposure duration, ot the end time of the bracketed peak interval.
                double totalTime = solutionComplete[_numberOfTimeSteps, 0];
                int maxTimeStep = Math.Min(_numberOfTimeSteps,Convert.ToInt32(_numberOfTimeSteps * time.InMinutes() / totalTime));

                //Debug.Assert(maxTimeStep == _numberOfTimeSteps);
                Debug.Assert(solutionComplete.GetUpperBound(0) >= maxTimeStep);
                meanAirConcentration = MassToAirConcentration(solutionComplete[maxTimeStep, IndexOfIntegratedAirConcentrationSeries] / time.InMinutes());
            }

            return AirConcentration.NewFromGramPerCubicMetre(meanAirConcentration);
        }

        private double MassToAirConcentration(double mass)
        {
            return mass / Vr;
        }

        /* Imported code from C++ Builder ConsExpo 4.0 en converted to C# using
           C++ to C# Converter 3.4 http://www.tangiblesoftwaresolutions.com/
            //--------------------------------------------------------------------------
            //C++ TO C# CONVERTER WARNING: The original C++ declaration of the following method implementation was not found:
            //ORIGINAL LINE: void cEvaporationScenario::AverageAirConcentration_Evaporation(const cTimeParameter& inTimeFrom, const cTimeParameter& inTimeTo, cDensityParameter& ioConcentration, bool inDistributed)
            //--------------------------------------------------------------------------
        */

        public double[,] Solve(double tMax, double productAmountStart, int numberOfTimeSteps)
        {
            double[] y0 = new double[NumberOfEquations];

            // initial values

            // y0[0] is the air concentration
            // y0[1] the amount in the product
            // y0[2] the integrated air concentration

            y0[0] = 0.0;
            y0[1] = productAmountStart;
            y0[2] = 0.0;

            double x0 = 0;
            double xf = tMax;

            double relTol = ConfigSettings.RelTolInhalationExposureVapourEvaporation;

            //using DotNumerics.ODE
            OdeFunction YDot = new OdeFunction(EvaporationModel);
            OdeJacobian Jac = new OdeJacobian(JacEvaporationModel);

            return OdeSolver.Solve(YDot, Jac, x0, xf, NumberOfEquations, y0, relTol, numberOfTimeSteps, true);
        }

        private double[] EvaporationModel(double t, double[] y)
        {
            // CE 4 units: meters, gram, Kelvin, minutes, but may be chosen differently

            // y[0] : amount active substance in the air in gram
            // y[1] : amount active substance in the product-layer in gram
            // y[2] : the integrated amount of substance in the air in gram
            double[] dydt = new double[NumberOfEquations];

            double k;
            double Cair;
            double Pair;
            double PeqGpmmin2;
            double dAp;   // applications rate (amount applied per unit time) [g/min]
            double Art;   // amount of non-substance material (i.e. rest) [g]

            CalculatePhysicalQuantities(t, y, out k, out Cair, out Pair, out PeqGpmmin2, out dAp, out Art);

            // differential equations
            if (!pureForm)
            {
                dydt[0] = k * (PeqGpmmin2 - Pair) - Q * Cair;  // note: Q in m3/min
                dydt[1] = -k * (PeqGpmmin2 - Pair) + dAp * wf;
            }
            else
            {
                // watch out: equations above for pure substance do not stop evaporating by themselves
                // check explicitly for layer depletion
                double dAlayerOut;
                if (y[1] > k * (PeqGpmmin2 - Pair))
                {
                    dAlayerOut = k * (PeqGpmmin2 - Pair);
                }
                else
                {
                    dAlayerOut = y[1]; // all what is left is emitted
                }

                dydt[0] = dAlayerOut - Q * Cair;
                dydt[1] = -dAlayerOut + dAp * wf;
            }

            // Integrated air concentration.
            dydt[2] = y[0];

            //Enable only when needed. Consumes a lot of resources in Monte Carlo simulations.
            //Debug.WriteLine("model:{0};{1};{2}", t, String.Join(";", y), y[0] + y[1]);
            return dydt;
        }

        private double[,] JacEvaporationModel(double t, double[] y)
        {
            // CE 4 units: meters, gram, Kelvin, minutes, but may be chosen differently

            // y[0] : amount active substance in the air in gram
            // y[1] : amount active substance in the product-layer in gram
            // y[2] : integrated amount of active substance in the air in gram

            double[,] jacobian = new double[NumberOfEquations, NumberOfEquations];

            // initialize by setting all elements to zero or to the value independent of model details.
            Array.Clear(jacobian, 0, jacobian.Length);

            double k;
            double Cair;
            double Pair;
            double PeqGpmmin2;
            double dAp;   // applications rate (amount applied per unit time) [g/min]
            double Art;   // amount of non-substance material (i.e. rest) [g]

            CalculatePhysicalQuantities(t, y, out k, out Cair, out Pair, out PeqGpmmin2, out dAp, out Art);

            // Jacobian
            // variables: 0: Aair
            //            1: Aproduct
            // derivative equations:
            // f[0] = k * (Peq - Pair) - Q * Vr * Cair;
            // f[1] = - k* (Peq - Pair) + dAp*wf;

            double DPeqDCv;
            if ((!pureForm) && (Art != 0))
            {
                var B = Art * m / Mr.Value;
                // derivative of Peq with respect to Aproduct
                DPeqDCv = (vp * Pressure.Pascal2GramPerMetrePerMinuteSquared * B) / ((y[1] + B) * (y[1] + B));
            }
            else
            {
                DPeqDCv = 0;
            }

            if (!pureForm)
            {
                // with respect to y[0] (Aair)
                jacobian[0, 0] = -k * R * Pressure.Pascal2GramPerMetrePerMinuteSquared * T / (Vr * m) - Q / Vr; // note: Q in m3/min
                                                                                                                // with respect to y[1] (Aprod)
                jacobian[0, 1] = k * DPeqDCv; // mind the chain rule
                                              // with respect to y[0] (Aair)
                jacobian[1, 0] = k * R * Pressure.Pascal2GramPerMetrePerMinuteSquared * T / (Vr * m);
                // with respect to y[1] (Aprod)
                jacobian[1, 1] = -k * DPeqDCv;
            } // mixture
            else
            {
                // watch out for depletion
                bool depletion = (k * (PeqGpmmin2 - Pair) > y[1]);
                if (depletion)
                {
                    // depletion
                    jacobian[0, 0] = -Q / Vr;
                }
                else
                {
                    jacobian[0, 0] = -k * R * Pressure.Pascal2GramPerMetrePerMinuteSquared * T / (Vr * m) - Q / Vr; // note: Q in m3/min !!
                }
                // pure substance
                jacobian[0, 1] = 0;
                if (depletion)
                {
                    jacobian[1, 0] = 0;
                }
                else
                {
                    jacobian[1, 0] = k * R * Pressure.Pascal2GramPerMetrePerMinuteSquared * T / (Vr * m);
                }
                // pure substance Peq always independent of Ap/As
                jacobian[1, 1] = 0;
            }
#warning ToDo: Is this correct?
            jacobian[2, 0] = 1.0; // derivative of y[0].

            //Debug.WriteLine($"jacobian:{t};{jacobian[0, 0]};{jacobian[1, 0]};{jacobian[2, 0]};{jacobian[1, 0]};{jacobian[1, 1]};{jacobian[1, 2]};{jacobian[2, 0]};{jacobian[2, 1]};{jacobian[2, 2]}");

            return jacobian;
        }

        private void CalculatePhysicalQuantities(double t, double[] y, out double k, out double Cair, out double Pair, out double PeqGpmmin2, out double dAp, out double Art)
        {
            // evaporation rate
            double S;   // surface area

            if (releaseAreaType == InhalationExposureReleaseAreaTypes.Constant || t > releaseDuration)
            {
                S = Stot;
                dAp = 0;
                Art = ApGram * (1 - wf);
            }
            else if (releaseAreaType == InhalationExposureReleaseAreaTypes.Increasing)
            {
                S = dS * t;
                dAp = ApGram / releaseDuration;
                Art = ApGram * (1 - wf) * t / releaseDuration;
            }
            else
            {
                throw new NotSupportedException(string.Format("Unsupported release area type '{0}'", releaseAreaType.ToString()));
            }

            // mass transfer
            if ((releaseAreaType == InhalationExposureReleaseAreaTypes.Constant) && (t > releaseDuration))
            {
                k = 0; // evaporating surface has been covered
            }
            else
            {
                k = S * Kt * m / (R * Pressure.Pascal2GramPerMetrePerMinuteSquared * T); // m/min -> min/m due to proportionality to Pressures rather than concentrations
            }

            Cair = y[0] / Vr;
            Pair = Cair * R * T / m * Pressure.Pascal2GramPerMetrePerMinuteSquared;

            // calculation of the equilibrium vapour pressure
            if (pureForm)
            {
                // pure substance
                PeqGpmmin2 = vp;
            }
            else
            {
                // vapour pressure of substance in mixture is estimated using Raoult's law
                if (y[1] > 0)
                {
                    PeqGpmmin2 = vp / (1 + (Art / y[1]) * m / Mr.Value);
                }
                else
                {
                    // continuous continuation
                    PeqGpmmin2 = 0;
                }
            }
            PeqGpmmin2 = PeqGpmmin2 * Pressure.Pascal2GramPerMetrePerMinuteSquared; //Unit conversion.
        }

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        public override bool ModelIsDistributed
        {
            get
            {
                return route.ProductAmount.IsDistributed
                || route.WeightFractionSubstance.IsDistributed
                || route.RoomVolume.IsDistributed
                || route.VentilationRate.IsDistributed
                || route.ApplicationTemperature.IsDistributed
                || route.VapourPressure.IsDistributed
                || route.MassTransferCoefficient.IsDistributed
                || route.ReleaseArea.IsDistributed
                || (route.ReleaseAreaType == InhalationExposureReleaseAreaTypes.Increasing && route.ApplicationDuration.IsDistributed)
                //Note: MolecularWeightMatrix is not distributable in CEweb, but was in CE4. It is save to leave the test here, as it will return false for a non-distributable parameter.
                || (!route.PureForm && route.MolecularWeightMatrix.IsDistributed)
                || (!route.PureForm && route.ProductInDilution && route.Dilution.IsDistributed)
                || (route.ReEntry && route.DailyDuration.IsDistributed)
                || (route.ReEntry && route.EmissionDurationReEntry.IsDistributed)
                || (!route.ReEntry && route.ExposureDuration.IsDistributed)
                || (!route.ReEntry && route.ReleaseAreaType == InhalationExposureReleaseAreaTypes.Constant && route.EmissionDurationEvaporation.IsDistributed);
            }
        }

        //Note: InhalationRate is not an intrinsic parameter for this model. It is only used in the conversion from Air Concentration to External Event Dose.

        /// <summary>
        /// Saves the ODE solution to a file.
        /// </summary>
        /// <param name="sol">The solution array.</param>
        private static void SaveOdeSolution(double[,] sol)
        {
            const string FileName = "C:\\Temp\\OdeSolution.csv";

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(FileName))
            {
                for (int x = 0; x < sol.GetLength(0); x++)
                {
                    for (int y = 0; y < sol.GetLength(1); y++)
                    {
                        file.Write(sol[x, y].ToString() + ";");
                    }
                    file.WriteLine("");
                }
            }
        }
    }
}