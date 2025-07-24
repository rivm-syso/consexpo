using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// The units of time durations.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class DurationUnits : UnitBase
    {
        public static readonly DurationUnits Second = new DurationUnits(1, "second", 0, ConversionFactors.MinutesPerSecond);

        public static readonly DurationUnits Minute = new DurationUnits(2, "minute", 1, 1);

        public static readonly DurationUnits Hour = new DurationUnits(3, "hour", 2, ConversionFactors.MinutesPerHour);

        public static readonly DurationUnits Day = new DurationUnits(4, "day", 3, ConversionFactors.MinutesPerDay);

        public static readonly DurationUnits Week = new DurationUnits(5, "week", 4, ConversionFactors.MinutesPerDay * ConversionFactors.DaysPerWeek);

        public static readonly DurationUnits Month = new DurationUnits(6, "month", 5, ConversionFactors.MinutesPerDay * ConversionFactors.DaysPerMonth);

        public static readonly DurationUnits Year = new DurationUnits(7, "year", 6, ConversionFactors.MinutesPerDay * ConversionFactors.DaysPerYear);

        public static readonly DurationUnits StandardUnit = Minute;

        [NotMapped]
        [XmlIgnore]
        public static IList<DurationUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<DurationUnits>(new[]
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

        protected DurationUnits()
        { }

        protected DurationUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}