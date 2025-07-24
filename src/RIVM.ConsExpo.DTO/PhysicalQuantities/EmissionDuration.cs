using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class EmissionDuration : Duration
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
                    if (unit != DurationUnits.Second)
                    {
                        availableUnits.Add(unit);
                    }
                }
                return availableUnits;
            }
        }

        [NotMapped]
        [XmlIgnore]
        public IEnumerable<UnitBase> AvailableUnits3
        {
            get
            {
                var availableUnits = new List<DurationUnits>();
                foreach (var unit in DurationUnits.AllUnits)
                {
                    if (unit != DurationUnits.Second)
                    {
                        availableUnits.Add(unit);
                    }
                }
                return availableUnits;
            }
        }

        [NotMapped]
        [XmlIgnore]
        public IList<UnitBase> AvailableUnits2
        {
            get
            {
                var availableUnits = new List<UnitBase>();
                foreach (var unit in DurationUnits.AllUnits)
                {
                    if (unit != DurationUnits.Second)
                    {
                        availableUnits.Add(unit);
                    }
                }
                return availableUnits;
            }
        }
    }
}