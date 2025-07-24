using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Tests.Helpers;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class OralExposureProductMouthingTests : OralExposureSubModelBase
    {
        [TestMethod]
        public void OralExposureProductMouthingExternalEventDoseTest()
        {
            var scenarioName = "Application";
            var frequencyValue = 1;
            var frequencyUnit = FrequencyUnits.Daily;
            var weightFraction = 0.01;
            var productWeightValue = 10;
            var productWeightUnit = MassUnits.Gram;
            var exposureDurationValue = 10;
            var exposureDurationUnit = DurationUnits.Minute;
            var initialMigrationRateValue = 0.01;
            var initialMigrationRateUnit = MigrationRateUnits.GramPerSquareCentimetrePerMinute;
            var contactAreaValue = 10;
            var contactAreaUnit = AreaUnits.SquareCentimetre;

            var scenario = GetScenarioOralExposureProductMouthing(scenarioName, weightFraction, frequencyValue, frequencyUnit,
                productWeightValue, productWeightUnit, exposureDurationValue, exposureDurationUnit,
                initialMigrationRateValue, initialMigrationRateUnit, contactAreaValue, contactAreaUnit);

            TestOralExposureExternalEventDose(1.67, scenario);
        }

        [TestMethod]
        public void OralExposureProductMouthingExposureFractionTest()
        {
            var scenarioName = "Application";
            var frequencyValue = 1;
            var frequencyUnit = FrequencyUnits.Daily;
            var weightFraction = 0.01;
            var productWeightValue = 10;
            var productWeightUnit = MassUnits.Gram;
            var exposureDurationValue = 10;
            var exposureDurationUnit = DurationUnits.Minute;
            var initialMigrationRateValue = 0.001;
            var initialMigrationRateUnit = MigrationRateUnits.GramPerSquareCentimetrePerMinute;
            var contactAreaValue = 10;
            var contactAreaUnit = AreaUnits.SquareCentimetre;

            var scenario = GetScenarioOralExposureProductMouthing(scenarioName, weightFraction, frequencyValue, frequencyUnit,
                productWeightValue, productWeightUnit, exposureDurationValue, exposureDurationUnit,
                initialMigrationRateValue, initialMigrationRateUnit, contactAreaValue, contactAreaUnit);

            // Value found by trial. It is only used for regression. Could not check with ConsExpo 4.1 Windows client, as it does not support this model.
            TestOralExposureExposureFraction(0.63, scenario);
        }

        private ScenarioModel GetScenarioOralExposureProductMouthing(string scenarioName,
            double weightFractionSubstance,
            double frequencyValue, FrequencyUnits frequencyUnit,
            double productWeightValue, MassUnits productWeightUnit,
            double exposureDurationValue, DurationUnits exposureDurationUnit,
            double initialMigrationRateValue, MigrationRateUnits initialMigrationUnit,
            double contactAreaValue, AreaUnits contactAreaUnit
            )
        {
            var scenario = new ScenarioModel()
            {
                Name = scenarioName,
                Assessment = ScenarioHelper.GetAssessment(60, 0.01),
                Frequency = new Frequency()
                {
                    Value = frequencyValue,
                    Unit = frequencyUnit
                },
                OralExposureRouteInUse = true,
                OralExposure = new OralExposureModel()
                {
                    SubmodelType = OralExposureSubmodelTypes.ProductMouthing,
                    WeightFractionSubstance = new Fraction
                    {
                        Value = weightFractionSubstance,
                        Unit = FractionUnits.Fraction
                    },
                    ProductAmount = new ProductAmount()
                    {
                        Value = productWeightValue,
                        Unit = productWeightUnit
                    },
                    ExposureDuration = new ExposureDuration()
                    {
                        Value = exposureDurationValue,
                        Unit = exposureDurationUnit
                    },
                    InitialMigrationRate = new MigrationRate()
                    {
                        Value = initialMigrationRateValue,
                        Unit = initialMigrationUnit
                    },
                    ContactAreaMouthing = new ContactAreaMouthing()
                    {
                        Value = contactAreaValue,
                        Unit = contactAreaUnit
                    },
                },
                OralAbsorptionRouteInUse = false,
            };
            return scenario;
        }
    }
}