using System;

namespace RIVM.ConsExpo.DTO.Extensions
{
    /// <summary>
    /// Extension methods for boolean manipulation
    /// </summary>
    public static class BoolExtensions
    {
        /// <summary>
        /// Parse the specified value and maps 0 to false and 1 to true. Other values cause an exception.
        /// </summary>
        /// <param name="boolString">The bool string.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException"></exception>
        public static bool Parse0Or1(this string boolString)
        {
            if (boolString == "0")
            {
                return false;
            }
            else if (boolString == "1")
            {
                return true;
            }
            else
            {
                throw new ArgumentException(string.Format("Textual value '{0}' is neither '0' nor '1' and cannot be parsed as a boolean by this method.", boolString));
            }
        }

        /// <summary>
        /// Converts the boolean to textual "yes" or "no".
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public static string ToyesOrno(this bool value)
        {
            return value ? "yes" : "no";
        }

        /// <summary>
        /// Converts the boolean to textual "Yes" or "No".
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public static string ToYesOrNo(this bool value)
        {
            return value ? "Yes" : "No";
        }
    }
}