using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class DurationAltTests
    {
        [TestMethod]
        public void DurationUnitConversionTest()
        {
            //All instances are 1 day, expressed in different units.
            Duration x = new EmissionDuration()
            {
                Value = 1,
                Unit = DurationUnits.Day
            };

            Duration y = new EmissionDuration()
            {
                Value = 24,
                Unit = DurationUnits.Hour
            };

            Duration z = new EmissionDuration()
            {
                Value = 24 * 60,
                Unit = DurationUnits.Minute
            };

            Duration s = new ReleaseDuration()
            {
                Value = 24 * 60 * 60,
                Unit = DurationUnits.Second
            };

            Duration u = new EmissionDuration()
            {
                Value = 1.0 / 7,
                Unit = DurationUnits.Week
            };

            Duration v = new EmissionDuration()
            {
                Value = 12.0 / 365,
                Unit = DurationUnits.Month
            };

            Duration w = new EmissionDuration()
            {
                Value = 1.0 / 365,
                Unit = DurationUnits.Year
            };

            TestHelpers.AreEqual(x.InSeconds(), y.InSeconds());
            TestHelpers.AreEqual(x.InSeconds(), z.InSeconds());
            TestHelpers.AreEqual(x.InSeconds(), u.InSeconds());
            TestHelpers.AreEqual(x.InSeconds(), v.InSeconds());
            TestHelpers.AreEqual(x.InSeconds(), w.InSeconds());
            TestHelpers.AreEqual(x.InSeconds(), s.InSeconds());
        }

        [TestMethod]
        public void DurationInSecondsTest()
        {
            Duration x = new ReleaseDuration()
            {
                Value = 1,
                Unit = DurationUnits.Second
            };

            TestHelpers.AreEqual(1, x.InSeconds());
        }

        [TestMethod]
        public void DurationInHoursTest()
        {
            Duration x = new EmissionDuration()
            {
                Value = 1,
                Unit = DurationUnits.Hour
            };

            TestHelpers.AreEqual(1, x.InHours());
        }

        [TestMethod]
        public void DurationInDaysTest()
        {
            Duration x = new EmissionDuration()
            {
                Value = 24,
                Unit = DurationUnits.Hour
            };

            TestHelpers.AreEqual(1, x.InDays());
        }

        [TestMethod]
        public void DurationInYearsTest()
        {
            Duration x = new IntermediateDuration()
            {
                Value = 365,
                Unit = DurationUnits.Day
            };

            TestHelpers.AreEqual(1, x.InYears());
        }
    }
}