using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class RateTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void RateCompleteUnitSupportTest()
        {
            var x = new Rate();
            foreach (var unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InTimesPerHour();
            }
        }

        [TestMethod]
        public void RateUnitConversionTest()
        {
            //All instances are 1 TimesPerHour, expressed in different units.
            var x = new Rate()
            {
                Value = 1,
                Unit = RateUnits.TimesPerHour
            };

            var y = new Rate()
            {
                Value = 24,
                Unit = RateUnits.TimesPerDay
            };

            TestHelpers.AreEqual(x.InTimesPerHour(), y.InTimesPerHour());
        }

        [TestMethod]
        public void MassInTimesPerSecondTest()
        {
            var x = new Rate()
            {
                Value = 3600,
                Unit = RateUnits.TimesPerHour
            };

            TestHelpers.AreEqual(1, x.InTimesPerSecond());
        }

        [TestMethod]
        public void MassInTimesPerMinuteTest()
        {
            var x = new Rate()
            {
                Value = 60,
                Unit = RateUnits.TimesPerHour
            };

            TestHelpers.AreEqual(1, x.InTimesPerMinute());
        }

        [TestMethod]
        public void MassInTimesPerHourTest()
        {
            var x = new Rate()
            {
                Value = 24,
                Unit = RateUnits.TimesPerDay
            };

            TestHelpers.AreEqual(1, x.InTimesPerHour());
        }
    }
}