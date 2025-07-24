using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RIVM.ConsExpo.DTO.Extensions
{
    /// <summary>
    /// Extensions methods for strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Convert to a friendly representation, in which camel casing is replaced by spaces.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns></returns>
        /// <see href="http://stackoverflow.com/questions/3621464/camelcase-conversion-to-friendly-name-i-e-enum-constants-problems/3622700#3622700"/>
        public static string ToFriendly(this string identifier)
        {
            return Regex.Replace(identifier, @"(?<a>(?<!^)((?:[A-Z][a-z])|(?:(?<!^[A-Z]+)[A-Z0-9]+(?:(?=[A-Z][a-z])|$))|(?:[0-9]+)))", m => " " + ToLowerExceptAllCaps(m));
        }

        private static string ToLowerExceptAllCaps(Match m)
        {
            string value = m.ToString();

            if (char.IsUpper(value.Last()))
            {
                return value;
            }
            else
            {
                return value.ToLower();
            }
        }

        /// <summary>
        /// Truncates the source to the specified maximum length, if it is longer than that.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns></returns>
        public static string Truncate(this string source, int maxLength)
        {
            if (source.Length > maxLength)
            {
                source = source.Substring(0, maxLength);
            }

            return source;
        }

        /// <summary>
        /// Improved C# Slug generator (or how to make friendly url from a post title)
        /// </summary>
        ///<see href="http://predicatet.blogspot.nl/2009/04/improved-c-slug-generator-or-how-to.html"/>
        /// <remarks>Here is an improved version of the Slug generator from Kamran Ayub.You can found original post here : http://www.intrepidstudios.com/blog/2009/2/10/function-to-generate-a-url-friendly-string.aspx
        /// I’ve added the ability to remove accent.</remarks>

        public static string GenerateSlug(this string phrase, int maxLength = 50)
        {
            string str = phrase.RemoveAccent();

            str = Regex.Replace(str, @"[^a-zA-Z0-9\s-]", ""); // invalid chars
            str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space
            str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim(); // cut and trim it

            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        /// <see href="https://stackoverflow.com/a/16999640/456456">Convert string to hex-string in C#</see>
        public static string ToHexString(this string txt)
        {
            byte[] ba = Encoding.Default.GetBytes(txt);

            return BitConverter.ToString(ba);
        }
    }
}