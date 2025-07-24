using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// The pseudo-units of a fraction. Fractions are actually dimensionless, but can still be expressed as fraction or as percentage.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class FractionUnits : UnitBase
    {
        public static readonly FractionUnits Fraction = new FractionUnits(1, "(fraction)", 1, 1);

        public static readonly FractionUnits Percentage = new FractionUnits(2, "%", 2, ConversionFactors.PercentageToFraction);

        public static readonly FractionUnits StandardUnit = FractionUnits.Fraction;

        [NotMapped]
        [XmlIgnore]
        public static IList<FractionUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<FractionUnits>(new[]
                 {
                     Fraction,
                     Percentage
                 });
            }
        }

        protected FractionUnits()
        { }

        protected FractionUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}