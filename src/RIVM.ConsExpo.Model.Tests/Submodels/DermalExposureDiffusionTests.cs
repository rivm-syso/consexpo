using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using System;
using RIVM.ConsExpo.Model.Tests.Helpers;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class DermalExposureDiffusionTests : DermalExposureSubModelBase
    {
        [TestMethod]
        public void DermalExposureDiffusionExternalEventDoseTest()
        {
            var scenarioName = "Diffusion";
            var frequencyValue = 182;
            var frequencyUnit = FrequencyUnits.Yearly;
            var substanceConcentrationValue = 2.56E-6 / Math.Pow(ConversionFactors.Centi2One, 3);
            var substanceConcentrationUnit = DensityUnits.GramPerCubicMetre;
            var exposedAreaValue = 0.145;
            var exposedAreaUnit = AreaUnits.SquareMetre;
            var diffusionCoefficientValue = 3.6E-7;
            var diffusionCoefficientUnit = SurfaceRateUnits.SquareMetrePerHour;
            var layerThicknessValue = 2.5;
            var layerThicknessUnit = LengthUnits.Centimetre;
            var exposureTimeValue = 2;
            var exposureTimeUnit = DurationUnits.Hour;

            var scenario = GetScenarioDermalExposureDiffusion(scenarioName, frequencyValue, frequencyUnit
                , substanceConcentrationValue, substanceConcentrationUnit, exposedAreaValue, exposedAreaUnit, diffusionCoefficientValue, diffusionCoefficientUnit
                , layerThicknessValue, layerThicknessUnit, exposureTimeValue, exposureTimeUnit);

            scenario.Assessment = ScenarioHelper.GetAssessment(9.85, 0.1);

            TestDermalExposureExternalEventDose(0.0043, scenario);
        }

        [TestMethod]
        public void DermalExposureDiffusionExposureFractionTest()
        {
            var scenarioName = "Diffusion";
            var frequencyValue = 182;
            var frequencyUnit = FrequencyUnits.Yearly;
            var substanceConcentrationValue = 2.56E-6 / Math.Pow(ConversionFactors.Centi2One, 3);
            var substanceConcentrationUnit = DensityUnits.GramPerCubicMetre;
            var exposedAreaValue = 0.145;
            var exposedAreaUnit = AreaUnits.SquareMetre;
            var diffusionCoefficientValue = 3.6E-7;
            var diffusionCoefficientUnit = SurfaceRateUnits.SquareMetrePerHour;
            var layerThicknessValue = 2.5;
            var layerThicknessUnit = LengthUnits.Centimetre;
            var exposureTimeValue = 2;
            var exposureTimeUnit = DurationUnits.Hour;

            var scenario = GetScenarioDermalExposureDiffusion(scenarioName, frequencyValue, frequencyUnit
                , substanceConcentrationValue, substanceConcentrationUnit, exposedAreaValue, exposedAreaUnit, diffusionCoefficientValue, diffusionCoefficientUnit
                , layerThicknessValue, layerThicknessUnit, exposureTimeValue, exposureTimeUnit);

            scenario.Assessment = ScenarioHelper.GetAssessment(9.85, 0.1);

            TestDermalExposureExposureFraction(0.00457, scenario);
        }

        private ScenarioModel GetScenarioDermalExposureDiffusion(string scenarioName, double frequencyValue, FrequencyUnits frequencyUnit
            , double substanceConcentrationValue, DensityUnits substanceConcentrationUnit
            , double exposedAreaValue, AreaUnits exposedAreaUnit
            , double diffusionCoefficientValue, SurfaceRateUnits diffusionCoefficientUnit
            , double layerThicknessValue, LengthUnits layerThicknessUnit
            , double exposureTimeValue, DurationUnits exposureTimeUnit)
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
                },
                DermalAbsorptionRouteInUse = false,
            };
            return scenario;
        }
    }
}