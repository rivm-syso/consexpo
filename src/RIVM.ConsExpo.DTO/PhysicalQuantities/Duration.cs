using RIVM.ConsExpo.DTO.PhysicalUnits;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// A base class for all durations. Derived classes are used to allow for subsets of supported units.
    /// </summary>
    public abstract class Duration : DistributablePhysicalQuantity<DurationUnits>
    {
        protected override double Standardized => ConvertedValue(DurationUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<DurationUnits> AvailableUnits => DurationUnits.AllUnits;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<DurationUnits> AllUnits => DurationUnits.AllUnits;

        /// <summary>
        /// Returns the duration in seconds.
        /// </summary>
        public double InSeconds()
        {
            return Standardized * ConversionFactors.SecondsPerMinute;
        }

        /// <summary>
        /// Gets the duration in minutes.
        /// </summary>
        public virtual double InMinutes()
        {
            return Standardized;
        }

        /// <summary>
        /// Gets the duration in hours.
        /// </summary>
        public double InHours()
        {
            return Standardized * ConversionFactors.HoursPerMinute;
        }

        /// <summary>
        /// Gets the duration in days.
        /// </summary>
        public double InDays()
        {
            return Standardized * ConversionFactors.DaysPerMinute;
        }

        public double InYears()
        {
            return Standardized * ConversionFactors.YearsPerMinute;
        }

        /// <summary>
        /// Converts to Time with the same unit. No conversion calculation.
        /// </summary>
        /// <value>
        /// The Time value and unit.
        /// </value>
        /// <exception cref="System.ApplicationException"></exception>
        public Time AsTime()
        {
            TimeUnits timeUnit;
            if (Unit == DurationUnits.Second)
            {
                timeUnit = TimeUnits.Second;
            }
            else if (Unit == DurationUnits.Minute)
            {
                timeUnit = TimeUnits.Minute;
            }
            else if (Unit == DurationUnits.Hour)
            {
                timeUnit = TimeUnits.Hour;
            }
            else if (Unit == DurationUnits.Day)
            {
                timeUnit = TimeUnits.Day;
            }
            else if (Unit == DurationUnits.Week)
            {
                timeUnit = TimeUnits.Week;
            }
            else if (Unit == DurationUnits.Month)
            {
                timeUnit = TimeUnits.Month;
            }
            else if (Unit == DurationUnits.Year)
            {
                timeUnit = TimeUnits.Year;
            }
            else
            {
                throw new NotSupportedException(string.Format("Unsupported duration unit '{0}'", Unit.ToString()));
            }

            return new Time(DerivedValue, timeUnit);
        }
    }
}