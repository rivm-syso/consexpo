using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class DensityTests
    {
        [TestMethod]
        public void MassConcentrationUnitConversionTest()
        {
            //All instances are 1 GramPerCubicMetre, expressed in different units.
            Density x = new SubstanceConcentration()
            {
                Value = 1,
                Unit = DensityUnits.GramPerCubicMetre
            };
            Density y = new SubstanceConcentration()
            {
                Value = 1E-3,
                Unit = DensityUnits.MilligramPerCubicCentimetre
            };

            Density z = new AirConcentration()
            {
                Value = 1E3,
                Unit = DensityUnits.MilligramPerCubicMetre
            };
            Density t = new SubstanceConcentrationPackaging()
            {
                Value = 1E-3,
                Unit = DensityUnits.GramPerLitre
            };
            Density u = new SubstanceConcentrationPackaging()
            {
                Value = 1E-6,
                Unit = DensityUnits.GramPerCubicCentimetre
            };
            Density v = new SubstanceConcentrationPackaging()
            {
                Value = 1E-6,
                Unit = DensityUnits.KilogramPerLitre
            };
            Density w = new DensitySolid()
            {
                Value = 1E-3,
                Unit = DensityUnits.KilogramPerCubicMetre
            };

            TestHelpers.AreEqual(x.InMilligramPerCubicCentimetre(), y.InMilligramPerCubicCentimetre());
            TestHelpers.AreEqual(x.InMilligramPerCubicCentimetre(), z.InMilligramPerCubicCentimetre());
            TestHelpers.AreEqual(x.InMilligramPerCubicCentimetre(), t.InMilligramPerCubicCentimetre());
            TestHelpers.AreEqual(x.InMilligramPerCubicCentimetre(), u.InMilligramPerCubicCentimetre());
            TestHelpers.AreEqual(x.InMilligramPerCubicCentimetre(), v.InMilligramPerCubicCentimetre());
            TestHelpers.AreEqual(x.InMilligramPerCubicCentimetre(), w.InMilligramPerCubicCentimetre());
        }

        [TestMethod]
        public void DensityInMilligramPerCubicCentimetreTest()
        {
            Density x = new SubstanceConcentration()
            {
                Value = 1,
                Unit = DensityUnits.GramPerCubicMetre
            };

            var y = x.InMilligramPerCubicCentimetre();

            Assert.AreEqual(y, 1000.0 / (100.0 * 100.0 * 100.0));
        }

        [TestMethod]
        public void DensityInMilligramPerCubicMetreTest()
        {
            Density x = new AirConcentration()
            {
                Value = 1,
                Unit = DensityUnits.MilligramPerCubicMetre
            };

            TestHelpers.AreEqual(1, x.InMilligramPerCubicMetre());
        }

        [TestMethod]
        public void DensityInGramPerCubicMetreTest()
        {
            Density x = new SubstanceConcentration()
            {
                Value = 1,
                Unit = DensityUnits.GramPerCubicMetre
            };

            TestHelpers.AreEqual(1, x.InGramPerCubicMetre());
        }

        [TestMethod]
        public void DensityInGramPerCubicCentimetreTest()
        {
            Density x = new SubstanceConcentration()
            {
                Value = 1,
                Unit = DensityUnits.GramPerCubicCentimetre
            };

            TestHelpers.AreEqual(1, x.InGramPerCubicCentimetre());
        }
    }
}