using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// Product amount is introduced as some submodel parameters are specified with other units:
    /// See dermal exposure migration and dermal exposure instant application.
    /// </summary>
    public class ProductAmount : Mass
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
                    MassUnits.Milligram
                };
                return units;
            }
        }
    }
}