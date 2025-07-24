using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// A room volume.
    /// </summary>
    public class RoomVolume : Volume
    {
        public RoomVolume()
        {
        }

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<VolumeUnits> AvailableUnits
        {
            get
            {
                var units = new List<VolumeUnits>
                {
                    VolumeUnits.CubicMetre
                };
                return units;
            }
        }

        protected override double MinForDefaultUnit => 0.001;
    }
}