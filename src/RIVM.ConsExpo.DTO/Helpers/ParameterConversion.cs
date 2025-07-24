using System;

namespace RIVM.ConsExpo.DTO.Helpers
{
    /// <summary>
    /// Methods for conversion and rounding of parameters and output values to and from SI-units, used in the model, to user-friendly values, used in the UI.
    /// </summary>
    public class ParameterConversion
    {
        /// <summary>
        /// Rounds the input value to the specified number of significant digits.
        /// </summary>
        public static double? SignificantDigits(double? value, int significantDigits)
        {
            if (value.HasValue)
            {
                return SignificantDigits(value.Value, significantDigits);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Rounds the input value to the specified number of significant digits,
        /// </summary>
        /// <param name="value">The value to round.</param>
        /// <param name="significantDigits">The significant digits.</param>
        /// <returns>Returns the value, rounded to the specified number of decimals</returns>
        /// <example>SignificantDigits(123, 2) returns 120.</example>
        /// <remarks>This method has to be implemented, because Round allows a specification of the number of digits after the comma, but no rounding before the comma.</remarks>
        public static double SignificantDigits(double value, int significantDigits)
        {
            if (value == 0.0)
            {
                return value;
            }
            else
            {
                //locationOfFirstDigit: location of leftmost digit not equal to zero, as in positions left of the decimal point.
                //321.0(-1)(-2)(-3)(-4), etc.
                //Examples:
                //123.456: 3
                //1.23456: 1
                //0.00123: -2.
                //int sign = Math.Sign(value);

                int locationOfFirstDigit = (int)Math.Ceiling(Math.Log10(Math.Abs(value)));

                int offset = significantDigits - locationOfFirstDigit;

                double rescaleFactor = Math.Pow(10, offset);

                double rescaledValue = value * rescaleFactor;

                double roundedValue = Math.Round(rescaledValue, 0);

                return roundedValue / rescaleFactor;
            }
        }
    }
}