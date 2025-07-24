using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class ProductAmountTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void ProductAmountCompleteUnitSupportTest()
        {
            var x = new ProductAmount();
            foreach (MassUnits unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InGram();
            }
        }

        [TestMethod]
        public void ProductWeightUnitConversionTest()
        {
            //All instances are 1 Kilogram, expressed in different units.
            var x = new BodyWeight()
            {
                Value = 1,
                Unit = MassUnits.Kilogram
            };

            var y = new ProductAmount()
            {
                Value = 1E3,
                Unit = MassUnits.Gram
            };

            //StandardUnit
            var z = new ProductAmount()
            {
                Value = 1E6,
                Unit = MassUnits.Milligram
            };

            var a = new ProductAmountPackaging()
            {
                Value = 1E9,
                Unit = MassUnits.Microgram
            };

            TestHelpers.AreEqual(x.InMilligram(), y.InMilligram());
            TestHelpers.AreEqual(x.InMilligram(), z.InMilligram());
            TestHelpers.AreEqual(x.InMilligram(), a.InMilligram());
        }
    }
}