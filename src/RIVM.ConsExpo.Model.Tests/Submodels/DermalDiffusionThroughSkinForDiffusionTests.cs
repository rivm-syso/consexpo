using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Models;
using RIVM.ConsExpo.Model.Models;
using RIVM.ConsExpo.Model.Tests.Helpers;
using RIVM.ConsExpo.TestFacilities;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class DermalDiffusionThroughSkinForDiffusionTests : DermalExposureSubModelBase
    {
        /// <summary>
        /// Based on deodorant diffusion + diffusion through skin.Ce4.
        /// </summary>
        [TestMethod]
        public void DiffusionThroughSkinForDiffusionDeodorantTest()
        {
            var scenarioName = "Diffusion";
            var frequencyValue = 365;
            var frequencyUnit = FrequencyUnits.Yearly;
            var exposedAreaValue = 100.0;
            var exposedAreaUnit = AreaUnits.SquareCentimetre;
            var substanceConcentrationValue = 1;
            var substanceConcentrationUnit = DensityUnits.GramPerCubicCentimetre;
            var diffusionCoefficientValue = 1E-5;
            var diffusionCoefficientUnit = SurfaceRateUnits.SquareMetrePerHour;
            var layerThicknessValue = 0.01;
            var layerThicknessUnit = LengthUnits.Centimetre;
            var skinPermeabilityValue = 6.64E-6;
            var skinPermeabilityUnit = VelocityUnits.CentimetrePerHour;
            var exposureTimeValue = 1.44E3;
            var exposureTimeUnit = DurationUnits.Minute;

            var scenario = GetScenarioDiffusionThroughSkinForDiffusion(scenarioName, frequencyValue, frequencyUnit, exposedAreaValue, exposedAreaUnit, substanceConcentrationValue, substanceConcentrationUnit, diffusionCoefficientValue, diffusionCoefficientUnit, layerThicknessValue, layerThicknessUnit, skinPermeabilityValue, skinPermeabilityUnit, exposureTimeValue, exposureTimeUnit);

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.05);

            TestExposureAndAbsorption(10, 0.243, scenario);
        }

        /// <summary>
        /// Based on technical test diffusion + diffusion through skin.Ce4
        /// </summary>
        [TestMethod]
        public void DiffusionThroughSkinForDiffusionTechnicalTest()
        {
            var scenarioName = "Diffusion";
            var frequencyValue = 3;
            var frequencyUnit = FrequencyUnits.Weekly;
            var exposedAreaValue = 200.0;
            var exposedAreaUnit = AreaUnits.SquareCentimetre;
            var substanceConcentrationValue = 2;
            var substanceConcentrationUnit = DensityUnits.GramPerCubicCentimetre;
            var diffusionCoefficientValue = 0.0003;
            var diffusionCoefficientUnit = SurfaceRateUnits.SquareMetrePerHour;
            var layerThicknessValue = 80;
            var layerThicknessUnit = LengthUnits.Micrometre;
            var skinPermeabilityValue = 0.0001;
            var skinPermeabilityUnit = VelocityUnits.CentimetrePerMinute;
            var exposureTimeValue = 2;
            var exposureTimeUnit = DurationUnits.Hour;

            var scenario = GetScenarioDiffusionThroughSkinForDiffusion(scenarioName, frequencyValue, frequencyUnit, exposedAreaValue, exposedAreaUnit, substanceConcentrationValue, substanceConcentrationUnit, diffusionCoefficientValue, diffusionCoefficientUnit, layerThicknessValue, layerThicknessUnit, skinPermeabilityValue, skinPermeabilityUnit, exposureTimeValue, exposureTimeUnit);

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.05);

            TestExposureAndAbsorption(16, 37.7, scenario);
        }

        /// <summary>
        /// Based on deodorant diffusion + diffusion through skin.Ce4.
        /// </summary>
        [TestMethod]
        public void DiffusionThroughSkinForDiffusionExposureFractionTest()
        {
            var scenarioName = "Diffusion";
            var frequencyValue = 365;
            var frequencyUnit = FrequencyUnits.Yearly;
            var exposedAreaValue = 100.0;
            var exposedAreaUnit = AreaUnits.SquareCentimetre;
            var substanceConcentrationValue = 1;
            var substanceConcentrationUnit = DensityUnits.GramPerCubicCentimetre;
            var diffusionCoefficientValue = 1E-10;
            var diffusionCoefficientUnit = SurfaceRateUnits.SquareMetrePerHour;
            var layerThicknessValue = 0.01;
            var layerThicknessUnit = LengthUnits.Centimetre;
            var skinPermeabilityValue = 6.64E-6;
            var skinPermeabilityUnit = VelocityUnits.CentimetrePerHour;
            var exposureTimeValue = 600;
            var exposureTimeUnit = DurationUnits.Minute;

            var scenario = GetScenarioDiffusionThroughSkinForDiffusion(scenarioName, frequencyValue, frequencyUnit, exposedAreaValue, exposedAreaUnit, substanceConcentrationValue, substanceConcentrationUnit, diffusionCoefficientValue, diffusionCoefficientUnit, layerThicknessValue, layerThicknessUnit, skinPermeabilityValue, skinPermeabilityUnit, exposureTimeValue, exposureTimeUnit);

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.05);

            // Value taken from ConsExpo web 1.0.7. Test is for regression issues only.
            TestDermalExposureExposureFraction(0.246, scenario);
        }

        protected ScenarioModel GetScenarioDiffusionThroughSkinForDiffusion(string scenarioName, double frequencyValue, FrequencyUnits frequencyUnit, double exposedAreaValue, AreaUnits exposedAreaUnit, double substanceConcentrationValue, DensityUnits substanceConcentrationUnit, double diffusionCoefficientValue, SurfaceRateUnits diffusionCoefficientUnit, double layerThicknessValue, LengthUnits layerThicknessUnit, double skinPermeabilityValue, VelocityUnits skinPermeabilityUnit, double exposureTimeValue, DurationUnits exposureTimeUnit)
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
                DermalAbsorptionRouteInUse = true
            };

            scenario.DermalExposure = new DermalExposureModel()
            {
                SubmodelType = DermalExposureSubmodelTypes.Diffusion,
                SubstanceConcentration = new SubstanceConcentration
                {
                    Value = substanceConcentrationValue,
                    Unit = substanceConcentrationUnit
                }
                ,
                DiffusionCoefficient = new DiffusionCoefficient
                {
                    Value = diffusionCoefficientValue,
                    Unit = diffusionCoefficientUnit
                }
                ,
                ExposedArea = new ExposedArea
                {
                    Value = exposedAreaValue,
                    Unit = exposedAreaUnit
                }
                ,
                LayerThickness = new Thickness
                {
                    Value = layerThicknessValue,
                    Unit = layerThicknessUnit
                },
                ExposureDuration = new ExposureDuration
                {
                    Value = exposureTimeValue,
                    Unit = exposureTimeUnit
                }
            };

            scenario.DermalAbsorption = GetAbsorptionModel(substanceConcentrationValue, substanceConcentrationUnit, skinPermeabilityValue, skinPermeabilityUnit);

            return scenario;
        }

        protected static DermalAbsorptionModel GetAbsorptionModel(double concentrationValue, DensityUnits concentrationUnit, double permeabilitySkinValue, VelocityUnits permeabilitySkinUnit)
        {
            var dermalAbsorptionModel = new DermalAbsorptionModel()
            {
                SubmodelType = DermalAbsorptionSubmodelTypes.DiffusionThroughSkinForDiffusion,
                ConcentrationInMatrix = new SubstanceConcentration()
                {
                    Value = concentrationValue,
                    Unit = concentrationUnit
                },
                SkinPermeability = new SkinPermeability()
                {
                    Value = permeabilitySkinValue,
                    Unit = permeabilitySkinUnit
                },
            };
            return dermalAbsorptionModel;
        }

        protected static void TestExposureAndAbsorption(double expectedDermalLoad, double expectedInternalEventDose, ScenarioModel scenario)
        {
            if (TestHelpers.ValidateInput(scenario))
            {
                IDermalSimulation dermalSimulation = new DermalSimulation();

                var dermalOutputValues = dermalSimulation.CalculatePointValues(scenario);

                var actualDermalLoad = dermalOutputValues.Exposure.AsDermalLoad.Value;
                Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedDermalLoad, actualDermalLoad.Value),
                    $"The actual dermal load value {actualDermalLoad} differs from the expected value {expectedDermalLoad} with more than the allowed tolerance.");

                var actualInternalEventDose = dermalOutputValues.Absorption.AsInternalEventDose.Value;
                Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedInternalEventDose, actualInternalEventDose.Value),
                    $"The actual internal event dose value {actualInternalEventDose} differs from the expected value {expectedInternalEventDose} with more than the allowed tolerance.");
            }
        }
    }
}