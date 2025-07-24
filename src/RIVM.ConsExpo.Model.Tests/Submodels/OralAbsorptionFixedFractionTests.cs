using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using RIVM.ConsExpo.Model.Submodels;
using RIVM.ConsExpo.Model.Tests.Helpers;
using RIVM.ConsExpo.TestFacilities;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class OralAbsorptionFixedFractionTests
    {
        [TestMethod]
        public void OralAbsorptionFixedFractionTest()
        {
            var scenarioName = "Oral absorption";
            var frequencyValue = 1;
            var frequencyUnit = FrequencyUnits.Daily;
            var absorptionFraction = 0.1;

            var scenario = GetScenarioOralAbsorptionFraction(scenarioName, frequencyValue, frequencyUnit, absorptionFraction);

            scenario.Assessment = ScenarioHelper.GetAssessment(60, 0.01);

            var acutePotentialDose = new Dose(1.7, DoseUnits.MgPerKgBodyWeight);

            TestOralAbsorptionChronicDoseEndPoint(0.17, scenario, acutePotentialDose);
        }

        private ScenarioModel GetScenarioOralAbsorptionFraction(string scenarioName, double frequencyValue, FrequencyUnits frequencyUnit, double absorptionFraction)
        {
            var scenario = new ScenarioModel()
            {
                Name = scenarioName,
                Frequency = new Frequency()
                {
                    Value = frequencyValue,
                    Unit = frequencyUnit
                },
                OralAbsorptionRouteInUse = true,
                OralAbsorption = new OralAbsorptionModel()
                {
                    SubmodelType = OralAbsorptionSubmodelTypes.Fraction,
                    AbsorptionFraction = new Fraction
                    {
                        Value = absorptionFraction,
                        Unit = FractionUnits.Fraction
                    }
                },
            };
            return scenario;
        }

        private static void TestOralAbsorptionChronicDoseEndPoint(double expectedInternalYearAverageDose, ScenarioModel scenario, Dose acutePotentialDose)
        {
            if (TestHelpers.ValidateInput(scenario))
            {
                IOralAbsorptionSubmodel oralAbsorptionSimulation = new OralAbsorptionFraction(scenario);
                double? amountOfSubstance = 1;

                var exposureOutputValues = new OralExposureOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, amountOfSubstance)
                {
                    Dose = acutePotentialDose
                };

                if (scenario.OralAbsorptionRouteInUse)
                {
                    var absorptionOutputValues = oralAbsorptionSimulation.CalculatePointValues(exposureOutputValues);

                    var actualInternalYearAverageDose = absorptionOutputValues.AsInternalYearAverageDose;

                    Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedInternalYearAverageDose, actualInternalYearAverageDose.Value.Value),
                        $"The actual internal year average dose value {actualInternalYearAverageDose.Value} differs from the expected value {expectedInternalYearAverageDose} with more than the allowed tolerance.");
                }
                else
                {
                    Assert.Inconclusive("Cannot test the absorption, because the absorption route is not in use.");
                }
            }
        }
    }
}