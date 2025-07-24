using RIVM.ConsExpo.DTO.PhysicalQuantities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Calculators
{
    /// <summary>
    /// A model that will store the information entered by the user, while selecting factsheet data to create a new scenario from a factsheet.
    /// </summary>
    public class InhalationRateCalculatorModel : IValidatableObject
    {
        [Display(Name = "Type of estimation")]
        public InhalationRateCalculationTypes CalculationType { get; set; }

        [Display(Name = "Exercise level")]
        public ExerciseLevels ExerciseLevel { get; set; }

        [Display(Name = "Body weight")]
        public BodyWeight BodyWeight { get; set; }

        public int InhalationRateUnitCode { get; set; }
        public int DefaultPopulationId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            switch (CalculationType)
            {
                case InhalationRateCalculationTypes.FromDefaultsDatabase:
#warning To Do: add validations.
                    break;

                case InhalationRateCalculationTypes.FromBodyweightAndExerciseLevel:
                    if (this.BodyWeight.Value == null)
                    {
                        validationResults.Add(new ValidationResult("Body weight is required"));
                    }
                    break;

                default:
                    Debug.Assert(false, string.Format("Unsupported calculation type '{0}'.", CalculationType.ToString()));
                    break;
            }

            return validationResults;
        }
    }
}