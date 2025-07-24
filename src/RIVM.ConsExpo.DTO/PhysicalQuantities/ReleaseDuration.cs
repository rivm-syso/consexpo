using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class ReleaseDuration : Duration
    {
        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<DurationUnits> AvailableUnits
        {
            get
            {
                var units = new List<DurationUnits>
                {
                    DurationUnits.Minute,
                    DurationUnits.Second,
                    DurationUnits.Hour,
                    DurationUnits.Day
                };
                return units;
            }
        }
    }
}