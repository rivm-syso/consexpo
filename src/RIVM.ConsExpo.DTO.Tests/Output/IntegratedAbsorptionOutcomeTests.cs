using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.Output
{
    [TestClass]
    public class IntegratedAbsorptionOutcomeTests
    {
        [TestMethod]
        public void IntegratedAbsorptionOutcomeWeeklyTest()
        {
            const double bodyWeightValue = 60;
            const double inhalationMass = 10.0;
            const double dermalMass = 20.0;
            const double doseReentry = 3.0;
            const double daysPerWeek = 7.0;
            const double timesPerWeek = 2.0;

            var bodyWeight = new BodyWeight { Value = bodyWeightValue, Unit = MassUnits.Kilogram };
            var frequency = new Frequency { Value = timesPerWeek, Unit = FrequencyUnits.Weekly };
            var target = new IntegratedAbsorptionOutcome(bodyWeight, frequency)
            {
                InhalationAbsorptionOutcome = new InhalationAbsorptionOutcome(bodyWeight, frequency, true),
                DermalAbsorptionOutcome = new DermalAbsorptionOutcome(bodyWeight, frequency),
                OralAbsorptionOutcome = new OralAbsorptionOutcome(bodyWeight, frequency)
            };

            target.InhalationAbsorptionOutcome.Dose = new Dose(inhalationMass, DoseUnits.Mg);
            target.InhalationAbsorptionOutcome.InternalYearAverageDoseReEntry = new Dose(doseReentry, DoseUnits.MgPerKgBodyWeightPerDay);
            target.DermalAbsorptionOutcome.Dose = new Dose(dermalMass, DoseUnits.Mg);
            target.OralAbsorptionOutcome.Dose = new Dose(null, DoseUnits.Mg);

            TestHelpers.AreEqual((inhalationMass + dermalMass) / bodyWeightValue, target.AsInternalEventDose.Value, 0.01);
            TestHelpers.AreEqual((inhalationMass + dermalMass) / bodyWeightValue, target.AsInternalDayDose.Value, 0.01);
            TestHelpers.AreEqual(doseReentry + dermalMass * timesPerWeek / (bodyWeightValue * daysPerWeek), target.AsInternalYearAverageDose.Value, 0.01);
        }

        [TestMethod]
        public void IntegratedAbsorptionOutcomeTwicePerDayTest()
        {
            const double bodyWeightValue = 70;
            const double inhalationMass = 15.0;
            const double dermalMass = 25.0;
            const double doseReentry = 3.0;
            const double timesPerDay = 2.0;

            var bodyWeight = new BodyWeight { Value = bodyWeightValue, Unit = MassUnits.Kilogram };
            var frequency = new Frequency { Value = timesPerDay, Unit = FrequencyUnits.Daily };
            var target = new IntegratedAbsorptionOutcome(bodyWeight, frequency)
            {
                InhalationAbsorptionOutcome = new InhalationAbsorptionOutcome(bodyWeight, frequency, true),
                DermalAbsorptionOutcome = new DermalAbsorptionOutcome(bodyWeight, frequency),
                OralAbsorptionOutcome = new OralAbsorptionOutcome(bodyWeight, frequency)
            };

            target.InhalationAbsorptionOutcome.Dose = new Dose(inhalationMass, DoseUnits.Mg);
            target.InhalationAbsorptionOutcome.InternalYearAverageDoseReEntry = new Dose(doseReentry, DoseUnits.MgPerKgBodyWeightPerDay);
            target.DermalAbsorptionOutcome.Dose = new Dose(dermalMass, DoseUnits.Mg);
            target.OralAbsorptionOutcome.Dose = new Dose(null, DoseUnits.Mg);

            TestHelpers.AreEqual((inhalationMass + dermalMass) / bodyWeightValue, target.AsInternalEventDose.Value, 0.01);
            TestHelpers.AreEqual(timesPerDay * (inhalationMass + dermalMass) / bodyWeightValue, target.AsInternalDayDose.Value, 0.01);
            TestHelpers.AreEqual(doseReentry + timesPerDay * dermalMass / bodyWeightValue, target.AsInternalYearAverageDose.Value, 0.01);
        }
    }
}