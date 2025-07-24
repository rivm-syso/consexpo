using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Submodels;
using RIVM.ConsExpo.Model.Tests.Helpers;
using RIVM.ConsExpo.TestFacilities;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class InhalationExposureSpraySprayingTests
    {
        [TestMethod]
        public void SpraySpraying_Test()
        {
            var submodelType = InhalationExposureSubmodelTypes.SpraySpraying;

            double expectedMeanAirConcentration = 0.448;

            var scenario = new ScenarioModel
            {
                Name = "inhalation exposure spray spraying",
                Frequency = new Frequency { Value = 90, Unit = FrequencyUnits.Yearly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,

                    SprayDuration = new SprayDuration { Value = 0.33, Unit = DurationUnits.Minute },
                    ExposureDuration = new ExposureDuration { Value = 240, Unit = DurationUnits.Minute },
                    RoomVolume = new RoomVolume { Value = 58, Unit = VolumeUnits.CubicMetre },
                    RoomHeight = new Height { Value = 2.5, Unit = LengthUnits.Metre },
                    VentilationRate = new Rate { Value = 0.5, Unit = RateUnits.TimesPerHour },

                    SprayingTowardsPerson = false,

                    MassGenerationRate = new MassGenerationRate { Value = 1.1, Unit = MassRateUnits.GramPerSecond },
                    AirborneFraction = new Fraction
                    {
                        Value = 0.3,
                        Unit = FractionUnits.Fraction
                    },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 0.1,
                        Unit = FractionUnits.Fraction
                    },
                    DensityNonVolatile = new DensityNonVolatile { Value = 1.8, Unit = DensityUnits.GramPerCubicCentimetre },

                    AerosolDiameterDistributionType = SizeDistributionTypes.LogNormal,
                    MedianDiameter = new FixedDiameter { Value = 15, Unit = LengthUnits.Micrometre },
                    ArithmicCoefficientOfVariation = 1.2,
                    MaximumDiameter = new FixedDiameter { Value = 50, Unit = LengthUnits.Micrometre },

                    InhalationCutOffDiameter = new Diameter { Value = 15, Unit = LengthUnits.Micrometre }
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.1);

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 222, Unit = MolecularWeightUnits.GramPerMol },
                CASNumber = "1181081-51-5"
            };

            TestHelpers.SetupValidateAndTest(scenario, expectedMeanAirConcentration);
        }

        [TestMethod]
        public void SpraySpraying_NormalDistributedTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.SpraySpraying;

            double expectedMeanAirConcentration = 0.121;

            var scenario = new ScenarioModel
            {
                Name = "inhalation exposure spray spraying",
                Frequency = new Frequency { Value = 90, Unit = FrequencyUnits.Yearly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,

                    SprayDuration = new SprayDuration { Value = 0.33, Unit = DurationUnits.Minute },
                    ExposureDuration = new ExposureDuration { Value = 240, Unit = DurationUnits.Minute },
                    RoomVolume = new RoomVolume { Value = 58, Unit = VolumeUnits.CubicMetre },
                    RoomHeight = new Height { Value = 2.5, Unit = LengthUnits.Metre },
                    VentilationRate = new Rate { Value = 0.5, Unit = RateUnits.TimesPerHour },

                    SprayingTowardsPerson = false,

                    MassGenerationRate = new MassGenerationRate { Value = 1.1, Unit = MassRateUnits.GramPerSecond },
                    AirborneFraction = new Fraction
                    {
                        Value = 0.3,
                        Unit = FractionUnits.Fraction
                    },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 0.1,
                        Unit = FractionUnits.Fraction
                    },
                    DensityNonVolatile = new DensityNonVolatile { Value = 1.8, Unit = DensityUnits.GramPerCubicCentimetre },

                    AerosolDiameterDistributionType = SizeDistributionTypes.Normal,
                    MeanDiameter = new FixedDiameter { Value = 15, Unit = LengthUnits.Micrometre },
                    StandardDeviation = new DiameterStandardDeviation { Value = 3.0, Unit = LengthUnits.Micrometre },
                    MaximumDiameter = new FixedDiameter { Value = 50, Unit = LengthUnits.Micrometre },

                    InhalationCutOffDiameter = new Diameter { Value = 15, Unit = LengthUnits.Micrometre }
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.1);

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 222, Unit = MolecularWeightUnits.GramPerMol },
                CASNumber = "1181081-51-5"
            };

#warning To Do: investigate: Results of this test differ more than expected, but no extremely so.
            // Assert.Inconclusive("Test fails with expected 0.121 and actual 0.129528504635801. Is this too much?");
            TestHelpers.SetupValidateAndTest(scenario, expectedMeanAirConcentration);
        }

        [TestMethod]
        public void SpraySpraying_TowardsPersonTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.SpraySpraying;

            double expectedMeanAirConcentration = 24.3;

            var scenario = new ScenarioModel
            {
                Name = "inhalation exposure spray spraying",
                Frequency = new Frequency { Value = 1, Unit = FrequencyUnits.Daily },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,

                    SprayDuration = new SprayDuration { Value = 50, Unit = DurationUnits.Second },
                    ExposureDuration = new ExposureDuration { Value = 5, Unit = DurationUnits.Minute },
                    RoomVolume = new RoomVolume { Value = 10, Unit = VolumeUnits.CubicMetre },
                    RoomHeight = new Height { Value = 2.5, Unit = LengthUnits.Metre },
                    VentilationRate = new Rate { Value = 2, Unit = RateUnits.TimesPerHour },

                    SprayingTowardsPerson = true,
                    CloudVolume = new CloudVolume { Value = 0.0625, Unit = VolumeUnits.CubicMetre },

                    MassGenerationRate = new MassGenerationRate { Value = 0.45, Unit = MassRateUnits.GramPerSecond },
                    AirborneFraction = new Fraction
                    {
                        Value = 0.9,
                        Unit = FractionUnits.Fraction
                    },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 0.03,
                        Unit = FractionUnits.Fraction
                    },
                    DensityNonVolatile = new DensityNonVolatile { Value = 1.8, Unit = DensityUnits.GramPerCubicCentimetre },

                    AerosolDiameterDistributionType = SizeDistributionTypes.LogNormal,
                    MedianDiameter = new FixedDiameter { Value = 15, Unit = LengthUnits.Micrometre },
                    ArithmicCoefficientOfVariation = 1.2,
                    MaximumDiameter = new FixedDiameter { Value = 50, Unit = LengthUnits.Micrometre },

                    InhalationCutOffDiameter = new Diameter { Value = 10, Unit = LengthUnits.Micrometre }
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(1, 0.1);

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 222, Unit = MolecularWeightUnits.GramPerMol },
                CASNumber = "1181081-51-5"
            };

            TestHelpers.SetupValidateAndTest(scenario, expectedMeanAirConcentration);
        }

        [TestMethod]
        public void SpraySpraying_ShortExposureTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.SpraySpraying;

            double expectedMeanAirConcentration = 58.5;

            var scenario = new ScenarioModel
            {
                Name = "inhalation exposure spray spraying",
                Frequency = new Frequency { Value = 90, Unit = FrequencyUnits.Yearly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,

                    SprayDuration = new SprayDuration { Value = 10, Unit = DurationUnits.Minute },
                    ExposureDuration = new ExposureDuration { Value = 9, Unit = DurationUnits.Minute },
                    RoomVolume = new RoomVolume { Value = 58, Unit = VolumeUnits.CubicMetre },
                    RoomHeight = new Height { Value = 2.5, Unit = LengthUnits.Metre },
                    VentilationRate = new Rate { Value = 0.5, Unit = RateUnits.TimesPerHour },

                    SprayingTowardsPerson = false,

                    MassGenerationRate = new MassGenerationRate { Value = 1.1, Unit = MassRateUnits.GramPerSecond },
                    AirborneFraction = new Fraction
                    {
                        Value = 0.3,
                        Unit = FractionUnits.Fraction
                    },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 0.1,
                        Unit = FractionUnits.Fraction
                    },
                    DensityNonVolatile = new DensityNonVolatile { Value = 1.8, Unit = DensityUnits.GramPerCubicCentimetre },

                    AerosolDiameterDistributionType = SizeDistributionTypes.LogNormal,
                    MedianDiameter = new FixedDiameter { Value = 15, Unit = LengthUnits.Micrometre },
                    ArithmicCoefficientOfVariation = 1.2,
                    MaximumDiameter = new FixedDiameter { Value = 50, Unit = LengthUnits.Micrometre },

                    InhalationCutOffDiameter = new Diameter { Value = 15, Unit = LengthUnits.Micrometre }
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.1);

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 222, Unit = MolecularWeightUnits.GramPerMol },
                CASNumber = "1181081-51-5"
            };

            TestHelpers.SetupValidateAndTest(scenario, expectedMeanAirConcentration);
        }

        [TestMethod]
        public void SpraySpraying_TowardsPersonShortExposureTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.SpraySpraying;

            double expectedMeanAirConcentration = 95.7;

            var scenario = new ScenarioModel
            {
                Name = "inhalation exposure spray spraying",
                Frequency = new Frequency { Value = 1, Unit = FrequencyUnits.Daily },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,

                    SprayDuration = new SprayDuration { Value = 10, Unit = DurationUnits.Minute },
                    ExposureDuration = new ExposureDuration { Value = 9, Unit = DurationUnits.Minute },
                    RoomVolume = new RoomVolume { Value = 10, Unit = VolumeUnits.CubicMetre },
                    RoomHeight = new Height { Value = 2.5, Unit = LengthUnits.Metre },
                    VentilationRate = new Rate { Value = 2, Unit = RateUnits.TimesPerHour },

                    SprayingTowardsPerson = true,
                    CloudVolume = new CloudVolume { Value = 0.0625, Unit = VolumeUnits.CubicMetre },

                    MassGenerationRate = new MassGenerationRate { Value = 0.45, Unit = MassRateUnits.GramPerSecond },
                    AirborneFraction = new Fraction
                    {
                        Value = 0.9,
                        Unit = FractionUnits.Fraction
                    },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 0.03,
                        Unit = FractionUnits.Fraction
                    },
                    DensityNonVolatile = new DensityNonVolatile { Value = 1.8, Unit = DensityUnits.GramPerCubicCentimetre },

                    AerosolDiameterDistributionType = SizeDistributionTypes.LogNormal,
                    MedianDiameter = new FixedDiameter { Value = 15, Unit = LengthUnits.Micrometre },
                    ArithmicCoefficientOfVariation = 1.2,
                    MaximumDiameter = new FixedDiameter { Value = 50, Unit = LengthUnits.Micrometre },

                    InhalationCutOffDiameter = new Diameter { Value = 10, Unit = LengthUnits.Micrometre }
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(1, 0.1);

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 222, Unit = MolecularWeightUnits.GramPerMol },
                CASNumber = "1181081-51-5"
            };

#warning To Do: investigate: Results of this test differ more than expected, but no extremely so.
            //     Assert.Inconclusive("The outcome of this test differs slightly more from the ConsExpo 4 outcome than the allowed tolerance. Needs further investigation.");
            TestHelpers.SetupValidateAndTest(scenario, expectedMeanAirConcentration);
        }

        [TestMethod]
        public void SpraySpraying_LowEliminiationTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.SpraySpraying;

            double expectedMeanAirConcentration = 5.62;

            var scenario = new ScenarioModel
            {
                Name = "inhalation exposure spray spraying",
                Frequency = new Frequency { Value = 90, Unit = FrequencyUnits.Yearly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,

                    SprayDuration = new SprayDuration { Value = 0.33, Unit = DurationUnits.Minute },
                    ExposureDuration = new ExposureDuration { Value = 240, Unit = DurationUnits.Minute },
                    RoomVolume = new RoomVolume { Value = 58, Unit = VolumeUnits.CubicMetre },
                    RoomHeight = new Height { Value = 1E14, Unit = LengthUnits.Metre },
                    VentilationRate = new Rate { Value = 0.0, Unit = RateUnits.TimesPerHour },

                    SprayingTowardsPerson = false,

                    MassGenerationRate = new MassGenerationRate { Value = 1.1, Unit = MassRateUnits.GramPerSecond },
                    AirborneFraction = new Fraction
                    {
                        Value = 0.3,
                        Unit = FractionUnits.Fraction
                    },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 0.1,
                        Unit = FractionUnits.Fraction
                    },
                    DensityNonVolatile = new DensityNonVolatile { Value = 1.8, Unit = DensityUnits.GramPerCubicCentimetre },

                    AerosolDiameterDistributionType = SizeDistributionTypes.LogNormal,
                    MedianDiameter = new FixedDiameter { Value = 15, Unit = LengthUnits.Micrometre },
                    ArithmicCoefficientOfVariation = 1.2,
                    MaximumDiameter = new FixedDiameter { Value = 50, Unit = LengthUnits.Micrometre },

                    InhalationCutOffDiameter = new Diameter { Value = 15, Unit = LengthUnits.Micrometre }
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.1);

            //Assert.Inconclusive("Rounding errors may occur when elimination is low. Required further investigation.");
            TestHelpers.SetupValidateAndTest(scenario, expectedMeanAirConcentration);
        }

        [TestMethod]
        public void SpraySpraying_NoEliminationTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.SpraySpraying;

            double expectedMeanAirConcentration = 5.66;

            var scenario = new ScenarioModel
            {
                Name = "inhalation exposure spray spraying",
                Frequency = new Frequency { Value = 90, Unit = FrequencyUnits.Yearly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,

                    SprayDuration = new SprayDuration { Value = 0.33, Unit = DurationUnits.Minute },
                    ExposureDuration = new ExposureDuration { Value = 240, Unit = DurationUnits.Minute },
                    RoomVolume = new RoomVolume { Value = 58, Unit = VolumeUnits.CubicMetre },
                    RoomHeight = new Height { Value = 1E6, Unit = LengthUnits.Metre },
                    VentilationRate = new Rate { Value = 0.0, Unit = RateUnits.TimesPerHour },

                    SprayingTowardsPerson = false,

                    MassGenerationRate = new MassGenerationRate { Value = 1.1, Unit = MassRateUnits.GramPerSecond },
                    AirborneFraction = new Fraction
                    {
                        Value = 0.3,
                        Unit = FractionUnits.Fraction
                    },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 0.1,
                        Unit = FractionUnits.Fraction
                    },
                    DensityNonVolatile = new DensityNonVolatile { Value = 1.8, Unit = DensityUnits.GramPerCubicCentimetre },

                    AerosolDiameterDistributionType = SizeDistributionTypes.LogNormal,
                    MedianDiameter = new FixedDiameter { Value = 15, Unit = LengthUnits.Micrometre },
                    ArithmicCoefficientOfVariation = 1.2,
                    MaximumDiameter = new FixedDiameter { Value = 50, Unit = LengthUnits.Micrometre },

                    InhalationCutOffDiameter = new Diameter { Value = 15, Unit = LengthUnits.Micrometre }
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.1);

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 222, Unit = MolecularWeightUnits.GramPerMol },
                CASNumber = "1181081-51-5"
            };

            TestHelpers.SetupValidateAndTest(scenario, expectedMeanAirConcentration);
        }

        /// <summary>
        /// Test for a discrepancy found in sprint 11 tests: CE2015-36221  US71 - Bevindingen uit sprint 11
        /// </summary>
        [TestMethod]
        public void SpraySpraying_TestCE4Discrepancy()
        {
            var submodelType = InhalationExposureSubmodelTypes.SpraySpraying;

            double expectedMeanAirConcentration = 0.708;

            var scenario = new ScenarioModel
            {
                Name = "inhalation exposure spray spraying",
                Frequency = new Frequency { Value = 1, Unit = FrequencyUnits.Weekly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,

                    SprayDuration = new SprayDuration { Value = 5, Unit = DurationUnits.Second },
                    ExposureDuration = new ExposureDuration { Value = 1, Unit = DurationUnits.Hour },
                    RoomVolume = new RoomVolume { Value = 15, Unit = VolumeUnits.CubicMetre },
                    RoomHeight = new Height { Value = 2, Unit = LengthUnits.Metre },
                    VentilationRate = new Rate { Value = 1, Unit = RateUnits.TimesPerHour },

                    SprayingTowardsPerson = false,

                    MassGenerationRate = new MassGenerationRate { Value = 1.5, Unit = MassRateUnits.GramPerSecond },
                    AirborneFraction = new Fraction { Value = 0.1, Unit = FractionUnits.Fraction },
                    WeightFractionSubstance = new Fraction { Value = 0.03, Unit = FractionUnits.Fraction },
                    DensityNonVolatile = new DensityNonVolatile { Value = 2, Unit = DensityUnits.GramPerCubicCentimetre },

                    AerosolDiameterDistributionType = SizeDistributionTypes.LogNormal,
                    MedianDiameter = new FixedDiameter { Value = 2, Unit = LengthUnits.Micrometre },
                    ArithmicCoefficientOfVariation = 1,
                    MaximumDiameter = new FixedDiameter { Value = 10, Unit = LengthUnits.Micrometre },

                    InhalationCutOffDiameter = new Diameter { Value = 10, Unit = LengthUnits.Micrometre }
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.1);

            TestHelpers.SetupValidateAndTest(scenario, expectedMeanAirConcentration);
        }

        [TestMethod]
        public void SpraySpraying_PeakIntervalAwayFromPersonTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.SpraySpraying;

            ScenarioModel scenario = GetScenarioForSprayingPeakInterval(submodelType);

            scenario.Name = "inhalation exposure spray spraying peak interval away from person";
            scenario.InhalationExposure.SprayingTowardsPerson = false;
            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.1);

            CheckPeakInterval(scenario);
        }

        [TestMethod]
        public void SpraySpraying_PeakIntervalTowardsPersonTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.SpraySpraying;

            ScenarioModel scenario = GetScenarioForSprayingPeakInterval(submodelType);

            scenario.Name = "inhalation exposure spray spraying peak interval towards person";
            scenario.InhalationExposure.SprayingTowardsPerson = true;
            scenario.InhalationExposure.CloudVolume = new CloudVolume
            {
                Value = 0.6,
                Unit = VolumeUnits.CubicMetre
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.1);

            CheckPeakInterval(scenario);
        }

        [TestMethod]
        public void SpraySpraying_NonParametricNormalDistributionTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.SpraySpraying;

            double expectedMeanAirConcentration = 1.8E1;
            double expectedPeakAirConcentration = 2.2E1;

            var scenario = new ScenarioModel
            {
                Name = "inhalation exposure spray spraying",
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,

                    SprayDuration = new SprayDuration { Value = 10, Unit = DurationUnits.Second },
                    ExposureDuration = new ExposureDuration { Value = 20, Unit = DurationUnits.Minute },
                    RoomVolume = new RoomVolume { Value = 20, Unit = VolumeUnits.CubicMetre },
                    RoomHeight = new Height { Value = 2.5, Unit = LengthUnits.Metre },
                    VentilationRate = new Rate { Value = 1.0, Unit = RateUnits.TimesPerHour },

                    SprayingTowardsPerson = false,

                    MassGenerationRate = new MassGenerationRate { Value = 3, Unit = MassRateUnits.GramPerSecond },
                    AirborneFraction = new Fraction
                    {
                        Value = 0.3,
                        Unit = FractionUnits.Fraction
                    },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 0.1,
                        Unit = FractionUnits.Fraction
                    },
                    DensityNonVolatile = new DensityNonVolatile { Value = 1.4, Unit = DensityUnits.GramPerCubicCentimetre },

                    AerosolDiameterDistributionType = SizeDistributionTypes.NonParametric,
                    NonParametricSizeDistribution = GetNonParametricSizeDistribution(),

                    MeanDiameter = new FixedDiameter { Value = 10, Unit = LengthUnits.Micrometre },
                    StandardDeviation = new DiameterStandardDeviation { Value = 1.0, Unit = LengthUnits.Micrometre },
                    MaximumDiameter = new FixedDiameter { Value = 50, Unit = LengthUnits.Micrometre },

                    InhalationCutOffDiameter = new Diameter { Value = 15, Unit = LengthUnits.Micrometre }
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.1);

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 222, Unit = MolecularWeightUnits.GramPerMol },
                CASNumber = "1181081-51-5"
            };

#warning To Do: investigate: Results of this test differ more than expected, but no extremely so.
            // Assert.Inconclusive("Test fails with expected 0.121 and actual 0.129528504635801. Is this too much?");
            TestHelpers.SetupValidateAndTest(scenario, expectedMeanAirConcentration, expectedPeakAirConcentration);
        }

        private NonParametricSizeDistribution GetNonParametricSizeDistribution()
        {
            var nonParametricSizeDistribution = new NonParametricSizeDistribution
            {
                Name = "Test"
            };

            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 0.5, RelativeMass = 4.54277E-22 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 1, RelativeMass = 5.25072E-20 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 1.5, RelativeMass = 4.72655E-18 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 2, RelativeMass = 3.31357E-16 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 2.5, RelativeMass = 1.80915E-14 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 3, RelativeMass = 7.69269E-13 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 3.5, RelativeMass = 2.54747E-11 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 4, RelativeMass = 6.57001E-10 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 4.5, RelativeMass = 1.31962E-08 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 5, RelativeMass = 2.06424E-07 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 5.5, RelativeMass = 2.51475E-06 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 6, RelativeMass = 2.38593E-05 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 6.5, RelativeMass = 0.000176298 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 7, RelativeMass = 0.001014524 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 7.5, RelativeMass = 0.004546781 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 8, RelativeMass = 0.015869826 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 8.5, RelativeMass = 0.043138659 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 9, RelativeMass = 0.091324543 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 9.5, RelativeMass = 0.150568716 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 10, RelativeMass = 0.193334058 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 10.5, RelativeMass = 0.193334058 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 11, RelativeMass = 0.150568716 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 11.5, RelativeMass = 0.091324543 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 12, RelativeMass = 0.043138659 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 12.5, RelativeMass = 0.015869826 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 13, RelativeMass = 0.004546781 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 13.5, RelativeMass = 0.001014524 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 14, RelativeMass = 0.000176298 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 14.5, RelativeMass = 2.38593E-05 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 15, RelativeMass = 2.51475E-06 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 15.5, RelativeMass = 2.06424E-07 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 16, RelativeMass = 1.31962E-08 });
            nonParametricSizeDistribution.Bins.Add(new NonParametricSizeBin { UpperBound = 16.5, RelativeMass = 6.57001E-10 });

            return nonParametricSizeDistribution;
        }

        private static void CheckPeakInterval(ScenarioModel scenario)
        {
            const double ExpectedPeakIntervalDuration = 15;

            for (int i = 0; i < 100; i++)
            {
                // This sampling of distributed parameters must be consistent with the ones specified as distributed in the scenario.
                scenario.InhalationExposure.ExposureDuration.Sample();
                scenario.InhalationExposure.SprayDuration.Sample();

                double exposureDurationInMinutes = scenario.InhalationExposure.ExposureDuration.InMinutes();
                var inhalatoryExposureSimulation = new InhalationExposureSpraySpraying(scenario);

                inhalatoryExposureSimulation.PrepareTimeSeries(scenario.InhalationExposure.ExposureDuration.AsTime());

                TimeInterval actualPeakInterval = inhalatoryExposureSimulation.PeakInterval();

                if (exposureDurationInMinutes > ExpectedPeakIntervalDuration)
                {
                    Assert.IsTrue(Comparisons.AlmostEqualMagnitude(ExpectedPeakIntervalDuration, actualPeakInterval.DurationInMinutes),
                        $"The actual peak interval duration {actualPeakInterval.DurationInMinutes} differs from the expected value {ExpectedPeakIntervalDuration} with more than the allowed tolerance.");
                }
                else
                {
                    Assert.IsTrue(Comparisons.AlmostEqualMagnitude(exposureDurationInMinutes, actualPeakInterval.DurationInMinutes),
                        $"The actual peak interval duration {actualPeakInterval.DurationInMinutes} differs from exposure duration {exposureDurationInMinutes} with more than the allowed tolerance.");
                }

                if (actualPeakInterval.EndTime < scenario.InhalationExposure.ExposureDuration.AsTime())
                {
                    double airConcentrationAtPeakStart = inhalatoryExposureSimulation.InstantaneousAirConcentration(actualPeakInterval.StartTime).AsMilligramPerCubicMetre().Value.Value;

                    double airConcentrationAtPeakEnd = inhalatoryExposureSimulation.InstantaneousAirConcentration(actualPeakInterval.EndTime).AsMilligramPerCubicMetre().Value.Value;

                    // The model has a discontinuity at t=0 if spraying towards person is True.
                    if (!scenario.InhalationExposure.SprayingTowardsPerson || actualPeakInterval.StartTime.InSeconds() > 0)
                    {
                        Assert.IsTrue(Comparisons.AlmostEqualMagnitude(airConcentrationAtPeakStart, airConcentrationAtPeakEnd, 0.2),
                            $"The air concentration at the start of the peak interval {airConcentrationAtPeakStart} differs air concentration at the end of the peak interval {airConcentrationAtPeakEnd} with more than the allowed tolerance.");
                    }
                }
            }
        }

        [TestMethod]
        public void SpraySpraying_ExposureFractionTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.SpraySpraying;

            double expectedMeanAirConcentration = 5.66;

            var scenario = new ScenarioModel
            {
                Name = "inhalation exposure spray spraying",
                Frequency = new Frequency { Value = 90, Unit = FrequencyUnits.Yearly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,

                    SprayDuration = new SprayDuration { Value = 0.33, Unit = DurationUnits.Minute },
                    ExposureDuration = new ExposureDuration { Value = 240, Unit = DurationUnits.Minute },
                    RoomVolume = new RoomVolume { Value = 58, Unit = VolumeUnits.CubicMetre },
                    RoomHeight = new Height { Value = 1E6, Unit = LengthUnits.Metre },
                    VentilationRate = new Rate { Value = 0.0, Unit = RateUnits.TimesPerHour },

                    SprayingTowardsPerson = false,

                    MassGenerationRate = new MassGenerationRate { Value = 1.1, Unit = MassRateUnits.GramPerSecond },
                    AirborneFraction = new Fraction
                    {
                        Value = 0.3,
                        Unit = FractionUnits.Fraction
                    },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 0.1,
                        Unit = FractionUnits.Fraction
                    },
                    DensityNonVolatile = new DensityNonVolatile { Value = 1.8, Unit = DensityUnits.GramPerCubicCentimetre },

                    AerosolDiameterDistributionType = SizeDistributionTypes.LogNormal,
                    MedianDiameter = new FixedDiameter { Value = 15, Unit = LengthUnits.Micrometre },
                    ArithmicCoefficientOfVariation = 1.2,
                    MaximumDiameter = new FixedDiameter { Value = 50, Unit = LengthUnits.Micrometre },

                    InhalationCutOffDiameter = new Diameter { Value = 15, Unit = LengthUnits.Micrometre },

                    InhalationRate = new VolumeRate { Value = 5, Unit = VolumeRateUnits.CubicMetrePerDay }
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.1);

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 222, Unit = MolecularWeightUnits.GramPerMol },
                CASNumber = "1181081-51-5"
            };

            TestHelpers.SetupValidateAndTest(scenario, expectedMeanAirConcentration);
        }

        private static ScenarioModel GetScenarioForSprayingPeakInterval(InhalationExposureSubmodelTypes submodelType)
        {
            return new ScenarioModel
            {
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,

                    SprayDuration = new SprayDuration
                    {
                        Distribution = new Distribution
                        {
                            DistributionType = DistributionTypes.Uniform,
                            LowerBound = 10,
                            UpperBound = 1200
                        },
                        Unit = DurationUnits.Second
                    },
                    ExposureDuration = new ExposureDuration
                    {
                        Distribution = new Distribution
                        {
                            DistributionType = DistributionTypes.Uniform,
                            LowerBound = 10,
                            UpperBound = 200
                        },
                        Unit = DurationUnits.Minute
                    },
                    RoomVolume = new RoomVolume { Value = 20, Unit = VolumeUnits.CubicMetre },
                    RoomHeight = new Height { Value = 2.5, Unit = LengthUnits.Metre },
                    VentilationRate = new Rate { Value = 1, Unit = RateUnits.TimesPerHour },

                    MassGenerationRate = new MassGenerationRate { Value = 10, Unit = MassRateUnits.GramPerSecond },
                    AirborneFraction = new Fraction
                    {
                        Value = 0.3,
                        Unit = FractionUnits.Fraction
                    },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 0.1,
                        Unit = FractionUnits.Fraction
                    },
                    DensityNonVolatile = new DensityNonVolatile { Value = 1.5, Unit = DensityUnits.GramPerCubicCentimetre },

                    AerosolDiameterDistributionType = SizeDistributionTypes.Normal,
                    MeanDiameter = new FixedDiameter { Value = 20, Unit = LengthUnits.Micrometre },
                    StandardDeviation = new DiameterStandardDeviation { Value = 10.0, Unit = LengthUnits.Micrometre },
                    MaximumDiameter = new FixedDiameter { Value = 50, Unit = LengthUnits.Micrometre },

                    InhalationCutOffDiameter = new Diameter { Value = 30, Unit = LengthUnits.Micrometre }
                },
            };
        }
    }
}