using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Diagnostics;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalUnits
{
    [TestClass]
    public class DurationUnitsTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var x = DurationUnits.Month;

            var y = DurationUnits.Week;

            Assert.AreNotEqual(x, y);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var x = DurationUnits.Month;

            var y = DurationUnits.Month;

            Assert.AreEqual(x, y);
        }

        [TestMethod]
        public void TestMethod3()
        {
            foreach (DurationUnits unit in DurationUnits.AllUnits)
            {
                Debug.WriteLine(unit.DisplayName);
            }
        }
    }
}