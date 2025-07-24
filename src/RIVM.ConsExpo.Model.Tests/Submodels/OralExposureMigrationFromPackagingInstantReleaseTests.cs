using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Tests.Helpers;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class OralExposureMigrationFromPackagingInstantReleaseTests : OralExposureSubModelBase
    {
        [TestMethod]
        public void OralExposureMigrationFromPackagingInstantReleaseExternalEventDoseTest1()
        {
            var scenarioName = "Diner";
            var frequencyValue = 1;
            var frequencyUnit = FrequencyUnits.Daily;
            var substanceConcentrationValue = 0.78;
            var substanceConcentrationUnit = DensityUnits.GramPerCubicCentimetre;
            var thicknessPackagingValue = 1.5;
            var thicknessPackagingUnit = LengthUnits.Millimetre;
            var contactAreaValue = 10;
            var contactAreaUnit = AreaUnits.SquareCentimetre;
            var packagedAmountValue = 45;
            var packagedAmountUnit = MassUnits.Gram;
            var ingestedAmountValue = 10;
            var ingestedAmountUnit = MassUnits.Gram;

            var scenario = GetScenarioOralExposureMigrationFromPackagingInstantRelease(
                scenarioName,
                frequencyValue, frequencyUnit,
                substanceConcentrationValue, substanceConcentrationUnit,
                thicknessPackagingValue, thicknessPackagingUnit,
                contactAreaValue, contactAreaUnit,
                packagedAmountValue, packagedAmountUnit,
                ingestedAmountValue, ingestedAmountUnit);

            TestOralExposureExternalEventDose(4.33, scenario);
        }

        [TestMethod]
        public void OralExposureMigrationFromPackagingInstantReleaseExternalEventDoseTest2()
        {
            var scenarioName = "BPA food can";
            var frequencyValue = 1;
            var frequencyUnit = FrequencyUnits.Weekly;
            var substanceConcentrationValue = 3.0E-4; // Unit micro-gram/cm^3 is no longer supported
            var substanceConcentrationUnit = DensityUnits.MilligramPerCubicCentimetre;
            var thicknessPackagingValue = 1.0;
            var thicknessPackagingUnit = LengthUnits.Millimetre;
            var contactAreaValue = 500;
            var contactAreaUnit = AreaUnits.SquareCentimetre;
            var packagedAmountValue = 400;
            var packagedAmountUnit = MassUnits.Gram;
            var ingestedAmountValue = 50;
            var ingestedAmountUnit = MassUnits.Gram;

            var scenario = GetScenarioOralExposureMigrationFromPackagingInstantRelease(
                scenarioName,
                frequencyValue, frequencyUnit,
                substanceConcentrationValue, substanceConcentrationUnit,
                thicknessPackagingValue, thicknessPackagingUnit,
                contactAreaValue, contactAreaUnit,
                packagedAmountValue, packagedAmountUnit,
                ingestedAmountValue, ingestedAmountUnit);

            TestOralExposureExternalEventDose(3.13E-5, scenario);
        }

        [TestMethod]
        public void OralExposureMigrationFromPackagingInstantReleaseExposureFractionTest()
        {
            var scenarioName = "Diner";
            var frequencyValue = 1;
            var frequencyUnit = FrequencyUnits.Daily;
            var substanceConcentrationValue = 0.78;
            var substanceConcentrationUnit = DensityUnits.GramPerCubicCentimetre;
            var thicknessPackagingValue = 1.5;
            var thicknessPackagingUnit = LengthUnits.Millimetre;
            var contactAreaValue = 10;
            var contactAreaUnit = AreaUnits.SquareCentimetre;
            var packagedAmountValue = 45;
            var packagedAmountUnit = MassUnits.Gram;
            var ingestedAmountValue = 10;
            var ingestedAmountUnit = MassUnits.Gram;

            var scenario = GetScenarioOralExposureMigrationFromPackagingInstantRelease(
                scenarioName,
                frequencyValue, frequencyUnit,
                substanceConcentrationValue, substanceConcentrationUnit,
                thicknessPackagingValue, thicknessPackagingUnit,
                contactAreaValue, contactAreaUnit,
                packagedAmountValue, packagedAmountUnit,
                ingestedAmountValue, ingestedAmountUnit);

            TestOralExposureExposureFraction(null, scenario);
        }

        private ScenarioModel GetScenarioOralExposureMigrationFromPackagingInstantRelease(
                string scenarioName,
                double frequencyValue, FrequencyUnits frequencyUnit,
                double substanceConcentrationValue, DensityUnits substanceConcentrationUnit,
                double thicknessPackagingValue, LengthUnits thicknessPackagingUnit,
                double contactAreaValue, AreaUnits contactAreaUnit,
                double packagedAmountValue, MassUnits packagedAmountUnit,
                double ingestedAmountValue, MassUnits ingestedAmountUnit
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
                    IngestedAmountPackaging = new ProductAmountPackaging() { Value = ingestedAmountValue, Unit = ingestedAmountUnit }
                },
                OralAbsorptionRouteInUse = false,
            };

            return scenario;
        }
    }
}