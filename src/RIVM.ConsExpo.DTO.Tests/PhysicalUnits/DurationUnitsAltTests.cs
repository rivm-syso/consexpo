using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalUnits
{
    [TestClass]
    public class DurationUnitsAltTests
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

        [TestMethod]
        public void TestMethod4()
        {
            var a = DurationUnits.Minute;

            if (a == DurationUnits.Minute)
            {
            }
            else
            {
            }
        }
    }
}