using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Distributions;
using System.Diagnostics;

namespace RIVM.ConsExpo.DTO.Tests.Distributions
{
    /// <summary>
    /// Tests for sampling the distribution class.
    /// </summary>
    [TestClass]
    public class DistributionSampleTests
    {
        [TestMethod]
        public void DistributionSampleUniformTest()
        {
            var distribution = new Distribution
            {
                DistributionType = DistributionTypes.Uniform, LowerBound = 10, UpperBound = 20
            };

            TestSampling(distribution);
        }

        [TestMethod]
        public void DistributionSampleNormalTest()
        {
            var distribution = new Distribution
            {
                DistributionType = DistributionTypes.Normal, Mean = 10, StandardDeviation = 2
            };

            TestSampling(distribution);
        }

        [TestMethod]
        public void DistributionSampleLogNormalTest()
        {
            var distribution = new Distribution
            {
                DistributionType = DistributionTypes.LogNormal, Median = 10, CoefficientOfVariation = 0.5
            };

            TestSampling(distribution);
        }

        [TestMethod]
        public void DistributionSampleTriangularTest()
        {
            var distribution = new Distribution
            {
                DistributionType = DistributionTypes.Triangular,
                Location = 1,
                Scale = 5,
                Shape = 2
            };

            TestSampling(distribution);
        }

        [TestMethod]
        public void DistributionSampleBetaTest()
        {
            var distribution = new Distribution
            {
                DistributionType = DistributionTypes.Beta,
                Alpha = 2,
                Beta = 0.5
            };

            TestSampling(distribution);
        }

        private static void TestSampling(Distribution distribution, double numberOfSamples = 100)
        {
            for (int i = 0; i < numberOfSamples; i++)
            {
                distribution.Sample();
                Debug.WriteLine("{0}\t{1}", i, distribution.SampledValue);
            }
        }

        [TestMethod]
        public void DistributionSampleNormalWithMinimumTest()
        {
            const double min = 0;
            var distribution = new Distribution
            {
                DistributionType = DistributionTypes.Normal, Mean = 5, StandardDeviation = 3
            };
            Debug.AutoFlush = false;
            for (int i = 0; i < 100000; i++)
            {
                distribution.Sample(min);
                double rand = distribution.SampledValue.Value;
                Assert.IsTrue(rand > min);
            }
            Debug.Flush();
        }

        [TestMethod]
        public void DistributionSampleNormalWithRangeTest()
        {
            const double min = 0;
            const double max = 10;
            var distribution = new Distribution
            {
                DistributionType = DistributionTypes.Normal, Mean = 5, StandardDeviation = 3
            };
            Debug.AutoFlush = false;
            for (int i = 0; i < 100000; i++)
            {
                distribution.Sample(min, max);
                double rand = distribution.SampledValue.Value;
                Assert.IsTrue(rand >= min && rand <= max);
                Debug.WriteLine(rand);
            }
            Debug.Flush();
        }

#warning To Do: rewrite to use RandomGenerator.
        ///// <summary>
        ///// Generates a dieharder set of random numbers.
        ///// </summary>
        ///// <see href="https://www.phy.duke.edu/~rgb/General/dieharder.php"/>
        //[TestMethod]
        //public void GenerateDieharderSet()
        //{
        //    const int NumberCount = 200000000;
        //    const int Seed = 1258501085;
        //    StreamWriter sr = new StreamWriter("N:\\Temp\\randomNR.txt", false, Encoding.ASCII, 100000);

        //    sr.WriteLine("#==================================================================");
        //    sr.WriteLine(string.Format("# generator .Net Assembly mscorlib.dll, v4.0.0.0 4.0.30319.18020 seed = {0}", Seed));
        //    sr.WriteLine("#==================================================================");
        //    sr.WriteLine("type: d");
        //    sr.WriteLine(string.Format("count: {0}", NumberCount));
        //    sr.WriteLine("numbit: 32");
        //    try
        //    {
        //        //Random random = new Random();
        //        Ran ran = new Ran(17);

        //        for (int i = 0; i < NumberCount; i++)
        //        {
        //            sr.WriteLine(string.Format("{0,10}", ran.int32()));
        //        }
        //    }
        //    finally
        //    {
        //        sr.Close();
        //    }
        //}
    }
}