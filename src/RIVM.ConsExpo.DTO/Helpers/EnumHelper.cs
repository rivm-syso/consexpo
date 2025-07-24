using RIVM.ConsExpo.DTO.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace RIVM.ConsExpo.DTO.Helpers
{
    /// <typeparam name="T"></typeparam>
    /// <see href="http://stackoverflow.com/questions/13099834/how-to-get-the-display-name-attribute-of-an-enum-member-via-mvc-razor-code/13100409#13100409"/>
    /// <remarks>Hack: labeled with suffix 2, to avoid name conflicts with EnumHelper in the shared project RIVM.VSP.MVC.Web</remarks>
    public static class EnumHelper2<T>
    {
        /// <summary>
        /// Gets a list of the values in the enumeration.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();

            foreach (FieldInfo fi in value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value.GetType(), fi.Name, false));
            }
            return enumValues;
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Gets all names of the enum.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IList<string> GetNames(Enum value)
        {
            return value.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        /// <summary>
        /// Gets all display values of the enum.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        /// <summary>
        /// Gets the display value from the Display attribute for the enum value. The can contain a user-friendly description of the enum value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes == null)
            {
                return value.ToString().ToFriendly();
            }
            else
            {
                return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString().ToFriendly();
            }
        }
    }
}