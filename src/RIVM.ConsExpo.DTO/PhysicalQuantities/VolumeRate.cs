using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class VolumeRate : DistributablePhysicalQuantity<VolumeRateUnits>
    {
        protected override double Standardized => ConvertedValue(VolumeRateUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<VolumeRateUnits> AvailableUnits => VolumeRateUnits.AllUnits;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<VolumeRateUnits> AllUnits => VolumeRateUnits.AllUnits;

        /// <summary>
        ///  Converts any VolumeRateUnit to the calculation unit of cubic metre per second (m3/sec)
        /// </summary>
        public double InCubicMetresPerSecond()
        {
            return Standardized;
        }

        public double InCubicMetresPerHour()
        {
            return Standardized / ConversionFactors.HoursPerSecond;
        }

        /// <summary>
        ///  Converts any VolumeRateUnit to the calculation unit of cubic metre per second (m3/sec)
        /// </summary>
        public double? InCubicMetresPerSecondIfSpecified()
        {
            double? internalValue = DerivedValue;

            if (internalValue.HasValue)
            {
                return InCubicMetresPerSecond();
            }
            else
            {
                return null;
            }
        }

        public double? InCubicMetresPerHourIfSpecified()
        {
            double? internalValue = DerivedValue;

            if (internalValue.HasValue)
            {
                return InCubicMetresPerHour();
            }
            else
            {
                return null;
            }
        }
    }
}