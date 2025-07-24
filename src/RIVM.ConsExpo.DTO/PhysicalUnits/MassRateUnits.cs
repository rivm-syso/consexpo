using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// The units of a weight (of material) divided by a time duration.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class MassRateUnits : UnitBase
    {
        public static readonly MassRateUnits MicrogramPerMinute = new MassRateUnits(1, "µg/min", 1, ConversionFactors.Micro2Milli);

        public static readonly MassRateUnits MicrogramPerHour = new MassRateUnits(2, "µg/hr", 2, ConversionFactors.Micro2Milli / ConversionFactors.MinutesPerHour);

        public static readonly MassRateUnits MicrogramPerDay = new MassRateUnits(3, "µg/day", 3, ConversionFactors.Micro2Milli / ConversionFactors.MinutesPerDay);

        public static readonly MassRateUnits MilligramPerMinute = new MassRateUnits(4, "mg/min", 4, 1);

        public static readonly MassRateUnits MilligramPerHour = new MassRateUnits(5, "mg/hr", 5, 1.0 / ConversionFactors.MinutesPerHour);

        public static readonly MassRateUnits MilligramPerDay = new MassRateUnits(6, "mg/day", 6, 1.0 / ConversionFactors.MinutesPerDay);

        public static readonly MassRateUnits GramPerSecond = new MassRateUnits(7, "g/s", 7, ConversionFactors.One2Milli / ConversionFactors.MinutesPerSecond);

        public static readonly MassRateUnits GramPerMinute = new MassRateUnits(8, "g/min", 8, ConversionFactors.One2Milli);

        public static readonly MassRateUnits GramPerHour = new MassRateUnits(9, "g/hr", 9, ConversionFactors.One2Milli / ConversionFactors.MinutesPerHour);

        public static readonly MassRateUnits GramPerDay = new MassRateUnits(10, "g/day", 10, ConversionFactors.One2Milli / ConversionFactors.MinutesPerDay);

        public static readonly MassRateUnits StandardUnit = MassRateUnits.MilligramPerMinute;

        [NotMapped]
        [XmlIgnore]
        public static IList<MassRateUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<MassRateUnits>(new[]
                 {
                     MicrogramPerMinute,
                     MicrogramPerHour,
                     MicrogramPerDay,
                     MilligramPerMinute,
                     MilligramPerHour,
                     MilligramPerDay,
                     GramPerSecond,
                     GramPerMinute,
                     GramPerHour,
                     GramPerDay,
                     StandardUnit
                 });
            }
        }

        protected MassRateUnits()
        { }

        protected MassRateUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}