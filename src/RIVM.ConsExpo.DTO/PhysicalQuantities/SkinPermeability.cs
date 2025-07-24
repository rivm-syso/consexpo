using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class SkinPermeability : Velocity
    {
        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<VelocityUnits> AvailableUnits
        {
            get
            {
                var units = new List<VelocityUnits>
                {
                    VelocityUnits.CentimetrePerHour,
                    VelocityUnits.CentimetrePerMinute,
                    VelocityUnits.MetrePerMinute,
                    VelocityUnits.MillimetrePerMinute,
                    VelocityUnits.MetrePerHour
                };

                return units;
            }
        }
    }
}