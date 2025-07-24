using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// The units of volume.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class VolumeUnits : UnitBase
    {
        public static readonly VolumeUnits CubicMetre = new VolumeUnits(1, "m³", 1, 1.0);

        public static readonly VolumeUnits Litre = new VolumeUnits(2, "l", 2, ConversionFactors.CubicMetresPerLitre);

        public static readonly VolumeUnits StandardUnit = VolumeUnits.CubicMetre;

        [NotMapped]
        [XmlIgnore]
        public static IList<VolumeUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<VolumeUnits>(new[]
                 {
                     CubicMetre,
                     Litre
                 });
            }
        }

        protected VolumeUnits()
        { }

        protected VolumeUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}