using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// The amount of product ingested, measured as mass.
    /// </summary>
    public class ProductAmountPackaging : Mass
    {
        public static readonly MassUnits StandardUnit = MassUnits.Milligram;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<MassUnits> AvailableUnits
        {
            get
            {
                var units = new List<MassUnits>
                {
                    MassUnits.Gram,
                    MassUnits.Microgram,
                    MassUnits.Milligram,
                    MassUnits.Kilogram
                };
                return units;
            }
        }
    }
}