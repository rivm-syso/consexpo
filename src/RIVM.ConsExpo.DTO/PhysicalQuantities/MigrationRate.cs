using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class MigrationRate : DistributablePhysicalQuantity<MigrationRateUnits>
    {
        protected override double Standardized => ConvertedValue(MigrationRateUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<MigrationRateUnits> AllUnits => MigrationRateUnits.AllUnits;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<MigrationRateUnits> AvailableUnits => MigrationRateUnits.AllUnits;

        public double InGramPerSquareCentimetrePerSecond()
        {
            return Standardized;
        }

        /// <summary>
        ///  Converts any MigrationRateUnit to the calculation unit of gram per square centimetre per second (g/m2/sec)
        /// </summary>
        public double InMilliGramPerSquareCentimetresPerSecond()
        {
            return Standardized * ConversionFactors.One2Milli;
        }
    }
}