using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class Height : Length
    {
        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<LengthUnits> AvailableUnits
        {
            get
            {
                var units = new List<LengthUnits>
                {
                    LengthUnits.Metre
                };
                return units;
            }
        }
    }
}