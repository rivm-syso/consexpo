#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.CE4
{
    public static class DermalParameterNames
    {
        public const string DermalExpoNone = "none";
        public const string DermalDirectProductExposureScenarioStr = "Direct dermal contact with product";
        public const string DermalAirExposure = "Dermal exposure via air";

        public const string DermalLoadNoneStr = "none";
        public const string DermalLoadFixedAmountStr = "fixed amount";
        public const string DermalLoadProductLeachingStr = "product leaching";
        public const string DermalLoadContactRateStr = "contact rate";
        public const string DermalLoadRubbingOffStr = "rubbing off";
        public const string DermalLoadDiffusionStr = "product diffusion";

        // display strings
        public const string DermalLoadFixedAmountDisplayString = "instant application";

        public const string DermalLoadProductLeachingDisplayString = "migration";
        public const string DermalLoadContactRateDisplayString = "constant rate";
        public const string DermalLoadRubbingOffDisplayString = "rubbing off";
        public const string DermalLoadDiffusionDisplayString = "diffusion";

        private const int kNoDiffusionEquations = 5; // number of segments in the numerical solution of the diffusion equation. 10 Should be enough for general conditions

        public const string DermalUptakeModelFraction = "fraction";
        public const string DermalUptakeModelDiffusion = "diffusion";

        public const string ExposureTimeParName = "exposure time";
        public const string ExposedAreaParName = "exposed area";
        public const string DermalProductDensityParName = "product density";
        public const string CompoundConcentrationParName = "compound concentration";
        public const string SkinPermeabilityParName = "skin permeability";
        public const string AppliedProductAmountParName = "applied amount";
        public const string ContactRateParName = "contact rate";
        public const string DermalReleaseDurationParName = "release duration";
        public const string LeachableFractionParName = "leachable fraction";
        public const string SkinContactFactorParName = "skin contact factor";
        public const string TransferCoefficientParName = "transfer coefficient";
        public const string RubbedSurfaceParName = "rubbed surface";
        public const string DermalDiffusionCoefficientParName = "diffusion coefficient";
        public const string DislodgeableFormulationParName = "dislodgeable amount";
        public const string LayerThicknessParName = "layer thickness";

        // in the 'rubbing off' model the parameter 'release duration' has different names, default: kDermalReleaseDurationParName
        public const string RubbingOffReleaseTimeName = "contact time";

        public const string KOWParName = "KOW";
        public const string WaterSolubilityParName = "water solubility";
    }
}