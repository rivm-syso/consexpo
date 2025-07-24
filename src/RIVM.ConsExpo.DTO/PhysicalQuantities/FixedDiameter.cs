using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// A diameter that cannot be specified with as a distribution.
    /// </summary>
    public class FixedDiameter : PhysicalQuantity<LengthUnits>
    {
        public static readonly LengthUnits StandardUnit = LengthUnits.Micrometre;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<LengthUnits> AllUnits => LengthUnits.AllUnits;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<LengthUnits> AvailableUnits
        {
            get
            {
                var units = new List<LengthUnits>();
#warning HACK: for test of effective diameter.
                units.Add(LengthUnits.Metre);
                units.Add(LengthUnits.Micrometre);
                return units;
            }
        }

        protected override double Standardized => ConvertedValue(StandardUnit);

        public double InMetre()
        {
            return Standardized * ConversionFactors.Micro2One;
        }

        public double InCm()
        {
            return Standardized * ConversionFactors.Micro2Centi;
        }

        public double InMm()
        {
            return Standardized;
        }

        public double InMicroMetre()
        {
            return Standardized;
        }
    }
}