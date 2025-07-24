using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.Output
{
    [TestClass]
    public class DermalExposureOutcomeTests
    {
        private const double BodyWeightValue = 60;

        private const double ExposedAreaValue = 20;

        [TestMethod]
        public void AsExternalEventDoseFromDermalLoadTest()
        {
            var bodyWeight = new BodyWeight { Value = BodyWeightValue, Unit = MassUnits.Kilogram };
            var scenarioFrequency = new Frequency();
            var exposedArea = new ExposedArea { Value = ExposedAreaValue, Unit = AreaUnits.SquareCentimetre };
            double amountOfSubstance = 1;

            var x = new DermalExposureOutcome(bodyWeight, scenarioFrequency, amountOfSubstance, exposedArea);

            const double valueInMgPerSquareCentimetre = 4.7;
            x.Dose = new Dose(valueInMgPerSquareCentimetre, DoseUnits.MgPerSquareCentimetre);

            double expectedExternalEventDose = valueInMgPerSquareCentimetre * ExposedAreaValue / BodyWeightValue;

            TestHelpers.AreEqualDoubles(expectedExternalEventDose, x.AsExternalEventDose.Value.Value);
        }

        [TestMethod]
        public void AsExternalEventDoseFromMgTest()
        {
            var bodyWeight = new BodyWeight { Value = BodyWeightValue, Unit = MassUnits.Kilogram };
            var scenarioFrequency = new Frequency();
            ExposedArea exposedArea = null;
            double amountOfSubstance = 1;

            var x = new DermalExposureOutcome(bodyWeight, scenarioFrequency, amountOfSubstance, exposedArea);

            const double valueInMg = 79.1;
            x.Dose = new Dose(valueInMg, DoseUnits.Mg);

            double expectedExternalEventDose = valueInMg / bodyWeight.InKilogram();

            TestHelpers.AreEqualDoubles(expectedExternalEventDose, x.AsExternalEventDose.Value.Value);
        }

        [TestMethod]
        public void AsDermalLoadFromMgTest()
        {
            var bodyWeight = new BodyWeight { Value = BodyWeightValue, Unit = MassUnits.Kilogram };
            var scenarioFrequency = new Frequency();
            var exposedArea = new ExposedArea { Value = ExposedAreaValue, Unit = AreaUnits.SquareCentimetre };
            double amountOfSubstance = 1;

            var x = new DermalExposureOutcome(bodyWeight, scenarioFrequency, amountOfSubstance, exposedArea);

            const double valueInMg = 16.3;
            x.Dose = new Dose(valueInMg, DoseUnits.Mg);

            double expectedLoad = valueInMg / exposedArea.InSquareCentimetre();

            TestHelpers.AreEqualDoubles(expectedLoad, x.AsDermalLoad.Value.Value);
        }

        [TestMethod]
        public void AsExternalEventDoseFromMgWithoutBodyWeightTest()
        {
            var exposedArea = new ExposedArea { Value = ExposedAreaValue, Unit = AreaUnits.SquareCentimetre };

            var x = new DermalExposureOutcome(null, null, null, exposedArea);

            const double valueInMgPerSquareCentimetre = 0.56;
            x.Dose = new Dose(valueInMgPerSquareCentimetre, DoseUnits.MgPerSquareCentimetre);

            Assert.IsNull(x.AsExternalEventDose.Value);
        }

        [TestMethod]
        public void AsExposureFractionFromExternalEventDoseTest()
        {
            BodyWeight bodyWeight = new BodyWeight { Value = 60, Unit = MassUnits.Kilogram };

            double? amountOfSubstance = 23.4;

            double expectedExposureFraction = 0.369; // = dose * body weight / amount

            var x = new DermalExposureOutcome(bodyWeight, null, amountOfSubstance, null);

            const double valueInMgPerKgBodyWeight = 0.144;
            x.Dose = new Dose(valueInMgPerKgBodyWeight, DoseUnits.MgPerKgBodyWeight);

            TestHelpers.AreEqualDoubles(expectedExposureFraction, x.AsExposureFraction.Value.Value, 1E-2);
        }

        [TestMethod]
        public void AsExposureFractionFromDermalLoadTest()
        {
            double exposedAreaValue = 4.32;
            double? amountOfSubstance = 12.3;
            double expectedExposureFraction = 0.822; // = load * area / amount.

            var exposedArea = new ExposedArea { Value = exposedAreaValue, Unit = AreaUnits.SquareCentimetre };

            var x = new DermalExposureOutcome(null, null, amountOfSubstance, exposedArea);

            const double valueInMgPerSquareCentimetre = 2.34;
            x.Dose = new Dose(valueInMgPerSquareCentimetre, DoseUnits.MgPerSquareCentimetre);

            TestHelpers.AreEqualDoubles(expectedExposureFraction, x.AsExposureFraction.Value.Value, 1E-2);
        }

        [TestMethod]
        public void AsExposureFractionFromExposedMassTest()
        {
            double? amountOfSubstance = 32.1;
            double expectedExposureFraction = 0.177; // = mass / amount.

            var x = new DermalExposureOutcome(null, null, amountOfSubstance, null);

            const double valueInMg = 5.67;
            x.Dose = new Dose(valueInMg, DoseUnits.Mg);

            TestHelpers.AreEqualDoubles(expectedExposureFraction, x.AsExposureFraction.Value.Value, 1E-2);
        }
    }
}