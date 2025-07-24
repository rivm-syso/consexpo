using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class MigrationRateTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void MigrationRateCompleteUnitSupportTest()
        {
            var x = new MigrationRate();
            foreach (MigrationRateUnits unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InGramPerSquareCentimetrePerSecond();
            }
        }

        [TestMethod]
        public void MigrationRateUnitConversionTest()
        {
            //All instances are 1 GramPerSquareCentimetrePerSecond, expressed in different units.
            var x = new MigrationRate()
            {
                Value = 1,
                Unit = MigrationRateUnits.GramPerSquareCentimetrePerSecond
            };

            var y = new MigrationRate()
            {
                Value = 60,
                Unit = MigrationRateUnits.GramPerSquareCentimetrePerMinute
            };

            TestHelpers.AreEqual(x.InGramPerSquareCentimetrePerSecond(), y.InGramPerSquareCentimetrePerSecond());
        }

        [TestMethod]
        public void MigrationRateInMilliGramPerSquareCentimetresPerSecondTest()
        {
            var x = new MigrationRate()
            {
                Value = 1,
                Unit = MigrationRateUnits.GramPerSquareCentimetrePerSecond
            };

            TestHelpers.AreEqual(1000.0, x.InMilliGramPerSquareCentimetresPerSecond());
        }
    }
}