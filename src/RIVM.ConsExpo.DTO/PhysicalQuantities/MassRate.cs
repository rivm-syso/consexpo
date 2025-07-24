using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using RIVM.ConsExpo.DTO.PhysicalUnits;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public abstract class MassRate : DistributablePhysicalQuantity<MassRateUnits>
    {
        protected override double Standardized => ConvertedValue(MassRateUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<MassRateUnits> AllUnits => MassRateUnits.AllUnits;

        public double InMilligramPerMinute()
        {
            return Standardized;
        }

        public double InGramPerSecond()
        {
            return Standardized * ConversionFactors.Milli2One / ConversionFactors.SecondsPerMinute;
        }

        public double InGramPerHour()
        {
            return Standardized * ConversionFactors.Milli2One / ConversionFactors.HoursPerMinute;
        }

        public double InGramPerDay()
        {
            return Standardized * ConversionFactors.Milli2One / ConversionFactors.DaysPerMinute;
        }
    }
}