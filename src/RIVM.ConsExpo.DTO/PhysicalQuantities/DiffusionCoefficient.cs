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
    public class DiffusionCoefficient : SurfaceRate
    {
        public static readonly SurfaceRateUnits StandardUnit = SurfaceRateUnits.SquareCentiMetrePerMinute;

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
    }
}