using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Submodels
{
    /// <summary>
    /// List of sub models of the dermal absorption, which the user may select to use in a dermal absorption scenario.
    /// </summary>
    public enum DermalAbsorptionSubmodelTypes
    {
        [Display(Name = "Fixed fraction")]
        Fraction = 0,

        [Display(Name = "Diffusion through skin")]
        DiffusionThroughSkinForInstantApplication = 1,

        [Display(Name = "Diffusion through skin")]
        DiffusionThroughSkinForDiffusion = 2
    }
}