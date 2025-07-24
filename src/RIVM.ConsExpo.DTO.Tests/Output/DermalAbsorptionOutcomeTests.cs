using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.Output
{
    /// <summary>
    /// Test for conversions of outcomes. Since, currently, all three routes and the integrated share the same absorption outcome base class, there is no need to test oral, inhalation and integrated as well.
    /// </summary>
    [TestClass]
    public class DermalAbsorptionOutcomeTests
    {
        [TestMethod]
        public void AsInternalEventDoseTestFromMg()
        {
            var bodyWeight = new BodyWeight() { Value = 65, Unit = MassUnits.Kilogram };
            Frequency frequency = null;

            var x = new DermalAbsorptionOutcome(bodyWeight, frequency);

            const double valueInMg = 0.3;
            x.Dose = new Dose(valueInMg, DoseUnits.Mg);

            double expectedInternalEventDose = valueInMg / bodyWeight.InKilogram();

            TestHelpers.AreEqualDoubles(expectedInternalEventDose, x.AsInternalEventDose.Value.Value);
        }

        [TestMethod]
        public void AsInternalYearAverageDoseTestFromMg()
        {
            var bodyWeight = new BodyWeight() { Value = 65, Unit = MassUnits.Kilogram };
            var frequency = new Frequency() { Value = 3, Unit = FrequencyUnits.Weekly };

            var x = new DermalAbsorptionOutcome(bodyWeight, frequency);

            const double valueInMg = 0.3;
            x.Dose = new Dose(valueInMg, DoseUnits.Mg);

            double expectedInternalYearAverageDose = valueInMg * frequency.InTimesPerDay() / bodyWeight.InKilogram();

            TestHelpers.AreEqualDoubles(expectedInternalYearAverageDose, x.AsInternalYearAverageDose.Value.Value);
        }

        [TestMethod]
        public void AsInternalYearAverageDoseTestFromMgWithoutBodyWeight()
        {
            BodyWeight bodyWeight = null;
            var frequency = new Frequency() { Value = 3, Unit = FrequencyUnits.Weekly };

            var x = new DermalAbsorptionOutcome(bodyWeight, frequency);

            const double valueInMg = 0.3;
            x.Dose = new Dose(valueInMg, DoseUnits.Mg);

            Assert.IsNull(x.AsInternalEventDose.Value);
            Assert.IsNull(x.AsInternalYearAverageDose.Value);
        }

        [TestMethod]
        public void AsInternalYearAverageDoseTestFromInternalDoseWithoutBodyWeight()
        {
            BodyWeight bodyWeight = null;
            var frequency = new Frequency() { Value = 3, Unit = FrequencyUnits.Weekly };

            var x = new DermalAbsorptionOutcome(bodyWeight, frequency);

            const double valueInMgPerKgBodyWeight = 7.6;
            x.Dose = new Dose(valueInMgPerKgBodyWeight, DoseUnits.MgPerKgBodyWeight);

            double expectedInternalEventDose = valueInMgPerKgBodyWeight;

            TestHelpers.AreEqualDoubles(expectedInternalEventDose, x.AsInternalEventDose.Value.Value);
        }
    }
}