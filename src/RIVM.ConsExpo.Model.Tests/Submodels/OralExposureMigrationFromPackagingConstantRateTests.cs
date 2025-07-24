using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Tests.Helpers;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class OralExposureMigrationFromPackagingConstantRateTests : OralExposureSubModelBase
    {
        [TestMethod]
        public void OralExposureMigrationFromPackagingConstantRateExternalEventDoseTest()
        {
            var scenarioName = "BPA food can";
            var frequencyValue = 1;
            var frequencyUnit = FrequencyUnits.Weekly;
            var substanceConcentrationValue = 3.0E-5; // Unit Microgram/cm^3 is no longer supported
            var substanceConcentrationUnit = DensityUnits.MilligramPerCubicCentimetre;
            var thicknessPackagingValue = 1.0;
            var thicknessPackagingUnit = LengthUnits.Millimetre;
            var contactAreaValue = 400;
            var contactAreaUnit = AreaUnits.SquareCentimetre;
            var packagedAmountValue = 400;
            var packagedAmountUnit = MassUnits.Gram;
            var ingestedAmountValue = 50;
            var ingestedAmountUnit = MassUnits.Gram;
            var migrationRatePackagingValue = 0.25 * 60 * 60 * 24; // Unit mg/sec is no longer supported
            var migrationRatePackagingUnit = MassRateUnits.MilligramPerDay;
            var storageTimeValue = 183.0;
            var storageTimeUnit = DurationUnits.Day;

            var scenario = GetScenarioOralExposureMigrationFromPackagingConstantRate(
                scenarioName,
                frequencyValue, frequencyUnit,
                substanceConcentrationValue, substanceConcentrationUnit,
                thicknessPackagingValue, thicknessPackagingUnit,
                contactAreaValue, contactAreaUnit,
                packagedAmountValue, packagedAmountUnit,
                ingestedAmountValue, ingestedAmountUnit,
                migrationRatePackagingValue, migrationRatePackagingUnit,
                storageTimeValue, storageTimeUnit
            );

            TestOralExposureExternalEventDose(2.5E-6, scenario);
        }

        [TestMethod]
        public void OralExposureMigrationFromPackagingConstantRateExposureFractionTest()
        {
            var scenarioName = "BPA food can";
            var frequencyValue = 2;
            var frequencyUnit = FrequencyUnits.Weekly;
            var substanceConcentrationValue = 3.0E-5; // Unit Microgram/cm^3 is no longer supported
            var substanceConcentrationUnit = DensityUnits.MilligramPerCubicCentimetre;
            var thicknessPackagingValue = 2.0;
            var thicknessPackagingUnit = LengthUnits.Millimetre;
            var contactAreaValue = 400;
            var contactAreaUnit = AreaUnits.SquareCentimetre;
            var packagedAmountValue = 800;
            var packagedAmountUnit = MassUnits.Gram;
            var ingestedAmountValue = 150;
            var ingestedAmountUnit = MassUnits.Gram;
            var migrationRatePackagingValue = 0.25 * 60 * 60 * 24; // Unit mg/sec is no longer supported
            var migrationRatePackagingUnit = MassRateUnits.MilligramPerDay;
            var storageTimeValue = 200.0;
            var storageTimeUnit = DurationUnits.Day;

            var scenario = GetScenarioOralExposureMigrationFromPackagingConstantRate(
                scenarioName,
                frequencyValue, frequencyUnit,
                substanceConcentrationValue, substanceConcentrationUnit,
                thicknessPackagingValue, thicknessPackagingUnit,
                contactAreaValue, contactAreaUnit,
                packagedAmountValue, packagedAmountUnit,
                ingestedAmountValue, ingestedAmountUnit,
                migrationRatePackagingValue, migrationRatePackagingUnit,
                storageTimeValue, storageTimeUnit
            );

            TestOralExposureExposureFraction(null, scenario);
        }

        private ScenarioModel GetScenarioOralExposureMigrationFromPackagingConstantRate(
                string scenarioName,
                double frequencyValue, FrequencyUnits frequencyUnit,
                double substanceConcentrationValue, DensityUnits substanceConcentrationUnit,
                double thicknessPackagingValue, LengthUnits thicknessPackagingUnit,
                double contactAreaValue, AreaUnits contactAreaUnit,
                double packagedAmountValue, MassUnits packagedAmountUnit,
                double ingestedAmountValue, MassUnits ingestedAmountUnit,
                double migrationRatePackagingValue, MassRateUnits migrationRatePackagingUnit,
                double storageTimeValue, DurationUnits storageTimeUnit
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
                    SubmodelType = OralExposureSubmodelTypes.MigrationFromPackagingInstantRelease,
                    SubstanceConcentration = new SubstanceConcentrationPackaging() { Value = substanceConcentrationValue, Unit = substanceConcentrationUnit },
                    ThicknessPackaging = new Thickness() { Value = thicknessPackagingValue, Unit = thicknessPackagingUnit },
                    ContactAreaPackaging = new ContactAreaPackaging() { Value = contactAreaValue, Unit = contactAreaUnit },
                    PackagedAmount = new ProductAmountPackaging() { Value = packagedAmountValue, Unit = packagedAmountUnit },
                    IngestedAmountPackaging = new ProductAmountPackaging() { Value = ingestedAmountValue, Unit = ingestedAmountUnit },
                    MigrationRatePackaging = new MassRatePackaging() { Value = migrationRatePackagingValue, Unit = migrationRatePackagingUnit },
                    StorageTime = new StorageTime() { Value = storageTimeValue, Unit = storageTimeUnit }
                },
                OralAbsorptionRouteInUse = false,
            };

            return scenario;
        }
    }
}