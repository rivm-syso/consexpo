using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class Rate : DistributablePhysicalQuantity<RateUnits>
    {
        protected override double Standardized => ConvertedValue(RateUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<RateUnits> AllUnits => RateUnits.AllUnits;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<RateUnits> AvailableUnits => RateUnits.AllUnits;

        /// <summary>
        /// Returns the rate in 1/s.
        /// </summary>
        /// <value>
        ///
        /// </value>
        public double InTimesPerSecond()
        {
            return Standardized / ConversionFactors.SecondsPerHour;
        }

        public double InTimesPerMinute()
        {
            return Standardized / ConversionFactors.MinutesPerHour;
        }

        public double InTimesPerHour()
        {
            return Standardized;
        }
    }
}