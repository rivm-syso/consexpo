using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// The weight of the quantity of one mol of substance.
    /// </summary>
    public class MolecularWeight : PhysicalQuantity<MolecularWeightUnits>
    {
        protected override double Standardized => ConvertedValue(MolecularWeightUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<MolecularWeightUnits> AvailableUnits => MolecularWeightUnits.AllUnits;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<MolecularWeightUnits> AllUnits => MolecularWeightUnits.AllUnits;

        public double InGramPerMol()
        {
            return Standardized;
        }

        public double InMgPerMol()
        {
            return Standardized * ConversionFactors.One2Milli;
        }
    }
}