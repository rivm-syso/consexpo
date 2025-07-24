using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class SurfaceRateTests
    {
        [TestMethod]
        public void SurfaceRateUnitConversionTest()
        {
            //All instances are 1 SquareMetrePerHour, expressed in different units.
            SurfaceRate x = new DiffusionCoefficient()
            {
                Value = 1,
                Unit = SurfaceRateUnits.SquareMetrePerHour
            };

            SurfaceRate y = new TransferCoefficient()
            {
                Value = 24,
                Unit = SurfaceRateUnits.SquareMetrePerDay
            };

            SurfaceRate z = new DiffusionCoefficient()
            {
                Value = 1E4,
                Unit = SurfaceRateUnits.SquareCentiMetrePerHour
            };

            SurfaceRate u = new DiffusionCoefficient()
            {
                Value = 166.6666666666666667,
                Unit = SurfaceRateUnits.SquareCentiMetrePerMinute
            };

            SurfaceRate v = new DiffusionCoefficient()
            {
                Value = 2.777777777777777778e-4,
                Unit = SurfaceRateUnits.SquareMetrePerSecond
            };

            TestHelpers.AreEqual(x.InSquareMetresPerSecond(), y.InSquareMetresPerSecond());
            TestHelpers.AreEqual(x.InSquareMetresPerSecond(), z.InSquareMetresPerSecond());
            TestHelpers.AreEqual(x.InSquareMetresPerSecond(), u.InSquareMetresPerSecond());
            TestHelpers.AreEqual(x.InSquareMetresPerSecond(), v.InSquareMetresPerSecond());
        }

        [TestMethod]
        public void SurfaceRateInSquareMetresPerSecondTest()
        {
            var x = new DiffusionCoefficient()
            {
                Value = 3600,
                Unit = SurfaceRateUnits.SquareMetrePerHour
            };

            TestHelpers.AreEqual(1, x.InSquareMetresPerSecond());
        }

        [TestMethod]
        public void SurfaceRateInSquareCentimetrePerSecond()
        {
            var x = new DiffusionCoefficient()
            {
                Value = 60,
                Unit = SurfaceRateUnits.SquareCentiMetrePerMinute
            };

            TestHelpers.AreEqual(1, x.InSquareCentimetrePerSecond());
        }

        [TestMethod]
        public void SurfaceRateInSquareCentiMetresPerMinuteTest()
        {
            var x = new DiffusionCoefficient()
            {
                Value = 1,
                Unit = SurfaceRateUnits.SquareMetrePerSecond
            };

            TestHelpers.AreEqual(60 * 100 * 100, x.InSquareCentimetrePerMinute());
        }

        [TestMethod]
        public void SurfaceRateInSquareMetresPerHourTest()
        {
            var x = new TransferCoefficient()
            {
                Value = 24,
                Unit = SurfaceRateUnits.SquareMetrePerDay
            };

            TestHelpers.AreEqual(1, x.InSquareMetresPerHour());
        }
    }
}