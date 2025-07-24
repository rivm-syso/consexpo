using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class StorageTime : Duration
    {
        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<DurationUnits> AvailableUnits
        {
            get
            {
                var units = new List<DurationUnits>
                {
                    DurationUnits.Hour,
                    DurationUnits.Day,
                    DurationUnits.Week,
                    DurationUnits.Month,
                    DurationUnits.Year
                };
                return units;
            }
        }
    }
}