using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// Some quantities do not have a dimensions, but can still be expressed in different units, like a linear value and a logarithmic value.
    /// </summary>
    public class Dimensionless : UnitBase
    {
        public static readonly Dimensionless Linear = new Dimensionless(1, "linear", 1);

        public static readonly Dimensionless Log10 = new Dimensionless(2, "10Log", 2);

        public static readonly Dimensionless StandardUnit = Linear;

        [NotMapped]
        [XmlIgnore]
        public static IList<Dimensionless> AllUnits
        {
            get
            {
                return new ReadOnlyCollection<Dimensionless>(new[]
                 {
                     Linear,
                     Log10
                 });
            }
        }

        protected Dimensionless()
        { }

        protected Dimensionless(int code, string displayName, int order)
            : base(code, displayName, order, 0)
        { }
    }
}