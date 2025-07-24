using System;

namespace RIVM.ConsExpo.DTO.Helpers
{
    /// <summary>
    /// Helper class for generating unique names, e.g. when duplicating scenarios.
    /// </summary>
    public static class UniqueNameHelper
    {
        /// <summary>
        /// Delegate to check the uniqueness of the specified name for the specified user.
        /// </summary>
        /// <returns></returns>
        public delegate bool CheckUniqueNameForUser(string name, int userId);

        /// <summary>
        /// Attempts to find a unique name.
        /// </summary>
        /// <param name="name">The name to derive a new unique name from.</param>
        /// <param name="uniqueNameChecker">The delegate to check newly generated names for uniqueness.</param>
        /// <param name="maxLength">The max length of the generated name. If the name would become to long, ellipses are added.</param>
        /// <param name="userId">The DB-id of the user to generate this names for.</param>
        /// <param name="suffixFormat">A string format to use for generating unique names.</param>
        /// <param name="suffixCounterStart">The lowest number to use in the suffix.</param>
        /// <returns>A new unique name for the user, derived from the specified name by adding a suffix.</returns>
        public static string GetUniqueNameForUser(string name, CheckUniqueNameForUser uniqueNameChecker, int maxLength, int userId, string suffixFormat = " ({0})", int suffixCounterStart = 2)
        {
            int suffixCounter = suffixCounterStart;

            string checkName = name;

            while (!uniqueNameChecker(checkName, userId))
            {
                string suffix = string.Format(suffixFormat, suffixCounter++);
                if (suffix.Length >= maxLength)
                {
                    throw new ApplicationException(
                        $"Cannot generate a unique name from name '{name}' with maximum length of {maxLength}, with suffix format '{suffixFormat}' and start suffix counter {suffixCounterStart}.");
                }

                if (name.Length + suffix.Length > maxLength)
                {
                    checkName = name.Substring(0, maxLength - suffix.Length - 1) + '…' + suffix;
                }
                else
                {
                    checkName = name + suffix;
                }
            }
            return checkName;
        }
    }
}