using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Distributions;
using System.Diagnostics;

namespace RIVM.ConsExpo.DTO.Tests.Distributions
{
    [TestClass]
    public class SizeDistributionTests
    {
        [TestMethod]
        public void InitLogNormalTest1()
        {
            const double median = 10;
            const double aCoV = 3;

            const double min = 0;
            const double max = 100;
            const int numberOfBins = 50;

            SizeDistribution sizeDistribution = new SizeDistribution();

            sizeDistribution.InitLogNormal(median, aCoV, min, max, numberOfBins, 1);

            DistributionIsNormalizedTest(min, max, numberOfBins, sizeDistribution);
        }

        [TestMethod]
        public void InitLogNormalTest2()
        {
            const double median = 15;
            const double aCoV = 1.0;

            const double min = 0;
            const double max = 100;
            const int numberOfBins = 100;

            SizeDistribution sizeDistribution = new SizeDistribution();

            sizeDistribution.InitLogNormal(median, aCoV, min, max, numberOfBins, 1);

            DistributionIsNormalizedTest(min, max, numberOfBins, sizeDistribution);
        }

        [TestMethod]
        public void InitLogNormalTest3()
        {
            const double median = 15;
            const double aCoV = 2.0;

            const double min = 0;
            const double max = 100;
            const int numberOfBins = 100;

            SizeDistribution sizeDistribution = new SizeDistribution();

            sizeDistribution.InitLogNormal(median, aCoV, min, max, numberOfBins, 1);

            DistributionIsNormalizedTest(min, max, numberOfBins, sizeDistribution);
        }

        [TestMethod]
        public void InitNormalTest1()
        {
            const double mean = 10;
            const double sd = 3;

            const double min = 0;
            const double max = 100;
            const int numberOfBins = 50;

            SizeDistribution sizeDistribution = new SizeDistribution();

            sizeDistribution.InitNormal(mean, sd, min, max, numberOfBins, 1);

            DistributionIsNormalizedTest(min, max, numberOfBins, sizeDistribution);
        }

        [TestMethod]
        public void InitNormalTest2()
        {
            const double mean = 15;
            const double sd = 4;

            const double min = 0;
            const double max = 40;
            const int numberOfBins = 100;

            SizeDistribution sizeDistribution = new SizeDistribution();

            sizeDistribution.InitNormal(mean, sd, min, max, numberOfBins, 1);

            DistributionIsNormalizedTest(min, max, numberOfBins, sizeDistribution);
        }

        [TestMethod]
        public void InitNormalTest3()
        {
            const double mean = 5;
            const double sd = 1;

            const double min = 0;
            const double max = 10;
            const int numberOfBins = 20;

            SizeDistribution sizeDistribution = new SizeDistribution();

            sizeDistribution.InitNormal(mean, sd, min, max, numberOfBins, 1);

            DistributionIsNormalizedTest(min, max, numberOfBins, sizeDistribution);
        }

        private static void DistributionIsNormalizedTest(double min, double max, int numberOfBins, SizeDistribution sizeDistribution)
        {
            double sum = 0;

            double delta = (max - min) / numberOfBins;

            foreach (var bin in sizeDistribution.Bins)
            {
                sum += bin.ProbabilityMass;
                Debug.WriteLine("{0:F5};{1:F5}", bin.Variable, bin.ProbabilityMass);
            }

            TestHelpers.AreEqualDoubles(1.0, sum, 0.1);
        }
    }
}