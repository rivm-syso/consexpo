using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.Parameters;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Computations;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using RIVM.ConsExpo.Model.Submodels;
using RIVM.ConsExpo.Model.Tests.Helpers;
using RIVM.ConsExpo.TestFacilities;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class InhalationExposureVapourEvaporationTests : InhalationExposureSubModelBase
    {
        [TestMethod]
        public void InhalationExposureEvaporationConstantAreaPureSubstanceTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.VapourEvaporation;

            double expectedMeanAirConcentration = 53.6; // in mg/m³

            var scenario = new ScenarioModel
            {
                Name = "inhalation exposure vapour evaporation constant area pure substance",
                Frequency = new Frequency { Value = 1, Unit = FrequencyUnits.Yearly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,
                    ExposureDuration = new ExposureDuration { Value = 132, Unit = DurationUnits.Minute },
                    ProductAmount = new ProductAmount { Value = 1E3, Unit = MassUnits.Gram },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 1.0,
                        Unit = FractionUnits.Fraction
                    },
                    RoomVolume = new RoomVolume { Value = 20, Unit = VolumeUnits.CubicMetre },
                    VentilationRate = new Rate { Value = 0.6, Unit = RateUnits.TimesPerHour },

                    ReleaseArea = new ReleaseArea { Value = 1E5, Unit = AreaUnits.SquareCentimetre },
                    EmissionDurationEvaporation = new EmissionDurationEvaporation { Value = 120, Unit = DurationUnits.Minute },

                    ReleaseAreaType = InhalationExposureReleaseAreaTypes.Constant,

                    ApplicationTemperature = new Temperature { Value = 20, Unit = TemperatureUnits.Celsius },
                    VapourPressure = new Pressure { Value = 2, Unit = PressureUnits.Pascal },
                    MassTransferCoefficient = new MassTransferCoefficient { Value = 0.6, Unit = VelocityUnits.MetrePerHour }, //Specified in CE 4.1 scenario as 0.01 m/min

                    PureForm = true,
                    ProductInDilution = false
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 1.0);

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 350, Unit = MolecularWeightUnits.GramPerMol },
                CASNumber = "1181081-51-5"
            };

            var inhalatoryExposureSimulation = new InhalationExposureVapourEvaporation(scenario);

            var meanAirConcentration = inhalatoryExposureSimulation.MeanAirConcentration();

            var meanAirConcentrationValue = meanAirConcentration.InMilligramPerCubicMetre();
            Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedMeanAirConcentration, meanAirConcentrationValue),
                $"The actual external event dose value {meanAirConcentrationValue} differs from the expected value {expectedMeanAirConcentration} with more than the allowed tolerance.");
        }

        [TestMethod]
        public void InhalationExposureEvaporationConstantAreaMixtureTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.VapourEvaporation;

            double expectedMeanAirConcentration = 1.28; // in mg/m³

            var scenario = new ScenarioModel
            {
                Name = "inhalation exposure vapour evaporation constant area mixture",
                Frequency = new Frequency { Value = 1, Unit = FrequencyUnits.Yearly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,
                    ExposureDuration = new ExposureDuration { Value = 132, Unit = DurationUnits.Minute },
                    ProductAmount = new ProductAmount { Value = 1E3, Unit = MassUnits.Gram },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 0.3,
                        Unit = FractionUnits.Fraction
                    },
                    RoomVolume = new RoomVolume { Value = 20, Unit = VolumeUnits.CubicMetre },
                    VentilationRate = new Rate { Value = 0.6, Unit = RateUnits.TimesPerHour },

                    ReleaseArea = new ReleaseArea { Value = 1E5, Unit = AreaUnits.SquareCentimetre },
                    EmissionDurationEvaporation = new EmissionDurationEvaporation { Value = 120, Unit = DurationUnits.Minute },

                    ReleaseAreaType = InhalationExposureReleaseAreaTypes.Constant,

                    ApplicationTemperature = new Temperature { Value = 20, Unit = TemperatureUnits.Celsius },
                    VapourPressure = new Pressure { Value = 2, Unit = PressureUnits.Pascal },
                    MassTransferCoefficient = new MassTransferCoefficient { Value = 0.6, Unit = VelocityUnits.MetrePerHour }, //Specified in CE 4.1 scenario as 0.01 m/min

                    PureForm = false,
                    MolecularWeightMatrix = new MolecularWeight { Value = 20, Unit = MolecularWeightUnits.GramPerMol },
                    ProductInDilution = false,
                    Dilution = new Dilution { Value = 1, UnitCode = 1 }
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.3);

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 350, Unit = MolecularWeightUnits.GramPerMol },
                CASNumber = "1181081-51-5"
            };

            var inhalatoryExposureSimulation = new InhalationExposureVapourEvaporation(scenario);

            var meanAirConcentration = inhalatoryExposureSimulation.MeanAirConcentration();

            var meanAirConcentrationValue = meanAirConcentration.InMilligramPerCubicMetre();
            Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedMeanAirConcentration, meanAirConcentrationValue),
                $"The actual external event dose value {meanAirConcentrationValue} differs from the expected value {expectedMeanAirConcentration} with more than the allowed tolerance.");
        }

        [TestMethod]
        public void InhalationExposureEvaporationConstantAreaLongExposureTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.VapourEvaporation;

            double expectedMeanAirConcentration = 39.3; // in mg/m³

            var scenario = new ScenarioModel
            {
                Name = "inhalation exposure vapour evaporation constant area long exposure",
                Frequency = new Frequency { Value = 2, Unit = FrequencyUnits.Yearly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,
                    ExposureDuration = new ExposureDuration { Value = 24, Unit = DurationUnits.Hour },
                    PureForm = false,
                    MolecularWeightMatrix = new MolecularWeight { Value = 56.6, Unit = MolecularWeightUnits.GramPerMol },
                    ProductInDilution = false,
                    Dilution = new Dilution { Value = 1, UnitCode = 1 },
                    ProductAmount = new ProductAmount { Value = 45.4, Unit = MassUnits.Gram },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 40,
                        Unit = FractionUnits.Percentage
                    },
                    RoomVolume = new RoomVolume { Value = 20, Unit = VolumeUnits.CubicMetre },
                    VentilationRate = new Rate { Value = 0.6, Unit = RateUnits.TimesPerHour },

                    VapourPressure = new Pressure { Value = 100, Unit = PressureUnits.Pascal },
                    ApplicationTemperature = new Temperature { Value = 20, Unit = TemperatureUnits.Celsius },
                    MassTransferCoefficient = new MassTransferCoefficient { Value = 1.76E5, Unit = VelocityUnits.MetrePerHour },

                    ReleaseAreaType = InhalationExposureReleaseAreaTypes.Constant,
                    ReleaseArea = new ReleaseArea { Value = 5.9, Unit = AreaUnits.SquareMetre },
                    EmissionDurationEvaporation = new EmissionDurationEvaporation { Value = 7, Unit = DurationUnits.Minute }
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(10, 0.4);

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 163, Unit = MolecularWeightUnits.GramPerMol }
            };

            var inhalatoryExposureSimulation = new InhalationExposureVapourEvaporation(scenario);

            var meanAirConcentration = inhalatoryExposureSimulation.MeanAirConcentration();

            var meanAirConcentrationValue = meanAirConcentration.InMilligramPerCubicMetre();
            Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedMeanAirConcentration, meanAirConcentrationValue),
                $"The actual external event dose value {meanAirConcentrationValue} differs from the expected value {expectedMeanAirConcentration} with more than the allowed tolerance.");
        }

        [TestMethod]
        public void InhalationExposureEvaporationIncreasingAreaPureSubstanceTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.VapourEvaporation;

            double expectedMeanAirConcentration = 23.6; // in mg/m³

            var scenario = new ScenarioModel
            {
                Name = "inhalation exposure vapour evaporation constant area pure substance",
                Frequency = new Frequency { Value = 1, Unit = FrequencyUnits.Yearly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,
                    ExposureDuration = new ExposureDuration { Value = 132, Unit = DurationUnits.Minute },
                    ProductAmount = new ProductAmount { Value = 1E3, Unit = MassUnits.Gram },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 1.0,
                        Unit = FractionUnits.Fraction
                    },
                    RoomVolume = new RoomVolume { Value = 20, Unit = VolumeUnits.CubicMetre },
                    VentilationRate = new Rate { Value = 0.6, Unit = RateUnits.TimesPerHour },

                    ReleaseArea = new ReleaseArea { Value = 1E5, Unit = AreaUnits.SquareCentimetre },
                    ApplicationDuration = new ApplicationDuration { Value = 120, Unit = DurationUnits.Minute },

                    ReleaseAreaType = InhalationExposureReleaseAreaTypes.Increasing,

                    ApplicationTemperature = new Temperature { Value = 20, Unit = TemperatureUnits.Celsius },
                    VapourPressure = new Pressure { Value = 2, Unit = PressureUnits.Pascal },
                    MassTransferCoefficient = new MassTransferCoefficient { Value = 0.6, Unit = VelocityUnits.MetrePerHour }, //Specified in CE 4.1 scenario as 0.01 m/min

                    PureForm = true,
                    ProductInDilution = false
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 1.0);

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 350, Unit = MolecularWeightUnits.GramPerMol },
                CASNumber = "1181081-51-5"
            };

            var inhalatoryExposureSimulation = new InhalationExposureVapourEvaporation(scenario);

            var meanAirConcentration = inhalatoryExposureSimulation.MeanAirConcentration();

            var meanAirConcentrationValue = meanAirConcentration.InMilligramPerCubicMetre();
            Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedMeanAirConcentration, meanAirConcentrationValue),
                $"The actual external event dose value {meanAirConcentrationValue} differs from the expected value {expectedMeanAirConcentration} with more than the allowed tolerance.");
        }

        [TestMethod]
        public void InhalationExposureEvaporationIncreasingAreaMixtureTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.VapourEvaporation;

            double expectedMeanAirConcentration = 0.563; // in mg/m³

            var scenario = new ScenarioModel
            {
                Name = "inhalation exposure vapour evaporation constant area mixture",
                Frequency = new Frequency { Value = 1, Unit = FrequencyUnits.Yearly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,
                    ExposureDuration = new ExposureDuration { Value = 132, Unit = DurationUnits.Minute },
                    ProductAmount = new ProductAmount { Value = 1E3, Unit = MassUnits.Gram },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 0.3,
                        Unit = FractionUnits.Fraction
                    },
                    RoomVolume = new RoomVolume { Value = 20, Unit = VolumeUnits.CubicMetre },
                    VentilationRate = new Rate { Value = 0.6, Unit = RateUnits.TimesPerHour },

                    ReleaseArea = new ReleaseArea { Value = 1E5, Unit = AreaUnits.SquareCentimetre },
                    ApplicationDuration = new ApplicationDuration { Value = 120, Unit = DurationUnits.Minute },

                    ReleaseAreaType = InhalationExposureReleaseAreaTypes.Increasing,

                    ApplicationTemperature = new Temperature { Value = 20, Unit = TemperatureUnits.Celsius },
                    VapourPressure = new Pressure { Value = 2, Unit = PressureUnits.Pascal },
                    MassTransferCoefficient = new MassTransferCoefficient { Value = 0.6, Unit = VelocityUnits.MetrePerHour }, //Specified in CE 4.1 scenario as 0.01 m/min

                    PureForm = false,
                    MolecularWeightMatrix = new MolecularWeight { Value = 20, Unit = MolecularWeightUnits.GramPerMol },
                    ProductInDilution = false,
                    Dilution = new Dilution { Value = 1, UnitCode = 1 }
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.3);

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 350, Unit = MolecularWeightUnits.GramPerMol },
                CASNumber = "1181081-51-5"
            };

            var inhalatoryExposureSimulation = new InhalationExposureVapourEvaporation(scenario);

            var meanAirConcentration = inhalatoryExposureSimulation.MeanAirConcentration();

            var meanAirConcentrationValue = meanAirConcentration.InMilligramPerCubicMetre();
            Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedMeanAirConcentration, meanAirConcentrationValue),
                $"The actual external event dose value {meanAirConcentrationValue} differs from the expected value {expectedMeanAirConcentration} with more than the allowed tolerance.");
        }

        [TestMethod]
        public void InhalationExposureEvaporationExposureFractionTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.VapourEvaporation;

            var scenario = new ScenarioModel
            {
                Name = "CE2015-58956 2020-009a",
                Frequency = new Frequency { Value = 1, Unit = FrequencyUnits.Yearly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,
                    ExposureDuration = new ExposureDuration { Value = 16, Unit = DurationUnits.Hour },
                    ProductAmount = new ProductAmount { Value = 1E3, Unit = MassUnits.Gram },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 0.3,
                        Unit = FractionUnits.Fraction
                    },
                    RoomVolume = new RoomVolume { Value = 20, Unit = VolumeUnits.CubicMetre },
                    VentilationRate = new Rate { Value = 0.6, Unit = RateUnits.TimesPerHour },

                    ReleaseArea = new ReleaseArea { Value = 1E5, Unit = AreaUnits.SquareCentimetre },
                    ApplicationDuration = new ApplicationDuration { Value = 120, Unit = DurationUnits.Minute },

                    ReleaseAreaType = InhalationExposureReleaseAreaTypes.Increasing,

                    ApplicationTemperature = new Temperature { Value = 20, Unit = TemperatureUnits.Celsius },
                    VapourPressure = new Pressure { Value = 260, Unit = PressureUnits.Pascal },
                    MassTransferCoefficient = new MassTransferCoefficient { Value = 0.6, Unit = VelocityUnits.MetrePerHour }, //Specified in CE 4.1 scenario as 0.01 m/min

                    PureForm = false,
                    MolecularWeightMatrix = new MolecularWeight { Value = 20, Unit = MolecularWeightUnits.GramPerMol },
                    ProductInDilution = false,
                    Dilution = new Dilution { Value = 1, UnitCode = 1 },
                    InhalationRate = new VolumeRate { Value = 5, Unit = VolumeRateUnits.CubicMetrePerDay }
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.3);

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 350, Unit = MolecularWeightUnits.GramPerMol }
            };

            // Value taken from ConsExpo web 1.0.7.This test is only for regression, as the test value has not be checked independently.
            TestInhalationExposureExposureFraction(0.00263, scenario);
        }

        /// <summary>
        /// Performs a Monte Carlo test on the calculation of the Peak interval and checks if the duration is OK.
        /// </summary>
        [TestMethod]
        public void InhalationExposureEvaporationPeakIntervalTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.VapourEvaporation;

            double expectedPeakIntervalDuration = 15;

            var scenario = new ScenarioModel
            {
                Name = "inhalation exposure vapour evaporation constant area mixture",
                Frequency = new Frequency { Value = 1, Unit = FrequencyUnits.Yearly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,
                    ExposureDuration = new ExposureDuration
                    {
                        Distribution = new Distribution
                        {
                            DistributionType = DistributionTypes.Uniform,
                            LowerBound = 15,
                            UpperBound = 150
                        },

                        Unit = DurationUnits.Minute
                    },

                    ProductAmount = new ProductAmount { Value = 1E3, Unit = MassUnits.Gram },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 0.3,
                        Unit = FractionUnits.Fraction
                    },
                    RoomVolume = new RoomVolume { Value = 20, Unit = VolumeUnits.CubicMetre },
                    VentilationRate = new Rate
                    {
                        Distribution = new Distribution
                        {
                            DistributionType = DistributionTypes.Uniform,
                            LowerBound = 0,
                            UpperBound = 10
                        },

                        Unit = RateUnits.TimesPerHour
                    },

                    ReleaseArea = new ReleaseArea { Value = 1E5, Unit = AreaUnits.SquareCentimetre },
                    EmissionDurationEvaporation = new EmissionDurationEvaporation { Value = 25, Unit = DurationUnits.Minute },

                    ReleaseAreaType = InhalationExposureReleaseAreaTypes.Constant,

                    ApplicationTemperature = new Temperature { Value = 20, Unit = TemperatureUnits.Celsius },
                    VapourPressure = new Pressure { Value = 200, Unit = PressureUnits.Pascal },
                    MassTransferCoefficient = new MassTransferCoefficient
                    {
                        Distribution = new Distribution
                        {
                            DistributionType = DistributionTypes.Uniform,
                            LowerBound = 1,
                            UpperBound = 10
                        },
                        Unit = VelocityUnits.MetrePerHour
                    },

                    PureForm = false,
                    MolecularWeightMatrix = new MolecularWeight { Value = 20, Unit = MolecularWeightUnits.GramPerMol },
                    ProductInDilution = false,
                    Dilution = new Dilution { Value = 1, UnitCode = 1 }
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.3);

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 35, Unit = MolecularWeightUnits.GramPerMol },
            };

            for (int i = 0; i < 100; i++)
            {
                scenario.InhalationExposure.ExposureDuration.Sample();
                scenario.InhalationExposure.VentilationRate.Sample();
                var inhalatoryExposureSimulation = new InhalationExposureVapourEvaporation(scenario);

                inhalatoryExposureSimulation.PrepareTimeSeries(scenario.InhalationExposure.ExposureDuration.AsTime());

                TimeInterval actualPeakInterval = inhalatoryExposureSimulation.PeakInterval();

                Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedPeakIntervalDuration, actualPeakInterval.DurationInMinutes),
                    $"The actual peak interval duration {actualPeakInterval.DurationInMinutes} differs from the expected value {expectedPeakIntervalDuration} with more than the allowed tolerance.");

                if (actualPeakInterval.EndTime < scenario.InhalationExposure.ExposureDuration.AsTime())
                {
                    double airConcentrationAtPeakStart = inhalatoryExposureSimulation.InstantaneousAirConcentration(actualPeakInterval.StartTime).AsMilligramPerCubicMetre().Value.Value;

                    double airConcentrationAtPeakEnd = inhalatoryExposureSimulation.InstantaneousAirConcentration(actualPeakInterval.EndTime).AsMilligramPerCubicMetre().Value.Value;

                    Assert.IsTrue(Comparisons.AlmostEqualMagnitude(airConcentrationAtPeakStart, airConcentrationAtPeakEnd, 0.2),
                        $"The air concentration at the start of the peak interval {airConcentrationAtPeakStart} differs air concentration at the end of the peak interval {airConcentrationAtPeakEnd} with more than the allowed tolerance.");
                }
            }
        }

        [TestMethod]
        public void InhalationExposureEvaporationMeanAirConcentrationPeakTest()
        {
            var scenario = new ScenarioModel
            {
                Name = "Evaporation - re-entry",
                Frequency = new Frequency { Value = 1, Unit = FrequencyUnits.Yearly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = InhalationExposureSubmodelTypes.VapourEvaporation,
                    ReEntry = true,
                    EmissionDurationReEntry = new EmissionDurationReEntry { Value = 1, Unit = DurationUnits.Week },
                    DailyDuration = new DailyDuration { Value = 5, Unit = DailyDurationUnits.HoursPerDay },
                    PureForm = false,
                    MolecularWeightMatrix = new MolecularWeight { Value = 20, Unit = MolecularWeightUnits.GramPerMol },
                    ProductInDilution = false,
                    ProductAmount = new ProductAmount { Value = 2E3, Unit = MassUnits.Gram },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 0.01,
                        Unit = FractionUnits.Fraction
                    },
                    RoomVolume = new RoomVolume { Value = 60, Unit = VolumeUnits.CubicMetre },
                    VentilationRate = new Rate { Value = 1, Unit = RateUnits.TimesPerHour },
                    InhalationRate = new VolumeRate { Value = 5, Unit = VolumeRateUnits.CubicMetrePerDay },
                    VapourPressure = new Pressure { Value = 35, Unit = PressureUnits.Pascal },
                    ApplicationTemperature = new Temperature { Value = 25, Unit = TemperatureUnits.Celsius },
                    MassTransferCoefficient = new MassTransferCoefficient { Value = 10, Unit = VelocityUnits.MetrePerHour },
                    ReleaseAreaType = InhalationExposureReleaseAreaTypes.Constant,
                    ReleaseArea = new ReleaseArea { Value = 5, Unit = AreaUnits.SquareMetre },
                },
                Assessment = ScenarioHelper.GetAssessment(65, 0.3)
            };

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 300, Unit = MolecularWeightUnits.GramPerMol }
            };

            var expectedMeanAirConcentrationPeak = 1.27; // Not determined independently. Only valid for regression.

            if (TestHelpers.ValidateInput(scenario))
            {
                InhalationExposureVapourEvaporation inhalationExposureSimulation = new InhalationExposureVapourEvaporation(scenario);

                inhalationExposureSimulation.PrepareTimeSeries(scenario.InhalationExposure.EmissionDurationReEntry.AsTime());

                if (scenario.InhalationExposureRouteInUse)
                {
                    var meanAirConcentrationPeak = inhalationExposureSimulation.MeanAirConcentrationPeak();

                    var actualMeanAirConcentrationPeak = meanAirConcentrationPeak.Value;
                    Assert.IsTrue(
                        Comparisons.AlmostEqualMagnitude(expectedMeanAirConcentrationPeak,
                            actualMeanAirConcentrationPeak.Value),
                        $"The actual peak air concentration value {actualMeanAirConcentrationPeak} differs from the expected value {expectedMeanAirConcentrationPeak} with more than the allowed tolerance.");
                }
            }
        }

        /// <summary>
        /// This test verifies whether or not the "STEP SIZE BECOMES TOO SMALL" exception is thrown from the DotNumerics radau5.cs class. A fix we applied to that codes should prevent this, so this test should succeed. The fix is toggled on by the parameter applyStepSizeFix passed from <see cref="OdeSolver.Solve" />. 
        /// </summary>
        [TestMethod]
        public void InhalationExposureEvaporationFinalStepSizeTest()
        {
            var submodelType = InhalationExposureSubmodelTypes.VapourEvaporation;

            double expectedMeanAirConcentrationPeakValue = 657.4; // in mg/m³

            var scenario = new ScenarioModel
            {
                Name = "CONSEXPO-144",
                Frequency = new Frequency { Value = 1, Unit = FrequencyUnits.Monthly },
                InhalationExposureRouteInUse = true,
                Assessment = ScenarioHelper.GetAssessment(68.8, 0.01),
                InhalationExposure = new InhalationExposureModel
                {
                    SubmodelType = submodelType,
                    ReEntry = true,
                    EmissionDurationReEntry = new EmissionDurationReEntry { Unit = DurationUnits.Week },
                    DailyDuration = new DailyDuration { Value = 5, Unit = DailyDurationUnits.HoursPerDay },
                    PureForm = true,
                    ProductAmount = new ProductAmount { Value = 2E+03, Unit = MassUnits.Gram },
                    WeightFractionSubstance = new Fraction { Value = 0.1, Unit = FractionUnits.Fraction },
                    RoomVolume = new RoomVolume { Value = 60, Unit = VolumeUnits.CubicMetre },
                    VentilationRate = new Rate { Value = 1, Unit = RateUnits.TimesPerHour },
                    InhalationRate = new VolumeRate { Value = 1, Unit = VolumeRateUnits.CubicMetrePerHour },
                    VapourPressure = new Pressure { Value = 35, Unit = PressureUnits.Pascal },
                    ApplicationTemperature = new Temperature { Value = 25, Unit = TemperatureUnits.Celsius },
                    MassTransferCoefficient = new MassTransferCoefficient { Value = 12.8, Unit = VelocityUnits.MetrePerHour },
                    ReleaseAreaType = InhalationExposureReleaseAreaTypes.Constant,
                    ReleaseArea = new ReleaseArea { Value = 5, Unit = AreaUnits.SquareMetre }
                },
                InhalationAbsorption = new InhalationAbsorptionModel
                {
                    AbsorptionFraction = new Fraction { Value = 0.1, Unit = FractionUnits.Fraction }
                }
            };

            scenario.Assessment.Substance = new SubstanceModel
            {
                MolecularWeight = new MolecularWeight { Value = 234, Unit = MolecularWeightUnits.GramPerMol }
            };

            for (double x = 0.8; x <= 0.9; x += 0.001)
            {
                Debug.WriteLine($"{nameof(scenario.InhalationExposure.EmissionDurationReEntry)}={x} {scenario.InhalationExposure.EmissionDurationReEntry.UnitDisplay}.");
                scenario.InhalationExposure.EmissionDurationReEntry.Value = x;
                var inhalatoryExposureSimulation = new InhalationExposureVapourEvaporation(scenario);

                // Calculate the mean air concentration, to make sure the solution is prepared.
                _ = inhalatoryExposureSimulation.MeanAirConcentration();

                var meanAirConcentrationPeak = inhalatoryExposureSimulation.MeanAirConcentrationPeak();

                var meanAirConcentrationPeakValue = meanAirConcentrationPeak.InMilligramPerCubicMetre();

                Assert.IsTrue(
                    Comparisons.AlmostEqualMagnitude(expectedMeanAirConcentrationPeakValue,
                        meanAirConcentrationPeakValue),
                    $"The actual external event dose value {meanAirConcentrationPeakValue} differs from the expected value {expectedMeanAirConcentrationPeakValue} with more than the allowed tolerance.");
            }
        }


    }
}