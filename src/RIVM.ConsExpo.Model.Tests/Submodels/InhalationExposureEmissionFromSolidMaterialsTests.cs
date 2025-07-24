using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Tests.Helpers;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class InhalationExposureEmissionFromSolidMaterialsTests : InhalationExposureSubModelBase
    {
        /// <summary>
        /// Test the model for a example scenario from the Emission model.
        /// </summary>
        [TestMethod]
        public void InhalatoryExposureEmissionFromSolidMaterialsTest()
        {
            const double expectedMeanAirConcentration = 87;
            var scenario = GetDefaultScenario();

            TestHelpers.SetupValidateAndTest(scenario, expectedMeanAirConcentration);
        }

        [TestMethod]
        public void InhalatoryExposureEmissionFromSolidMaterialsExposureFractionTest()
        {
            var scenario = GetDefaultScenario();

            TestInhalationExposureExposureFraction(null, scenario);
        }

        [TestMethod]
        public void InhalatoryExposureEmissionFromSolidMaterialsShortDailyExposureDuration()
        {
            const double expectedMeanAirConcentration = 200;
            const double expectedMeanAirConcentrationPeak = 1385;

            var scenario = GetDefaultScenario();

            scenario.InhalationExposure.ReEntry = true;
            scenario.InhalationExposure.EmissionDurationReEntry = new EmissionDurationReEntry { Value = 1, Unit = DurationUnits.Week };
            scenario.InhalationExposure.DailyDuration = new DailyDuration { Value = 0.1, Unit = DailyDurationUnits.MinutesPerDay };

            TestHelpers.SetupValidateAndTest(scenario, expectedMeanAirConcentration, null, expectedMeanAirConcentrationPeak);
        }

        protected static ScenarioModel GetDefaultScenario()
        {
            var scenario = new ScenarioModel
            {
                Name = "Inhalation emission from solid materials",
                Frequency = new Frequency { Value = 1, Unit = FrequencyUnits.Yearly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = InhalationExposureSubmodelTypes.EmissionFromSolidMaterials,
                    ProductSurfaceArea = new ReleaseArea { Value = 10, Unit = AreaUnits.SquareMetre },
                    ProductThickness = new ThicknessForEmission { Value = 10, Unit = LengthUnits.Millimetre },
                    ProductDensity = new DensitySolid { Value = 2, Unit = DensityUnits.GramPerCubicCentimetre },
                    DiffusionCoefficientForEmission = new DiffusionCoefficientForEmission { Value = 3.6e-8, Unit = SurfaceRateUnits.SquareMetrePerHour },
                    WeightFractionSubstanceForEmission = new WeightFractionSubstanceForEmission { Value = 0.03, Unit = FractionUnits.Fraction },
                    ProductAirPartitionCoefficient = new ProductAirPartitionCoefficient { Value = 6000, Unit = Dimensionless.Linear },
                    RoomVolume = new RoomVolume { Value = 50, Unit = VolumeUnits.CubicMetre },
                    VentilationRate = new Rate { Value = 1, Unit = RateUnits.TimesPerHour },
                    InhalationRate = new VolumeRate { Value = 13, Unit = VolumeRateUnits.CubicMetrePerDay },
                    StartExposure = new IntermediateDuration { Value = 200, Unit = DurationUnits.Hour },
                    ExposureDurationForEmissionModel = new IntermediateDuration { Value = 24, Unit = DurationUnits.Hour },
                    MassTransferCoefficient = new MassTransferCoefficient { Value = 0.002, Unit = VelocityUnits.MetrePerSecond }
                },
                Assessment = ScenarioHelper.GetAssessment(65, 0.1)
            };

            scenario.Assessment.Substance = new SubstanceModel();

            return scenario;
        }
    }
}
