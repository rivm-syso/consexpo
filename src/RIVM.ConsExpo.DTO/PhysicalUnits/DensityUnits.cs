using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// Units of mass concentration, specified as weight / volume.
    /// g/cm3, mg/cm3, kg/l, g/m3, kg/m3
    /// </summary>
    public class DensityUnits : UnitBase
    {
        public static readonly DensityUnits MilligramPerCubicCentimetre = new DensityUnits(1, "mg/cm³", 1, 1.0 / Math.Pow(ConversionFactors.Centi2One, 3));

        public static readonly DensityUnits MilligramPerCubicMetre = new DensityUnits(2, "mg/m³", 2, 1.0);

        public static readonly DensityUnits GramPerCubicCentimetre = new DensityUnits(3, "g/cm³", 3, ConversionFactors.One2Milli / Math.Pow(ConversionFactors.Centi2One, 3));

        public static readonly DensityUnits GramPerLitre = new DensityUnits(4, "g/l", 4, ConversionFactors.One2Milli / ConversionFactors.CubicMetresPerLitre);

        public static readonly DensityUnits GramPerCubicMetre = new DensityUnits(5, "g/m³", 5, ConversionFactors.One2Milli);

        public static readonly DensityUnits KilogramPerLitre = new DensityUnits(6, "kg/l", 6, ConversionFactors.Kilo2Milli / ConversionFactors.CubicMetresPerLitre);

        public static readonly DensityUnits KilogramPerCubicMetre = new DensityUnits(7, "kg/m³", 7, ConversionFactors.Kilo2Milli);

        public static readonly DensityUnits StandardUnit = DensityUnits.MilligramPerCubicMetre;

        [NotMapped]
        [XmlIgnore]
        public static IList<DensityUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<DensityUnits>(new[]
                 {
                     MilligramPerCubicCentimetre,
                     MilligramPerCubicMetre,
                     GramPerCubicCentimetre,
                     GramPerLitre,
                     GramPerCubicMetre,
                     KilogramPerLitre,
                     KilogramPerCubicMetre,
                 });
            }
        }

        protected DensityUnits()
        { }

        protected DensityUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}