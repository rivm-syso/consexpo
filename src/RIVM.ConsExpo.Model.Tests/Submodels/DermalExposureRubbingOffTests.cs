using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Tests.Helpers;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class DermalExposureRubbingOffTests : DermalExposureSubModelBase
    {
        [TestMethod]
        public void DermalExposureRubbingOffExposureDurationMoreThanContactDurationTest()
        {
            var scenarioName = "Dusting powder - RubbingOff";
            var frequencyValue = 70;
            var frequencyUnit = FrequencyUnits.Yearly;
            var weightFraction = 0.1;
            var releaseDurationValue = 10;
            var releaseDurationUnit = DurationUnits.Hour;
            var transferCoefficientValue = 0.6;
            var transferCoefficientUnit = SurfaceRateUnits.SquareMetrePerHour;
            var dislodgeableAmountValue = 15.3;
            var dislodgeableAmountUnit = AreaDensityUnits.GramPerSquareMetre;
            var contactDurationValue = 60;
            var contactDurationUnit = DurationUnits.Minute;
            var contactedSurfaceValue = 10000;
            var contactedSurfaceUnit = AreaUnits.SquareCentimetre;

            var scenario = GetScenarioDermalExposureRubbingOff(scenarioName, frequencyValue, frequencyUnit, weightFraction, releaseDurationValue, releaseDurationUnit,
            transferCoefficientValue, transferCoefficientUnit, dislodgeableAmountValue, dislodgeableAmountUnit, contactDurationValue, contactDurationUnit, contactedSurfaceValue, contactedSurfaceUnit);

            scenario.Assessment = ScenarioHelper.GetAssessment(8.69, 0.1);

            TestDermalExposureExternalEventDose(106, scenario);
        }

        [TestMethod]
        public void DermalExposureRubbingOffExposureFractionTest()
        {
            var scenarioName = "CE2015-58956 2020-009a";
            var frequencyValue = 1;
            var frequencyUnit = FrequencyUnits.Monthly;
            var weightFraction = 0.1;
            var releaseDurationValue = 2;
            var releaseDurationUnit = DurationUnits.Day;
            var transferCoefficientValue = 0.03;
            var transferCoefficientUnit = SurfaceRateUnits.SquareMetrePerHour;
            var dislodgeableAmountValue = 15.3;
            var dislodgeableAmountUnit = AreaDensityUnits.GramPerSquareMetre;
            var contactDurationValue = 60;
            var contactDurationUnit = DurationUnits.Minute;
            var contactedSurfaceValue = 1;
            var contactedSurfaceUnit = AreaUnits.SquareMetre;

            var scenario = GetScenarioDermalExposureRubbingOff(scenarioName, frequencyValue, frequencyUnit, weightFraction, releaseDurationValue, releaseDurationUnit,
                transferCoefficientValue, transferCoefficientUnit, dislodgeableAmountValue, dislodgeableAmountUnit, contactDurationValue, contactDurationUnit, contactedSurfaceValue, contactedSurfaceUnit);

            scenario.Assessment = ScenarioHelper.GetAssessment(8.69, 0.1);

            TestDermalExposureExposureFraction(null, scenario);
        }

        private ScenarioModel GetScenarioDermalExposureRubbingOff(string scenarioName, double frequencyValue, FrequencyUnits frequencyUnit, double weightFraction,
            double releaseDurationValue, DurationUnits releaseDurationUnit,
            double transferCoefficientValue, SurfaceRateUnits transferCoefficientUnit,
            double dislodgeableAmountValue, AreaDensityUnits dislodgeableAmountUnit, double contactDurationValue, DurationUnits contactDurationUnit,
            double contactedSurfaceValue, AreaUnits contactedSurfaceUnit)
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
                    SubmodelType = DermalExposureSubmodelTypes.RubbingOff,
                    WeightFractionSubstance = new Fraction
                    {
                        Value = weightFraction,
                        Unit = FractionUnits.Fraction
                    },
                    ReleaseDuration = new ReleaseDuration()
                    {
                        Value = releaseDurationValue,
                        Unit = releaseDurationUnit
                    },
                    TransferCoefficient = new TransferCoefficient()
                    {
                        Value = transferCoefficientValue,
                        Unit = transferCoefficientUnit
                    },
                    DislodgeableAmount = new AreaDensity()
                    {
                        Value = dislodgeableAmountValue,
                        Unit = dislodgeableAmountUnit
                    },
                    ContactDuration = new ExposureDuration()
                    {
                        Value = contactDurationValue,
                        Unit = contactDurationUnit
                    },
                    ContactedSurface = new RubbingContactArea()
                    {
                        Value = contactedSurfaceValue,
                        Unit = contactedSurfaceUnit
                    }
                },
                DermalAbsorptionRouteInUse = false,
            };
            return scenario;
        }
    }
}