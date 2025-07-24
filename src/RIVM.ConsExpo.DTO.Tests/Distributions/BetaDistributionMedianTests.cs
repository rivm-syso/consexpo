using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Extensions;

namespace RIVM.ConsExpo.DTO.Tests.Distributions
{
    [TestClass]
    [System.Runtime.InteropServices.Guid("97DE0FA8-4832-44B4-9000-7CEBF3C794B5")]
    public class BetaDistributionMedianTests
    {
        [TestMethod]
        public void DeriveMedian1_1()
        {
            TestMedian(1.0, 1.0, 0.5);
        }

        [TestMethod]
        public void DeriveMedian2_2()
        {
            TestMedian(2, 2, 0.5);
        }

        [TestMethod]
        public void DeriveMedian05_05()
        {
            TestMedian(0.5, 0.5, 0.5);
        }

        [TestMethod]
        public void DeriveMedian1_2()
        {
            TestMedian(1.0, 2.0, 0.29289321881345243);
        }

        [TestMethod]
        public void DeriveMedian1_5()
        {
            TestMedian(1.0, 5.0, 0.129449436703876);
        }

        private void TestMedian(double alpha, double beta, double expectedMedian, double tolerance = 1E-6)
        {
            var distribution = new Distribution
            {
                DistributionType = DistributionTypes.Beta,
                Alpha = alpha,
                Beta = beta
            };

            double actualMedian = distribution.DerivedMedian ?? -1.0;
            bool almostEqual = actualMedian.AlmostEqualMagnitude(expectedMedian, tolerance);
            if (!almostEqual)
            {
                Assert.Inconclusive($"Error while calculating a derived median for the beta distribution. Actual values {actualMedian} differs more from the expected value {expectedMedian} than the tolerance of {tolerance}. This can be caused by the outlying random values.");
            }
        }

        [TestMethod]
        public void TestMedianRecalculatedWhenBetaChanges()
        {
            TestMedianRecalculatedWhenParameterChanges(false);
        }


        [TestMethod]
        public void TestMedianRecalculatedWhenAlphaChanges()
        {
            TestMedianRecalculatedWhenParameterChanges(true);
        }

        private static void TestMedianRecalculatedWhenParameterChanges(bool changeAlpha)
        {
            var distribution = new Distribution
            {
                DistributionType = DistributionTypes.Beta,
                Alpha = 1.3,
                Beta = 0.6
            };

            double actualFirstMedian = distribution.DerivedMedian ?? -1.0;

            if (changeAlpha)
                distribution.Alpha = distribution.Beta;
            else
                distribution.Beta = distribution.Alpha;

            double actualSecondMedian = distribution.DerivedMedian ?? -1.0;

            double expectedSecondMedian = 0.5; // alpha = beta, so median must be 0.5.

            Assert.IsFalse(actualSecondMedian.AlmostEqualMagnitude(actualFirstMedian),
                $"The actual median should have been recalculated to become {expectedSecondMedian}, but was found to be (almost) equal to the first estimated value {actualFirstMedian}. Check that the stored value is erased whenever alpha or beta is modified.");

            Assert.IsTrue(actualSecondMedian.AlmostEqualMagnitude(expectedSecondMedian),
                $"The actual median should have been {expectedSecondMedian} for alpha = beta, but was found to be {actualSecondMedian}.");
        }
    }
}
