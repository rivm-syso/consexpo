using DotNumerics.ODE;
using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Computations;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using RIVM.ConsExpo.Model.Settings;
using RIVM.ConsExpo.Model.Submodels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RIVM.ConsExpo.Model.SubModels
{
    /// <summary>
    /// Model which simulates transition from a product layer into the skin, by dividing the product layer into a series of small layers between which diffusion occurs.
    /// The library DotNumerics is used to solve the corresponding set of differential equations. This is a set of 'stiff' equations, which require an implicit method.
    ///
    /// This model is special, as it is both an exposure and an absorption model. It implements both interfaces, with some interface properties explicitly for either interface, such as type.
    /// </summary>
    /// <remarks>Update: as a result of a misunderstanding, this model was implemented as both an exposure and an absorption model. This is no longer needed. For the exposure end points, the model DermalExposureDiffusion is used. For the absorption end points, this model.
    /// There is no strong urge to remove exposure code from this model, so it is left here, but not used.</remarks>
    /// <seealso>Press, Numerical Recipes, third edition; 17.1 Runge-Kutta Method, 17.5 Stiff sets of equations;</seealso>
    internal class DermalDiffusionThroughSkinForDiffusion : DermalExposureBase, IDermalExposureSubmodel, IDermalAbsorptionSubmodel
    {
        private const int NumberOfProductLayers = 4; //Excluding the skin.
        private const int NumberOfEquations = NumberOfProductLayers + 1; //Including the skin.
        private const int IndexOfSkinConcentration = 1;
        private const int NumberOfTimeSteps = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="DermalDiffusionThroughSkinForDiffusion" /> class.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        public DermalDiffusionThroughSkinForDiffusion(ScenarioModel scenario)
            : base(scenario, ExposureType)
        {
            this.scenario = scenario;
        }

        /// <summary>
        /// Updates the scenario. Since this model is both exposure and absorption submodel, it must preserve calculation results. They are requested by both the exposure and absorption handling code.
        /// If the scenario is changed, e.g. by Monte Carlo sampling, the scenario must be updated.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        internal void UpdateScenario(ScenarioModel scenario)
        {
            //Make sure all state is updated to reflect the modified scenario.
            this.scenario = scenario;
        }

        /// <summary>
        /// Calculation of the amount of substance (in mg) released: [substance concentration] x [layer thickness] x [exposed area]
        /// </summary>
        public override double? AmountOfSubstance
        {
            get
            {
                double substanceConcentration = route.SubstanceConcentration.InMilligramPerCubicCentimetre();
                double layerThickness = route.LayerThickness.InCentimetre();
                double exposedArea = route.ExposedArea.InSquareCentimetre();

                return substanceConcentration * layerThickness * exposedArea;
            }
        }

        private const DermalExposureSubmodelTypes ExposureType = DermalExposureSubmodelTypes.Diffusion;

        DermalExposureSubmodelTypes IDermalExposureSubmodel.Type => ExposureType;

        private const DermalAbsorptionSubmodelTypes AbsorptionType = DermalAbsorptionSubmodelTypes.DiffusionThroughSkinForDiffusion;

        DermalAbsorptionSubmodelTypes IDermalAbsorptionSubmodel.Type => AbsorptionType;

        public override bool IsTimeDependent => true;

        public override Duration ApplicableExposureDuration => scenario.DermalExposure.ExposureDuration;

        public override Time DefaultTimeMax => ApplicableExposureDuration.AsTime();

        /// <summary>
        /// Validates the specified scenario on completeness and consistency of the input parameters.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate()
        {
            const string MessageFormat = "'{{0}}' is required for dermal submodel '{0}'.";

            IList<ValidationResult> validationResults = ValidateDurationAndFrequency(route.ExposureDuration, scenario.Frequency);

            string diffusionThroughSkinForDiffusionMessageFormat = string.Format(MessageFormat, EnumHelper2<DermalAbsorptionSubmodelTypes>.GetDisplayValue(DermalAbsorptionSubmodelTypes.DiffusionThroughSkinForDiffusion));

            if (!scenario.DermalExposure.ExposureDuration.HasValue)
            {
                validationResults.Add(new ValidationResult(string.Format(diffusionThroughSkinForDiffusionMessageFormat, "Exposure time")));
            }

            if (!scenario.DermalExposure.DiffusionCoefficient.HasValue)
            {
                validationResults.Add(new ValidationResult(string.Format(diffusionThroughSkinForDiffusionMessageFormat, "Diffusion coefficient")));
            }

            if (!scenario.DermalExposure.ExposedArea.HasValue)
            {
                validationResults.Add(new ValidationResult(string.Format(diffusionThroughSkinForDiffusionMessageFormat, "Exposed area")));
            }

            if (!scenario.DermalExposure.LayerThickness.HasValue)
            {
                validationResults.Add(new ValidationResult(string.Format(diffusionThroughSkinForDiffusionMessageFormat, "Layer thickness")));
            }

            if (!scenario.DermalAbsorption.SkinPermeability.HasValue)
            {
                validationResults.Add(new ValidationResult(string.Format(diffusionThroughSkinForDiffusionMessageFormat, "SkinPermeability")));
            }

            return validationResults;
        }

        bool IExposureSubmodel<DermalExposureOutcome>.ModelIsDistributed => ExposureModelIsDistributed;

        private bool ExposureModelIsDistributed =>
            route.SubstanceConcentration.IsDistributed
            || route.DiffusionCoefficient.IsDistributed
            || route.LayerThickness.IsDistributed
            || route.ExposureDuration.IsDistributed;

        bool IAbsorptionSubmodel<DermalExposureOutcome, DermalAbsorptionOutcome>.ModelIsDistributed => AbsorptionModelIsDistributed;

        private bool AbsorptionModelIsDistributed =>
            scenario.DermalAbsorption.ConcentrationInMatrix.IsDistributed
            || scenario.DermalAbsorption.SkinPermeability.IsDistributed;

        DistributedDermalExposureEndPoints IDermalExposureSubmodel.DistributedEndPoints
        {
            get
            {
                bool modelIsDistributed = ExposureModelIsDistributed;

                DistributedDermalExposureEndPoints endPoints = new DistributedDermalExposureEndPoints();

                //This model is calculates the skin concentration. Therefore, it doesn't need the Exposed Area to calculate the load...
                endPoints.DermalLoadIsDistributed = modelIsDistributed;

                //... In stead, it needs the Exposed Area to calculate the amount of substance
                endPoints.ExternalEventDoseIsDistributed = modelIsDistributed || scenario.Assessment.Population.BodyWeight.IsDistributed || route.ExposedArea.IsDistributed;

                endPoints.ExternalDayDoseIsDistributed = endPoints.ExternalEventDoseIsDistributed || scenario.Frequency.IsDistributed;

                endPoints.ExposureFractionIsDistributed = modelIsDistributed;

                return endPoints;
            }
        }

        DistributedAbsorptionEndPoints IAbsorptionSubmodel<DermalExposureOutcome, DermalAbsorptionOutcome>.DistributedEndPoints(bool externalEventDoseIsDistributed)
        {
            var distributedAbsorptionEndPoints = new DistributedAbsorptionEndPoints();

            distributedAbsorptionEndPoints.InternalEventDoseIsDistributed =
                externalEventDoseIsDistributed
                || AbsorptionModelIsDistributed
                || scenario.Assessment.Population.BodyWeight.IsDistributed;

            distributedAbsorptionEndPoints.InternalDayDoseIsDistributed =
                distributedAbsorptionEndPoints.InternalEventDoseIsDistributed
                || scenario.Frequency.IsDistributed;

            distributedAbsorptionEndPoints.InternalYearAverageDoseIsDistributed =
                distributedAbsorptionEndPoints.InternalEventDoseIsDistributed
                || scenario.Frequency.IsDistributed;

            return distributedAbsorptionEndPoints;
        }

        public override List<DoseMeasureType> EndPointsForSensitivityAnalysis()
        {
            return base.EndPointsForSensitivityAnalysis();
        }

        List<ModelParameters> ISubmodel.ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                ModelParameters.DermalAbsorptionSkinPermeability
            };
            return modelParameters;
        }

        DermalExposureOutcome IDermalExposureSubmodel.CalculatePointValues()
        {
            return GetExposurePointValues(scenario.DermalExposure.ExposureDuration.AsTime(), false);
        }

        DermalAbsorptionOutcome IDermalAbsorptionSubmodel.CalculatePointValues(DermalExposureOutcome exposure, Time time)
        {
            return GetAbsorptionPointValues(time, true);
        }

        private DermalAbsorptionOutcome GetAbsorptionPointValues(Time time, bool requirePreparedSolution)
        {
            double load;

            if (scenario.DermalExposure.LayerThickness.InCentimetre() == 0.0)
            {
                load = 0.0;
            }
            else if (time.InMinutes() <= 0.0)
            {
                load = 0.0;
            }
            else
            {
                if (!solutionPrepared)
                {
                    if (requirePreparedSolution)
                    {
                        throw new ApplicationException("A time series must be prepared to make sure the solution has been integrated up to the correct end time.");
                    }

                    Solve(time);
                    solutionPrepared = true;
                }

                double totalTime = scenario.DermalExposure.ExposureDuration.InMinutes();
                int timestep = Convert.ToInt32(NumberOfTimeSteps * time.InMinutes() / totalTime);

                load = solution[timestep, IndexOfSkinConcentration];
            }

            var outcome = new DermalAbsorptionOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency);
            outcome.Dose = new Dose(load, DoseUnits.Mg);
            return outcome;
        }

        DermalAbsorptionOutcome IAbsorptionSubmodel<DermalExposureOutcome, DermalAbsorptionOutcome>.CalculatePointValues(DermalExposureOutcome exposure)
        {
            return GetAbsorptionPointValues(scenario.DermalExposure.ExposureDuration.AsTime(), false);
        }

        DermalExposureOutcome IDermalExposureSubmodel.CalculatePointValues(Time time)
        {
            return GetExposurePointValues(time, true);
        }

        /// <summary>
        /// Calculates the point values at the specified time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="requirePreparedSolution">if set to <c>true</c> [require prepared solution].</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">A time series must be prepared to make sure the solution has been integrated up to the correct end time.</exception>
        /// <remarks>
        /// Since numerical integration is used to calculate point values, the intermediate point are calculated on the fly and integrating once up to the exposure time suffices to generate a time series.
        /// </remarks>
        private DermalExposureOutcome GetExposurePointValues(Time time, bool requirePreparedSolution)
        {
            double load;

            if (scenario.DermalExposure.LayerThickness.InCentimetre() == 0.0)
            {
                load = 0.0;
            }
            else if (time.InMinutes() <= 0.0)
            {
                load = 0.0;
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
                        Solve(time);
                        solutionPrepared = true;
                    }
                }

                double totalTime = scenario.DermalExposure.ExposureDuration.InMinutes();
                int timestep = Convert.ToInt32(NumberOfTimeSteps * time.InMinutes() / totalTime);

                load = solution[timestep, IndexOfSkinConcentration];
            }

            var outcome = new DermalExposureOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, AmountOfSubstance, scenario.DermalExposure.ExposedArea);
            outcome.Dose = new Dose(load, DoseUnits.Mg);
            return outcome;
        }

        private double SkinConcentration2DermalLoad(double skinConcentration)
        {
            return skinConcentration * L / NumberOfProductLayers;
        }

        /// <summary>
        /// Value indicating whether integration has been completed.
        /// </summary>
        private bool solutionPrepared = false;

        public override void PrepareTimeSeries(Time timeMax)
        {
            Solve(timeMax);
            solutionPrepared = true;
        }

        /// <summary>
        /// The array of derivatives.
        /// </summary>
        /// <remarks>Declared once, for better performance and reduced garbage collection.</remarks>
        private double[] dydt = new double[NumberOfEquations];

        /// <summary>
        /// The jacobian of the set of differential equations.
        /// </summary>
        /// <remarks>Declared once, for better performance and reduced garbage collection.</remarks>
        private double[,] jacobian = new double[NumberOfEquations, NumberOfEquations];

        /// <summary>
        /// The solution of the model.
        /// </summary>
        private double[,] solution;

        // note: no strong unit checking here, use units cm, min, mg always, also in calling routine!!

        private double C;
        private double D;
        private double L;
        private double P;
        private double A;
        private double h;
        private double h2;
        private double dh2;

        /// <summary>
        /// Solves the differential equations.
        /// </summary>
        /// <param name="exposureDuration">Duration of the exposure.</param>
        private void Solve(Time exposureDuration)
        {
            if (!Validate().Any())
            {
                C = scenario.DermalExposure.SubstanceConcentration.InMilligramPerCubicCentimetre();
                D = scenario.DermalExposure.DiffusionCoefficient.InSquareCentimetrePerMinute();
                L = scenario.DermalExposure.LayerThickness.InCentimetre();
                P = scenario.DermalAbsorption.SkinPermeability.InCentimetrePerMinute();
                A = scenario.DermalExposure.ExposedArea.InSquareCentimetre();

                // distribute layers equally over the product layer equations, note that Equation 0 corresponds to the skin, which is not a part of the layer (concentration = 0 initially),
                h = L / NumberOfProductLayers;
                h2 = h * h;
                dh2 = D / h2;

                double[] y0 = new double[NumberOfEquations];

#warning (Copied from CE 4.1 code) todo-1: what to do with the weight fraction in the diffusion model?

                // Set boundary conditions t = 0.

                // Y[0] is the skin concentration
                // Y[1] - Y[NumberOfProductLayers] : the concentration in the product, Y[1] being the bottom layer (ordered from bottom to top)

                y0[0] = 0.0; //Initially, there is no product in the skin.

                for (int theLayer = 1; theLayer <= NumberOfProductLayers; theLayer++)
                {
                    y0[theLayer] = C;
                }

                double x0 = 0;
                double xf = exposureDuration.InMinutes();

                double relTol = ConfigSettings.RelTolDermalDiffusionThroughSkinForDiffusion;

                //using DotNumerics.ODE
                OdeFunction YDot = new OdeFunction(DiffusionModel);
                OdeJacobian Jac = new OdeJacobian(JacobianDiffusionModel);

                solution = OdeSolver.Solve(YDot, Jac, x0, xf, NumberOfEquations, y0, relTol, NumberOfTimeSteps, false);

                // mResults[0] contains the amount in the skin layer (mass fluxes have been calculated!
                // this situation is different from the dermal load calculations, where concentration
                // changes heve been calculated)
                // note that Equation 0 corresponds to the skin, which is not
                // a part of the layer (concentration = 0 initially),
            }
        }

        public double[] DiffusionModel(double t, double[] y)
        {
            Array.Clear(dydt, 0, dydt.Length);

            for (int i = 2; i < NumberOfProductLayers; i++)
            {
                dydt[i] = D / h2 * (y[i + 1] - 2 * y[i] + y[i - 1]);
            }

            // handle cases 0, 1 and NumberOfProductLayers separately
            // dydt[0] = D/h2 * (y[1]); // nothing flows out from y[0], which represents the all absorbing skin, we just need to count what comes in, assuming that nothing comes out
            dydt[0] = P * A * y[1]; // in layer 0 the mass is registered, in all others: the concentration

            dydt[1] = -P / h * y[1] + D / h2 * (y[2] - y[1]); // again, nothing flows back from y[0]

            //  dydt[1] = D/h2 * (y[2] - 2*y[1]); // again, nothing flows back from y[0]
            dydt[NumberOfProductLayers] = D / h2 * (-y[NumberOfProductLayers] + y[NumberOfEquations - 2]);

            return dydt;
        }

        public double[,] JacobianDiffusionModel(double t, double[] y)
        {
            // system represented by layers, the topmost (index = 0) of which represents the skin and is kept at concentration = 0,
            // thus calculating the potential load

            // note: no strong unit checking here, use units cm, min, mg always, also in calling routine!!

            // distribute layers equally over the product layer equations (note: equation 0 corresponds to skin)
            double h = L / (NumberOfProductLayers);
            double h2 = h * h;

            //DotNumerics does not support specification of the direct derivative.
            //for (int i = 0; i < NumberOfEquations; i++)
            //{
            //    dfdx[i] = 0;
            //}

            // Jacobian is a sparse matrix, re-initialize between calls by setting all elements to zero.
            Array.Clear(jacobian, 0, jacobian.Length);

            for (int i = 2; i < NumberOfProductLayers; i++)
            {
                for (int j = 2; j < NumberOfProductLayers; j++)
                {
                    if (i == j)
                    {
                        jacobian[i, j] = -2 * D / h2;
                    }
                    else
                    {
                        if ((i == (j - 1)) || (i == (j + 1)))
                        {
                            jacobian[i, j] = D / h2;
                        }
                    }
                }
            }
            // handle cases 0, 1 and NumberOfEquations separately
            jacobian[0, 0] = 0;
            jacobian[0, 1] = P * A;
            jacobian[1, 0] = 0;
            jacobian[1, 1] = -D / h2 - P / h;
            jacobian[1, 2] = D / h2;
            jacobian[2, 1] = D / h2;

            jacobian[NumberOfEquations - 2, NumberOfEquations - 1] = D / h2;
            jacobian[NumberOfEquations - 1, NumberOfEquations - 2] = D / h2;
            jacobian[NumberOfEquations - 1, NumberOfEquations - 1] = -D / h2;

            return jacobian;
        }
    }
}