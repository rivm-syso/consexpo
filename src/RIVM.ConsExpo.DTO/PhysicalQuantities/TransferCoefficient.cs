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
    public class TransferCoefficient : SurfaceRate
    {
        public static readonly SurfaceRateUnits StandardUnit = SurfaceRateUnits.SquareMetrePerHour;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<SurfaceRateUnits> AvailableUnits
        {
            get
            {
                var units = new List<SurfaceRateUnits>
                {
                    SurfaceRateUnits.SquareMetrePerHour,
                    SurfaceRateUnits.SquareMetrePerDay
                };
                return units;
            }
        }
    }
}