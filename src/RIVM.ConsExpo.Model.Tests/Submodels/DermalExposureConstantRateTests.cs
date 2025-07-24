using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Settings;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Models;
using RIVM.ConsExpo.Model.Models;
using RIVM.ConsExpo.Model.Tests.Helpers;
using RIVM.ConsExpo.TestFacilities;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class DermalExposureConstantRateTests : DermalExposureSubModelBase
    {
        [TestMethod]
        public void DermalExposureConstantRateHandCreamTest()
        {
            var scenarioName = "Application";
            var frequencyValue = 7.3E2;
            var frequencyUnit = FrequencyUnits.Yearly;
            var weightFraction = 0.1;
            var contactRateValue = 30;
            var contactRateUnit = MassRateUnits.MicrogramPerMinute;
            var releaseDurationValue = 4.3E4;
            var releaseDurationUnit = DurationUnits.Second;

            var scenario = GetScenarioDermalExposureConstantRate(scenarioName, frequencyValue, frequencyUnit, weightFraction, contactRateValue, contactRateUnit, releaseDurationValue, releaseDurationUnit);

            scenario.Assessment = ScenarioHelper.GetAssessment(60, 0.1);

            TestDermalExposureExternalEventDose(0.036, scenario);
        }

        [TestMethod]
        public void DermalExposureConstantRatePaintTest()
        {
            var scenarioName = "ConstantRate - Paint";
            var frequencyValue = 1;
            var frequencyUnit = FrequencyUnits.Yearly;
            var weightFraction = 0.1;
            var contactRateValue = 30;
            var contactRateUnit = MassRateUnits.MilligramPerMinute;
            var releaseDurationValue = 7200;
            var releaseDurationUnit = DurationUnits.Second;

            var scenario = GetScenarioDermalExposureConstantRate(scenarioName, frequencyValue, frequencyUnit, weightFraction, contactRateValue, contactRateUnit, releaseDurationValue, releaseDurationUnit);

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.1);

            TestDermalExposureExternalEventDose(5.54, scenario);
        }

        [TestMethod]
        public void DermalExposureConstantRateExposureFractionTest()
        {
            var scenarioName = "ConstantRate - Paint";
            var frequencyValue = 1;
            var frequencyUnit = FrequencyUnits.Yearly;
            var weightFraction = 0.1;
            var contactRateValue = 30;
            var contactRateUnit = MassRateUnits.MilligramPerMinute;
            var releaseDurationValue = 120;
            var releaseDurationUnit = DurationUnits.Minute;

            var scenario = GetScenarioDermalExposureConstantRate(scenarioName, frequencyValue, frequencyUnit, weightFraction, contactRateValue, contactRateUnit, releaseDurationValue, releaseDurationUnit);

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.1);

            // This simple model will release all of its product: Contact rate] x [release duration] x [weight fraction]
            double expectedExposureFraction = 1.0;
            TestDermalExposureExposureFraction(expectedExposureFraction, scenario);
        }

        [TestMethod]
        public void DermalExposureSensitivityAnalysisBodyWeightTest()
        {
            var scenarioName = "Application";
            var frequencyValue = 7.3E2;
            var frequencyUnit = FrequencyUnits.Yearly;
            var weightFraction = 0.1;
            var contactRateValue = 30;
            var contactRateUnit = MassRateUnits.MicrogramPerMinute;
            var releaseDurationValue = 4.3E4;
            var releaseDurationUnit = DurationUnits.Second;

            var scenario = GetScenarioDermalExposureConstantRate(scenarioName, frequencyValue, frequencyUnit, weightFraction, contactRateValue, contactRateUnit, releaseDurationValue, releaseDurationUnit);

            scenario.Assessment = ScenarioHelper.GetAssessment(60, 0.1);

            if (TestHelpers.ValidateInput(scenario))
            {
                IDermalSimulation dermalExposureSimulation = new DermalSimulation();
                ISimulation simulation = new Simulation(null, dermalExposureSimulation, null);
                var runSettings = new SensitivityAnalysisSettings()
                {
                    RouteToAnalyse = RouteTypes.Dermal,
                    EndPointToAnalyse = DoseMeasureType.ExternalEventDose,
                    ModelParameterToAnalyse = ModelParameters.AssessmentBodyWeight,
                    LowerBound = 50,
                    UpperBound = 100,
                    UnitCode = MassUnits.Kilogram.Code
                };

                if (scenario.DermalExposureRouteInUse)
                {
                    var exposureOutputValue = simulation.CalculateSensitivityAnalysis(scenario, runSettings);
                    double expectedEventDose = 0.043;
                    var actualEventDose = exposureOutputValue.Points[0].EndPointValue;
                    Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedEventDose, actualEventDose.Value.Value),
                        $"The actual potential chronic dose value {actualEventDose.Value} differs from the expected value {expectedEventDose} with more than the allowed tolerance.");
                }
            }
        }

        private ScenarioModel GetScenarioDermalExposureConstantRate(string scenarioName, double frequencyValue, FrequencyUnits frequencyUnit, double weightFraction, int contactRateValue, MassRateUnits contactRateUnit, double releaseDurationValue, DurationUnits releaseDurationUnit)
        {
            var scenario = new ScenarioModel()
            {
                Name = scenarioName,
                Frequency = new Frequency()
                {
                    Value = frequencyValue,
                    Unit = frequencyUnit
                },
                DermalExposureRouteInUse = true,
                DermalExposure = new DermalExposureModel()
                {
                    SubmodelType = DermalExposureSubmodelTypes.ConstantRate,
                    WeightFractionSubstance = new Fraction
                    {
                        Value = weightFraction,
                        Unit = FractionUnits.Fraction
                    },
                    ContactRate = new ContactRate()
                    {
                        Value = contactRateValue,
                        Unit = contactRateUnit
                    },
                    ReleaseDuration = new ReleaseDuration()
                    {
                        Value = releaseDurationValue,
                        Unit = releaseDurationUnit
                    }
                },
                DermalAbsorptionRouteInUse = false,
            };
            return scenario;
        }
    }
}