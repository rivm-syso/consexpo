using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Submodels
{
    /// <summary>
    /// List of sub models of the Inhalation exposure, which the user may select to use in a Inhalation absorption scenario.
    /// </summary>
    /// <remarks>These values are also switch in js, see 'data-discriminators' attributes.</remarks>
    public enum InhalationExposureSubmodelTypes
    {
        [Display(Name = "Instantaneous release")]
        VapourInstantaneousRelease = 0,

        [Display(Name = "Constant rate")]
        VapourConstantRate = 1,

        [Display(Name = "Evaporation")]
        VapourEvaporation = 2,

        [Display(Name = "Instantaneous release")]
        SprayInstantaneousRelease = 3,

        [Display(Name = "Spraying")]
        SpraySpraying = 4,

        [Display(Name = "Emission from solid materials")]
        EmissionFromSolidMaterials = 5
    }
}