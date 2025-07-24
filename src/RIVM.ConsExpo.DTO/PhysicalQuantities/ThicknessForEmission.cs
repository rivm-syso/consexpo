using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class ThicknessForEmission : Length
    {
        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<LengthUnits> AvailableUnits
        {
            get
            {
                var units = new List<LengthUnits>
                {
                    LengthUnits.Millimetre,
                    LengthUnits.Micrometre,
                    LengthUnits.Centimetre
                };
                return units;
            }
        }

        protected override double MinForDefaultUnit => 1.0;
    }
}