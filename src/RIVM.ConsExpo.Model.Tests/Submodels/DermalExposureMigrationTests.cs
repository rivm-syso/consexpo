using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Tests.Helpers;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class DermalExposureMigrationTests : DermalExposureSubModelBase
    {
        [TestMethod]
        public void DermalExposureMigrationExternalEventDoseTest()
        {
            var scenarioName = "Onesie - Migration";
            var frequencyValue = 730;
            var frequencyUnit = FrequencyUnits.Yearly;
            var productAmountValue = 8;
            var productAmountUnit = MassUnits.Gram;
            var leachableFraction = 0.01;
            var skinContactFactor = 0.8;

            var scenario = GetScenarioDermalExposureMigration(scenarioName, frequencyValue, frequencyUnit, productAmountValue, productAmountUnit, leachableFraction, skinContactFactor);
            scenario.Assessment = ScenarioHelper.GetAssessment(61, 0.1);
            TestDermalExposureExternalEventDose(1.05, scenario);
        }

        [TestMethod]
        public void DermalExposureMigrationExposureFractionTest()
        {
            var scenarioName = "Onesie - Migration";
            var frequencyValue = 730;
            var frequencyUnit = FrequencyUnits.Yearly;
            var productAmountValue = 8;
            var productAmountUnit = MassUnits.Gram;
            var leachableFraction = 0.01;
            var skinContactFactor = 0.8;

            var scenario = GetScenarioDermalExposureMigration(scenarioName, frequencyValue, frequencyUnit, productAmountValue, productAmountUnit, leachableFraction, skinContactFactor);
            scenario.Assessment = ScenarioHelper.GetAssessment(61, 0.1);
            TestDermalExposureExposureFraction(null, scenario);
        }

        private ScenarioModel GetScenarioDermalExposureMigration(string scenarioName, double frequencyValue, FrequencyUnits frequencyUnit, double productAmountValue, MassUnits productAmountUnit, double leachableFraction, double skinContactFactor)
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
                    SubmodelType = DermalExposureSubmodelTypes.Migration,
                    LeachableFraction = new Fraction
                    {
                        Value = leachableFraction,
                        Unit = FractionUnits.Fraction
                    },
                    SkinContactFactor = new Fraction
                    {
                        Value = skinContactFactor,
                        Unit = FractionUnits.Fraction
                    },
                    ProductAmount = new ProductAmount()
                    {
                        Value = productAmountValue,
                        Unit = productAmountUnit
                    }
                },
                DermalAbsorptionRouteInUse = false,
            };
            return scenario;
        }
    }
}