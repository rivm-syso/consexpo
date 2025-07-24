using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class VelocityTests
    {
        [TestMethod]
        public void VelocityConversionTest()
        {
            //All instances are 1 metre per second. Test if they are all equal when express in metres per minute.
            var x = new MassTransferCoefficient()
            {
                Value = 1,
                Unit = VelocityUnits.MetrePerSecond
            };

            var y = new SkinPermeability()
            {
                Value = 60,
                Unit = VelocityUnits.MetrePerMinute
            };

            var z = new SkinPermeability()
            {
                Value = 3600,
                Unit = VelocityUnits.MetrePerHour
            };

            var u = new SkinPermeability()
            {
                Value = 100 * 60,
                Unit = VelocityUnits.CentimetrePerMinute
            };

            var v = new SkinPermeability()
            {
                Value = 100 * 3600,
                Unit = VelocityUnits.CentimetrePerHour
            };

            var w = new SkinPermeability()
            {
                Value = 1000 * 60,
                Unit = VelocityUnits.MillimetrePerMinute
            };

            TestHelpers.AreEqual(x.InMetresPerMinute(), y.InMetresPerMinute());
            TestHelpers.AreEqual(x.InMetresPerMinute(), z.InMetresPerMinute());
            TestHelpers.AreEqual(x.InMetresPerMinute(), u.InMetresPerMinute());
            TestHelpers.AreEqual(x.InMetresPerMinute(), v.InMetresPerMinute());
            TestHelpers.AreEqual(x.InMetresPerMinute(), w.InMetresPerMinute());
        }

        [TestMethod]
        public void VelocityInMetrePerMinuteTest()
        {
            var x = new SkinPermeability()
            {
                Value = 60,
                Unit = VelocityUnits.MetrePerHour
            };

            TestHelpers.AreEqual(1, x.InMetresPerMinute());
        }

        [TestMethod]
        public void VelocityInMetresPerHourTest()
        {
            var x = new SkinPermeability()
            {
                Value = 1,
                Unit = VelocityUnits.MetrePerHour
            };

            TestHelpers.AreEqual(1, x.InMetresPerHour());
        }

        [TestMethod]
        public void VelocityInCentimetrePerMinuteTest()
        {
            var x = new SkinPermeability()
            {
                Value = 60,
                Unit = VelocityUnits.CentimetrePerHour
            };

            TestHelpers.AreEqual(1, x.InCentimetrePerMinute());
        }
    }
}