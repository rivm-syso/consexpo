using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// The units of a factor.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class FactorUnits : UnitBase
    {
        public static readonly FactorUnits Times = new FactorUnits(1, "times", 1, 1.0);

        public static readonly FactorUnits StandardUnit = FactorUnits.Times;

        [NotMapped]
        [XmlIgnore]
        public static IList<FactorUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<FactorUnits>(new[]
                 {
                     Times
                 });
            }
        }

        protected FactorUnits()
        { }

        protected FactorUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}