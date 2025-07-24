using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class MassTests
    {
        [TestMethod]
        public void MassUnitConversionTest()
        {
            //All instances are 1 kg. Test if the convert correctly to another unit.
            Mass w = new BodyWeight()
            {
                Value = 1,
                Unit = MassUnits.Kilogram
            };

            Mass x = new ProductAmount()
            {
                Value = 1E3,
                Unit = MassUnits.Gram
            };

            Mass y = new ProductAmount()
            {
                Value = 1E6,
                Unit = MassUnits.Milligram
            };
            Mass z = new ProductAmountPackaging()
            {
                Value = 1E9,
                Unit = MassUnits.Microgram
            };

            TestHelpers.AreEqual(x.InKilogram(), w.InKilogram());
            TestHelpers.AreEqual(x.InKilogram(), y.InKilogram());
            TestHelpers.AreEqual(x.InKilogram(), z.InKilogram());
        }

        [TestMethod]
        public void MassInMilligramTest()
        {
            Mass x = new ProductAmount()
            {
                Value = 1,
                Unit = MassUnits.Milligram
            };

            TestHelpers.AreEqual(1, x.InMilligram());
        }

        [TestMethod]
        public void MassInKilogramTest()
        {
            Mass x = new BodyWeight()
            {
                Value = 1,
                Unit = MassUnits.Kilogram
            };

            TestHelpers.AreEqual(1, x.InKilogram());
        }
    }
}