using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class TimeTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void TimeCompleteUnitSupportTest()
        {
            var x = new Time();
            foreach (var unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InSeconds();
                x.ConvertedTo(TimeUnits.Second);
            }
        }

        [TestMethod]
        public void ConvertedToTestWeek2Hour()
        {
            var ThreeWeeksInWeeks = new Time(3, TimeUnits.Week);

            var ThreeWeeksInHours = ThreeWeeksInWeeks.ConvertedTo(TimeUnits.Hour);

            Assert.AreEqual<double>(3 * 7 * 24, ThreeWeeksInHours.Value.Value);
        }

        [TestMethod]
        public void TimeUnitConversionTest()
        {
            //All instances are 1 day, expressed in different units.
            var v = new Time()
            {
                Value = 1.0 / 365,
                Unit = TimeUnits.Year
            };

            var w = new Time()
            {
                Value = 12 / 365,
                Unit = TimeUnits.Month
            };

            var t = new Time()
            {
                Value = 7,
                Unit = TimeUnits.Week
            };

            var x = new Time()
            {
                Value = 1,
                Unit = TimeUnits.Day
            };

            var y = new Time()
            {
                Value = 24,
                Unit = TimeUnits.Hour
            };

            var z = new Time()
            {
                Value = 24 * 60,
                Unit = TimeUnits.Minute
            };

            var u = new Time()
            {
                Value = 24 * 60 * 60,
                Unit = TimeUnits.Second
            };

            TestHelpers.AreEqual(x.InSeconds(), y.InSeconds());
            TestHelpers.AreEqual(x.InSeconds(), z.InSeconds());
            TestHelpers.AreEqual(x.InSeconds(), u.InSeconds());
            TestHelpers.AreEqual(x.InSeconds(), v.InSeconds());
        }

        [TestMethod]
        public void TimeInSecondsTest()
        {
            var x = new Time()
            {
                Value = 1,
                Unit = TimeUnits.Second
            };

            TestHelpers.AreEqual(1, x.InSeconds());
        }

        [TestMethod]
        public void TimeInHoursTest()
        {
            var x = new Time()
            {
                Value = 3600,
                Unit = TimeUnits.Second
            };

            TestHelpers.AreEqual(1, x.InHours());
        }

        [TestMethod]
        public void TimeAddTest1()
        {
            var x = new Time() { Value = 30, Unit = TimeUnits.Minute };
            var y = new Time() { Value = 2, Unit = TimeUnits.Hour };

            var z = x.Add(y);

            Assert.AreEqual<double>(150, z.Value.Value);
            Assert.AreEqual<TimeUnits>(x.Unit, z.Unit);
        }

        [TestMethod]
        public void TimeAddTest2()
        {
            var x = new Time() { Value = 2, Unit = TimeUnits.Hour };
            var y = new Time() { Value = 30, Unit = TimeUnits.Minute };

            var z = x.Add(y);

            Assert.AreEqual<double>(2.5, z.Value.Value);
            Assert.AreEqual<TimeUnits>(x.Unit, z.Unit);
        }

        [TestMethod]
        public void TimeSubtractTest1()
        {
            var x = new Time() { Value = 2, Unit = TimeUnits.Hour };
            var y = new Time() { Value = 30, Unit = TimeUnits.Minute };

            var z = x.Subtract(y);

            Assert.AreEqual<double>(1.5, z.Value.Value);
            Assert.AreEqual<TimeUnits>(x.Unit, z.Unit);
        }
    }
}