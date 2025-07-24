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
    /// Implementation of the oral exposure submodel 'Product Mouthing'.
    /// </summary>
    internal class OralExposureProductMouthing : OralExposureBase, IOralExposureSubmodel
    {
        private const OralExposureSubmodelTypes type = OralExposureSubmodelTypes.ProductMouthing;

        public OralExposureSubmodelTypes Type => type;

        public OralExposureProductMouthing(ScenarioModel scenario)
            : base(scenario, type)
        {
            this.scenario = scenario;
        }

        public override bool IsTimeDependent => true;

        public override Duration ApplicableExposureDuration => scenario.OralExposure.ExposureDuration;

        public override Time DefaultTimeMax => ApplicableExposureDuration.AsTime();

        public override List<DoseMeasureType> EndPointsForSensitivityAnalysis()
        {
            return base.EndPointsForSensitivityAnalysis();
        }

        /// <summary>
        /// The amount of substance released (in mg): [Product amount] x [weight fraction]
        /// </summary>
        public override double? AmountOfSubstance => AmountOfSubstanceByProductAmount;

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.OralExposureWeightFractionSubstance,
                DTO.Models.ModelParameters.OralExposureProductAmount,
                DTO.Models.ModelParameters.OralExposureExposureDuration,
                DTO.Models.ModelParameters.OralExposureContactAreaMouthing,
                DTO.Models.ModelParameters.OralExposureInitialMigrationRate
            };
            return modelParameters;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            IList<ValidationResult> validationResults = ValidateDurationAndFrequency(route.ExposureDuration, scenario.Frequency);

            RequireWeightFractionSubstance(validationResults);
            RequireProductAmount(validationResults);
            RequireExposureDuration(validationResults);
            RequireContactArea(validationResults);
            RequireInitialMigrationRate(validationResults);

            return validationResults;
        }

        public OralExposureOutcome CalculatePointValues()
        {
            return CalculatePointValues(scenario.OralExposure.ExposureDuration.AsTime());
        }

        public OralExposureOutcome CalculatePointValues(Time time)
        {
            var outcome = new OralExposureOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, AmountOfSubstance);
            outcome.Dose = InputToExposure(time);
            return outcome;
        }

        private Dose InputToExposure(Time time)
        {
            double exposureDoseValue;
            double migrationRate;

            //ConsExpo sample code.
            // Wf = WeightFraction
            // t  = ExposureDuration[min]
            // R = MigrationRate [mg/cm2/min]
            // A = ProductAmount [mg]
            // S = ContactArea [cm2]
            //
            //  if (Wf * A > 0) {
            //      l = R * S / (A * Wf); [1/min]
            //  } else {
            //      l = 0;
            //  }

            //Dpot  =  A * Wf * (1 - exp (- l * t));

            if ((scenario.OralExposure.ProductAmount.InMilligram() * scenario.OralExposure.WeightFractionSubstance.AsFraction()) > 0)
            {
                migrationRate = scenario.OralExposure.InitialMigrationRate.InMilliGramPerSquareCentimetresPerSecond() * scenario.OralExposure.ContactAreaMouthing.InSquareCentimetre()
                    / (scenario.OralExposure.ProductAmount.InMilligram() * scenario.OralExposure.WeightFractionSubstance.AsFraction());
            }
            else
            {
                migrationRate = 0;
            }

            exposureDoseValue = scenario.OralExposure.ProductAmount.InMilligram() * scenario.OralExposure.WeightFractionSubstance.AsFraction()
                * (1 - System.Math.Exp(-migrationRate * time.InSeconds()));

            var dose = new Dose(exposureDoseValue, DoseUnits.Mg);

            return dose;
        }

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        public override bool ModelIsDistributed =>
            route.ContactAreaMouthing.IsDistributed
            || route.ExposureDuration.IsDistributed
            || route.InitialMigrationRate.IsDistributed
            || route.ProductAmount.IsDistributed
            || route.WeightFractionSubstance.IsDistributed;
    }
}