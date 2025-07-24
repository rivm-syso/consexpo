#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// Physical constants used in model calculations.
    /// </summary>
    public static class PhysicalConstants
    {
        /// <summary>
        /// The gas constant in Pa*m3/mol/K.
        /// </summary>
        public const double GasConstant = 8.3144621;

        /// <summary>
        /// Gravity constant in m/s2
        /// </summary>
        public const double GravityConstant = 9.8;

        /// <summary>
        /// Dynamic viscosity of air in g/m/s
        /// </summary>
        public const double DynamicViscosityOfAir = 2.0e-2;
    }
}