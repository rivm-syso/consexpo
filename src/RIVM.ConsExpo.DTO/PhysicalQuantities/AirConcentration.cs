using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class AirConcentration : Density
    {
        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<DensityUnits> AllUnits => DensityUnits.AllUnits;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<DensityUnits> AvailableUnits
        {
            get
            {
                var units = new List<DensityUnits>
                {
                    DensityUnits.MilligramPerCubicMetre
                };
                return units;
            }
        }

        public AirConcentration AsMilligramPerCubicMetre()
        {
            return new AirConcentration
            {
                Value = InMilligramPerCubicMetre(),
                Unit = DensityUnits.MilligramPerCubicMetre
            };
        }

        public static AirConcentration NewFromGramPerCubicMetre(double value)
        {
            var airConcentration = new AirConcentration
            {
                Unit = DensityUnits.StandardUnit,
                Value = ConversionFactors.One2Milli * value
            };

            return airConcentration;
        }
    }
}