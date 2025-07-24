using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using RIVM.ConsExpo.DTO.PhysicalUnits;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public abstract class Velocity : DistributablePhysicalQuantity<VelocityUnits>
    {
        protected override double Standardized => ConvertedValue(VelocityUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<VelocityUnits> AllUnits => VelocityUnits.AllUnits;

        public double InMetresPerMinute()
        {
            return Standardized / ConversionFactors.MinutesPerSecond;
        }

        public double InMetresPerHour()
        {
            return Standardized / ConversionFactors.HoursPerSecond;
        }

        public double InCentimetrePerMinute()
        {
            return Standardized * ConversionFactors.One2Centi / ConversionFactors.MinutesPerSecond;
        }
    }
}