using DotNumerics.ODE;
using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Computations;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using RIVM.ConsExpo.Model.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RIVM.ConsExpo.Model.Submodels
{
    /// <summary>
    /// Model which simulates transition from a product layer to the skin, by dividing the product layer into a series of small layers between which diffusion occurs.
    /// The library DotNumerics is used to solve the corresponding set of differential equations. This is a set of 'stiff' equations, which require an implicit method.
    /// </summary>
    /// <seealso>Press, Numerical Recipes, third edition; 17.1 Runge-Kutta Method, 17.5 Stiff sets of equations;</seealso>
    internal class DermalExposureDiffusion : DermalExposureBase, IDermalExposureSubmodel
    {
        private const int NumberOfProductLayers = 4; //Excluding the skin.
        private const int NumberOfEquations = NumberOfProductLayers + 1; //Including the skin.
        private const int IndexOfSkinConcentration = 1;
        private const int NumberOfTimeSteps = 100;

        private const DermalExposureSubmodelTypes type = DermalExposureSubmodelTypes.Diffusion;

        public DermalExposureSubmodelTypes Type => DermalExposureSubmodelTypes.Diffusion;

        public DermalExposureDiffusion(ScenarioModel scenario)
            : base(scenario, type)
        {
            this.scenario = scenario;
        }

        public override bool IsTimeDependent => true;

        public override Duration ApplicableExposureDuration => scenario.DermalExposure.ExposureDuration;

        public override Time DefaultTimeMax => ApplicableExposureDuration.AsTime();

        public override List<DoseMeasureType> EndPointsForSensitivityAnalysis()
        {
            return base.EndPointsForSensitivityAnalysis();
        }

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.DermalExposureSubstanceConcentration,
                DTO.Models.ModelParameters.DermalExposureDiffusionCoefficient,
                DTO.Models.ModelParameters.DermalExposureLayerThickness,
                DTO.Models.ModelParameters.DermalExposureExposureDuration
            };
            return modelParameters;
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

        public IEnumerable<ValidationResult> Validate()
        {
            IList<ValidationResult> validationResults = ValidateDurationAndFrequency(route.ExposureDuration, scenario.Frequency);

            RequireExposedArea(validationResults);
            RequireSubstanceConcentration(validationResults);
            RequireDiffusionCoefficient(validationResults);
            RequireLayerThickness(validationResults);
            RequireExposureTime(validationResults);

            return validationResults;
        }

        public DistributedDermalExposureEndPoints DistributedEndPoints
        {
            get
            {
                bool modelIsDistributed = ModelIsDistributed;

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

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        public bool ModelIsDistributed =>
            route.SubstanceConcentration.IsDistributed
            || route.DiffusionCoefficient.IsDistributed
            || route.LayerThickness.IsDistributed
            || route.ExposureDuration.IsDistributed;

        public override void PrepareTimeSeries(Time timeMax)
        {
            Solve(timeMax.InSeconds());
            solutionPrepared = true;
        }

        public DermalExposureOutcome CalculatePointValues()
        {
            return CalculatePointValues(scenario.DermalExposure.ExposureDuration.AsTime(), false);
        }

        /// <summary>
        /// Value indicating whether integration has been completed.
        /// </summary>
        private bool solutionPrepared = false;

        public DermalExposureOutcome CalculatePointValues(Time time)
        {
            return CalculatePointValues(time, true);
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
        private DermalExposureOutcome CalculatePointValues(Time time, bool requirePreparedSolution)
        {
            double load;

            if (scenario.DermalExposure.LayerThickness.InCentimetre() == 0.0)
            {
                load = 0.0;
            }
            else if (time.InSeconds() <= 0.0)
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
                        Solve(time.InSeconds());
                        solutionPrepared = true;
                    }
                }

                double totalTime = scenario.DermalExposure.ExposureDuration.InSeconds();
                int timestep = Convert.ToInt32(NumberOfTimeSteps * time.InSeconds() / totalTime);

                var skinConcentration = solution[timestep, IndexOfSkinConcentration];
                load = SkinConcentration2DermalLoad(skinConcentration);
            }

            var outcome = new DermalExposureOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, AmountOfSubstance, scenario.DermalExposure.ExposedArea);
            outcome.Dose = new Dose(load, DoseUnits.MgPerSquareCentimetre);
            return outcome;
        }

        private double SkinConcentration2DermalLoad(double skinConcentration)
        {
            return skinConcentration * L / NumberOfProductLayers;
        }

        /// <summary>
        /// Thickness of the product layer.
        /// </summary>
        private double L;   // product thickness

        /// <summary>
        /// Concentration of the substance.
        /// </summary>
        private double C;

        /// <summary>
        /// Diffusion coefficient
        /// </summary>
        private double D;

        /// <summary>
        /// Diffusion coefficient divided by the layer thickness squared;
        /// </summary>
        private double dh2;

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

        /// <summary>
        /// Solves the differential equations.
        /// </summary>
        /// <param name="exposureDuration">Duration of the exposure.</param>
        private void Solve(double exposureDuration)
        {
            C = scenario.DermalExposure.SubstanceConcentration.InMilligramPerCubicCentimetre();
            L = scenario.DermalExposure.LayerThickness.InCentimetre();
            D = scenario.DermalExposure.DiffusionCoefficient.InSquareCentimetrePerSecond();

            // distribute layers equally over the product layer equations, note that Equation 0 corresponds to the skin, which is not a part of the layer (concentration = 0 initially),
            double h = L / NumberOfProductLayers;
            dh2 = D / (h * h);

            double[] y0 = new double[NumberOfEquations];

            // Set boundary conditions t = 0.

            // Y[0] is the skin concentration
            // Y[1] - Y[NumberOfProductLayers] : the concentration in the product, Y[1] being the bottom layer (ordered from bottom to top)

            y0[0] = 0.0; //Initially, there is no product in the skin.

            for (int theLayer = 1; theLayer <= NumberOfProductLayers; theLayer++)
            {
                y0[theLayer] = C;
            }

            double x0 = 0;
            double xf = exposureDuration;

            OdeFunction YDot = new OdeFunction(DiffusionModel);
            OdeJacobian Jac = new OdeJacobian(JacobianDiffusionModel);

            double relTol = ConfigSettings.RelTolDermalExposureDiffusion;

            solution = OdeSolver.Solve(YDot, Jac, x0, xf, NumberOfEquations, y0, relTol, NumberOfTimeSteps, false);
        }

        /// <summary>
        /// The set of differential equations describing the emission model.
        /// </summary>
        /// <param name="t">The time.</param>
        /// <param name="y">The vector of concentrations in the skin and in all product layers.</param>
        /// <returns></returns>
        private double[] DiffusionModel(double t, double[] y)
        {
            Array.Clear(dydt, 0, dydt.Length);

            // model equations

            // handle cases 0, 1 and NumberOfProductLayers separately, as adjacent layers are different here.
            dydt[0] = dh2 * y[1];
            dydt[1] = dh2 * (y[2] - 2 * y[1]); // again, nothing flows back from Y[0], the skin.

            for (int i = 2; i <= NumberOfProductLayers - 1; i++)
            {
                dydt[i] = dh2 * (y[i + 1] - 2 * y[i] + y[i - 1]);
            }

            dydt[NumberOfProductLayers] = dh2 * (-y[NumberOfProductLayers] + y[NumberOfProductLayers - 1]); //Topmost layer does not interchange with the air.

            //Enable only when needed. Consumes a lot of resource in Monte Carlo simulations.
            //Debug.WriteLine("{0};{1}", t, String.Join(";", y));

            return dydt;
        }

        /// <summary>
        /// The jacobian of the diffusion model. Needed for implicit integration.
        /// </summary>
        /// <param name="t">The time.</param>
        /// <param name="y">The vector of concentrations in the skin and in all product layers.</param>
        /// <returns></returns>
        public double[,] JacobianDiffusionModel(double t, double[] y)
        {
            // Jacobian is a sparse matrix, re-initialize between calls by setting all elements to zero.
            Array.Clear(jacobian, 0, jacobian.Length);

            for (int i = 2; i <= NumberOfProductLayers - 1; i++)
            {
                jacobian[i, i - 1] = dh2;
                jacobian[i, i] = -2 * dh2;
                jacobian[i, i + 1] = dh2;
            }

            // handle cases 0, 1 and NumberOfProductLayers separately
            jacobian[0, 0] = 0;
            jacobian[0, 1] = dh2;
            jacobian[1, 0] = 0;
            jacobian[1, 1] = -2 * dh2;
            jacobian[1, 2] = dh2;
            jacobian[2, 1] = dh2;

            jacobian[NumberOfProductLayers - 1, NumberOfProductLayers] = dh2;
            jacobian[NumberOfProductLayers, NumberOfProductLayers - 1] = dh2;
            jacobian[NumberOfProductLayers, NumberOfProductLayers] = -dh2;

            return jacobian;
        }
    }
}