using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// A length or thickness.
    /// </summary>
    public class DiffusionCoefficientForEmission : SurfaceRate
    {
        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<SurfaceRateUnits> AvailableUnits
        {
            get
            {
                var units = new List<SurfaceRateUnits>
                {
                    SurfaceRateUnits.SquareCentiMetrePerMinute,
                    SurfaceRateUnits.SquareCentiMetrePerHour,
                    SurfaceRateUnits.SquareMetrePerHour,
                    SurfaceRateUnits.SquareMetrePerSecond
                };
                return units;
            }
        }

        protected override double MinForDefaultUnit => 1E-15 * ConversionFactors.SecondsPerHour; //1E-5 m^2/s

        protected override double? MaxForDefaultUnit => 1E-8 * ConversionFactors.SecondsPerHour; //1E-5 m^2/s
    }
}