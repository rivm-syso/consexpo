using RIVM.ConsExpo.DTO.PhysicalUnits;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// The pressure of a the gas phase of a substance in equilibrium with a condensed or solid phase below.
    /// This depends on the substance and the temperature.
    /// </summary>
    /// <seealso href="https://en.wikipedia.org/wiki/Vapor_pressure"/>
    public class Pressure : DistributablePhysicalQuantity<PressureUnits>
    {
        public static double Pascal2GramPerMetrePerMinuteSquared = ConversionFactors.Kilo2One / Math.Pow(ConversionFactors.MinutesPerSecond, 2);

        protected override double Standardized => ConvertedValue(PressureUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<PressureUnits> AllUnits => PressureUnits.AllUnits;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<PressureUnits> AvailableUnits => PressureUnits.AllUnits;

        public double InPascal()
        {
            return Standardized;
        }

        public double InGramPerMetrePerMinuteSquared()
        {
            return Standardized * Pascal2GramPerMetrePerMinuteSquared;
        }
    }
}