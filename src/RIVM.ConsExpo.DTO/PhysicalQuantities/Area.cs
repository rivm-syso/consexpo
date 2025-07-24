using RIVM.ConsExpo.DTO.PhysicalUnits;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// A room Surface.
    /// </summary>
    public abstract class Area : DistributablePhysicalQuantity<AreaUnits>
    {
        protected override double Standardized => ConvertedValue(AreaUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<AreaUnits> AllUnits => AreaUnits.AllUnits;

        public double InSquareCentimetre()
        {
            return Standardized;
        }

        public double InSquareMetre()
        {
            return Standardized * Math.Pow(ConversionFactors.Centi2One, 2);
        }
    }
}