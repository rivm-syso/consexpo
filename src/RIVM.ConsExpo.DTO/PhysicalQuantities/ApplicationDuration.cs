using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// The duration of the application of a product.
    /// </summary>
    /// <remarks>Although it has the same set of available units as exposure duration, this is another physical quantity, so it has its own type.</remarks>
    [DisplayName("Application duration")]
    public class ApplicationDuration : Duration
    {
        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<DurationUnits> AvailableUnits
        {
            get
            {
                var units = new List<DurationUnits>
                {
                    DurationUnits.Minute,
                    DurationUnits.Hour,
                    DurationUnits.Day
                };
                return units;
            }
        }

        protected override double MinForDefaultUnit => 1.0 * ConversionFactors.MinutesPerSecond;
    }
}