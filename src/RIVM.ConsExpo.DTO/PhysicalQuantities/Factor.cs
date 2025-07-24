using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class Factor : DistributablePhysicalQuantity<FactorUnits>
    {
        public Factor(string displayName, FactorUnits minMaxUnit, double min, double? max = null) : base(displayName, minMaxUnit, min, max)
        { }

        protected override double Standardized => ConvertedValue(FactorUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<FactorUnits> AllUnits => FactorUnits.AllUnits;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<FactorUnits> AvailableUnits => FactorUnits.AllUnits;

        public double InTimes()
        {
            return Standardized;
        }
    }
}