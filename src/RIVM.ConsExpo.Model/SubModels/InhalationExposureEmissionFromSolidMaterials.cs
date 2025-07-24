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
using System.Diagnostics;
using System.Linq;

namespace RIVM.ConsExpo.Model.Submodels
{
    /// <summary>
    /// In this model, emission from solid materials is described.
    /// </summary>
    internal class InhalationExposureEmissionFromSolidMaterials : InhalationExposureBase, IInhalationExposureSubmodel
    {
        private const InhalationExposureSubmodelTypes type = InhalationExposureSubmodelTypes.EmissionFromSolidMaterials;

        public InhalationExposureSubmodelTypes Type => type;

        // These two fields are used to store a helper solution with a high time resolution, for determining a peak interval.
        private TimeInterval _peakInterval;

        private InhalationExposureEmissionFromSolidMaterials _helperEmission;

        public InhalationExposureEmissionFromSolidMaterials(ScenarioModel scenario)
            : base(scenario, type, false)
        { }

        /// <summary>
        /// The amount of substance cannot be inferred. Null is returned.
        /// </summary>
        public override double? AmountOfSubstance => AmountOfSubstanceNotSupported;

        #region constants

        private const int NumberOfProductLayers = 40;

        //Index in the solution. The Y vector has indices of one lower, since time is the first column in the solution.
        private const int IndexOfTimeSeries = 0;

        private const int IndexOfInstantaneousAirConcentration = 1;
        private const int IndexOfMeanAirConcentration = NumberOfProductLayers + 2;
        private const int DefaultNumberOfTimeSteps = 100;

        #endregion constants

        #region fields

        private double timeMaxInHours; // the total duration of the simulation.

        private double S;   // emission surface area
        private double D;   // diffusion coefficient
        private double Vr;  // room volume
        private double q;   // ventilation fold
        private double hm;  // mass-transfer coefficient
        private double K;   // material/air partition coefficient
        private double L;   // product thickness
        private double p;   // density product
        private double wf;  // weight fraction of the compound

        #endregion fields

        public override bool IsTimeDependent => true;

        public override Duration ApplicableExposureDuration
        {
            get
            {
                if (scenario.InhalationExposure.ReEntry)
                {
                    return scenario.InhalationExposure.EmissionDurationReEntry;
                }

                return scenario.InhalationExposure.ExposureDurationForEmissionModel;
            }
        }

        public override Time StartTimeOfExposure
        {
            get
            {
                if (route.ReEntry)
                {
                    // StartTimeOfExposure is not supported for re-entry, yet.
                    return new Time(0, TimeUnits.StandardUnit);
                }
                else
                {
                    return route.StartExposure.AsTime();
                }
            }
        }

        public override Time EndTimeOfExposure
        {
            get
            {
                if (route.ReEntry)
                {
                    Debug.Assert((StartTimeOfExposure?.Value ?? 0) == 0);
                    return route.EmissionDurationReEntry.AsTime();
                }
                else
                {
                    return route.ExposureDurationForEmissionModel.AsTime().Add(StartTimeOfExposure);
                }
            }
        }

        public override Time DefaultTimeMax => EndTimeOfExposure;

        /// <summary>
        /// The unit in which the numerical solution is expressed.
        /// </summary>
        /// <remarks>Should be consistent with the units in which the input parameters to the model are expressed.</remarks>
        private readonly TimeUnits _unitOfSolution = TimeUnits.Hour;

        /// <remarks>To be able to support this, the algorithm that calculates the peak interval, must take a possible delayed start into account.</remarks>
        public override bool SupportsPeakAirConcentration => false;

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

        public override List<DoseMeasureType> EndPointsForSensitivityAnalysis()
        {
#warning check if re-entry requires modifications.
            return base.EndPointsForSensitivityAnalysis();
        }

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.InhalationExposureProductSurfaceArea,
                DTO.Models.ModelParameters.InhalationExposureProductThickness,
                DTO.Models.ModelParameters.InhalationExposureProductDensity,
                DTO.Models.ModelParameters.InhalationExposureDiffusionCoefficientForEmission,
                DTO.Models.ModelParameters.InhalationExposureWeightFractionSubstanceForEmission,
                DTO.Models.ModelParameters.InhalationExposureProductAirPartitionCoefficient,
                DTO.Models.ModelParameters.InhalationExposureRoomVolume,
                DTO.Models.ModelParameters.InhalationExposureVentilationRate,
                DTO.Models.ModelParameters.InhalationExposureMassTransferCoefficient
            };

            if (route.ReEntry)
            {
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureEmissionDurationReEntry);
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureDailyDuration);
            }
            else
            {
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureExposureDurationForEmissionModel);
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureStartExposure);
            }

            return modelParameters;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (route.ReEntry)
            {
                RequireDailyDuration(validationResults);
                RequireEmissionDurationReEntry(validationResults);
                validationResults.AddRange(ValidateDurationAndFrequency(route.EmissionDurationReEntry, scenario.Frequency));
            }
            else
            {
                RequireExposureDurationForEmissionModel(validationResults);
                RequireStartExposure(validationResults);

                if ((route.StartExposure?.Value.HasValue ?? false) && (route.ExposureDurationForEmissionModel?.HasValue ?? false) && EndTimeOfExposure.InDays() > 0 && scenario.Frequency.Value.HasValue && scenario.Frequency.InTimesPerDay() > (1.0 / EndTimeOfExposure.InDays()))
                {
                    validationResults.Add(new ValidationResult(
                        $"The combination of an end time of exposure of {EndTimeOfExposure.Value.Value} {EndTimeOfExposure.UnitDisplay} and a frequency of {scenario.Frequency.Value.Value} {scenario.Frequency.UnitDisplay} results in overlapping events, which is not supported."));
                }
            }

            RequireProductSurfaceArea(validationResults);
            RequireProductThickness(validationResults);
            RequireProductDensity(validationResults);
            RequireDiffusionCoefficient(validationResults);
            RequireWeightFractionSubstanceForEmission(validationResults);
            RequireProductAirPartitionCoefficient(validationResults);
            RequireMassTransferCoefficient(validationResults);
            RequireRoomVolume(validationResults);
            RequireVentilationRate(validationResults);

            return validationResults;
        }

        /// <summary>
        /// Prepares the time series. Typically needed for numerical solution methods.
        /// </summary>
        /// <param name="timeMax">The maximum time.</param>
        public override void PrepareTimeSeries(Time timeMax)
        {
            PrepareTimeSeries(timeMax, DefaultNumberOfTimeSteps);
        }

        public virtual void PrepareTimeSeries(Time timeMax, int numberOfTimeSteps)
        {
            Solve(timeMax, numberOfTimeSteps);
            solutionPrepared = true;
        }

        public AirConcentration InstantaneousAirConcentration(Time timeTo)
        {
            return InstantaneousAirConcentration(timeTo, true);
        }

        /// <summary>
        /// The solution of the model.
        /// </summary>
        private double[,] solution;

        /// <summary>
        /// Value indicating whether integration has been completed.
        /// </summary>
        private bool solutionPrepared = false;

        private int _numberOfTimeSteps;

        /// <summary>
        /// Calculates the air concentration at the specified time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="requirePreparedSolution">if set to <c>true</c> [require prepared solution].</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">A time series must be prepared to make sure the solution has been integrated up to the correct end time.</exception>
        /// <remarks>
        /// Since numerical integration is used to calculate point values, the intermediate point are calculated on the fly and integrating once up to the exposure time suffices to generate a time series.
        /// </remarks>
        private AirConcentration InstantaneousAirConcentration(Time time, bool requirePreparedSolution)
        {
            double instantaneousAirConcentrationValue;

            if (scenario.InhalationExposure.ProductThickness.InMetre() == 0.0)
            {
                instantaneousAirConcentrationValue = 0.0;
            }
            else if (time.InHours() <= 0.0)
            {
                instantaneousAirConcentrationValue = 0.0;
            }
            else
            {
                if (!solutionPrepared)
                {
                    if (requirePreparedSolution)
                    {
                        throw new ApplicationException("A time series must be prepared to make sure the solution has been integrated up to the correct end time.");
                    }

                    Solve(time, _numberOfTimeSteps);
                    solutionPrepared = true;
                }

                int timeStep = Convert.ToInt32(_numberOfTimeSteps * time.InHours() / timeMaxInHours);

                instantaneousAirConcentrationValue = solution[timeStep, IndexOfInstantaneousAirConcentration];
            }

            return new AirConcentration()
            {
                Value = instantaneousAirConcentrationValue,
                Unit = DensityUnits.MilligramPerCubicMetre
            };
        }

        public AirConcentration MeanAirConcentration()
        {
            return MeanAirConcentration(EndTimeOfExposure, false);
        }

        public override AirConcentration MeanAirConcentration(Time time)
        {
            return MeanAirConcentration(time, true);
        }

        protected AirConcentration MeanAirConcentration(Time time, bool requirePreparedSolution)
        {
            double meanAirConcentrationValue;

            if (time.InHours() <= StartTimeOfExposure.InHours())
            {
                meanAirConcentrationValue = 0;
            }
            else
            {
                if (!solutionPrepared)
                {
                    if (requirePreparedSolution)
                    {
                        throw new ApplicationException("A time series must be prepared to make sure the solution has been integrated up to the correct end time.");
                    }
                    else
                    {
                        Solve(time, DefaultNumberOfTimeSteps);
                        solutionPrepared = true;
                    }
                }

                int indexOfStartOfExposure = Convert.ToInt32(_numberOfTimeSteps * StartTimeOfExposure.InHours() / timeMaxInHours);

                //The end time is the smaller of the sample time and the end of the exposure, because when sample time exceeds the end of the exposure, the exposure stops.
                double sampleTimeInHours = Math.Min(time.InHours(), EndTimeOfExposure.InHours());
                int indexOfSampleTime = Convert.ToInt32(_numberOfTimeSteps * sampleTimeInHours / timeMaxInHours);

                double integratedConcentrationAtStartOfExposure = solution[indexOfStartOfExposure, IndexOfMeanAirConcentration];
                double integratedConcentrationAtSampleTime = solution[indexOfSampleTime, IndexOfMeanAirConcentration];

                //The integrated concentration at sample time is determined at the end time of exposure or at the sample time, whichever comes first.
                //The time average is calculated from 0 to sample time.
                meanAirConcentrationValue = (integratedConcentrationAtSampleTime - integratedConcentrationAtStartOfExposure) / (sampleTimeInHours - StartTimeOfExposure.InHours());
            }

            return new AirConcentration() { Value = meanAirConcentrationValue, Unit = DensityUnits.MilligramPerCubicMetre };
        }

        public override AirConcentration PeakAirConcentration(Time time)
        {
            // Needed to initialize the _helperEmission instance.
            _ = this.PeakInterval(time);

            double peakAirConcentration = PeakAirConcentration(_peakInterval, _helperEmission.MeanAirConcentration(_peakInterval.StartTime).Value.Value, _helperEmission.MeanAirConcentration(_peakInterval.EndTime).Value.Value);

            return new AirConcentration
            {
                Value = peakAirConcentration,
                Unit = DensityUnits.MilligramPerCubicMetre
            };
        }

        /// <summary>
        /// Solves the set of differential equation from time = 0, start of emission, to time max.
        /// </summary>
        /// <param name="timeMax">The maximum time of the integration.</param>
        /// <remarks>
        /// OdeImplicitRungeKutta5 class is used to solve the diffusion-emission equations.
        /// </remarks>
        private void Solve(Time timeMax, int numberOfTimeSteps)
        {
            _numberOfTimeSteps = numberOfTimeSteps;

            this.timeMaxInHours = timeMax.InHours();

            S = scenario.InhalationExposure.ProductSurfaceArea.InSquareMetre();
            D = scenario.InhalationExposure.DiffusionCoefficientForEmission.InSquareMetresPerHour();
            Vr = scenario.InhalationExposure.RoomVolume.InCubicMetres();
            q = scenario.InhalationExposure.VentilationRate.InTimesPerHour();
            hm = scenario.InhalationExposure.MassTransferCoefficient.InMetresPerHour();
            K = scenario.InhalationExposure.ProductAirPartitionCoefficient.AsLinear();
            L = scenario.InhalationExposure.ProductThickness.InMetre();
            p = scenario.InhalationExposure.ProductDensity.InMilligramPerCubicMetre();
            wf = scenario.InhalationExposure.WeightFractionSubstanceForEmission.AsFraction();

            double[] y0 = new double[NumberOfProductLayers + 2];

            // initial values

            // Y[0] : the instantaneous the air concentration
            // Y[1] - Y[NumberOfProductLayers] : the concentration in the product, Y[1] being the top layer (ordered from top to bottom)
            // Y[NumberOfProductLayers + 1] : the average air concentration.

            y0[0] = 0.0;
            y0[NumberOfProductLayers + 1] = 0.0;
            double theCo = p * wf;
            for (int theLayer = 1; theLayer <= NumberOfProductLayers; theLayer++)
            {
                y0[theLayer] = theCo;
            }

            double x0 = 0;
            double xf = timeMax.InHours();

            double relTol = ConfigSettings.RelTolInhalationExposureEmissionFromSolidMaterials;

            //using DotNumerics.ODE
            OdeFunction YDot = new OdeFunction(EmissionModel);
            OdeJacobian Jac = new OdeJacobian(JacEmissionModel);

            solution = OdeSolver.Solve(YDot, Jac, x0, xf, NumberOfProductLayers + 2, y0, relTol, numberOfTimeSteps, false);
        }

        /// <summary>
        /// The set of differential equations describing the emission model.
        /// </summary>
        /// <param name="t">The time.</param>
        /// <param name="y">The vector of concentrations in air, all product layers and the mean air concentration.</param>
        private double[] EmissionModel(double t, double[] y)
        {
            double[] dydt = new double[NumberOfProductLayers + 2];
            double h = L / NumberOfProductLayers;
            double dh2 = D / (h * h);

            // model equations

            // air concentration
            var emissionFromTopLayer = (S * hm / Vr) * (y[1] / K - y[0]);
            dydt[0] = emissionFromTopLayer - q * y[0];        // air concentration

            // concentration in product layers
            var emissionToAir = -(hm / h) * (y[1] / K - y[0]);
            dydt[1] = emissionToAir + dh2 * (y[2] - y[1]); // concentration in top layer

            for (int i = 2; i < NumberOfProductLayers + 1; i++)
            {
                dydt[i] = dh2 * (y[i + 1] + y[i - 1] - 2 * y[i]); // concentration in middle product layers
            }

            dydt[NumberOfProductLayers] = dh2 * (y[NumberOfProductLayers - 1] - y[NumberOfProductLayers]); // concentration in the bottom layer

            // cumulative air concentration (used to calculate average air concentration)
            dydt[NumberOfProductLayers + 1] = y[0];
            return dydt;
        }

        /// <summary>
        /// The jacobian of the emission model. Needed for implicit integration.
        /// </summary>
        /// <param name="t">The time.</param>
        /// <param name="y">The vector of concentrations in air, all product layers and the mean air concentration.</param>
        /// <returns></returns>
        public double[,] JacEmissionModel(double t, double[] y)
        {
            double[,] jacobian = new double[NumberOfProductLayers + 2, NumberOfProductLayers + 2];
            double h = L / NumberOfProductLayers;
            double dh2 = D / (h * h);

            // Jacobian is a sparse matrix, initialize by setting all elements to zero
            Array.Clear(jacobian, 0, jacobian.Length);

            jacobian[0, 0] = -(S * hm / Vr) - q;
            jacobian[0, 1] = (S * hm / Vr) / K;
            jacobian[1, 0] = (hm / h);
            jacobian[1, 1] = -(hm / h) / K - dh2;
            jacobian[1, 2] = dh2;

            for (int i = 2; i < NumberOfProductLayers + 1; i++)
            {
                jacobian[i, i - 1] = dh2;
                jacobian[i, i] = -2 * dh2;
                jacobian[i, i + 1] = dh2;
            }

            jacobian[NumberOfProductLayers, NumberOfProductLayers - 1] = dh2;
            jacobian[NumberOfProductLayers, NumberOfProductLayers] = -dh2;

            jacobian[NumberOfProductLayers + 1, 0] = 1;

            return jacobian;
        }

        /// <remarks>The peak interval can only be used for re-entry, as this algorithm does not support a delayed start of exposure.</remarks>
        public override TimeInterval PeakInterval(Time intervalLength)
        {
            if (ApplicableExposureDuration.AsTime() <= intervalLength)
            {
                // The peak interval is simply the whole exposure interval.
                _peakInterval = new TimeInterval(0, ApplicableExposureDuration.InMinutes(), TimeUnits.Minute);
                return _peakInterval;
            }

            // The initial bracket is a wide interval that is guaranteed to contain de peak interval.
            var initialBracketTimeInterval = InitialBracket(solution, ApplicableExposureDuration.AsTime(), intervalLength, _numberOfTimeSteps, IndexOfInstantaneousAirConcentration);

            // Take a large number of points for a good interpolation.
            const int StepsPerMinute = 6;
            int numberOfTimeStepsInInitialBracket = (int)Math.Round(StepsPerMinute * initialBracketTimeInterval.EndTime.InMinutes());

            // Create a new model instance to recalculate the solution over the peak interval, with a much better resolution.
            _helperEmission = new InhalationExposureEmissionFromSolidMaterials(scenario);
            _helperEmission.PrepareTimeSeries(initialBracketTimeInterval.EndTime, numberOfTimeStepsInInitialBracket);
            double[,] helperEmissionSolution = _helperEmission.solution;

            if (FindPeakInterval(intervalLength, numberOfTimeStepsInInitialBracket,
                    initialBracketTimeInterval, helperEmissionSolution, IndexOfTimeSeries,
                    IndexOfInstantaneousAirConcentration, _unitOfSolution, out _peakInterval))
            {
                return _peakInterval;
            }

#warning To Do: handle correctly
            throw new ApplicationException("Could not find a peak interval");
        }

        public override AirConcentration MeanAirConcentrationPeak()
        {
            return route.ReEntry ? PeakAirConcentration(new Time(scenario.InhalationExposure.DailyDuration.InMinutesPerDay(), TimeUnits.Minute)) : null;
        }

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        public override bool ModelIsDistributed
        {
            get
            {
                bool isDistributed = route.ProductSurfaceArea.IsDistributed
                   || route.ProductThickness.IsDistributed
                   || route.ProductDensity.IsDistributed
                   || route.DiffusionCoefficientForEmission.IsDistributed
                   || route.WeightFractionSubstanceForEmission.IsDistributed
                   || route.ProductAirPartitionCoefficient.IsDistributed
                   || route.RoomVolume.IsDistributed
                   || route.VentilationRate.IsDistributed
                   || route.MassTransferCoefficient.IsDistributed;

                if (route.ReEntry)
                {
                    isDistributed = isDistributed || route.EmissionDurationReEntry.IsDistributed || route.DailyDuration.IsDistributed;
                }
                else
                {
                    isDistributed = isDistributed || route.ExposureDuration.IsDistributed || route.StartExposure.IsDistributed;
                }

                return isDistributed;
            }
        }

        //Note: InhalationRate is not an intrinsic parameter for this model. It is only used in the conversion from Air Concentration to External Event Dose.
    }
}