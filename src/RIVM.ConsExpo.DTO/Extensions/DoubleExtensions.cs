using System;
using System.Diagnostics;
using System.Text;

namespace RIVM.ConsExpo.DTO.Extensions
{
    /// <summary>
    /// Extension methods for the C# standard data type double.
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>
        /// Tests if two parameters are almost equal. I.e., differ by less than the specified relative tolerance. This is needed to ignore small rounding errors.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns></returns>
        public static bool AlmostEqualMagnitude(this double value1, double value2, double tolerance = 1E-9)
        {
            return value1.RelativeDifference(value2) < tolerance;
        }

        /// <summary>
        /// Compare two nullable doubles.
        /// </summary>
        /// <returns>false if any value is null. Otherwise, true if they differ less than the tolerance, false otherwise.</returns>
        public static bool AlmostEqualMagnitude(this double? value1, double? value2, double tolerance = 1E-9)
        {
            if (value1 == null || value2 == null) return false;

            return value1.Value.RelativeDifference(value2.Value) < tolerance;
        }

        /// <summary>
        /// Returns the relative magnitude of the number to the number it is compared with.
        /// </summary>
        /// <returns></returns>
        public static double RelativeDifference(this double value1, double value2)
        {
            return Math.Abs(1 - value1 / value2);
        }

        /// <summary>
        /// Returns a value in scientific format, like 2.9 × 10, or an empty string if the value is null.
        /// </summary>
        /// <returns></returns>
        /// <see href="http://stackoverflow.com/questions/9442243/draw-string-with-normalized-scientific-notation-superscripted/9442631#9442631"/>
        public static string FormatAsPowerOfTen(this double? value)
        {
            if (!value.HasValue)
            {
                return string.Empty;
            }
            return FormatAsPowerOfTen(value.Value);
        }

        /// <summary>
        /// Returns a value in scientific format, like 2.9 × 10, or an empty string if the value is null.
        /// </summary>
        /// <returns></returns>
        /// <see href="http://stackoverflow.com/questions/9442243/draw-string-with-normalized-scientific-notation-superscripted/9442631#9442631"/>
        public static string FormatAsPowerOfTen(this double value)
        {
            return FormatAsPowerOfTen(value, -1);
        }

        /// <summary>
        /// Returns a value in scientific format, like 2.9 × 10, or an empty string if the value is null.
        /// </summary>
        /// <returns></returns>
        /// <see href="http://stackoverflow.com/questions/9442243/draw-string-with-normalized-scientific-notation-superscripted/9442631#9442631"/>
        public static string FormatAsPowerOfTen(this double? value, int decimals)
        {
            if (!value.HasValue)
            {
                return string.Empty;
            }
            return FormatAsPowerOfTen(value.Value, decimals);
        }

        /// <summary>
        /// Returns a value in scientific format, like 2.9 × 10.
        /// </summary>
        /// <returns></returns>
        /// <see href="http://stackoverflow.com/questions/9442243/draw-string-with-normalized-scientific-notation-superscripted/9442631#9442631"/>
        public static string FormatAsPowerOfTen(this double value, int decimals)
        {
            if (value == 0.0)
            {
                return "0";
            }

            if (double.IsNaN(value))
            {
                Debug.Assert(false, "Calculation resulted in NaN (Not a Number), probably as a result of a division of zero by zero.");
                return "<not a number>";
            }

            if (double.IsInfinity(value))
            {
                Debug.Assert(false, "Calculation resulted in Infinity, probably as a result of a division by zero.");
                return "<infinity>";
            }

            double expHelperValue;
            if (value > 0.0)
            {
                expHelperValue = value;
            }
            else
            {
                expHelperValue = -value;
            }

            var exp = Convert.ToInt32(Math.Floor(Math.Log10(expHelperValue)));

            string fmtMantissa;
            if (decimals < 0)
            {
                fmtMantissa = "{0}";
            }
            else
            {
                fmtMantissa = $"{{0:F{decimals}}}";
            }
            var formattedValue = string.Format(fmtMantissa, value / Math.Pow(10, exp));

            var formattedExponent = "";
            if (exp != 0)
            {
                const string fmtExponent = " × 10{0}";

                formattedExponent = string.Format(fmtExponent, FormatExponentWithSuperscript(exp));
            }

            return string.Concat(formattedValue, formattedExponent);
        }

        /// <summary>
        /// Constructs a string of special unicode superscript digits to format the exponent of the value.
        /// </summary>
        /// <param name="exp">exponent (power of ten)</param>
        /// <returns></returns>
        /// <see href="http://stackoverflow.com/questions/9442243/draw-string-with-normalized-scientific-notation-superscripted/9442631#9442631"/>
        private static string FormatExponentWithSuperscript(int exp)
        {
            var sb = new StringBuilder();

            bool isNegative = false;

            if (exp < 0)
            {
                isNegative = true;
                exp = -exp;
            }

            while (exp != 0)
            {
                sb.Insert(0, GetSuperscript(exp % 10));

                exp = exp / 10;
            }

            if (isNegative)
            {
                sb.Insert(0, '⁻'); //SUPERSCRIPT MINUS
            }

            return sb.ToString();
        }

        /// <summary>
        /// Returns the superscript unicode character of the specified digit.
        /// </summary>
        /// <returns></returns>
        /// <see href="http://stackoverflow.com/questions/9442243/draw-string-with-normalized-scientific-notation-superscripted/9442631#9442631"/>
        private static char GetSuperscript(int digit)
        {
            switch (digit)
            {
                case 0:
                    return '\x2070';

                case 1:
                    return '\x00B9';

                case 2:
                    return '\x00B2';

                case 3:
                    return '\x00B3';

                case 4:
                    return '\x2074';

                case 5:
                    return '\x2075';

                case 6:
                    return '\x2076';

                case 7:
                    return '\x2077';

                case 8:
                    return '\x2078';

                case 9:
                    return '\x2079';

                default:
                    throw new ApplicationException($"'{digit}' is not a valid digit.");
            }
        }

        public static string RoundToSignificantDigits(this double d, int digits)
        {
            if (d == 0)
                return "0";

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(d))) + 1);
            return (scale * Math.Round(d / scale, digits, MidpointRounding.AwayFromZero)).ToString();
        }
    }
}