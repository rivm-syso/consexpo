using DataAnnotationsExtensions;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// The weight of the quantity of one mol of the pure substance.
    /// </summary>
    [ComplexType]
    public class BodyWeight : Mass
    {
        [Min(0.1)]
        public override double? Value
        {
            get => base.Value;
            set => base.Value = value;
        }

        /// <summary>
        /// Gets the available units for this physical quantity.
        /// </summary>
        /// <value>
        /// The available units.
        /// </value>
        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<MassUnits> AvailableUnits
        {
            get
            {
                var units = new List<MassUnits> { MassUnits.Kilogram };
                return units;
            }
        }

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<MassUnits> AllUnits => MassUnits.AllUnits;

        /// <remarks>
        /// Default unit for mass is mg.
        /// </remarks>
        protected override double MinForDefaultUnit => 1.0 * ConversionFactors.Kilo2Milli;

        /// <summary>
        /// Gets the body weight in kilograms, if it was specified. Body weight is an optional parameter.
        /// </summary>
        public double? InKilogramIfSpecified()
        {
            double? internalValue = DerivedValue;

            if (internalValue.HasValue)
            {
                return InKilogram();
            }

            return null;
        }
    }
}