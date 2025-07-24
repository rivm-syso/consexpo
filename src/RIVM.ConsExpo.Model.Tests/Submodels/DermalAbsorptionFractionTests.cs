using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using RIVM.ConsExpo.Model.Submodels;
using RIVM.ConsExpo.Model.Tests.Helpers;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class DermalAbsorptionFractionTests
    {
        [TestMethod]
        public void DermalAbsorptionFractionHandCreamTest()
        {
            var scenarioName = "Application";
            var frequencyValue = 7.3E2;
            var frequencyUnit = FrequencyUnits.Yearly;
            var absorptionFraction = 0.2;

            var scenario = GetScenarioDermalAbsorptionFraction(scenarioName, frequencyValue, frequencyUnit, absorptionFraction);

            scenario.DermalExposure = new DermalExposureModel();
            scenario.Assessment = ScenarioHelper.GetAssessment(60, 0.1);

            var acutePotentialDose = new Dose(0.036, DoseUnits.MgPerKgBodyWeight);

            TestDermalAbsorptionChronicDoseEndPoint(0.014, scenario, acutePotentialDose);
        }

        protected ScenarioModel GetScenarioDermalAbsorptionFraction(string scenarioName, double frequencyValue, FrequencyUnits frequencyUnit, double absorptionFraction)
        {
            var scenario = new ScenarioModel()
            {
                Name = scenarioName,
                Frequency = new Frequency()
                {
                    Value = frequencyValue,
                    Unit = frequencyUnit
                },
                DermalAbsorptionRouteInUse = true,
                DermalAbsorption = new DermalAbsorptionModel()
                {
                    SubmodelType = DermalAbsorptionSubmodelTypes.Fraction,
                    AbsorptionFraction = new Fraction
                    {
                        Value = absorptionFraction,
                        Unit = FractionUnits.Fraction
                    }
                },
            };
            return scenario;
        }

        protected static void TestDermalAbsorptionChronicDoseEndPoint(double expectedInternalYearAverageDose, ScenarioModel scenario, Dose acutePotentialDose)
        {
            if (TestHelpers.ValidateInput(scenario))
            {
                IDermalAbsorptionSubmodel dermalAbsorptionSimulation = new DermalAbsorptionFraction(scenario);
                double amountOfSubstance = 1;

                var exposureOutputValues = new DermalExposureOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, amountOfSubstance, scenario.DermalExposure.ExposedArea);

                exposureOutputValues.Dose = acutePotentialDose;

                if (scenario.DermalAbsorptionRouteInUse)
                {
                    var absorptionOutputValues = dermalAbsorptionSimulation.CalculatePointValues(exposureOutputValues);

                    var actualInternalYearAverageDose = absorptionOutputValues.AsInternalYearAverageDose;

                    Assert.IsTrue(RIVM.ConsExpo.TestFacilities.Comparisons.AlmostEqualMagnitude(expectedInternalYearAverageDose, actualInternalYearAverageDose.Value.Value),
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