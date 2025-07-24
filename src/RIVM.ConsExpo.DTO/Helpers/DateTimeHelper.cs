using System;

namespace RIVM.ConsExpo.DTO.Helpers
{
    /// <summary>
    /// Helper for DateTime functions.
    /// </summary>
    public class DateTimeHelper
    {
        /// <summary>
        /// Gets a string representation of the current system date and time, specifying time zone information.
        /// </summary>
        /// <value>
        /// The system time string.
        /// </value>
        public static string SystemTimeString
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss '(GMT 'zzz')'");
            }
        }
    }
}