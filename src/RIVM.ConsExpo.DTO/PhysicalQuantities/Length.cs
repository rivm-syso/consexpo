using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using RIVM.ConsExpo.DTO.PhysicalUnits;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// A length or thickness.
    /// </summary>
    public abstract class Length : DistributablePhysicalQuantity<LengthUnits>
    {
        protected override double Standardized => ConvertedValue(LengthUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<LengthUnits> AllUnits => LengthUnits.AllUnits;

        public double InMetre()
        {
            return Standardized * ConversionFactors.Milli2One;
        }

        public double InCentimetre()
        {
            return Standardized * ConversionFactors.Milli2Centi;
        }

        public double InMillimetre()
        {
            return Standardized;
        }

        public double InMicrometre()
        {
            return Standardized * ConversionFactors.Milli2Micro;
        }
    }
}