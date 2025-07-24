using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// Units of frequency, specified as 1 / duration.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class FrequencyUnits : UnitBase
    {
        public static readonly FrequencyUnits Daily = new FrequencyUnits(1, "per day", 1, 1);

        public static readonly FrequencyUnits Weekly = new FrequencyUnits(2, "per week", 2, 1.0 / ConversionFactors.DaysPerWeek);

        public static readonly FrequencyUnits Monthly = new FrequencyUnits(3, "per month", 3, 1.0 / ConversionFactors.DaysPerMonth);

        public static readonly FrequencyUnits Yearly = new FrequencyUnits(4, "per year", 4, 1.0 / ConversionFactors.DaysPerYear);

        public static readonly FrequencyUnits StandardUnit = FrequencyUnits.Daily;

        [NotMapped]
        [XmlIgnore]
        public static IList<FrequencyUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<FrequencyUnits>(new[]
                 {
                     Daily,
                     Weekly,
                     Monthly,
                     Yearly
                 });
            }
        }

        protected FrequencyUnits()
        { }

        protected FrequencyUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}