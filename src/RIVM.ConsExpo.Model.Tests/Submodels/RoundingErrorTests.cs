using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class RoundingErrors
    {
        /// <summary>
        /// Test the effect on the air concentration for small ventilation rates. The air concentration is calculated using the correct formula, which behaves badly for low ventilation rates and for the zeroth and first order Taylor expansions, which are inaccurate for larger ventilation rates.
        ///
        /// The results show that zeroth order Taylor expansion and the correct formula differ the least for about 10^-12.
        /// For higher values, the formulas differ increasingly.
        /// For lower values, rounding errors in the correct formula start to dominate.
        /// </summary>
        /// <remarks>This is not a true unit test, but a unit that writes calculation result to Debug output.</remarks>
        [TestMethod]
        public void SmallqTest()
        {
            var t_R = new ReleaseDuration() { Value = 1, Unit = DurationUnits.Hour }.InSeconds();
            var P = new ProductAmount() { Value = 50, Unit = MassUnits.Gram }.InMilligram();
            var f = new Fraction() { Value = 0.1, Unit = FractionUnits.Fraction }.AsFraction();
            var V = new RoomVolume() { Value = 58, Unit = VolumeUnits.CubicMetre }.InCubicMetres();

            var E = P * f / t_R;                                                            // (A.1)

            Debug.WriteLine("Ventilation rate	True exponential formula	Zeroth order Taylor expansion	First order Taylor expansion");

            for (double exp = 0; exp >= -20; exp = exp - 0.25)
            {
                var q = Math.Pow(10, exp);
                Debug.WriteLine("{0}\t{1}\t{2}\t{3}", q, (E / (q * V)) * (1 - Math.Exp(-(q * t_R))), E * t_R / V, E * t_R / V - E * t_R * t_R * q / (2 * V));
            }
        }
    }
}
