using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Calculators
{
    public enum InhalationRateCalculationTypes
    {
        [Display(Name = "Use population defaults database")]
        FromDefaultsDatabase = 1,

        [Display(Name = "Estimate, using bodyweight and exercise level")]
        FromBodyweightAndExerciseLevel = 2
    }
}