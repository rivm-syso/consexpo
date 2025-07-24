using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class VolumeRateTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void VolumeRateCompleteUnitSupportTest()
        {
            var x = new VolumeRate();
            foreach (var unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InCubicMetresPerSecond();
            }
        }

        [TestMethod]
        public void VolumeRateUnitConversionTest()
        {
            //All instances are 1000 LiterPerMinute, expressed in different units.
            var x = new VolumeRate()
            {
                Value = 1000,
                Unit = VolumeRateUnits.LiterPerMinute
            };

            var y = new VolumeRate()
            {
                Value = 60,
                Unit = VolumeRateUnits.CubicMetrePerHour
            };

            var z = new VolumeRate()
            {
                Value = 24 * 60,
                Unit = VolumeRateUnits.CubicMetrePerDay
            };

            TestHelpers.AreEqualDoubles(x.InCubicMetresPerSecond(), y.InCubicMetresPerSecond());
            TestHelpers.AreEqualDoubles(x.InCubicMetresPerSecond(), z.InCubicMetresPerSecond());
        }

        [TestMethod]
        public void VolumeRateInCubicMetresPerSecondTest()
        {
            var x = new VolumeRate()
            {
                Value = 3600,
                Unit = VolumeRateUnits.CubicMetrePerHour
            };

            TestHelpers.AreEqual(1, x.InCubicMetresPerSecond());
        }

        [TestMethod]
        public void VolumeRateInCubicMetresPerHourTest()
        {
            var x = new VolumeRate()
            {
                Value = 1,
                Unit = VolumeRateUnits.CubicMetrePerHour
            };

            TestHelpers.AreEqual(1, x.InCubicMetresPerHour());
        }
    }
}