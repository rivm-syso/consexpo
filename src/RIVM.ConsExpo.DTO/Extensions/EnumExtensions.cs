using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace RIVM.ConsExpo.DTO.Extensions
{
    /// <summary>
    /// Some useful methods for enums, related to reading attribute info.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the name in <see cref="DisplayAttribute"/> of the Enum.
        /// </summary>
        /// <param name="enumeration">A <see cref="Enum"/> that the method is extended to.</param>
        /// <returns>A name string in the <see cref="DisplayAttribute"/> of the Enum.</returns>
        public static string GetDisplayName(this Enum enumeration)
        {
            Type enumType = enumeration.GetType();
            string enumName = Enum.GetName(enumType, enumeration);
            string displayName = enumName;
            try
            {
                MemberInfo member = enumType.GetMember(enumName)[0];

                object[] attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);
                DisplayAttribute attribute = (DisplayAttribute)attributes[0];
                displayName = attribute.Name;

                if (attribute.ResourceType != null)
                {
                    displayName = attribute.GetName();
                }
            }
            catch { }
            return displayName;
        }
    }
}