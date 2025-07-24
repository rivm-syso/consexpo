using RIVM.ConsExpo.DTO.PhysicalUnits;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// The concentration of mass in the air
    /// </summary>
    /// <see href="https://en.wikipedia.org/wiki/Concentration#Mass_concentration"/>
    public abstract class Density : DistributablePhysicalQuantity<DensityUnits>
    {
        protected override double Standardized => ConvertedValue(DensityUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<DensityUnits> AllUnits => DensityUnits.AllUnits;

        /// <summary>
        /// Gets the density in mg per cubic cm.
        /// </summary>
        /// <value>
        /// The in mg per cubic cm.
        /// </value>
        public double InMilligramPerCubicCentimetre()
        {
            return Standardized / Math.Pow(ConversionFactors.One2Centi, 3);
        }

        /// <summary>
        /// Gets the density in milligram per cubic metre.
        /// </summary>
        public double InMilligramPerCubicMetre()
        {
            return Standardized;
        }

        /// <summary>
        /// Gets the density in gram per cubic metre.
        /// </summary>
        public double InGramPerCubicMetre()
        {
            return Standardized * ConversionFactors.Milli2One;
        }

        /// <summary>
        /// Gets the density in gram per cubic centimetre.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public double InGramPerCubicCentimetre()
        {
            return Standardized * ConversionFactors.Milli2One / Math.Pow(ConversionFactors.One2Centi, 3);
        }
    }
}