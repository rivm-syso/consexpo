using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Output;
using System;
using System.Collections.Generic;

namespace RIVM.ConsExpo.DTO.Tests.Output
{
    [TestClass]
    public class StatisticsTests
    {
        private Statistics statistics;

        [TestInitialize]
        public void SetupDistribution()
        {
            List<double> outcomes = new List<double>();

            for (int repeat = 0; repeat < 10; repeat++)
            {
                for (int outcome = 1; outcome <= 100; outcome++)
                {
                    outcomes.Add(outcome);
                }
            }

            statistics = new Statistics(outcomes, DoseUnits.Mg);
        }

        [TestMethod]
        public void MeanTest()
        {
            const double expectedValue = 50;
            Assert.AreEqual(expectedValue, statistics.Median.Value.Value);
        }

        [TestMethod]
        public void StandardDeviation()
        {
            double expectedValue = Math.Sqrt((100.0 * 100.0 - 1.0) / 12);
            TestHelpers.AreEqualDoubles(expectedValue, statistics.StandardDeviation.Value.Value);
        }

        [TestMethod]
        public void MedianTest()
        {
            const double expectedValue = 50;
            Assert.AreEqual(expectedValue, statistics.Median.Value.Value);
        }

        [TestMethod]
        public void Percentile90Test()
        {
            const double expectedValue = 95;
            Assert.AreEqual(expectedValue, statistics.Percentile95.Value.Value);
        }

        [TestMethod]
        public void Percentile95Test()
        {
            const double expectedValue = 99;
            Assert.AreEqual(expectedValue, statistics.Percentile99.Value.Value);
        }
    }
}