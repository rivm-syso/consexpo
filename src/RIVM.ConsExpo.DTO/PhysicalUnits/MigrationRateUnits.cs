using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// The units of a migration rate.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class MigrationRateUnits : UnitBase
    {
        public static readonly MigrationRateUnits GramPerSquareCentimetrePerSecond = new MigrationRateUnits(1, "g/cm²/s", 1, 1);

        public static readonly MigrationRateUnits GramPerSquareCentimetrePerMinute = new MigrationRateUnits(2, "g/cm²/min", 2, 1.0 / ConversionFactors.SecondsPerMinute);

        public static readonly MigrationRateUnits StandardUnit = MigrationRateUnits.GramPerSquareCentimetrePerSecond;

        [NotMapped]
        [XmlIgnore]
        public static IList<MigrationRateUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<MigrationRateUnits>(new[]
                 {
                     GramPerSquareCentimetrePerSecond,
                     GramPerSquareCentimetrePerMinute
                 });
            }
        }

        protected MigrationRateUnits()
        { }

        protected MigrationRateUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}