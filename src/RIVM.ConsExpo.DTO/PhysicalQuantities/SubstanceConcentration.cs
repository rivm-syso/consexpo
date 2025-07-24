using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class SubstanceConcentration : Density
    {
        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<DensityUnits> AvailableUnits
        {
            get
            {
                var units = new List<DensityUnits>
                {
                    DensityUnits.GramPerCubicCentimetre,
                    DensityUnits.MilligramPerCubicCentimetre,
                    DensityUnits.KilogramPerLitre,
                    DensityUnits.GramPerCubicMetre,
                    DensityUnits.KilogramPerCubicMetre
                };
                return units;
            }
        }
    }
}