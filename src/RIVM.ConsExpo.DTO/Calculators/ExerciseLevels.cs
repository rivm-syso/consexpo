using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Calculators
{
    /// <summary>
    /// List of models of the ConsExpo file types (extensions).
    /// </summary>
    /// <remarks></remarks>
    public enum ExerciseLevels
    {
        [Display(Name = "sleep")]
        Sleep = 0,

        [Display(Name = "rest")]
        Rest = 1,

        [Display(Name = "light exercise")]
        LightExercise = 2,

        [Display(Name = "heavy exercise")]
        HeavyExercise = 3
    }
}