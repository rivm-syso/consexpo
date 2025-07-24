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
    /// In this model, all of the substance is released at once, subsequently removed by ventilation.
    /// </summary>
    internal class OralExposureDirectIntake : OralExposureBase, IOralExposureSubmodel
    {
        private const OralExposureSubmodelTypes type = OralExposureSubmodelTypes.DirectIntake;

        public OralExposureSubmodelTypes Type => type;

        public OralExposureDirectIntake(ScenarioModel scenario)
            : base(scenario, type)
        {
            this.scenario = scenario;
        }

        /// <summary>
        /// Inputs to exposure.
        /// </summary>
        /// <returns></returns>
        /// <seealso>Borland C++ code: 'classOralScenarios.cpp' lines 231-235</seealso>
        private Dose InputToExposure()
        {
            double A = scenario.OralExposure.IngestedAmountMouthing.InMilligram();
            double wf = scenario.OralExposure.WeightFractionSubstance.AsFraction();

            double exposure = wf * A;

            Dose dose = new Dose(exposure, DoseUnits.Mg);

            return dose;
        }

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.OralExposureWeightFractionSubstance,
                DTO.Models.ModelParameters.OralExposureIngestedAmountMouthing
            };

            return modelParameters;
        }

        public override bool IsTimeDependent => false;

        public override Duration ApplicableExposureDuration => null;

        public IEnumerable<ValidationResult> Validate()
        {
            IList<ValidationResult> validationResults = new List<ValidationResult>();

            RequireAmountIngested(validationResults);
            RequireWeightFractionSubstance(validationResults);

            return validationResults;
        }

        public OralExposureOutcome CalculatePointValues()
        {
            var outcome = new OralExposureOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, AmountOfSubstance);
            outcome.Dose = InputToExposure();
            return outcome;
        }

        /// <summary>
        /// The amount of substance released (in mg): [Ingested amount] x [weight fraction]
        /// </summary>
        public override double? AmountOfSubstance
        {
            get
            {
                double amountIngested = route.IngestedAmountMouthing.InMilligram();
                double weightFractionSubstance = route.WeightFractionSubstance.AsFraction();

                return amountIngested * weightFractionSubstance;
            }
        }

        public OralExposureOutcome CalculatePointValues(Time time)
        {
            return CalculatePointValues();
        }

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        public override bool ModelIsDistributed =>
            route.IngestedAmountMouthing.IsDistributed
            || route.WeightFractionSubstance.IsDistributed;
    }
}