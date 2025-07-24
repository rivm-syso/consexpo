using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class AreaDensityTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void AreaDensityCompleteUnitSupportTest()
        {
            var x = new AreaDensity();
            foreach (AreaDensityUnits unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InMilligramPerSquareMetre();
            }
        }

        [TestMethod]
        [TestCategory("Unit conversion")]
        public void AreaDensityUnitConversionTest()
        {
            //All instances are 1 gram per square metre, expressed in different units.
            var x = new AreaDensity()
            {
                Value = 1,
                Unit = AreaDensityUnits.GramPerSquareMetre
            };

            var y = new AreaDensity()
            {
                Value = 1E3,
                Unit = AreaDensityUnits.MilligramPerSquareMetre
            };

            var z = new AreaDensity()
            {
                Value = 1E6,
                Unit = AreaDensityUnits.MicrogramPerSquareMetre
            };

            TestHelpers.AreEqual(x.InMilligramPerSquareMetre(), y.InMilligramPerSquareMetre());
            TestHelpers.AreEqual(x.InMilligramPerSquareMetre(), z.InMilligramPerSquareMetre());
        }
    }
}