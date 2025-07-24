using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Submodels
{
    /// <summary>
    /// List of sub models of the dermal exposure, which the user may select to use in a dermal absorption scenario.
    /// </summary>
    /// <remarks>These values are also switch in js</remarks>
    public enum DermalExposureSubmodelTypes
    {
        [Display(Name = "Instant application")]
        InstantApplication = 0,

        [Display(Name = "Constant rate")]
        ConstantRate = 1,

        [Display(Name = "Rubbing off")]
        RubbingOff = 2,

        [Display(Name = "Migration")]
        Migration = 3,

        [Display(Name = "Diffusion")]
        Diffusion = 4
    }
}