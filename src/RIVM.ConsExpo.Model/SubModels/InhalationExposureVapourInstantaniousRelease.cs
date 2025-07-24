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
    /// In this model, all of the substance is released as vapour at once, subsequently removed by ventilation.
    /// </summary>
    internal class InhalationExposureVapourInstantaniousRelease : InhalationExposureInstantaniousReleaseBase, IInhalationExposureSubmodel
    {
        private const InhalationExposureSubmodelTypes type = InhalationExposureSubmodelTypes.VapourInstantaneousRelease;

        public InhalationExposureSubmodelTypes Type => type;

        public InhalationExposureVapourInstantaniousRelease(ScenarioModel scenario)
            : base(scenario, type, true)
        { }

        public override bool IsTimeDependent => true;

        public override Duration ApplicableExposureDuration => scenario.InhalationExposure.ExposureDuration;

        public override Time DefaultTimeMax => ApplicableExposureDuration.AsTime();

        public override bool SupportsPeakAirConcentration => true;

        /// <summary>
        /// The amount of substance released (in mg): [Product amount] x [weight fraction]
        /// </summary>
        public override double? AmountOfSubstance => AmountOfSubstanceByProductAmount;

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.InhalationExposureExposureDuration,
                DTO.Models.ModelParameters.InhalationExposureProductAmount,
                DTO.Models.ModelParameters.InhalationExposureWeightFractionSubstance,
                DTO.Models.ModelParameters.InhalationExposureRoomVolume,
                DTO.Models.ModelParameters.InhalationExposureVentilationRate
            };

            if (scenario.InhalationExposure.LimitConcentrationToSaturatedAirConcentration)
            {
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureVapourPressure);
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureApplicationTemperature);
                modelParameters.Add(DTO.Models.ModelParameters.AssessmentMolecularWeight);
            }
            return modelParameters;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            IList<ValidationResult> validationResults = ValidateDurationAndFrequency(route.ExposureDuration, scenario.Frequency);

            RequireExposureDuration(validationResults);
            RequireProductAmount(validationResults);
            RequireWeightFractionSubstance(validationResults);
            RequireRoomVolume(validationResults);
            RequireVentilationRate(validationResults);

            if (route.LimitConcentrationToSaturatedAirConcentration)
            {
                RequireVapourPressure(validationResults);
                RequireApplicationTemperature(validationResults);
                RequireMolecularWeight(validationResults, scenario.Assessment.Substance);
            }

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
            A = scenario.InhalationExposure.ProductAmount.InMilligram() * wf;
            limitConcentrationToSaturatedAirConcentration = scenario.InhalationExposure.LimitConcentrationToSaturatedAirConcentration;
            if (limitConcentrationToSaturatedAirConcentration)
            {
                vapourPressure = scenario.InhalationExposure.VapourPressure;
                molecularWeight = scenario.Assessment.Substance.MolecularWeight.InGramPerMol();
                applicationTemperature = scenario.InhalationExposure.ApplicationTemperature.InKelvin();
            }
        }

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        public override bool ModelIsDistributed =>
            route.ExposureDuration.IsDistributed
            || route.ProductAmount.IsDistributed
            || route.WeightFractionSubstance.IsDistributed
            || route.RoomVolume.IsDistributed
            || route.VentilationRate.IsDistributed
            || (route.LimitConcentrationToSaturatedAirConcentration && route.ApplicationTemperature.IsDistributed);

        //Note: InhalationRate is not an intrinsic parameter for this model. It is only used in the conversion from Air Concentration to External Event Dose.
    }
}