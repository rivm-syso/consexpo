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
    internal class DermalExposureMigration : DermalExposureBase, IDermalExposureSubmodel
    {
        private const DermalExposureSubmodelTypes type = DermalExposureSubmodelTypes.Migration;

        public DermalExposureSubmodelTypes Type => DermalExposureSubmodelTypes.Migration;

        public DermalExposureMigration(ScenarioModel scenario)
            : base(scenario, type)
        {
            this.scenario = scenario;
        }

        /// <summary>
        /// Implementation of the dermal exposure submodel 'Migration'.
        /// </summary>
        /// <returns></returns>
        /* B2B code  Migration:
         * - external event dose: rr 486-491
         * - internal event dose: rr 741-758
         * theAmount = mAppliedProductAmount.GetDistributedNumber (kMiligramString);
         * theLeachableFraction = mLeachableFraction.GetDistributedNumber (kFractionString);
         * theSkinContactFactor = mSkinContactFactor.GetDistributedNumber (kFractionString);
         * outDose.SetInMiligrams (theAmount*theLeachableFraction*theSkinContactFactor);
        */

        public override bool IsTimeDependent => false;

        public override Duration ApplicableExposureDuration => null;

        /// <summary>
        /// The amount of substance cannot be inferred. Null is returned.
        /// </summary>
        public override double? AmountOfSubstance => AmountOfSubstanceNotSupported;

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.DermalExposureProductAmount,
                DTO.Models.ModelParameters.DermalExposureLeachableFraction,
                DTO.Models.ModelParameters.DermalExposureSkinContactFactor
            };

            return modelParameters;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            IList<ValidationResult> validationResults = new List<ValidationResult>();

            RequireProductAmount(validationResults);
            RequireLeachableFraction(validationResults);
            RequireSkinContactFactor(validationResults);

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
        /// <remarks>This specific submodel is time-indepedent. The time parameters can be ignored.</remarks>
        public DermalExposureOutcome CalculatePointValues(Time time)
        {
            return CalculatePointValues();
        }

        private Dose InputToExposure()
        {
            double exposureDoseValue;

            exposureDoseValue = route.ProductAmount.InMilligram() * route.LeachableFraction.AsFraction()
                * route.SkinContactFactor.AsFraction();

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
        public virtual bool ModelIsDistributed =>
            route.ProductAmount.IsDistributed
            || route.LeachableFraction.IsDistributed
            || route.SkinContactFactor.IsDistributed;
    }
}