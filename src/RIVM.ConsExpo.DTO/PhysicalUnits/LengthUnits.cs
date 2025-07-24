using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// The units of a volume.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class LengthUnits : UnitBase
    {
        public static readonly LengthUnits Micrometre = new LengthUnits(1, "µm", 1, ConversionFactors.Micro2Milli);

        public static readonly LengthUnits Millimetre = new LengthUnits(2, "mm", 0, 1);

        public static readonly LengthUnits Centimetre = new LengthUnits(3, "cm", 2, ConversionFactors.Centi2Milli);

        public static readonly LengthUnits Metre = new LengthUnits(4, "m", 3, ConversionFactors.One2Milli);

        public static readonly LengthUnits StandardUnit = LengthUnits.Millimetre;

        [NotMapped]
        [XmlIgnore]
        public static IList<LengthUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<LengthUnits>(new[]
                 {
                     Micrometre,
                     Millimetre,
                     Centimetre,
                     Metre
                 });
            }
        }

        protected LengthUnits()
        { }

        protected LengthUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}