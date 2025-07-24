#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.CE4
{
    public static class OralParameterNames
    {
        public const string OralExpoNone = "none";
        public const string OralIngestionScenarioStr = "Oral exposure to product";
        public const string OralMigrationFromPackagingStr = "Migration from packaging material";

        public const string OralUptakeModelFraction = "Fraction";

        // string id's for file format
        public const string IngestionModeNoneStr = "none";

        public const string IngestionModeDirectStr = "direct";
        public const string IngestionModeRateStr = "rate";
        public const string IngestionModeMigrationStr = "migration";

        // descriptive strings for ingestion modes
        public const string IngestionModeDirectDisplayString = "direct intake";

        public const string IngestionModeRateDisplayString = "constant rate";
        public const string IngestionModeMigrationDisplayString = "migration";

        public const string ReleaseModeNoneStr = "none";
        public const string ReleaseModeInstantaneousStr = "instantaneous";
        public const string ReleaseModeTimeConstantRateStr = "constant rate";

        // descriptive strings for migration release
        public const string ReleaseModeInstantaneousDisplayString = "instantaneous release";

        public const string ReleaseModeTimeConstantRateDisplayString = "release at constant rate";

        // parameter names
        public const string ProductAmountIngestedParName = "amount ingested";

        public const string ProductConcentrationParName = "product density";
        public const string ProductAmountParName = "product amount";
        public const string LeachRateParName = "leach rate";
        public const string ContactAreaParName = "contact area";
        public const string IngestionRateParName = "ingestion rate";
        public const string OralExposureTimeParName = "exposure time";
        public const string CompoundConcentrationPackagingParName = "compound concentration packaging";
        public const string ThicknessPackagingParName = "thickness packaging";
        public const string PackagedAmountParName = "packaged amount";
        public const string IngestedProductAmountParName = "ingested product amount";
        public const string MigrationRateParName = "migration rate";
        public const string StorageTimeParName = "storage time";
    }
}