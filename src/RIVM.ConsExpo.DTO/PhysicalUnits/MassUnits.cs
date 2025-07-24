using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// Units of product weight. Since they differ in range from body weight, the units are product weight specific.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class MassUnits : UnitBase
    {
        public static readonly MassUnits Kilogram = new MassUnits(1, "kg", 1, ConversionFactors.Kilo2Milli);

        public static readonly MassUnits Gram = new MassUnits(2, "g", 2, ConversionFactors.One2Milli);

        public static readonly MassUnits Milligram = new MassUnits(3, "mg", 3, 1);

        public static readonly MassUnits Microgram = new MassUnits(4, "µg", 4, ConversionFactors.Micro2Milli);

        public static readonly MassUnits StandardUnit = MassUnits.Milligram;

        [NotMapped]
        [XmlIgnore]
        public static IList<MassUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<MassUnits>(new[]
                 {
                      Kilogram,
                      Gram,
                      Milligram,
                      Microgram,
                 });
            }
        }

        protected MassUnits()
        { }

        protected MassUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}