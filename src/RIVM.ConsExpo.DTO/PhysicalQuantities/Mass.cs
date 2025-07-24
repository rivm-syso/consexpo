using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using RIVM.ConsExpo.DTO.PhysicalUnits;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// The weight of a quantity of some substance.
    /// </summary>
    public abstract class Mass : DistributablePhysicalQuantity<MassUnits>
    {
        protected override double Standardized => ConvertedValue(MassUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<MassUnits> AllUnits => MassUnits.AllUnits;

        /// <summary>
        /// Gets the value in milligram.
        /// </summary>
        public double InMilligram()
        {
            return Standardized;
        }

        /// <summary>
        /// Gets the value in gram.
        /// </summary>
        public double InGram()
        {
            return Standardized * ConversionFactors.Milli2One;
        }

        /// <summary>
        /// Gets the value in kilogram.
        /// </summary>
        public double InKilogram()
        {
            return Standardized * ConversionFactors.Milli2Kilo;
        }
    }
}