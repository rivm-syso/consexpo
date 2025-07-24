using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class DailyDuration : DistributablePhysicalQuantity<DailyDurationUnits>
    {
        private const string displayName = "Daily exposure duration";

        public DailyDuration() : base(displayName, DailyDurationUnits.MinutesPerDay, 1.0, ConversionFactors.MinutesPerDay)
        { }

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<DailyDurationUnits> AvailableUnits
        {
            get
            {
                var availableUnits = new List<DailyDurationUnits>();
                foreach (var unit in DailyDurationUnits.AllUnits)
                {
                    availableUnits.Add(unit);
                }
                return availableUnits;
            }
        }

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<DailyDurationUnits> AllUnits => DailyDurationUnits.AllUnits;

        /// <summary>
        /// Returns the daily duration as a Time instance, with unit minutes.
        /// </summary>
        public Time AsTimePerDay()
        {
            return new Time(Standardized, TimeUnits.Minute);
        }

        /// <inheritdoc/>
        protected override double Standardized => ConvertedValue(DailyDurationUnits.StandardUnit);

        /// <summary>
        /// Returns the daily duration in minutes per day.
        /// </summary>
        public double InSecondsPerDay()
        {
            return Standardized * ConversionFactors.SecondsPerMinute;
        }

        /// <summary>
        /// Returns the daily duration in minutes per day.
        /// </summary>
        public double InMinutesPerDay()
        {
            return Standardized;
        }

        /// <summary>
        /// Gets the duration in hours per day.
        /// </summary>
        public double InHoursPerDay()
        {
            return Standardized * ConversionFactors.HoursPerMinute;
        }

        /// <summary>
        /// Get the fraction of the time.
        /// </summary>
        /// <returns></returns>
        public double AsFraction()
        {
            return Standardized * ConversionFactors.DaysPerMinute;
        }
    }
}