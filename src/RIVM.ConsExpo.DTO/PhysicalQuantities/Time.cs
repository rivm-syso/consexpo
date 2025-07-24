using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public sealed class Time : PhysicalQuantity<TimeUnits>
    {
        public Time()
        { }

        public Time(double? value, TimeUnits unit)
        {
            Value = value;
            Unit = unit;
        }

        protected override double Standardized => ConvertedValue(TimeUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<TimeUnits> AvailableUnits => TimeUnits.AllUnits;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<TimeUnits> AllUnits => TimeUnits.AllUnits;

        public static bool operator <(Time time1, Time time2)
        {
            return time1.InSeconds() < time2.InSeconds();
        }

        public static bool operator >(Time time1, Time time2)
        {
            return time1.InSeconds() > time2.InSeconds();
        }

        public static bool operator <=(Time time1, Time time2)
        {
            return !(time1.InSeconds() > time2.InSeconds());
        }

        public static bool operator >=(Time time1, Time time2)
        {
            return !(time1.InSeconds() < time2.InSeconds());
        }

        public static Time operator +(Time time1, Time time2)
        {
            return time1.Add(time2);
        }

        public static Time operator -(Time time1, Time time2)
        {
            return time1.Subtract(time2);
        }

        /// <summary>
        /// Returns an new Time instance, based on the current instance, but with the unit changed to the target unit and the value converted to value corresponding with this target unit.
        /// </summary>
        /// <param name="targetUnit">The target unit.</param>
        /// <returns></returns>
        public Time ConvertedTo(TimeUnits targetUnit)
        {
            double? targetValue;

            if (this.HasValue)
            {
                targetValue = ConvertedValue(targetUnit);
            }
            else
            {
                targetValue = null;
            }

            return new Time(targetValue, targetUnit);
        }

        /// <summary>
        /// Returns the time in seconds.
        /// </summary>
        public double InSeconds()
        {
            return Standardized;
        }

        public double InMinutes()
        {
            return ConvertedTo(TimeUnits.Minute).Value.Value;
        }

        public double InHours()
        {
            return ConvertedTo(TimeUnits.Hour).Value.Value;
        }

        public double InDays()
        {
            return ConvertedTo(TimeUnits.Day).Value.Value;
        }

        public Time Add(Time timeToAdd)
        {
            Time tsInTargetUnit = timeToAdd.ConvertedTo(Unit);

            return new Time(this.Value + tsInTargetUnit.Value, Unit);
        }

        public Time Subtract(Time timeToSubtract)
        {
            Time tsInTargetUnit = timeToSubtract.ConvertedTo(Unit);

            return new Time(this.Value - tsInTargetUnit.Value, Unit);
        }
    }
}