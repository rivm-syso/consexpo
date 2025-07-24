using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// Units of molecular weight of substances.
    /// </summary>
    /// <remarks>Never change the numerical values of this enumeration, as the values are included in entities and the numerical values are stored in the database.</remarks>
    public class MolecularWeightUnits : UnitBase
    {
        public static readonly MolecularWeightUnits GramPerMol = new MolecularWeightUnits(1, "g/mol", 1, 1.0);

        public static readonly MolecularWeightUnits MilliGramPerMol = new MolecularWeightUnits(2, "mg/mol", 2, ConversionFactors.Milli2One);

        public static readonly MolecularWeightUnits StandardUnit = MolecularWeightUnits.GramPerMol;

        [NotMapped]
        [XmlIgnore]
        public static IList<MolecularWeightUnits> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<MolecularWeightUnits>(new[]
                 {
                     GramPerMol,
                     MilliGramPerMol
                 });
            }
        }

        protected MolecularWeightUnits()
        { }

        protected MolecularWeightUnits(int code, string displayName, int order, double conversionFactor)
            : base(code, displayName, order, conversionFactor)
        { }
    }
}