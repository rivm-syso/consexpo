using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class FrequencyTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void FrequencyCompleteUnitSupportTest()
        {
            var x = new Frequency();
            foreach (FrequencyUnits unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InTimesPerDay();
            }
        }

        [TestMethod]
        public void FrequencyUnitConversionTest()
        {
            //All instances are 1 day, expressed in different units.
            var x = new Frequency()
            {
                Value = 1,
                Unit = FrequencyUnits.Daily
            };

            var y = new Frequency()
            {
                Value = 7,
                Unit = FrequencyUnits.Weekly
            };

            var z = new Frequency()
            {
                Value = 365.0 / 12,
                Unit = FrequencyUnits.Monthly
            };

            var u = new Frequency()
            {
                Value = 365,
                Unit = FrequencyUnits.Yearly
            };

            TestHelpers.AreEqual(x.InTimesPerDay(), y.InTimesPerDay());
            TestHelpers.AreEqual(x.InTimesPerDay(), z.InTimesPerDay());
            TestHelpers.AreEqual(x.InTimesPerDay(), u.InTimesPerDay());
        }

        [TestMethod]
        public void FreqencyUnitInTimePerYearTest()
        {
            //All instances are 1 day, expressed in different units.
            var x = new Frequency()
            {
                Value = 1,
                Unit = FrequencyUnits.Daily
            };

            var y = new Frequency()
            {
                Value = 7,
                Unit = FrequencyUnits.Weekly
            };

            var z = new Frequency()
            {
                Value = 365.0 / 12,
                Unit = FrequencyUnits.Monthly
            };

            var u = new Frequency()
            {
                Value = 365,
                Unit = FrequencyUnits.Yearly
            };

            TestHelpers.AreEqual(u.InTimesPerYear(), x.InTimesPerYear());
            TestHelpers.AreEqual(u.InTimesPerYear(), y.InTimesPerYear());
            TestHelpers.AreEqual(u.InTimesPerYear(), z.InTimesPerYear());
        }
    }
}