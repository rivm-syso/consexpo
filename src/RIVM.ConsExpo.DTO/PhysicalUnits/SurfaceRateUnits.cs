using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// The units of a surface rate.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class SurfaceRateUnits : UnitBase
    {
        public static readonly SurfaceRateUnits SquareMetrePerHour = new SurfaceRateUnits(1, "m²/hr", 1, 1);

        public static readonly SurfaceRateUnits SquareMetrePerDay = new SurfaceRateUnits(2, "m²/day", 2, 1.0 / ConversionFactors.HoursPerDay);

        public static readonly SurfaceRateUnits SquareMetrePerSecond = new SurfaceRateUnits(3, "m²/s", 3, 1.0 / ConversionFactors.HoursPerSecond);

        public static readonly SurfaceRateUnits SquareCentiMetrePerMinute = new SurfaceRateUnits(4, "cm²/min", 4, Math.Pow(ConversionFactors.Centi2One, 2) / ConversionFactors.HoursPerMinute);

        public static readonly SurfaceRateUnits SquareCentiMetrePerHour = new SurfaceRateUnits(5, "cm²/hr", 5, Math.Pow(ConversionFactors.Centi2One, 2));

        public static readonly SurfaceRateUnits StandardUnit = SurfaceRateUnits.SquareMetrePerHour;

        [NotMapped]
        [XmlIgnore]
        public static IList<SurfaceRateUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<SurfaceRateUnits>(new[]
                 {
                     SquareMetrePerHour,
                     SquareMetrePerDay,
                     SquareMetrePerSecond,
                     SquareCentiMetrePerMinute,
                     SquareCentiMetrePerHour
                 });
            }
        }

        protected SurfaceRateUnits()
        { }

        protected SurfaceRateUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}