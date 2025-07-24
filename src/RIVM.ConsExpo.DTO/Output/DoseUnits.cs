using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// The dose units for the various output measures.
    /// </summary>
    /// <remarks>The units for all routes are here together.</remarks>
    public enum DoseUnits
    {
        [Display(Name = "mg")]
        Mg = 0,

        [Display(Name = "mg/kg bw")]
        MgPerKgBodyWeight = 1,

        [Display(Name = "mg/kg bw/day")]
        MgPerKgBodyWeightPerDay = 2,

        [Display(Name = "mg/m³")]
        MgPerCubicMetre = 3,

        [Display(Name = "mg/cm²")]
        MgPerSquareCentimetre = 4,

        [Display(Name = "")]
        Fraction = 5
    }
}