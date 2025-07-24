using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// The units of a volume rate.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class VolumeRateUnits : UnitBase
    {
        public static readonly VolumeRateUnits CubicMetrePerHour = new VolumeRateUnits(1, "m³/hr", 1, 1.0 / ConversionFactors.SecondsPerHour);

        public static readonly VolumeRateUnits CubicMetrePerDay = new VolumeRateUnits(2, "m³/day", 2, 1.0 / ConversionFactors.SecondsPerDay);

        public static readonly VolumeRateUnits LiterPerMinute = new VolumeRateUnits(3, "l/min", 3, ConversionFactors.CubicMetresPerLitre / ConversionFactors.SecondsPerMinute);

        public static readonly VolumeRateUnits CubicMetrePerSecond = new VolumeRateUnits(4, "m³/s", 4, 1.0);

        public static readonly VolumeRateUnits StandardUnit = CubicMetrePerSecond;

        [NotMapped]
        [XmlIgnore]
        public static IList<VolumeRateUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<VolumeRateUnits>(new[]
                 {
                     CubicMetrePerHour,
                     CubicMetrePerDay,
                     LiterPerMinute
                    //Note: StandardUnit is not in the set of units offered to the user.
                 });
            }
        }

        protected VolumeRateUnits()
        { }

        protected VolumeRateUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}