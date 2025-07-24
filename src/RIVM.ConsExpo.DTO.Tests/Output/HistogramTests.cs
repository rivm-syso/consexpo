using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Output;

namespace RIVM.ConsExpo.DTO.Tests.Output
{
    [TestClass]
    public class HistogramTests
    {
        [TestMethod]
        public void HistogramLinearEquallyDistributedTest()
        {
            const int binCount = 10;
            const int dosesPerBin = 5;

            var doses = new List<double>();

            for (int i = 0; i < binCount; i++)
            {
                for (int j = 0; j < dosesPerBin; j++)
                {
                    doses.Add(i);
                }
            }

            var target = new Histogram(doses, binCount, ScaleType.Linear, DoseUnits.Mg);

            int outcomeCount = 0;

            foreach (Bin bin in target.Bins)
            {
                Assert.AreEqual(dosesPerBin, bin.NumberOfOutcomes);

                outcomeCount += bin.NumberOfOutcomes;
            }

            Assert.AreEqual(binCount * dosesPerBin, outcomeCount);
        }

        [TestMethod]
        public void HistogramLogEquallyDistributedTest()
        {
            const int binCount = 10;
            const int dosesPerBin = 5;

            var doses = new List<double>();

            for (int i = 0; i < binCount; i++)
            {
                // Fill bins with a set of powers of 2, do get an equally distributed log histogram.
                for (int j = 0; j < dosesPerBin; j++)
                {
                    doses.Add(Math.Pow(2, i));
                }
            }

            var target = new Histogram(doses, binCount, ScaleType.Logarithmic, DoseUnits.Mg);

            int outcomeCount = 0;

            foreach (Bin bin in target.Bins)
            {
                Assert.AreEqual(dosesPerBin, bin.NumberOfOutcomes);

                outcomeCount += bin.NumberOfOutcomes;
            }

            Assert.AreEqual(binCount * dosesPerBin, outcomeCount);
        }

        [TestMethod]
        public void HistogramLinearAllSameValueTest()
        {
            const int binCount = 10;
            const int dosesCount = 5;

            const double value = 1.5;

            var doses = new List<double>();

            for (int i = 0; i < dosesCount; i++)
            {
                doses.Add(value);
            }

            var target = new Histogram(doses, binCount, ScaleType.Linear, DoseUnits.Mg);

            int outcomeCount = 0;

            foreach (Bin bin in target.Bins)
            {

                Assert.AreEqual((value >= bin.LowerBound && value < bin.UpperBound) ? dosesCount : 0, bin.NumberOfOutcomes);

                outcomeCount += bin.NumberOfOutcomes;
            }

            Assert.AreEqual(dosesCount, outcomeCount);
        }

        [TestMethod]
        public void HistogramLogAllSameValueTest()
        {
            const int binCount = 10;
            const int dosesCount = 5;

            const double value = 1.5;

            var doses = new List<double>();

            for (int i = 0; i < dosesCount; i++)
            {
                doses.Add(value);
            }

            var target = new Histogram(doses, binCount, ScaleType.Logarithmic, DoseUnits.Mg);

            int outcomeCount = 0;

            foreach (Bin bin in target.Bins)
            {
                Assert.AreEqual((value >= bin.LowerBound && value < bin.UpperBound) ? dosesCount : 0, bin.NumberOfOutcomes);

                outcomeCount += bin.NumberOfOutcomes;
            }

            Assert.AreEqual(dosesCount, outcomeCount);
        }

        [TestMethod]
        public void HistogramLinearTwoValuesTest()
        {
            const int binCount = 10;
            const int dosesPerValue = 4;

            const double value1 = 1.5;
            const double value2 = 2.5;

            var doses = new List<double>();

            for (int i = 0; i < dosesPerValue; i++)
            {
                doses.Add(value1);
                doses.Add(value2);
            }

            var target = new Histogram(doses, binCount, ScaleType.Linear, DoseUnits.Mg);

            int outcomeCount = 0;

            foreach (Bin bin in target.Bins)
            {
                int expected = (value1 >= bin.LowerBound && value1 < bin.UpperBound) || (value2 >= bin.LowerBound && value2 <= bin.UpperBound) ? dosesPerValue : 0;

                Assert.AreEqual(expected, bin.NumberOfOutcomes, $"Expected {expected} outcomes, but found {bin.NumberOfOutcomes}, in bin with lower bound {bin.LowerBound} and upper bound {bin.UpperBound}.");

                outcomeCount += bin.NumberOfOutcomes;
            }

            Assert.AreEqual(2 * dosesPerValue, outcomeCount);
        }

        [TestMethod]
        public void HistogramLogTwoValuesTest()
        {
            const int binCount = 10;
            const int dosesPerValue = 4;

            const double value1 = 3.5;
            const double value2 = 5.5;
            const double delta = 1E-8; // Work-around for rounding errors.

            var doses = new List<double>();

            for (int i = 0; i < dosesPerValue; i++)
            {
                doses.Add(value1);
                doses.Add(value2);
            }

            var target = new Histogram(doses, binCount, ScaleType.Logarithmic, DoseUnits.Mg);

            int outcomeCount = 0;

            foreach (Bin bin in target.Bins)
            {
                int expected = (value1 + delta >= bin.LowerBound && value1 + delta < bin.UpperBound) || (value2 - delta >= bin.LowerBound && value2 - delta <= bin.UpperBound) ? dosesPerValue : 0;

                Assert.AreEqual(expected, bin.NumberOfOutcomes, $"Expected {expected} outcomes, but found {bin.NumberOfOutcomes}, in bin with lower bound {bin.LowerBound} and upper bound {bin.UpperBound}.");

                outcomeCount += bin.NumberOfOutcomes;
            }

            Assert.AreEqual(2 * dosesPerValue, outcomeCount);
        }

        [TestMethod]
        public void HistogramLinearTwoAlmostEqualValuesTest()
        {
            const int binCount = 10;
            const int dosesPerValue = 4;

            const double value1 = 6.6;
            const double value2 = 6.601;

            var doses = new List<double>();

            for (int i = 0; i < dosesPerValue; i++)
            {
                doses.Add(value1);
                doses.Add(value2);
            }

            var target = new Histogram(doses, binCount, ScaleType.Linear, DoseUnits.Mg);

            int outcomeCount = 0;

            foreach (Bin bin in target.Bins)
            {
                int expected = (value1 >= bin.LowerBound && value1 < bin.UpperBound) || (value2 >= bin.LowerBound && value2 <= bin.UpperBound) ? dosesPerValue : 0;

                Assert.AreEqual(expected, bin.NumberOfOutcomes, $"Expected {expected} outcomes, but found {bin.NumberOfOutcomes}, in bin with lower bound {bin.LowerBound} and upper bound {bin.UpperBound}.");

                outcomeCount += bin.NumberOfOutcomes;
            }

            Assert.AreEqual(2 * dosesPerValue, outcomeCount);
        }

        [TestMethod]
        public void HistogramLogTwoAlmostEqualTest()
        {
            const int binCount = 10;
            const int dosesPerValue = 4;

            const double value1 = 3.0;
            const double value2 = 3.001;
            const double delta = 1E-8; // Work-around for rounding errors.

            var doses = new List<double>();

            for (int i = 0; i < dosesPerValue; i++)
            {
                doses.Add(value1);
                doses.Add(value2);
            }

            var target = new Histogram(doses, binCount, ScaleType.Logarithmic, DoseUnits.Mg);

            int outcomeCount = 0;

            foreach (Bin bin in target.Bins)
            {
                int expected = (value1 + delta >= bin.LowerBound && value1 + delta < bin.UpperBound) || (value2 - delta >= bin.LowerBound && value2 - delta <= bin.UpperBound) ? dosesPerValue : 0;

                Assert.AreEqual(expected, bin.NumberOfOutcomes, $"Expected {expected} outcomes, but found {bin.NumberOfOutcomes}, in bin with lower bound {bin.LowerBound} and upper bound {bin.UpperBound}.");

                outcomeCount += bin.NumberOfOutcomes;
            }

            Assert.AreEqual(2 * dosesPerValue, outcomeCount);
        }

    }
}