using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Extensions;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Computations;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RIVM.ConsExpo.Model.Submodels
{
    /// <summary>
    /// In this model, exposure is from the inhalation exposure SprayingNonRespirableMaterial model. Part of it may be taken in orally.
    /// </summary>
    internal class OralExposureSprayingNonRespirableMaterial : OralExposureBase, IOralExposureSubmodel
    {
        private const OralExposureSubmodelTypes type = OralExposureSubmodelTypes.SprayingNonRespirableMaterial;

        public OralExposureSubmodelTypes Type => type;

        private readonly ExposureSpraySprayingComputations _exposureSpraySprayingComputations;

        public OralExposureSprayingNonRespirableMaterial(ScenarioModel scenario)
            : base(scenario, type)
        {
            _exposureSpraySprayingComputations = new ExposureSpraySprayingComputations(scenario, false);
        }

        public override bool IsTimeDependent => true;

        public override Duration ApplicableExposureDuration => scenario.InhalationExposure.ExposureDuration;

        public override Time DefaultTimeMax => ApplicableExposureDuration.AsTime();

        public override void PrepareTimeSeries(Time timeMax)
        {
            _exposureSpraySprayingComputations.PrepareSolution(timeMax);
        }

        /// <summary>
        /// The amount of substance released (in mg): [Spray duration] x [Mass generation rate] x [weight fraction]
        /// </summary>
        public override double? AmountOfSubstance
        {
            get
            {
                double sprayDuration = scenario.InhalationExposure.SprayDuration.InMinutes();
                double massGenerationRate = scenario.InhalationExposure.MassGenerationRate.InMilligramPerMinute();
                double weightFractionSubstance = scenario.InhalationExposure.WeightFractionSubstance.AsFraction();

                return sprayDuration * massGenerationRate * weightFractionSubstance;
            }
        }

        public List<ModelParameters> ModelParameters()
        {
            //Note: copied from InhalatoryExposureSpraySpraying.
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.InhalationExposureSprayDuration,
                DTO.Models.ModelParameters.InhalationExposureExposureDuration,
                DTO.Models.ModelParameters.InhalationExposureWeightFractionSubstance,
                DTO.Models.ModelParameters.InhalationExposureRoomVolume,
                DTO.Models.ModelParameters.InhalationExposureRoomHeight,
                DTO.Models.ModelParameters.InhalationExposureVentilationRate
            };
            if (scenario.InhalationExposure.SprayingTowardsPerson)
            {
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureCloudVolume);
            }
            modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureMassGenerationRate);
            modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureAirborneFraction);
            modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureDensityNonVolatile);
            modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureInhalationCutOffDiameter);

            switch (scenario.InhalationExposure.AerosolDiameterDistributionType)
            {
                case SizeDistributionTypes.LogNormal:
                    modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureMedianDiameter);
#warning ToDo: this is not a physical quantity. Should we make it one (e.g. Dimensionless)?
                    //modelParametersForSensitivityAnalysis.Add(ModelParameters.ArithmicCoefficientOfVariation);
                    modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureMaximumDiameter);
                    break;

                case SizeDistributionTypes.Normal:
                    modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureMeanDiameter);
                    modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureStandardDeviation);
                    modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureMaximumDiameter);
                    break;

                case SizeDistributionTypes.NonParametric:
                    // None. The selected non-parametric size distribution is not a true model parameter. It is just a reference Id.
                    break;

                default:
                    throw new NotSupportedException(string.Format("Unsupported distribution type '{0}'", scenario.InhalationExposure.AerosolDiameterDistributionType.ToString()));
            }
            return modelParameters;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            IList<ValidationResult> validationResults = new List<ValidationResult>();

            if (scenario.InhalationExposure == null || scenario.InhalationExposure.SubmodelType != InhalationExposureSubmodelTypes.SpraySpraying || !scenario.InhalationExposureRouteInUse)
            {
                validationResults.Add(new ValidationResult($"The oral submodel '{type.GetDisplayName()}' can only be used if you also select the inhalation submodel '{InhalationExposureSubmodelTypes.SpraySpraying.GetDisplayName()}'."));
            }

            return validationResults;
        }

        public OralExposureOutcome CalculatePointValues()
        {
            var outcome = new OralExposureOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, AmountOfSubstance);

            AirConcentration meanAirConcentration;

            if (scenario.InhalationExposure.InhalationCutOffDiameter.InMicrometre() >= scenario.InhalationExposure.MaximumDiameter.InMicroMetre())
            {
                meanAirConcentration = new AirConcentration() { Value = 0.0, Unit = DensityUnits.MilligramPerCubicMetre };
            }
            else
            {
                meanAirConcentration = _exposureSpraySprayingComputations.MeanAirConcentration();
            }

            Dose load = new Dose(
                meanAirConcentration.InMilligramPerCubicMetre() * scenario.InhalationExposure.InhalationRate.InCubicMetresPerSecondIfSpecified() * DefaultTimeMax.InSeconds(),
                DoseUnits.Mg
            );

            outcome.Dose = load;

            return outcome;
        }

        public OralExposureOutcome CalculatePointValues(Time time)
        {
            var outcome = new OralExposureOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, AmountOfSubstance);

            AirConcentration meanAirConcentration;
            if (time.InSeconds() <= 0)
            {
                meanAirConcentration = _exposureSpraySprayingComputations.InstantaneousAirConcentration(time);
            }
            else
            {
                meanAirConcentration = _exposureSpraySprayingComputations.MeanAirConcentration(time);
            }

            Dose load = new Dose(meanAirConcentration.InMilligramPerCubicMetre() * scenario.InhalationExposure.InhalationRate.InCubicMetresPerSecondIfSpecified() * time.InSeconds(), DoseUnits.Mg);

            outcome.Dose = load;

            return outcome;
        }

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        public override bool ModelIsDistributed => _exposureSpraySprayingComputations.ModelIsDistributed;
    }
}