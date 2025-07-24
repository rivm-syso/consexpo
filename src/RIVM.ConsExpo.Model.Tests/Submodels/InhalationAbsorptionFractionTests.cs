using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using RIVM.ConsExpo.Model.Submodels;
using RIVM.ConsExpo.Model.Tests.Helpers;
using RIVM.ConsExpo.TestFacilities;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class InhalationAbsorptionFractionTests
    {
        [TestMethod]
        public void InhalationAbsorptionFractionTest()
        {
            var scenarioName = "Inhalation Absorption";

            var frequency = new Frequency()
            {
                Value = 2,
                Unit = FrequencyUnits.Weekly
            };

            var absorptionFraction = 0.1;

            var inhalationRate = new VolumeRate()
            {
                Value = 24.1,
                Unit = VolumeRateUnits.LiterPerMinute
            };

            // param from Inhalation Exposure Model
            var exposureDuration = new ExposureDuration()
            {
                Value = 1,
                Unit = DurationUnits.Hour
            };

            var scenario = GetScenarioInhalationAbsorptionFraction(scenarioName, frequency, absorptionFraction, inhalationRate, exposureDuration);

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.1);

            var meanAirConcentration = new AirConcentration()
            {
                Value = 543,
                Unit = DensityUnits.MilligramPerCubicMetre
            };

            TestInhalationAbsorptionChronicDoseEndPoint(0.34, scenario, meanAirConcentration);
        }

        private ScenarioModel GetScenarioInhalationAbsorptionFraction(string scenarioName, Frequency frequency, double absorptionFraction, VolumeRate inhalationRate, ExposureDuration exposureDuration)
        {
            var scenario = new ScenarioModel()
            {
                Name = scenarioName,
                Frequency = frequency,
                InhalationExposure = new InhalationExposureModel()
                {
                    ExposureDuration = exposureDuration,
                    InhalationRate = inhalationRate
                },
                InhalationAbsorptionRouteInUse = true,
                InhalationAbsorption = new InhalationAbsorptionModel()
                {
                    AbsorptionFraction = new Fraction
                    {
                        Value = absorptionFraction,
                        Unit = FractionUnits.Fraction
                    },
                },
            };
            return scenario;
        }

        private static void TestInhalationAbsorptionChronicDoseEndPoint(double expectedInternalYearAverageDose, ScenarioModel scenario, AirConcentration meanAirConcentration)
        {
            if (TestHelpers.ValidateInput(scenario))
            {
                IInhalationAbsorptionSubmodel inhalationAbsorptionSimulation = new InhalationAbsorptionFraction(scenario);
                double amountOfSubstance = 1;

                var exposureOutputValues = new InhalationExposureOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, amountOfSubstance, scenario.InhalationExposure.InhalationRate, new Time() { Value = 0, Unit = TimeUnits.Second }, scenario.InhalationExposure.ExposureDuration.AsTime());

                exposureOutputValues.SetMeanAirConcentration(meanAirConcentration, scenario.InhalationExposure.ExposureDuration.AsTime());

                if (scenario.InhalationAbsorptionRouteInUse)
                {
                    var absorptionOutputValues = inhalationAbsorptionSimulation.CalculatePointValues(exposureOutputValues);

                    var actualInternalYearAverageDose = absorptionOutputValues.AsInternalYearAverageDose;

                    Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedInternalYearAverageDose, actualInternalYearAverageDose.Value.Value),
                        $"The actual internal year average dose value {actualInternalYearAverageDose.Value} differs from the expected value {expectedInternalYearAverageDose} with more than the allowed tolerance.");
                }
                else
                {
                    Assert.Inconclusive("Cannot test the absorption, because the the absorption route is not in use.");
                }
            }
        }
    }
}