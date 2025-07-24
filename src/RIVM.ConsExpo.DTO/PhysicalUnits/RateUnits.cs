using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// The units of a rate.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class RateUnits : UnitBase
    {
        public static readonly RateUnits TimesPerHour = new RateUnits(1, "per hour", 1, 1.0);

        public static readonly RateUnits TimesPerDay = new RateUnits(2, "per day", 2, 1.0 / ConversionFactors.HoursPerDay);

        public static readonly RateUnits StandardUnit = RateUnits.TimesPerHour;

        [NotMapped]
        [XmlIgnore]
        public static IList<RateUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<RateUnits>(new[]
                 {
                     TimesPerHour,
                     TimesPerDay
                 });
            }
        }

        protected RateUnits()
        { }

        protected RateUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}