using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// The units in which temperature may be expressed.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class TemperatureUnits : UnitBase
    {
        public static readonly TemperatureUnits Celsius = new TemperatureUnits(1, "°C", 1, ConversionFactors.CelsiusOffset);

        public static readonly TemperatureUnits Kelvin = new TemperatureUnits(2, "K", 2, 0);

        public static readonly TemperatureUnits StandardUnit = TemperatureUnits.Kelvin;

        [NotMapped]
        [XmlIgnore]
        public static IList<TemperatureUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<TemperatureUnits>(new[]
                 {
                     Celsius,
                     Kelvin
                 });
            }
        }

        protected const double MinTemperatureInCelsius = -100;

        protected double offset;

        public double Offset => offset;

        protected TemperatureUnits()
        { }

        protected TemperatureUnits(int code, string displayName, int order, double offset)

            : base(code, displayName, order, 0)
        {
            this.offset = offset;
        }

#warning: ConversionFactors do not apply to these units, since Kelvin and Celsius do not differ by a factor, but by an offset.
    }
}