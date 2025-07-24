using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Submodels
{
    /// <summary>
    /// List of models of the Inhalation exposure.
    /// </summary>
    /// <remarks>These values are also switch in js, search for function ApplyInhalationExposureSubmodel()</remarks>
    public enum InhalationExposureModelTypes
    {
        [Display(Name = "Exposure to vapour")]
        Vapour = 0,

        [Display(Name = "Exposure to spray")]
        Spray = 1,

        [Display(Name = "Emission from solid materials")]
        Emission = 2,
    }
}