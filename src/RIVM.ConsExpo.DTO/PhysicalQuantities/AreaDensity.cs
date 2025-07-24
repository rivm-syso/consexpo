using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// The concentration of mass in the air
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Area_density"/>
    public class AreaDensity : DistributablePhysicalQuantity<AreaDensityUnits>
    {
        protected override double Standardized => ConvertedValue(AreaDensityUnits.StandardUnit);

        /// <summary>
        /// Returns the value, expressed in mg/m².
        /// </summary>
        /// <returns></returns>
        public double InMilligramPerSquareMetre()
        {
            return ConvertedValue(AreaDensityUnits.MilligramPerSquareMetre);
        }

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<AreaDensityUnits> AvailableUnits => AreaDensityUnits.AllUnits;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<AreaDensityUnits> AllUnits => AreaDensityUnits.AllUnits;
    }
}