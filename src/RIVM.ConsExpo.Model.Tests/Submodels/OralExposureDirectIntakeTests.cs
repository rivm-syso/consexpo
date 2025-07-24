using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class OralExposureDirectIntakeTests : OralExposureSubModelBase
    {
        [TestMethod]
        public void OralExposureDirectIntakeExternalEventDoseTest()
        {
            var scenarioName = "Application";
            var frequencyValue = 7.3E2;
            var frequencyUnit = FrequencyUnits.Yearly;
            var weightFraction = 0.01;
            var productWeightValue = 0.08;
            var productWeightUnit = MassUnits.Gram;

            var scenario = GetScenarioOralExposureDirectIntake(scenarioName, frequencyValue, frequencyUnit, weightFraction, productWeightValue, productWeightUnit);

            TestOralExposureExternalEventDose(0.0123, scenario);
        }

        [TestMethod]
        public void OralExposureDirectIntakeExposureFractionTest()
        {
            var scenarioName = "Application";
            var frequencyValue = 7.3E2;
            var frequencyUnit = FrequencyUnits.Yearly;
            var weightFraction = 0.01;
            var productWeightValue = 0.08;
            var productWeightUnit = MassUnits.Gram;

            var scenario = GetScenarioOralExposureDirectIntake(scenarioName, frequencyValue, frequencyUnit, weightFraction, productWeightValue, productWeightUnit);

            TestOralExposureExposureFraction(1.0, scenario);
        }

        private ScenarioModel GetScenarioOralExposureDirectIntake(string scenarioName, double frequencyValue, FrequencyUnits frequencyUnit, double weightFractionSubstance, double amountIngestedValue, MassUnits amountIngestedUnit)
        {
            var scenario = new ScenarioModel()
            {
                Name = scenarioName,
                Assessment = GetAssessment(),
                Frequency = new Frequency()
                {
                    Value = frequencyValue,
                    Unit = frequencyUnit
                },
                OralExposureRouteInUse = true,
                OralExposure = new OralExposureModel()
                {
                    SubmodelType = OralExposureSubmodelTypes.DirectIntake,
                    WeightFractionSubstance = new Fraction
                    {
                        Value = weightFractionSubstance,
                        Unit = FractionUnits.Fraction
                    },
                    IngestedAmountMouthing = new ProductAmount()
                    {
                        Value = amountIngestedValue,
                        Unit = amountIngestedUnit
                    }
                },
                OralAbsorptionRouteInUse = false,
            };
            return scenario;
        }
    }
}