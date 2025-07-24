using System;
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
    public class AreaUnits : UnitBase
    {
        public static readonly AreaUnits SquareMillimetre = new AreaUnits(1, "mm²", 1, Math.Pow(ConversionFactors.Milli2Centi, 2));

        public static readonly AreaUnits SquareCentimetre = new AreaUnits(2, "cm²", 2, 1.0);

        public static readonly AreaUnits SquareDecimetre = new AreaUnits(3, "dm²", 3, Math.Pow(ConversionFactors.Deci2Centi, 2));

        public static readonly AreaUnits SquareMetre = new AreaUnits(4, "m²", 4, Math.Pow(ConversionFactors.One2Centi, 2));

        public static readonly AreaUnits StandardUnit = AreaUnits.SquareCentimetre;

        [NotMapped]
        [XmlIgnore]
        public static IList<AreaUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<AreaUnits>(new[]
                 {
                     SquareMillimetre,
                     SquareCentimetre,
                     SquareDecimetre,
                     SquareMetre
                 });
            }
        }

        protected AreaUnits()
        { }

        protected AreaUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}