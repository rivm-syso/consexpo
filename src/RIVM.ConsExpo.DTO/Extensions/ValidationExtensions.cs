using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RIVM.ConsExpo.DTO.Extensions
{
    /// <summary>
    /// Some useful extensions to the Validation results classes.
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Adds the specified error message.
        /// </summary>
        /// <param name="validationResults">The validation results.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <remarks>
        /// This method allows you to add a new validation result to the list, without the need to create an instance first.
        /// </remarks>
        public static void Add(this IList<ValidationResult> validationResults, string errorMessage)
        {
            validationResults.Add(new ValidationResult(errorMessage));
        }

        /// <summary>
        /// Adds a message by key and message.
        /// </summary>
        public static void Add(this IList<ValidationResult> validationResults, string memberName, string errorMessage)
        {
            validationResults.Add(new ValidationResult(errorMessage, new List<string> { memberName }));
        }

        /// <summary>
        /// Adds the specified error message.
        /// </summary>
        /// <param name="validationResults">The validation results.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="validationContext">The validation context.</param>
        /// <remarks>This method allows you to add a new validation result to the list, without the need to create an instance first.</remarks>
        public static void Add(this IList<ValidationResult> validationResults, string errorMessage, ValidationContext validationContext)
        {
            Add(validationResults, validationContext.MemberName, errorMessage);
        }
    }
}