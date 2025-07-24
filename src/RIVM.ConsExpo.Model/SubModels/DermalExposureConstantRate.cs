using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RIVM.ConsExpo.Model.Submodels
{
    internal class DermalExposureConstantRate : DermalExposureBase, IDermalExposureSubmodel
    {
        private const DermalExposureSubmodelTypes type = DermalExposureSubmodelTypes.ConstantRate;

        public DermalExposureSubmodelTypes Type => type;

        public DermalExposureConstantRate(ScenarioModel scenario)
            : base(scenario, type)
        {
            this.scenario = scenario;
        }

        public override bool IsTimeDependent => true;

        public override Duration ApplicableExposureDuration => scenario.DermalExposure.ReleaseDuration;

        /// <summary>
        /// Gets the default time maximum for charts.
        /// </summary>
        /// <value>
        /// The default time maximum.
        /// </value>
        public override Time DefaultTimeMax => ApplicableExposureDuration.AsTime();

        /// <summary>
        /// Calculation of the amount of substance released: [Contact rate] x [release duration] x [weight fraction]
        /// </summary>
        public override double? AmountOfSubstance
        {
            get
            {
                double contactRate = route.ContactRate.InMilligramPerMinute();
                double releaseDuration = route.ReleaseDuration.InMinutes();
                double weightFractionSubstance = route.WeightFractionSubstance.AsFraction();

                return contactRate * releaseDuration * weightFractionSubstance;
            }
        }

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.DermalExposureWeightFractionSubstance,
                DTO.Models.ModelParameters.DermalExposureContactRate,
                DTO.Models.ModelParameters.DermalExposureReleaseDuration
            };
            return modelParameters;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            IList<ValidationResult> validationResults = ValidateDurationAndFrequency(route.ReleaseDuration, scenario.Frequency);

            RequireWeightFractionSubstance(validationResults);
            RequireContactRate(validationResults);
            RequireReleaseDuration(validationResults);

            return validationResults;
        }

        public DermalExposureOutcome CalculatePointValues()
        {
            return CalculatePointValues(scenario.DermalExposure.ReleaseDuration.AsTime());
        }

        public DermalExposureOutcome CalculatePointValues(Time time)
        {
            var outcome = new DermalExposureOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, AmountOfSubstance, scenario.DermalExposure.ExposedArea);
            outcome.Dose = InputToExposure(scenario.DermalExposure.WeightFractionSubstance.AsFraction(), scenario.DermalExposure.ContactRate, time);
            return outcome;
        }

        /// <summary>
        /// Implementation of the dermal absorption submodel 'Constant Rate'.
        /// </summary>
        /// <param name="weightFraction">The weight fraction.</param>
        /// <param name="contactRate">The contact rate.</param>
        /// <param name="time">Duration of the release.</param>
        /// <returns></returns>
        private static Dose InputToExposure(double? weightFraction, ContactRate contactRate, Time time)
        {
            double exposureDoseValue;

            exposureDoseValue = weightFraction.Value
                * contactRate.InMilligramPerMinute()
                * time.InMinutes();

            var dose = new Dose(exposureDoseValue, DoseUnits.Mg);

            return dose;
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
            route.WeightFractionSubstance.IsDistributed
            || route.ContactRate.IsDistributed
            || route.ReleaseDuration.IsDistributed;
    }
}