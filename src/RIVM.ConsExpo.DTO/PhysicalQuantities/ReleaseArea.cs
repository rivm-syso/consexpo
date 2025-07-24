using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class ReleaseArea : Area
    {
        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<AreaUnits> AvailableUnits
        {
            get
            {
                var units = new List<AreaUnits>
                {
                    AreaUnits.SquareCentimetre,
                    AreaUnits.SquareMetre
                };
                return units;
            }
        }
    }
}