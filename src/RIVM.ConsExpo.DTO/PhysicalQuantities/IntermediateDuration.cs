using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class IntermediateDuration : Duration
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
                    DurationUnits.Day
                };
                return units;
            }
        }

        protected override double? MaxForDefaultUnit => 1.0 * ConversionFactors.MinutesPerYear;
    }
}