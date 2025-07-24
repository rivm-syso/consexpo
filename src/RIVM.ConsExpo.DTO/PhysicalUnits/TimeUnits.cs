using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// The units of time.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class TimeUnits : UnitBase
    {
        public static readonly TimeUnits Second = new TimeUnits(1, "s", 1, 1);

        public static readonly TimeUnits Minute = new TimeUnits(2, "min", 2, ConversionFactors.SecondsPerMinute);

        public static readonly TimeUnits Hour = new TimeUnits(3, "hour", 3, ConversionFactors.SecondsPerHour);

        public static readonly TimeUnits Day = new TimeUnits(4, "day", 4, ConversionFactors.SecondsPerDay);

        public static readonly TimeUnits Week = new TimeUnits(5, "week", 5, ConversionFactors.SecondsPerWeek);

        public static readonly TimeUnits Month = new TimeUnits(6, "month", 6, ConversionFactors.SecondsPerMonth);

        public static readonly TimeUnits Year = new TimeUnits(7, "year", 7, ConversionFactors.SecondsPerYear);

        /// <summary>
        /// The standard unit of Time.
        /// </summary>
        public static readonly TimeUnits StandardUnit = TimeUnits.Second;

        [NotMapped]
        [XmlIgnore]
        public static IList<TimeUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<TimeUnits>(new[]
                 {
                     Second,
                     Minute,
                     Hour,
                     Day,
                     Week,
                     Month,
                     Year
                 });
            }
        }

        protected TimeUnits()
        { }

        protected TimeUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}