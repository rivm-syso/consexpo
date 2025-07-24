using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Computations;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RIVM.ConsExpo.Model.Submodels
{
    /// <summary>
    /// In this model, all of the substance is released as spray at once, subsequently removed by ventilation.
    /// </summary>
    internal class InhalationExposureSprayInstantaniousRelease : InhalationExposureInstantaniousReleaseBase, IInhalationExposureSubmodel
    {
        private const InhalationExposureSubmodelTypes type = InhalationExposureSubmodelTypes.SprayInstantaneousRelease;

        public InhalationExposureSubmodelTypes Type => type;

        public InhalationExposureSprayInstantaniousRelease(ScenarioModel scenario)
            : base(scenario, type, true)
        { }

        public override bool IsTimeDependent => true;

        public override bool SupportsPeakAirConcentration => true;

        public override Duration ApplicableExposureDuration => scenario.InhalationExposure.ExposureDuration;

        public override Time DefaultTimeMax => ApplicableExposureDuration.AsTime();

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.InhalationExposureExposureDuration,
                DTO.Models.ModelParameters.InhalationExposureReleasedMass,
                DTO.Models.ModelParameters.InhalationExposureWeightFractionSubstance,
                DTO.Models.ModelParameters.InhalationExposureRoomVolume,
                DTO.Models.ModelParameters.InhalationExposureVentilationRate
            };
            return modelParameters;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            IList<ValidationResult> validationResults = ValidateDurationAndFrequency(route.ExposureDuration, scenario.Frequency);

            RequireExposureDuration(validationResults);
            RequireReleasedMass(validationResults);
            RequireWeightFractionSubstance(validationResults);
            RequireRoomVolume(validationResults);
            RequireVentilationRate(validationResults);

            return validationResults;
        }

        /// <summary>
        /// Parses the scenario and maps all relevant parameters to the values used in the calculation.
        /// </summary>
        protected override void ParseScenario()
        {
            T0 = 0.0;
            wf = scenario.InhalationExposure.WeightFractionSubstance.AsFraction();
            V = scenario.InhalationExposure.RoomVolume.InCubicMetres();
            q = scenario.InhalationExposure.VentilationRate.InTimesPerSecond();
            A = scenario.InhalationExposure.ReleasedMass.InMilligram() * wf;
            //If saturation occurs, spray aerosols will stop to evaporate, but will be ventilated just as well.
            //So, saturation must not be taken into account.
            limitConcentrationToSaturatedAirConcentration = false;
        }

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        public override bool ModelIsDistributed =>
            route.ExposureDuration.IsDistributed
            || route.ReleasedMass.IsDistributed
            || route.WeightFractionSubstance.IsDistributed
            || route.RoomVolume.IsDistributed
            || route.VentilationRate.IsDistributed;

        /// <summary>
        /// The amount of substance released.
        /// </summary>
        public override double? AmountOfSubstance
        {
            get
            {
                double releasedMass = route.ReleasedMass.InMilligram();
                double weightFractionSubstance = route.WeightFractionSubstance.AsFraction();

                return releasedMass * weightFractionSubstance;
            }
        }

        //Note: InhalationRate is not an intrinsic parameter for this model. It is only used in the conversion from Air Concentration to External Event Dose.
    }
}