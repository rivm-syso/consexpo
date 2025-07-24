using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class FractionTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void FractionCompleteUnitSupportTest()
        {
            var x = new Fraction();
            foreach (var unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.AsFraction();
            }
        }

        [TestMethod]
        public void FractionAsPercentageTest()
        {
            var x = new Fraction()
            {
                Value = 1,
                Unit = FractionUnits.Fraction
            };

            TestHelpers.AreEqual(100, x.AsPercentage());
        }

        [TestMethod]
        public void FractionAsFractionTest()
        {
            var x = new Fraction()
            {
                Value = 100,
                Unit = FractionUnits.Percentage
            };

            TestHelpers.AreEqual(1, x.AsFraction());
        }
    }
}