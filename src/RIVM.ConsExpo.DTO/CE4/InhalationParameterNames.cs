#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.CE4
{
    public static class InhalationParameterNames
    {
        public const string InhExpoNone = "none";
        public const string InhEvaporationScenarioStr = "Exposure to vapour";
        public const string InhExpSprayScenarioStr = "Exposure to spray";

        // display strings
        public const string ReleaseInstantDisplayString = "instantaneous release";

        public const string ReleaseConstantEmissionDisplayString = "constant rate";
        public const string ReleaseEvaporationDisplayString = "evaporation";

        public const string UptakeModelFraction = "Fraction";
        public const string UptakeModelFlow = "Flow";
        public const string UptakeModelDiffusion = "Diffusion";

        public const string DefaultAirConcentrationUnits = "mg/m3";

        public const string ExposureDurationParName = "exposure duration";
        public const string RoomVolumeParName = "room volume";
        public const string VentilationRateParName = "ventilation rate";
        public const string AppliedAmountParName = "applied amount";
        public const string InhalationRateParName = "inhalation rate";
        public const string NonRespirableUptakeFractionParName = "non-respirable uptake fraction";

        public const string ReleaseDurationParName = "release duration";
        public const string ProductDensityParName = "product density";
        public const string ReleaseRateParName = "release rate";
        public const string ReleaseAreaParName = "release area";
        public const string ApplicationDurationParName = "application duration";
        public const string ReleaseAreaGrowthParName = "release area growth";
        public const string MassTransferRateParName = "mass transfer rate";
        public const string VapourPressureParName = "vapour pressure";
        public const string MolecularWeightParName = "molecular weight";
        public const string MolWeightMatrixParName = "mol weight matrix";
        public const string TemperatureParName = "temperature";
        public const string DiffusionCoefficientParName = "diffusion coefficient"; // obsolete

        public const string ParticleDistributionId = "particle distribution";
        public const string ParticleDistributionFormatId = "particle distribution format";
        public const string DistributionFieldSeparator = "|";

        public const string SprayToExposedPersonStr = "spray to person";
        public const string ImmediateSolventEvap = "solvent evaporation immediate";

        public const string MassGenerationParName = "mass generation rate";
        public const string SprayVolumeRateParName = "spray volume rate";
        public const string SprayDurationParName = "spray duration";
        public const string AirbornFractionParName = "airborn fraction";
        public const string WeightFractionPropellantParName = "weight fraction propellant";
        public const string WeightFractionSolventParName = "weight fraction solvent";
        public const string DensitySolventParName = "density solvent";
        public const string DensityNonVolatileParName = "density non-volatile";
        public const string WeightFractionNonVolatileParName = "weight fraction non-volatile";
        public const string RoomHeightParName = "room height";
        public const string InhalationCutoffParName = "inhalation cut-off diameter";
        public const string CloudVolumeParName = "cloud volume";
    }
}