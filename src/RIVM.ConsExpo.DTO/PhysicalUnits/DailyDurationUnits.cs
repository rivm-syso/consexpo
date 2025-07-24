using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    public class DailyDurationUnits : UnitBase
    {
        /// <summary>
        /// The units of daily durations.
        /// </summary>

        public static readonly DailyDurationUnits MinutesPerDay = new DailyDurationUnits(1, "minutes/day", 1, 1);

        public static readonly DailyDurationUnits HoursPerDay = new DailyDurationUnits(2, "hours/day", 0, ConversionFactors.MinutesPerHour);

        public static readonly DailyDurationUnits StandardUnit = MinutesPerDay;

        [NotMapped]
        [XmlIgnore]
        public static IList<DailyDurationUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<DailyDurationUnits>(new[]
                {
                    HoursPerDay,
                    MinutesPerDay
                });
            }
        }

        public DailyDurationUnits()
        { }

        protected DailyDurationUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}