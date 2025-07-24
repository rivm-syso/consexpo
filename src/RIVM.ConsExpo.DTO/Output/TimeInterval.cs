using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// An interval of time, specified by a start and an end time.
    /// </summary>
    public class TimeInterval
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeInterval"/> class.
        /// </summary>
        /// <param name="startIntervalValue">The start interval value.</param>
        /// <param name="endIntervalValue">The end interval value.</param>
        /// <param name="timeUnit">The time unit for both values.</param>
        public TimeInterval(double startIntervalValue, double endIntervalValue, TimeUnits timeUnit)
        {
            StartTime = new Time(startIntervalValue, timeUnit);
            EndTime = new Time(endIntervalValue, timeUnit);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeInterval"/> class.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        public TimeInterval(Time startTime, Time endTime)
        {
            this.StartTime = startTime;
            this.EndTime = endTime;
        }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        public Time StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        public Time EndTime { get; set; }

        /// <summary>
        /// Returns the duration of the interval in seconds.
        /// </summary>
        public double DurationInSeconds
        {
            get
            {
                return EndTime.InSeconds() - StartTime.InSeconds();
            }
        }

        /// <summary>
        /// Returns the duration of the interval in minutes.
        /// </summary>
        public double DurationInMinutes
        {
            get
            {
                return EndTime.InMinutes() - StartTime.InMinutes();
            }
        }
    }
}