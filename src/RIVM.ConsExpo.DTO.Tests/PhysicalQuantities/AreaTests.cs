using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class AreaTests
    {
        [TestMethod]
        public void ExposedAreaConversionTest()
        {
            Area x = new ExposedArea()
            {
                Value = 1,
                Unit = AreaUnits.SquareMetre
            };

            Area y = new ExposedArea()
            {
                Value = 10 * 10,
                Unit = AreaUnits.SquareDecimetre
            };

            Area z = new ExposedArea()
            {
                Value = 100 * 100,
                Unit = AreaUnits.SquareCentimetre
            };

            TestHelpers.AreEqual(x.InSquareCentimetre(), y.InSquareCentimetre());
            TestHelpers.AreEqual(x.InSquareCentimetre(), z.InSquareCentimetre());
        }

        [TestMethod]
        public void AreaInSquareMetreTest()
        {
            Area x = new ExposedArea()
            {
                Value = 1,
                Unit = AreaUnits.SquareMetre
            };

            TestHelpers.AreEqual(1, x.InSquareMetre());
        }

        [TestMethod]
        public void AreaInSquareCentimeterTest()
        {
            Area x = new ExposedArea()
            {
                Value = 1,
                Unit = AreaUnits.SquareCentimetre
            };

            TestHelpers.AreEqual(1, x.InSquareCentimetre());
        }
    }
}