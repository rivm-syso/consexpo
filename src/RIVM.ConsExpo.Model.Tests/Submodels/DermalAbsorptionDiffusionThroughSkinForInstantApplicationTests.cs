using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Models;
using RIVM.ConsExpo.Model.Models;
using RIVM.ConsExpo.Model.Tests.Helpers;
using RIVM.ConsExpo.TestFacilities;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class DermalAbsorptionDiffusionThroughSkinForInstantApplicationTests
    {
        [TestMethod]
        public void DiffusionThroughSkinForInstantApplicationTest()
        {
            var scenarioName = "Application";
            var frequencyValue = 365;
            var frequencyUnit = FrequencyUnits.Yearly;
            var weightFraction = 0.05;
            var productWeightValue = 0.5;
            var productWeightUnit = MassUnits.Gram;
            var exposedAreaValue = 100.0;
            var exposedAreaUnit = AreaUnits.SquareCentimetre;
            var concentrationValue = 1;
            var concentrationUnit = DensityUnits.GramPerCubicCentimetre;
            var permeabilitySkinValue = 6.64E-6;
            var permeabilitySkinUnit = VelocityUnits.MetrePerHour;
            var exposureTimeValue = 1.44E3;
            var exposureTimeUnit = DurationUnits.Minute;

            var scenario = GetScenarioDiffusionThroughSkinForInstantApplication(scenarioName, frequencyValue, frequencyUnit, weightFraction, productWeightValue, productWeightUnit, exposedAreaValue, exposedAreaUnit, concentrationValue, concentrationUnit, permeabilitySkinValue, permeabilitySkinUnit, exposureTimeValue, exposureTimeUnit);

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.05);

            TestExposureAndAbsorption(0.385, scenario);
        }

        protected ScenarioModel GetScenarioDiffusionThroughSkinForInstantApplication
(string scenarioName, double frequencyValue, FrequencyUnits frequencyUnit, double weightFraction, double productAmountValue, MassUnits productAmountUnit, double exposedAreaValue, AreaUnits exposedAreaUnit, double concentrationValue, DensityUnits concentrationUnit, double permeabilitySkinValue, VelocityUnits permeabilitySkinUnit, double exposureTimeValue, DurationUnits exposureTimeUnit)
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
                DermalAbsorptionRouteInUse = true
            };

            scenario.DermalExposure = new DermalExposureModel()
            {
                SubmodelType = DermalExposureSubmodelTypes.InstantApplication,
                ExposedArea = new ExposedArea()
                {
                    Value = exposedAreaValue,
                    Unit = exposedAreaUnit
                },
                WeightFractionSubstance = new Fraction
                {
                    Value = weightFraction,
                    Unit = FractionUnits.Fraction
                },
                ProductAmount = new ProductAmount()
                {
                    Value = productAmountValue,
                    Unit = productAmountUnit
                },
                RetentionFactor = new Fraction()
                {
                    Value = 1,
                    Unit = FractionUnits.Fraction
                }
            };

            scenario.DermalAbsorption = GetAbsorptionModel(concentrationValue, concentrationUnit, permeabilitySkinValue, permeabilitySkinUnit, exposureTimeValue, exposureTimeUnit);

            return scenario;
        }

        protected static DermalAbsorptionModel GetAbsorptionModel(double concentrationValue, DensityUnits concentrationUnit, double permeabilitySkinValue, VelocityUnits permeabilitySkinUnit, double exposureTimeValue, DurationUnits exposureTimeUnit)
        {
            var dermalAbsorptionModel = new DermalAbsorptionModel()
            {
                SubmodelType = DermalAbsorptionSubmodelTypes.DiffusionThroughSkinForInstantApplication,
                ConcentrationInMatrix = new SubstanceConcentration()
                {
                    Value = concentrationValue,
                    Unit = concentrationUnit
                },
                SkinPermeability = new SkinPermeability()
                {
                    Value = permeabilitySkinValue,
                    Unit = permeabilitySkinUnit
                },
                ExposureDuration = new ExposureDuration()
                {
                    Value = exposureTimeValue,
                    Unit = exposureTimeUnit
                }
            };
            return dermalAbsorptionModel;
        }

        protected static void TestExposureAndAbsorption(double expectedInternalEventDose, ScenarioModel scenario)
        {
            if (TestHelpers.ValidateInput(scenario))
            {
                IDermalSimulation dermalSimulation = new DermalSimulation();

                var dermalOutputValues = dermalSimulation.CalculatePointValues(scenario);

                var actualInternalEventDose = dermalOutputValues.Absorption.AsInternalEventDose.Value;
                Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedInternalEventDose, actualInternalEventDose.Value),
                    $"The actual internal event dose value {actualInternalEventDose} differs from the expected value {expectedInternalEventDose} with more than the allowed tolerance.");
            }
        }
    }
}