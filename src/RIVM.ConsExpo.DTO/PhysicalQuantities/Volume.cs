using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using RIVM.ConsExpo.DTO.PhysicalUnits;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// A volume.
    /// </summary>
    public abstract class Volume : DistributablePhysicalQuantity<VolumeUnits>
    {
        protected override double Standardized => ConvertedValue(VolumeUnits.StandardUnit);

        public double InCubicMetres()
        {
            return Standardized;
        }

        public double InLitres()
        {
            return Standardized * ConversionFactors.LitresPerCubicMetre;
        }

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<VolumeUnits> AllUnits => VolumeUnits.AllUnits;
    }
}