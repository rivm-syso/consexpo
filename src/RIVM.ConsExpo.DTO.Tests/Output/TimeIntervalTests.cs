using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.Output
{
    [TestClass]
    public class TimeIntervalTests
    {
        [TestMethod]
        public void DurationInSecondsTestWithValue()
        {
            TimeInterval interval = new TimeInterval
            (
                new Time { Value = 30, Unit = TimeUnits.Minute },
                new Time { Value = 2, Unit = TimeUnits.Hour }
            );

            Assert.AreEqual(interval.DurationInSeconds, 5400);
        }
    }
}