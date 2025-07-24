using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class ReleaseDurationTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void ReleaseDurationCompleteUnitSupportTest()
        {
            var x = new ReleaseDuration();
            foreach (var unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InSeconds();
            }
        }

        [TestMethod]
        public void ReleaseDurationConversionTest()
        {
            ReleaseDuration rl1 = new ReleaseDuration() { Value = 60.0, Unit = DurationUnits.Second };

            double? rl2 = rl1.InMinutes();

            Assert.AreEqual(1.0, rl2.Value);
        }
    }
}