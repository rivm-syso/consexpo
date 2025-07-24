using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using System;

namespace RIVM.ConsExpo.DTO.Tests.Output
{
    [TestClass]
    public class InhalationExposureOutcomeTests
    {
        private const double BodyWeightValue = 60;

        private const double InhalationRateValue = 1.4;

        private const double StartTimeValue = 12.0;

        private const double EndTimeValue = 32.0;

        [TestMethod]
        public void AsExternalEventDoseFromAirConcentrationTest()
        {
            var bodyWeight = new BodyWeight() { Value = BodyWeightValue, Unit = MassUnits.Kilogram };
            Frequency scenarioFrequency = null;
            double? amountOfSubstance = null;

            var inhalationRate = new VolumeRate() { Value = InhalationRateValue, Unit = VolumeRateUnits.CubicMetrePerHour };

            Time startTime = new Time() { Value = StartTimeValue, Unit = TimeUnits.Hour };
            Time endTime = new Time() { Value = EndTimeValue, Unit = TimeUnits.Hour };

            var x = new InhalationExposureOutcome(bodyWeight, scenarioFrequency, amountOfSubstance, inhalationRate, startTime, endTime);

            const double valueInMgPerCubicMetre = 4.7;
            var airConcentration = new AirConcentration() { Value = valueInMgPerCubicMetre, Unit = DensityUnits.MilligramPerCubicMetre };
            x.SetMeanAirConcentration(airConcentration);

            double expectedExternalEventDose = valueInMgPerCubicMetre * InhalationRateValue * (EndTimeValue - StartTimeValue) / BodyWeightValue;

            TestHelpers.AreEqualDoubles(expectedExternalEventDose, x.AsExternalEventDose.Value.Value);
        }

        [TestMethod]
        public void AsExternalEventDoseFromAirConcentrationWithoutBodyWeightTest()
        {
            BodyWeight bodyWeight = null;
            Frequency scenarioFrequency = null;
            double? amountOfSubstance = null;

            var inhalationRate = new VolumeRate() { Value = InhalationRateValue, Unit = VolumeRateUnits.CubicMetrePerHour };

            Time startTime = new Time() { Value = StartTimeValue, Unit = TimeUnits.Hour };
            Time endTime = new Time() { Value = EndTimeValue, Unit = TimeUnits.Hour };

            var x = new InhalationExposureOutcome(bodyWeight, scenarioFrequency, amountOfSubstance, inhalationRate, startTime, endTime);

            const double valueInMgPerCubicMetre = 0.56;
            var airConcentration = new AirConcentration() { Value = valueInMgPerCubicMetre, Unit = DensityUnits.MilligramPerCubicMetre };
            x.SetMeanAirConcentration(airConcentration);

            Assert.IsNull(x.AsExternalEventDose.Value);
        }

        [TestMethod]
        public void AsExternalEventDoseFromAirConcentrationWithoutInhalationRateTest()
        {
            var bodyWeight = new BodyWeight() { Value = BodyWeightValue, Unit = MassUnits.Kilogram };
            Frequency scenarioFrequency = null;
            double? amountOfSubstance = null;
            VolumeRate inhalationRate = null;

            Time startTime = new Time() { Value = StartTimeValue, Unit = TimeUnits.Hour };
            Time endTime = new Time() { Value = EndTimeValue, Unit = TimeUnits.Hour };

            var x = new InhalationExposureOutcome(bodyWeight, scenarioFrequency, amountOfSubstance, inhalationRate, startTime, endTime);

            const double valueInMgPerCubicMetre = 0.56;
            var airConcentration = new AirConcentration() { Value = valueInMgPerCubicMetre, Unit = DensityUnits.MilligramPerCubicMetre };
            x.SetMeanAirConcentration(airConcentration);

            Assert.IsNull(x.AsExternalEventDose.Value);
        }

        [TestMethod]
        public void AsMeanDayConcentrationFromAirConcentrationTest()
        {
#warning To Do: write unit test.
        }

        [TestMethod]
        public void AsMeanYearConcentrationFromAirConcentrationTest()
        {
#warning To Do: write unit test.
        }

        [TestMethod]
        public void AsExternalDayDoseFromAirConcentrationTestLowFrequency()
        {
            AsExternalDayDoseFromAirConcentration(0.5);
        }

        [TestMethod]
        public void AsExternalDayDoseFromAirConcentrationTestHighFrequency()
        {
            AsExternalDayDoseFromAirConcentration(5);
        }

        private void AsExternalDayDoseFromAirConcentration(double frequencyInPerDay)
        {
            var bodyWeight = new BodyWeight() { Value = BodyWeightValue, Unit = MassUnits.Kilogram };
            var scenarioFrequency = new Frequency() { Value = frequencyInPerDay, Unit = FrequencyUnits.Daily };
            double? amountOfSubstance = null;
            var inhalationRate = new VolumeRate() { Value = InhalationRateValue, Unit = VolumeRateUnits.CubicMetrePerHour };

            Time startTime = new Time() { Value = StartTimeValue, Unit = TimeUnits.Hour };
            Time endTime = new Time() { Value = EndTimeValue, Unit = TimeUnits.Hour };

            var x = new InhalationExposureOutcome(bodyWeight, scenarioFrequency, amountOfSubstance, inhalationRate, startTime, endTime);

            const double valueInMgPerCubicMetre = 4.7;
            var airConcentration = new AirConcentration() { Value = valueInMgPerCubicMetre, Unit = DensityUnits.MilligramPerCubicMetre };
            x.SetMeanAirConcentration(airConcentration);

            double expectedExternalDayDose = valueInMgPerCubicMetre * InhalationRateValue * (EndTimeValue - StartTimeValue) * Math.Max(frequencyInPerDay, 1) / BodyWeightValue;

            TestHelpers.AreEqualDoubles(expectedExternalDayDose, x.AsExternalDayDose.Value.Value);
        }
    }
}