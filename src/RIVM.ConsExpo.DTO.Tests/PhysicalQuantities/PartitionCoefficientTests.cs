using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class PartitionCoefficientTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void PartitionCoefficientCompleteUnitSupportTest()
        {
            var x = new ProductAirPartitionCoefficient();
            foreach (Dimensionless unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.AsLinear();
            }
        }

        [TestMethod]
        public void PartitionCoefficientUnitConversionTest()
        {
            PartitionCoefficient x = new ProductAirPartitionCoefficient()
            {
                Value = 3,
                Unit = Dimensionless.Log10
            };

            PartitionCoefficient y = new ProductAirPartitionCoefficient()
            {
                Value = 1000,
                Unit = Dimensionless.Linear
            };

            TestHelpers.AreEqual(x.AsLinear(), y.AsLinear());
        }

        [TestMethod]
        public void PartitionCoefficientConvertedValueFromLog10ToLinearTest()
        {
            PartitionCoefficient x = new ProductAirPartitionCoefficient()
            {
                Value = 3.0,
                Unit = Dimensionless.Log10
            };

            TestHelpers.AreEqual(x.ConvertedValue(Dimensionless.Linear), 1000.0);
        }

        [TestMethod]
        public void PartitionCoefficientConvertedValueFromLog10ToLog10Test()
        {
            PartitionCoefficient x = new ProductAirPartitionCoefficient()
            {
                Value = 3.0,
                Unit = Dimensionless.Log10
            };

            TestHelpers.AreEqual(x.ConvertedValue(Dimensionless.Log10), 3.0);
        }

        [TestMethod]
        public void PartitionCoefficientConvertedValueFromLinearToLinearTest()
        {
            PartitionCoefficient x = new ProductAirPartitionCoefficient()
            {
                Value = 100.0,
                Unit = Dimensionless.Linear
            };

            TestHelpers.AreEqual(x.ConvertedValue(Dimensionless.Linear), 100.0);
        }

        [TestMethod]
        public void PartitionCoefficientConvertedValueFromLinearToLog10Test()
        {
            PartitionCoefficient x = new ProductAirPartitionCoefficient()
            {
                Value = 100.0,
                Unit = Dimensionless.Linear
            };

            TestHelpers.AreEqual(x.ConvertedValue(Dimensionless.Log10), 2.0);
        }
    }
}