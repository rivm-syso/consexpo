using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// The units of a transfer coefficient.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class VelocityUnits : UnitBase
    {
        public static readonly VelocityUnits CentimetrePerHour = new VelocityUnits(1, "cm/hr", 1, ConversionFactors.Centi2One / ConversionFactors.SecondsPerHour);

        public static readonly VelocityUnits CentimetrePerMinute = new VelocityUnits(2, "cm/min", 2, ConversionFactors.Centi2One / ConversionFactors.SecondsPerMinute);

        public static readonly VelocityUnits MetrePerMinute = new VelocityUnits(3, "m/min", 3, 1.0 / ConversionFactors.SecondsPerMinute);

        public static readonly VelocityUnits MillimetrePerMinute = new VelocityUnits(4, "mm/min", 4, ConversionFactors.Milli2One / ConversionFactors.SecondsPerMinute);

        public static readonly VelocityUnits MetrePerHour = new VelocityUnits(5, "m/hr", 5, 1.0 / ConversionFactors.SecondsPerHour);

        public static readonly VelocityUnits MetrePerSecond = new VelocityUnits(6, "m/s", 6, 1);

        public static readonly VelocityUnits StandardUnit = VelocityUnits.MetrePerSecond;

        [NotMapped]
        [XmlIgnore]
        public static IList<VelocityUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<VelocityUnits>(new[]
                 {
                      CentimetrePerHour,
                      CentimetrePerMinute,
                      MetrePerMinute,
                      MillimetrePerMinute,
                      MetrePerHour,
                      MetrePerSecond,
                 });
            }
        }

        protected VelocityUnits()
        { }

        protected VelocityUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}