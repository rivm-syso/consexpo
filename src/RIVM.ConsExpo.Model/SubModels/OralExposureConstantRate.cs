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
    /// <summary>
    /// Implementation of the oral absorption submodel 'Constant Rate'.
    /// </summary>
    internal class OralExposureConstantRate : OralExposureBase, IOralExposureSubmodel
    {
        private const OralExposureSubmodelTypes type = OralExposureSubmodelTypes.ConstantRate;

        public OralExposureSubmodelTypes Type => type;

        public OralExposureConstantRate(ScenarioModel scenario)
            : base(scenario, type)
        {
        }

        public override bool IsTimeDependent => true;

        public override Duration ApplicableExposureDuration => scenario.OralExposure.ExposureDuration;

        public override Time DefaultTimeMax => ApplicableExposureDuration.AsTime();

        /// <summary>
        /// Calculation of the amount of substance (in mg) released: [Ingestion rate] x [Exposure duration] x [weight fraction]
        /// </summary>
        public override double? AmountOfSubstance
        {
            get
            {
                {
                    double ingestionRate = route.IngestionRate.InMilligramPerMinute();
                    double exposureDuration = route.ExposureDuration.InMinutes();
                    double weightFractionSubstance = route.WeightFractionSubstance.AsFraction();

                    return ingestionRate * exposureDuration * weightFractionSubstance;
                }
            }
        }

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.OralExposureWeightFractionSubstance,
                DTO.Models.ModelParameters.OralExposureIngestionRate,
                DTO.Models.ModelParameters.OralExposureExposureDuration
            };
            return modelParameters;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            IList<ValidationResult> validationResults = ValidateDurationAndFrequency(route.ExposureDuration, scenario.Frequency);

            RequireWeightFractionSubstance(validationResults);
            RequireIngestionRate(validationResults);
            RequireExposureDuration(validationResults);

            return validationResults;
        }

        private Dose InputToExposure(Time time)
        {
            double exposureDoseValue;

            //ConsExpo sample code.
            //theWeightFraction = GetWeightFraction().GetInFraction();
            //theIngestionRate = mIngestionRate.GetInMilligramPerMinute() * theWeightFraction;
            //outDose.SetInMilligram(theIngestionRate * theContactDuration);

            exposureDoseValue = scenario.OralExposure.WeightFractionSubstance.AsFraction()
                * scenario.OralExposure.IngestionRate.InMilligramPerMinute()
                * time.InMinutes();

            var dose = new Dose(exposureDoseValue, DoseUnits.Mg);

            return dose;
        }

        public OralExposureOutcome CalculatePointValues()
        {
            return CalculatePointValues(scenario.OralExposure.ExposureDuration.AsTime());
        }

        public OralExposureOutcome CalculatePointValues(Time time)
        {
            var outcome = new OralExposureOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, AmountOfSubstance)
            { Dose = InputToExposure(time) };
            return outcome;
        }

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        public override bool ModelIsDistributed =>
            route.ExposureDuration.IsDistributed
            || route.IngestionRate.IsDistributed
            || route.WeightFractionSubstance.IsDistributed;
    }
}