using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using System;

namespace RIVM.ConsExpo.DTO.Models
{
    /// <summary>
    /// Helper for model parameters used in scenario routes.
    /// </summary>
    public class ModelParameterHelpers
    {
        /// <summary>
        /// Gets the model parameter instance for the specified model parameter in the specified scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <param name="modelParameterEnum">The model parameter enum.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static IPhysicalQuantityBase GetModelParameterInstance(ScenarioModel scenario, ModelParameters modelParameterEnum)
        {
            switch (modelParameterEnum)
            {
                //Assessment
                //Note: these model parameters depend on Assessment being initialized in the scenario. This should be the case for a scenario received from the context.
                case ModelParameters.AssessmentBodyWeight:
                    return scenario.Assessment.Population.BodyWeight;

                case ModelParameters.AssessmentMolecularWeight:
                    //Note: this depends on Assessment being initialized in the scenario. This should be the case for a scenario received from the context.
                    return scenario.Assessment.Substance.MolecularWeight;

                //Scenario
                case ModelParameters.ScenarioFrequency:
                    return scenario.Frequency;

                case ModelParameters.DermalExposureContactDuration:
                    return scenario.DermalExposure.ContactDuration;

                case ModelParameters.DermalExposureContactedSurface:
                    return scenario.DermalExposure.ContactedSurface;

                case ModelParameters.DermalExposureContactRate:
                    return scenario.DermalExposure.ContactRate;

                case ModelParameters.DermalExposureDiffusionCoefficient:
                    return scenario.DermalExposure.DiffusionCoefficient;

                case ModelParameters.DermalExposureDislodgeableAmount:
                    return scenario.DermalExposure.DislodgeableAmount;

                case ModelParameters.DermalExposureExposedArea:
                    return scenario.DermalExposure.ExposedArea;

                case ModelParameters.DermalExposureExposureDuration:
                    return scenario.DermalExposure.ExposureDuration;

                case ModelParameters.DermalExposureLayerThickness:
                    return scenario.DermalExposure.LayerThickness;

                case ModelParameters.DermalExposureLeachableFraction:
                    return scenario.DermalExposure.LeachableFraction;

                case ModelParameters.DermalExposureProductAmount:
                    return scenario.DermalExposure.ProductAmount;

                case ModelParameters.DermalExposureReleaseDuration:
                    return scenario.DermalExposure.ReleaseDuration;

                case ModelParameters.DermalExposureSkinContactFactor:
                    return scenario.DermalExposure.SkinContactFactor;

                case ModelParameters.DermalExposureSubstanceConcentration:
                    return scenario.DermalExposure.SubstanceConcentration;

                case ModelParameters.DermalExposureTransferCoefficient:
                    return scenario.DermalExposure.TransferCoefficient;

                case ModelParameters.DermalExposureWeightFractionSubstance:
                    return scenario.DermalExposure.WeightFractionSubstance;

                case ModelParameters.DermalExposureRetentionFactor:
                    return scenario.DermalExposure.RetentionFactor;

                // {RIVM.ConsExpo.DTO.Models.DermalAbsorptionModel}
                case ModelParameters.DermalAbsorptionAbsorptionFraction:
                    return scenario.DermalAbsorption.AbsorptionFraction;

                case ModelParameters.DermalAbsorptionConcentrationInMatrix:
                    return scenario.DermalAbsorption.ConcentrationInMatrix;

                case ModelParameters.DermalAbsorptionExposureDuration:
                    return scenario.DermalAbsorption.ExposureDuration;

                case ModelParameters.DermalAbsorptionSkinPermeability:
                    return scenario.DermalAbsorption.SkinPermeability;

                // {RIVM.ConsExpo.DTO.Models.InhalationExposureModel}
                case ModelParameters.InhalationExposureAirborneFraction:
                    return scenario.InhalationExposure.AirborneFraction;

                case ModelParameters.InhalationExposureApplicationDuration:
                    return scenario.InhalationExposure.ApplicationDuration;

                case ModelParameters.InhalationExposureApplicationTemperature:
                    return scenario.InhalationExposure.ApplicationTemperature;

                case ModelParameters.InhalationExposureCloudVolume:
                    return scenario.InhalationExposure.CloudVolume;

                case ModelParameters.InhalationExposureDensityNonVolatile:
                    return scenario.InhalationExposure.DensityNonVolatile;

                case ModelParameters.InhalationExposureDiffusionCoefficientForEmission:
                    return scenario.InhalationExposure.DiffusionCoefficientForEmission;

                case ModelParameters.InhalationExposureEmissionDuration:
                    return scenario.InhalationExposure.EmissionDuration;

                case ModelParameters.InhalationExposureEmissionDurationEvaporation:
                    return scenario.InhalationExposure.EmissionDurationEvaporation;
                
                case ModelParameters.InhalationExposureEmissionDurationReEntry:
                    return scenario.InhalationExposure.EmissionDurationReEntry;

                case ModelParameters.InhalationExposureDailyDuration:
                    return scenario.InhalationExposure.DailyDuration;

                case ModelParameters.InhalationExposureExposureDuration:
                    return scenario.InhalationExposure.ExposureDuration;

                case ModelParameters.InhalationExposureExposureDurationForEmissionModel:
                    return scenario.InhalationExposure.ExposureDurationForEmissionModel;

                case ModelParameters.InhalationExposureInhalationCutOffDiameter:
                    return scenario.InhalationExposure.InhalationCutOffDiameter;

                case ModelParameters.InhalationExposureInhalationRate:
                    return scenario.InhalationExposure.InhalationRate;

                case ModelParameters.InhalationExposureMassGenerationRate:
                    return scenario.InhalationExposure.MassGenerationRate;

                case ModelParameters.InhalationExposureMassTransferCoefficient:
                    return scenario.InhalationExposure.MassTransferCoefficient;

                case ModelParameters.InhalationExposureMaximumDiameter:
                    return scenario.InhalationExposure.MaximumDiameter;

                case ModelParameters.InhalationExposureMeanDiameter:
                    return scenario.InhalationExposure.MeanDiameter;

                case ModelParameters.InhalationExposureMedianDiameter:
                    return scenario.InhalationExposure.MedianDiameter;

                case ModelParameters.InhalationExposureMolecularWeightMatrix:
                    return scenario.InhalationExposure.MolecularWeightMatrix;

                case ModelParameters.InhalationExposureDilution:
                    return scenario.InhalationExposure.Dilution;

                case ModelParameters.InhalationExposureProductAirPartitionCoefficient:
                    return scenario.InhalationExposure.ProductAirPartitionCoefficient;

                case ModelParameters.InhalationExposureProductAmount:
                    return scenario.InhalationExposure.ProductAmount;

                case ModelParameters.InhalationExposureProductDensity:
                    return scenario.InhalationExposure.ProductDensity;

                case ModelParameters.InhalationExposureProductSurfaceArea:
                    return scenario.InhalationExposure.ProductSurfaceArea;

                case ModelParameters.InhalationExposureProductThickness:
                    return scenario.InhalationExposure.ProductThickness;

                case ModelParameters.InhalationExposureReleaseArea:
                    return scenario.InhalationExposure.ReleaseArea;

                case ModelParameters.InhalationExposureReleasedMass:
                    return scenario.InhalationExposure.ReleasedMass;

                case ModelParameters.InhalationExposureRoomHeight:
                    return scenario.InhalationExposure.RoomHeight;

                case ModelParameters.InhalationExposureRoomVolume:
                    return scenario.InhalationExposure.RoomVolume;

                case ModelParameters.InhalationExposureSprayDuration:
                    return scenario.InhalationExposure.SprayDuration;

                case ModelParameters.InhalationExposureStandardDeviation:
                    return scenario.InhalationExposure.StandardDeviation;

                case ModelParameters.InhalationExposureStartExposure:
                    return scenario.InhalationExposure.StartExposure;

                case ModelParameters.InhalationExposureVapourPressure:
                    return scenario.InhalationExposure.VapourPressure;

                case ModelParameters.InhalationExposureVentilationRate:
                    return scenario.InhalationExposure.VentilationRate;

                case ModelParameters.InhalationExposureWeightFractionSubstance:
                    return scenario.InhalationExposure.WeightFractionSubstance;

                case ModelParameters.InhalationExposureWeightFractionSubstanceForEmission:
                    return scenario.InhalationExposure.WeightFractionSubstanceForEmission;

                // {RIVM.ConsExpo.DTO.Models.InhalationAbsorptionModel}
                case ModelParameters.InhalationAbsorptionAbsorptionFraction:
                    return scenario.InhalationAbsorption.AbsorptionFraction;

                // {RIVM.ConsExpo.DTO.Models.OralExposureModel}
                case ModelParameters.OralExposureContactAreaMouthing:
                    return scenario.OralExposure.ContactAreaMouthing;

                case ModelParameters.OralExposureContactAreaPackaging:
                    return scenario.OralExposure.ContactAreaPackaging;

                case ModelParameters.OralExposureExposureDuration:
                    return scenario.OralExposure.ExposureDuration;

                case ModelParameters.OralExposureIngestedAmountMouthing:
                    return scenario.OralExposure.IngestedAmountMouthing;

                case ModelParameters.OralExposureIngestedAmountPackaging:
                    return scenario.OralExposure.IngestedAmountPackaging;

                case ModelParameters.OralExposureIngestionRate:
                    return scenario.OralExposure.IngestionRate;

                case ModelParameters.OralExposureInitialMigrationRate:
                    return scenario.OralExposure.InitialMigrationRate;

                case ModelParameters.OralExposureMigrationRatePackaging:
                    return scenario.OralExposure.MigrationRatePackaging;

                case ModelParameters.OralExposurePackagedAmount:
                    return scenario.OralExposure.PackagedAmount;

                case ModelParameters.OralExposureProductAmount:
                    return scenario.OralExposure.ProductAmount;

                case ModelParameters.OralExposureStorageTime:
                    return scenario.OralExposure.StorageTime;

                case ModelParameters.OralExposureSubstanceConcentration:
                    return scenario.OralExposure.SubstanceConcentration;

                case ModelParameters.OralExposureThicknessPackaging:
                    return scenario.OralExposure.ThicknessPackaging;

                case ModelParameters.OralExposureWeightFractionSubstance:
                    return scenario.OralExposure.WeightFractionSubstance;

                // {RIVM.ConsExpo.DTO.Models.OralAbsorptionModel}
                case ModelParameters.OralAbsorptionAbsorptionFraction:
                    return scenario.OralAbsorption.AbsorptionFraction;

                default:
                    throw new NotSupportedException(string.Format("Unsupported model parameter '{0}'", modelParameterEnum.ToString()));
            }
        }
    }
}