using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Submodels
{
    /// <summary>
    /// List of sub models of the Oral absorption, which the user may select to use in a Oral absorption scenario.
    /// </summary>
    public enum OralAbsorptionSubmodelTypes
    {
        [Display(Name = "Fixed fraction")]
        Fraction = 0
    }
}