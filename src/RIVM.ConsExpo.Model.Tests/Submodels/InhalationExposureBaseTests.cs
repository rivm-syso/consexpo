using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.Model.Submodels;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class InhalationExposureBaseTests
    {
        private const int IndexOfValueToMaximize = 1;
        private const int NumberOfTimeSteps = 100;

        private const int PeakIntervalSteps = 10;

        [TestMethod]
        public void InitialBracketMaxAtStartTest()
        {
            var solution = new double[NumberOfTimeSteps + 1, 2];

            const int stepInterval = 10;

            for (int i = 0; i <= NumberOfTimeSteps; i++)
            {
                solution[i, 0] = stepInterval * i; // Time
                solution[i, 1] = 200.0 - i; // Value
            }

            var timeMax = new Time(stepInterval * NumberOfTimeSteps, TimeUnits.Minute);
            var peakInterval = new Time(stepInterval * PeakIntervalSteps, TimeUnits.Minute);

            var initialBracket = InhalationExposureBase.InitialBracket(solution, timeMax, peakInterval, NumberOfTimeSteps, IndexOfValueToMaximize);

            Assert.AreEqual(0.0, initialBracket.StartTime.InMinutes());
            Assert.AreEqual(stepInterval * (PeakIntervalSteps + 1), initialBracket.EndTime.InMinutes());
        }

        [TestMethod]
        public void InitialBracketMaxAtEndTest()
        {
            var solution = new double[NumberOfTimeSteps + 1, 2];

            const int stepInterval = 10;

            for (int i = 0; i <= NumberOfTimeSteps; i++)
            {
                solution[i, 0] = stepInterval * i; // Time
                solution[i, 1] = 100.0 + i; // Value
            }

            var timeMax = new Time(stepInterval * NumberOfTimeSteps, TimeUnits.Minute);
            var peakInterval = new Time(stepInterval * PeakIntervalSteps, TimeUnits.Minute);

            var initialBracket = InhalationExposureBase.InitialBracket(solution, timeMax, peakInterval, NumberOfTimeSteps, IndexOfValueToMaximize);

            Assert.AreEqual(stepInterval * (NumberOfTimeSteps - PeakIntervalSteps - 1), initialBracket.StartTime.InMinutes());
            Assert.AreEqual(stepInterval * NumberOfTimeSteps, initialBracket.EndTime.InMinutes());
        }

        [TestMethod]
        public void InitialBracketMaxInTheMiddleTest()
        {
            var solution = new double[NumberOfTimeSteps + 1, 2];

            const int stepInterval = 10;

            for (int i = 0; i <= NumberOfTimeSteps; i++)
            {
                solution[i, 0] = stepInterval * i; // Time
                solution[i, 1] = 100.0 - Math.Abs(i - 50); // Increase from 50 to 100, then decline back to 50.
            }

            var timeMax = new Time(stepInterval * NumberOfTimeSteps, TimeUnits.Minute);
            var peakInterval = new Time(stepInterval * PeakIntervalSteps, TimeUnits.Minute);

            var initialBracket = InhalationExposureBase.InitialBracket(solution, timeMax, peakInterval, NumberOfTimeSteps, IndexOfValueToMaximize);

            // The bracket is around the middle value.
            Assert.AreEqual(stepInterval * (NumberOfTimeSteps / 2 - PeakIntervalSteps - 1), initialBracket.StartTime.InMinutes());
            Assert.AreEqual(stepInterval * (NumberOfTimeSteps / 2 + PeakIntervalSteps + 1), initialBracket.EndTime.InMinutes());
        }
    }
}