using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RIVM.ConsExpo.DTO.Tests
{
    /// <summary>
    /// Tests to confirm expected behaviour of C# and the .Net Framework.
    /// </summary>
    [TestClass]
    public class CsTests
    {
        /// <summary>
        /// Test to confirm that the product of a nullable double with value null and a double with a value does not raise an exception, but just returns null.
        /// </summary>
        [TestMethod]
        public void ProductOfDoubleAndNull()
        {
            double? x = null;
            double y = 3.4;

            double? z = x * y;

            Assert.IsNull(z);
        }

        /// <summary>
        /// Test to confirm that the product of a nullable double with a value and a double with a value just returns the numeric product.
        /// </summary>
        [TestMethod]
        public void ProductOfDoubleAndNullableDouble()
        {
            double? x = 2.0;
            double y = 3.4;

            double? z = x * y;

            Assert.AreEqual(6.8, z);
        }

        /// <summary>
        /// Test to confirm that the sum of a nullable double with value null and a double with a value does not raise an exception, but just returns null.
        /// </summary>
        [TestMethod]
        public void SumOfDoubleAndNull()
        {
            double? x = null;
            double y = 3.4;

            double? z = x + y;

            Assert.IsNull(z);
        }

        /// <summary>
        /// Test to confirm that the sum of a nullable double with a value and a double with a value just returns the numeric sum.
        /// </summary>
        [TestMethod]
        public void SumOfDoubleAndNullableDouble()
        {
            double? x = 2.0;
            double y = 3.4;

            double? z = x + y;

            Assert.AreEqual(5.4, z);
        }
    }
}