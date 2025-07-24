using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class PressureTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void PressureCompleteUnitSupportTest()
        {
            var x = new Pressure();
            foreach (var unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InPascal();
            }
        }

        [TestMethod]
        public void PressureUnitConversionTest()
        {
            //All instances are 1 Bar, expressed in different units. https://en.wikipedia.org/wiki/Bar_%28unit%29#Usage
            var x = new Pressure()
            {
                Value = 1E5,
                Unit = PressureUnits.Pascal
            };

            var y = new Pressure()
            {
                Value = 1,
                Unit = PressureUnits.Bar
            };

            var z = new Pressure()
            {
                Value = 1000,
                Unit = PressureUnits.Millibar
            };

            var u = new Pressure()
            {
                Value = 750.0616,
                Unit = PressureUnits.MmHg
            };

            var v = new Pressure()
            {
                Value = 0.986923,
                Unit = PressureUnits.Atmosphere
            };

            TestHelpers.AreEqual(x.InPascal(), y.InPascal());
            TestHelpers.AreEqual(x.InPascal(), z.InPascal());
            TestHelpers.AreEqual(x.InPascal(), u.InPascal(), 1e-6);
            TestHelpers.AreEqual(x.InPascal(), v.InPascal(), 1e-6);
        }

        [TestMethod]
        public void InGramPerMetrePerMinuteSquaredTest()
        {
            var x = new Pressure()
            {
                Value = 1,
                Unit = PressureUnits.Pascal
            };

            //Test based on ConsExpo 4 conversion: kilogram -> gram and second -> minute.
            TestHelpers.AreEqual(x.InPascal() * 3600 * 1000, x.InGramPerMetrePerMinuteSquared());
        }
    }
}