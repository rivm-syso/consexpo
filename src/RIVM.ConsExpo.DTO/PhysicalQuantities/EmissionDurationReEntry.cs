using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class EmissionDurationReEntry : Duration
    {
        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<DurationUnits> AvailableUnits
        {
            get
            {
                var availableUnits = new List<DurationUnits>();
                foreach (var unit in DurationUnits.AllUnits)
                {
                    if (unit == DurationUnits.Day)
                    {
                        availableUnits.Add(unit);
                    }
                    else if (unit == DurationUnits.Week)
                    {
                        availableUnits.Add(unit);
                    }
                }
                return availableUnits;
            }
        }
    }
}