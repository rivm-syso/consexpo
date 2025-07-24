using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class SubstanceConcentrationTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void SubstanceConcentrationCompleteUnitSupportTest()
        {
            var x = new SubstanceConcentration();
            foreach (DensityUnits unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InGramPerCubicMetre();
            }
        }

        [TestMethod]
        public void SubstanceConcentrationUnitConversionTest()
        {
            var t = new SubstanceConcentration()
            {
                Value = 1,
                Unit = DensityUnits.GramPerCubicCentimetre
            };

            var u = new SubstanceConcentration()
            {
                Value = 1,
                Unit = DensityUnits.GramPerCubicCentimetre
            };

            var v = new SubstanceConcentration()
            {
                Value = 1,
                Unit = DensityUnits.GramPerCubicCentimetre
            };

            var w = new SubstanceConcentration()
            {
                Value = 1,
                Unit = DensityUnits.GramPerCubicCentimetre
            };

            var x = new SubstanceConcentration()
            {
                Value = 1,
                Unit = DensityUnits.GramPerCubicCentimetre
            };

            var y = new SubstanceConcentration()
            {
                Value = 1,
                Unit = DensityUnits.GramPerCubicCentimetre
            };

            var z = new SubstanceConcentration()
            {
                Value = 1,
                Unit = DensityUnits.GramPerCubicCentimetre
            };

            TestHelpers.AreEqual(x.InGramPerCubicMetre(), t.InGramPerCubicMetre());
            TestHelpers.AreEqual(x.InGramPerCubicMetre(), u.InGramPerCubicMetre());
            TestHelpers.AreEqual(x.InGramPerCubicMetre(), v.InGramPerCubicMetre());
            TestHelpers.AreEqual(x.InGramPerCubicMetre(), w.InGramPerCubicMetre());
            TestHelpers.AreEqual(x.InGramPerCubicMetre(), y.InGramPerCubicMetre());
            TestHelpers.AreEqual(x.InGramPerCubicMetre(), z.InGramPerCubicMetre());
        }

        [TestMethod]
        public void SubstanceConcentrationInMilligramTest()
        {
            var x = new SubstanceConcentration()
            {
                Value = 1,
                Unit = DensityUnits.GramPerCubicMetre
            };

            TestHelpers.AreEqual(1000, x.InMilligramPerCubicMetre());
        }

        [TestMethod]
        public void SubstanceConcentrationInKilogramTest()
        {
            var x = new SubstanceConcentration()
            {
                Value = 1,
                Unit = DensityUnits.KilogramPerCubicMetre
            };

            TestHelpers.AreEqual(1000, x.InGramPerCubicMetre());
        }
    }
}