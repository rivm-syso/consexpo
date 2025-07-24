using RIVM.ConsExpo.DTO.PhysicalUnits;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public abstract class SurfaceRate : DistributablePhysicalQuantity<SurfaceRateUnits>
    {
        protected override double Standardized => ConvertedValue(SurfaceRateUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<SurfaceRateUnits> AllUnits => SurfaceRateUnits.AllUnits;

        /// <summary>
        ///  Converts any SurfaceRateUnit to the calculation unit of square metre per second (m2/sec)
        /// </summary>
        public double InSquareMetresPerSecond()
        {
            return Standardized / ConversionFactors.SecondsPerHour;
        }

        public double InSquareCentimetrePerSecond()
        {
            return Math.Pow(ConversionFactors.One2Centi, 2) * Standardized / ConversionFactors.SecondsPerHour;
        }

        public double InSquareCentimetrePerMinute()
        {
            return Math.Pow(ConversionFactors.One2Centi, 2) * Standardized / ConversionFactors.MinutesPerHour;
        }

        public double InSquareMetresPerHour()
        {
            return Standardized;
        }
    }
}