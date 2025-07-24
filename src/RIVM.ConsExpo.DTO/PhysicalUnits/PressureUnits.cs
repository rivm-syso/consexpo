using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// The units of vapour pressure.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class PressureUnits : UnitBase
    {
        public static readonly PressureUnits Pascal = new PressureUnits(1, "Pa", 1, 1.0);

        public static readonly PressureUnits MmHg = new PressureUnits(2, "mm-Hg", 2, ConversionFactors.MmHg2Pascal);

        /// <summary>
        /// Standard atmosphere (atm), _not_ technical atmosphere (at).
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Atmosphere_(unit)"/>
        public static readonly PressureUnits Atmosphere = new PressureUnits(3, "atmosphere", 3, ConversionFactors.Atmosphere2Pascal);

        public static readonly PressureUnits Bar = new PressureUnits(4, "bar", 4, ConversionFactors.Bar2Pascal);

        public static readonly PressureUnits Millibar = new PressureUnits(5, "millibar", 5, ConversionFactors.Millibar2Pascal);

        public static readonly PressureUnits StandardUnit = PressureUnits.Pascal;

        [NotMapped]
        [XmlIgnore]
        public static IList<PressureUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<PressureUnits>(new[]
                 {
                     Pascal,
                     MmHg,
                     Atmosphere,
                     Bar,
                     Millibar
                 });
            }
        }

        protected PressureUnits()
        { }

        protected PressureUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}