using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Tests.Helpers;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class OralExposureConstantRateTests : OralExposureSubModelBase
    {
        [TestMethod]
        public void OralExposureConstantRateExternalEventDoseTest()
        {
            var scenarioName = "Application";
            var frequencyValue = 70;
            var frequencyUnit = FrequencyUnits.Yearly;
            var weightFraction = 0.01;
            var ingestionRateValue = 1;
            var ingestionRateUnit = MassRateUnits.MilligramPerMinute;
            var exposureDurationValue = 1;
            var exposureDurationUnit = DurationUnits.Hour;

            var scenario = GetScenarioOralExposureConstantRate(scenarioName, frequencyValue, frequencyUnit, weightFraction,
                ingestionRateValue, ingestionRateUnit, exposureDurationValue, exposureDurationUnit);

            TestOralExposureExternalEventDose(0.069, scenario);
        }

        [TestMethod]
        public void OralExposureConstantRateExposureFractionTest()
        {
            var scenarioName = "Application";
            var frequencyValue = 70;
            var frequencyUnit = FrequencyUnits.Yearly;
            var weightFraction = 0.01;
            var ingestionRateValue = 1;
            var ingestionRateUnit = MassRateUnits.GramPerHour;
            var exposureDurationValue = 1;
            var exposureDurationUnit = DurationUnits.Hour;

            var scenario = GetScenarioOralExposureConstantRate(scenarioName, frequencyValue, frequencyUnit, weightFraction,
                ingestionRateValue, ingestionRateUnit, exposureDurationValue, exposureDurationUnit);

            TestOralExposureExposureFraction(1, scenario);
        }

        private ScenarioModel GetScenarioOralExposureConstantRate(string scenarioName, double frequencyValue, FrequencyUnits frequencyUnit,
            double weightFractionSubstance, double amountIngestionRateValue, MassRateUnits amountIngestionRateUnit,
            double exposureDurationValue, DurationUnits exposureDurationUnit)
        {
            var scenario = new ScenarioModel()
            {
                Name = scenarioName,
                Assessment = ScenarioHelper.GetAssessment(8.69, 0.01),
                Frequency = new Frequency()
                {
                    Value = frequencyValue,
                    Unit = frequencyUnit
                },
                OralExposureRouteInUse = true,
                OralExposure = new OralExposureModel()
                {
                    SubmodelType = OralExposureSubmodelTypes.ConstantRate,
                    WeightFractionSubstance = new Fraction
                    {
                        Value = weightFractionSubstance,
                        Unit = FractionUnits.Fraction
                    },
                    IngestionRate = new IngestionRate()
                    {
                        Value = amountIngestionRateValue,
                        Unit = amountIngestionRateUnit
                    },
                    ExposureDuration = new ExposureDuration()
                    {
                        Value = exposureDurationValue,
                        Unit = exposureDurationUnit
                    }
                },
                OralAbsorptionRouteInUse = false,
            };
            return scenario;
        }
    }
}