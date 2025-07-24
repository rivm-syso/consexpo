using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Submodels
{
    /// <summary>
    /// List of sub models of the Oral exposure, which the user may select to use in a Oral absorption scenario.
    /// </summary>
    public enum OralExposureSubmodelTypes
    {
        [Display(Name = "Direct oral intake")]
        DirectIntake = 0,

        [Display(Name = "Constant rate")]
        ConstantRate = 1,

        [Display(Name = "Product mouthing")]
        ProductMouthing = 2,

        [Display(Name = "Spraying non-respirable material")]
        SprayingNonRespirableMaterial = 3,

        [Display(Name = "Instant release")]
        MigrationFromPackagingInstantRelease = 4,

        [Display(Name = "Constant rate")]
        MigrationFromPackagingConstantRate = 5
    }
}