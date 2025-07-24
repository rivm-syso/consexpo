using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RIVM.ConsExpo.Model.Submodels
{
    internal class DermalExposureInstantApplication : DermalExposureBase, IDermalExposureSubmodel
    {
        private const DermalExposureSubmodelTypes type = DermalExposureSubmodelTypes.InstantApplication;

        public DermalExposureSubmodelTypes Type => type;

        public DermalExposureInstantApplication(ScenarioModel scenario)
            : base(scenario, type)
        {
            this.scenario = scenario;
        }

        public override bool IsTimeDependent =>
            scenario.DermalAbsorptionRouteInUse
            && scenario.DermalAbsorption.SubmodelType == DermalAbsorptionSubmodelTypes.DiffusionThroughSkinForInstantApplication;

        public override Duration ApplicableExposureDuration
        {
            get
            {
                Duration duration;

                // Tech Debt: it would be a nicer solution if absorption models would also provide a DefaultTimeMax and that the Simulation classes would compare the default time max of exposure and absorption to decide which would be the default time for the time series.
                if (scenario.DermalAbsorptionRouteInUse)
                {
                    switch (scenario.DermalAbsorption.SubmodelType)
                    {
                        case DermalAbsorptionSubmodelTypes.Fraction:
                            duration = null;
                            break;

                        case DermalAbsorptionSubmodelTypes.DiffusionThroughSkinForInstantApplication:
                            duration = scenario.DermalAbsorption.ExposureDuration;
                            break;

                        default:
                            throw new NotSupportedException(
                                $"Unsupported dermal absorption submodel '{scenario.DermalAbsorption.SubmodelType.ToString()}'");
                    }
                }
                else
                {
                    duration = null;
                }

                return duration;
            }
        }

        /// <summary>
        /// Gets the default time maximum for charts.
        /// </summary>
        /// <value>
        /// The default time maximum.
        /// </value>
        /// <remarks>Although this submodel is not time-dependent, it may be used in combination with a time-dependent absorption model.</remarks>
        public override Time DefaultTimeMax => ApplicableExposureDuration.AsTime();

        /// <summary>
        /// The amount of substance released (in mg): [Product amount] x [weight fraction]
        /// </summary>
        public override double? AmountOfSubstance => AmountOfSubstanceByProductAmount;

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.DermalExposureProductAmount,
                DTO.Models.ModelParameters.DermalExposureWeightFractionSubstance,
                DTO.Models.ModelParameters.DermalExposureRetentionFactor
            };
            return modelParameters;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            IList<ValidationResult> validationResults = new List<ValidationResult>();

            RequireProductAmount(validationResults);
            RequireWeightFractionSubstance(validationResults);
            RequireRetentionFactor(validationResults);

            return validationResults;
        }

        public DermalExposureOutcome CalculatePointValues()
        {
            var outcome = new DermalExposureOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, AmountOfSubstance, scenario.DermalExposure.ExposedArea);
            outcome.Dose = InputToExposure();
            return outcome;
        }

        /// <summary>
        /// Calculates the point values at the specified time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        /// <remarks>This specific submodel is time-independent. The time parameters can be ignored.</remarks>
        public DermalExposureOutcome CalculatePointValues(Time time)
        {
            return CalculatePointValues();
        }

        /// <summary>
        /// Implementation of the dermal exposure submodel 'Instant Application'.
        /// </summary>
        /// <returns></returns>
        private Dose InputToExposure()
        {
            double exposureDoseValue;

            exposureDoseValue = scenario.DermalExposure.ProductAmount.InMilligram()
                * scenario.DermalExposure.RetentionFactor.AsFraction()
                * scenario.DermalExposure.WeightFractionSubstance.AsFraction();

            return new Dose(exposureDoseValue, DoseUnits.Mg);
        }

        public DistributedDermalExposureEndPoints DistributedEndPoints
        {
            get
            {
                bool modelIsDistributed = ModelIsDistributed;

                DistributedDermalExposureEndPoints endPoints = new DistributedDermalExposureEndPoints();

                endPoints.DermalLoadIsDistributed = modelIsDistributed || route.ExposedArea.IsDistributed;
                endPoints.ExternalEventDoseIsDistributed = modelIsDistributed || scenario.Assessment.Population.BodyWeight.IsDistributed;
                endPoints.ExternalDayDoseIsDistributed = endPoints.ExternalEventDoseIsDistributed || scenario.Frequency.IsDistributed;
                endPoints.ExposureFractionIsDistributed = modelIsDistributed;

                return endPoints;
            }
        }

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        public bool ModelIsDistributed =>
            route.ProductAmount.IsDistributed
            || route.WeightFractionSubstance.IsDistributed
            || route.RetentionFactor.IsDistributed;
    }
}