using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class Frequency : DistributablePhysicalQuantity<FrequencyUnits>
    {
        protected override double Standardized => ConvertedValue(FrequencyUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<FrequencyUnits> AllUnits => FrequencyUnits.AllUnits;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<FrequencyUnits> AvailableUnits => FrequencyUnits.AllUnits;

        public double InTimesPerDay()
        {
            return Standardized;
        }

        /// <summary>
        /// Gets the frequency in times per day, if it was specified. Frequency is an optional parameter.
        /// </summary>
        public double? InTimesPerDayIfSpecified()
        {
            double? internalValue = DerivedValue;

            if (internalValue.HasValue)
            {
                return InTimesPerDay();
            }

            return null;
        }

        public double InTimesPerYear()
        {
            return ConvertedValue(FrequencyUnits.Yearly);
        }

        /// <summary>
        /// Gets the frequency in times per year, if it was specified. Frequency is an optional parameter.
        /// </summary>
        public double? InTimesPerYearIfSpecified()
        {
            double? internalValue = DerivedValue;

            if (internalValue.HasValue)
            {
                return InTimesPerYear();
            }

            return null;
        }
    }
}