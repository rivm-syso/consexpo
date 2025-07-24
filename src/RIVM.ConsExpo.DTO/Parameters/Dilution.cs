using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Parameters
{
    public class Dilution : Factor
    {
        private const string displayName = "Dilution";

        public Dilution() : base(displayName, FactorUnits.Times, 1.0, 1.0E12)
        { }

        /// <summary>
        /// The default value to be used for new scenarios.
        /// </summary>
        public const double DefaultValue = 1.0;
    }
}