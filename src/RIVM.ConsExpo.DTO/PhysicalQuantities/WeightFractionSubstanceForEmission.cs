using System.Collections.Generic;
using RIVM.ConsExpo.DTO.PhysicalUnits;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public sealed class WeightFractionSubstanceForEmission : FractionBase
    {
        public WeightFractionSubstanceForEmission()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeightFractionSubstanceForEmission"/> class.
        /// </summary>
        /// <param name="fraction">The fraction.</param>
        /// <remarks>Needed to copy a fraction into a WeightFractionSubstanceForEmission</remarks>
        public WeightFractionSubstanceForEmission(FractionBase fraction)
        {
            Value = fraction.Value;
            UnitCode = fraction.UnitCode;
            Distribution = fraction.Distribution;
        }

        protected override double MinForDefaultUnit => 1E-06;
    }
}