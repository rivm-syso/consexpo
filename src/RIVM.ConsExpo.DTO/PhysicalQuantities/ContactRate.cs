using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class ContactRate : MassRate
    {
        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<MassRateUnits> AvailableUnits
        {
            get
            {
                var units = new List<MassRateUnits>
                {
                    MassRateUnits.MicrogramPerMinute,
                    MassRateUnits.MilligramPerMinute,
                    MassRateUnits.GramPerHour,
                    MassRateUnits.GramPerDay
                };
                return units;
            }
        }

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<MassRateUnits> AllUnits => MassRateUnits.AllUnits;
    }
}