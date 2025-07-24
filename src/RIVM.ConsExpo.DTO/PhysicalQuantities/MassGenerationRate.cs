using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class MassGenerationRate : MassRate
    {
        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<MassRateUnits> AvailableUnits
        {
            get
            {
                var units = new List<MassRateUnits>
                {
                    MassRateUnits.GramPerSecond,
                    MassRateUnits.GramPerMinute,
                    MassRateUnits.MilligramPerMinute
                };
                return units;
            }
        }
    }
}