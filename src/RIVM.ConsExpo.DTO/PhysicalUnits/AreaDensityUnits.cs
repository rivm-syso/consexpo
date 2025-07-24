using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    public class AreaDensityUnits : UnitBase
    {
        public static readonly AreaDensityUnits GramPerSquareMetre = new AreaDensityUnits(1, "g/m²", 1, ConversionFactors.One2Milli);

        public static readonly AreaDensityUnits MilligramPerSquareMetre = new AreaDensityUnits(2, "mg/m²", 2, 1.0);

        public static readonly AreaDensityUnits MicrogramPerSquareMetre = new AreaDensityUnits(3, "µg/m²", 3, ConversionFactors.Micro2Milli);

        /// <summary>
        /// The standard unit.
        /// </summary>
        public static readonly AreaDensityUnits StandardUnit = AreaDensityUnits.MilligramPerSquareMetre;

        [NotMapped]
        [XmlIgnore]
        public static IList<AreaDensityUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<AreaDensityUnits>(new[]
                 {
                     GramPerSquareMetre ,
                     MilligramPerSquareMetre,
                     MicrogramPerSquareMetre
                 });
            }
        }

        protected AreaDensityUnits()
        { }

        protected AreaDensityUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}