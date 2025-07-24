using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class DailyDurationTests
    {
        [TestMethod]
        public void DailyDurationUnitConversionTest()
        {
            //All instances are 1 day, expressed in different units.
            var x = new DailyDuration
            {
                Value = 60,
                Unit = DailyDurationUnits.MinutesPerDay
            };

            var y = new DailyDuration
            {
                Value = 1,
                Unit = DailyDurationUnits.HoursPerDay
            };

            TestHelpers.AreEqual(x.ConvertedValue(DailyDurationUnits.StandardUnit), y.ConvertedValue(DailyDurationUnits.StandardUnit));
        }

        [TestMethod]
        public void DailyDurationHoursPerDayInSecondsPerDayTest()
        {
            var x = new DailyDuration
            {
                Value = 1,
                Unit = DailyDurationUnits.HoursPerDay
            };

            TestHelpers.AreEqual(3600, x.InSecondsPerDay());
        }

        [TestMethod]
        public void DailyDurationMinutesPerDayInSecondsPerDayTest()
        {
            var x = new DailyDuration
            {
                Value = 1,
                Unit = DailyDurationUnits.MinutesPerDay
            };

            TestHelpers.AreEqual(60, x.InSecondsPerDay());
        }

        [TestMethod]
        public void DailyDurationMinutesPerDayAsTimePerDayTest()
        {
            var x = new DailyDuration
            {
                Value = 1,
                Unit = DailyDurationUnits.MinutesPerDay
            };

            TestHelpers.AreEqual(1, x.AsTimePerDay().Value);
            TestHelpers.AreEqual(TimeUnits.Minute.Code, x.AsTimePerDay().Unit.Code);
        }

        [TestMethod]
        public void DailyDurationHoursPerDayAsTimePerDayTest()
        {
            var x = new DailyDuration
            {
                Value = 1,
                Unit = DailyDurationUnits.HoursPerDay
            };

            TestHelpers.AreEqual(60, x.AsTimePerDay().Value);
            TestHelpers.AreEqual(TimeUnits.Minute.Code, x.AsTimePerDay().Unit.Code);
        }

        [TestMethod]
        public void HoursPerDayMaxTest()
        {
            var x = new DailyDuration
            {
                Unit = DailyDurationUnits.HoursPerDay
            };

            Assert.AreEqual<double?>(24, x.Max);
        }

        [TestMethod]
        public void MinutesPerDayMaxTest()
        {
            var x = new DailyDuration
            {
                Unit = DailyDurationUnits.MinutesPerDay
            };

            Assert.AreEqual<double?>(1440, x.Max);
        }
    }
}