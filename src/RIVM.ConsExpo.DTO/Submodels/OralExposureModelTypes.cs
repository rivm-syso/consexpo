using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Submodels
{
    /// <summary>
    /// List of models of the Oral exposure.
    /// </summary>
    /// <remarks>These values are also switch in js, search for function ApplyOralExposureSubmodel()</remarks>
    public enum OralExposureModelTypes
    {
        [Display(Name = "Direct product contact")]
        Direct = 0,

        [Display(Name = "Migration from packaging")]
        Packaging = 1,

        [Display(Name = "Non-respirable spray model")]
        Spray = 2,
    }
}