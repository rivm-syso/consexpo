using System;

namespace RIVM.ConsExpo.DTO.Extensions
{
    /// <summary>
    ///
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Truncates the specified time span.
        /// </summary>
        /// <param name="dateTime">The date time to truncate.</param>
        /// <param name="timeSpan">A time span to define truncation.</param>
        /// <returns></returns>
        /// <example>dateTime = dateTime.Truncate(TimeSpan.FromSeconds(1)); // Truncate to whole second</example>
        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero)
            {
                return dateTime; // Or could throw an ArgumentException
            }

            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }
    }
}