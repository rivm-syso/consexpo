using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class ExposedArea : Area
    {
        protected override double MinForDefaultUnit => 1.0;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<AreaUnits> AvailableUnits
        {
            get
            {
                var units = new List<AreaUnits>
                {
                    AreaUnits.SquareCentimetre,
                    AreaUnits.SquareDecimetre,
                    AreaUnits.SquareMetre
                };
                return units;
            }
        }

        /// <summary>
        /// Gets the exposed area in square centimetre, if it was specified. Exposed area is an optional parameter.
        /// </summary>
        public double? InSquareCentimetreIfSpecified
        {
            get
            {
                double? internalValue = DerivedValue;

                if (internalValue.HasValue)
                {
                    return InSquareCentimetre();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}