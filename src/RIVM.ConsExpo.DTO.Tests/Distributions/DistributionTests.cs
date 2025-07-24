using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Distributions;

namespace RIVM.ConsExpo.DTO.Tests.Distributions
{
    [TestClass]
    public class DistributionTests
    {
        [TestMethod]
        public void DerivedMedianTestPointValue()
        {
            var pointValue = new Distribution()
            {
                DistributionType = DistributionTypes.PointValue,
            };

            Assert.IsNull(pointValue.DerivedMedian);
        }

        [TestMethod]
        public void DerivedMedianTestUniform()
        {
            const double expected = 20;

            var uniform = new Distribution()
            {
                DistributionType = DistributionTypes.Uniform,
                LowerBound = 10,
                UpperBound = 30
            };

            TestHelpers.AreEqualDoubles(expected, uniform.DerivedMedian.Value);
        }

        [TestMethod]
        public void DerivedMedianTestNormal()
        {
            const double expected = 10;

            var normal = new Distribution()
            {
                DistributionType = DistributionTypes.Normal,
                Mean = 10,
                StandardDeviation = 5
            };

            TestHelpers.AreEqualDoubles(expected, normal.DerivedMedian.Value);
        }

        [TestMethod]
        public void DerivedMedianTestLogNormal()
        {
            const double expected = 10;

            var logNormal = new Distribution()
            {
                DistributionType = DistributionTypes.LogNormal,
                Median = 10,
                CoefficientOfVariation = 2
            };

            TestHelpers.AreEqualDoubles(expected, logNormal.DerivedMedian.Value);
        }

        [TestMethod]
        public void DerivedMedianTestTriangular()
        {
            const double expected = 20;

            var triangular = new Distribution()
            {
                DistributionType = DistributionTypes.Triangular,
                Location = 10,
                Scale = 30,
                Shape = 20
            };

            TestHelpers.AreEqualDoubles(expected, triangular.DerivedMedian.Value);
        }

        [TestMethod]
        public void DerivedMedianTestBeta()
        {
            const double expected = 0.5;

            var beta = new Distribution()
            {
                DistributionType = DistributionTypes.Beta,
                Alpha = 1,
                Beta = 1
            };

            TestHelpers.AreEqualDoubles(expected, beta.DerivedMedian.Value);
        }
    }
}