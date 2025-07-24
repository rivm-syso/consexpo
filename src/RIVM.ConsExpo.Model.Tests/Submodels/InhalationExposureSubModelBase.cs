using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Models;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using RIVM.ConsExpo.Model.Models;
using RIVM.ConsExpo.Model.Submodels;
using RIVM.ConsExpo.Model.Tests.Helpers;
using RIVM.ConsExpo.TestFacilities;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    public class InhalationExposureSubModelBase
    {
        protected static void TestInhalationExposureExposureFraction(double? expectedExposureFraction, ScenarioModel scenario)
        {
            if (TestHelpers.ValidateInput(scenario))
            {
                IInhalationSimulation inhalationExposureSimulation = new InhalationSimulation();

                if (scenario.InhalationExposureRouteInUse)
                {
                    var exposureOutputValues = inhalationExposureSimulation.CalculatePointValues(scenario);

                    var actualExposureFraction = exposureOutputValues.Exposure.AsExposureFraction;

                    if (expectedExposureFraction == null)
                    {
                        Assert.IsNull(actualExposureFraction.Value, "The actual exposure fraction should be null, as this model does not support exposure fractions.");
                    }
                    else
                    {
                        Assert.IsNotNull(actualExposureFraction.Value, $"The actual exposure fraction should be {expectedExposureFraction}, but null was returned.");
                        Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedExposureFraction.Value,
                                actualExposureFraction.Value.Value),
                            $"The actual exposure fraction {actualExposureFraction.Value} differs from the expected value {expectedExposureFraction} with more than the allowed tolerance.");
                    }
                }
            }
        }

        protected void TestInhalationExposure(InhalationExposureSubmodelTypes submodelType, bool limitConcentrationToSaturatedAirConcentration, double expectedMeanAirConcentration)
        {
            var scenario = GetDefaultScenario(submodelType, limitConcentrationToSaturatedAirConcentration);

            TestHelpers.SetupValidateAndTest(scenario, expectedMeanAirConcentration);
        }

        protected static ScenarioModel GetDefaultScenario(InhalationExposureSubmodelTypes submodelType, bool limitConcentrationToSaturatedAirConcentration)
        {
            var scenario = new ScenarioModel()
            {
                Name = "inhalation vapour constant rate",
                Frequency = new Frequency() { Value = 104, Unit = FrequencyUnits.Yearly },
                InhalationExposureRouteInUse = true,
                InhalationExposure = new InhalationExposureModel()
                {
                    ExposureDuration = new ExposureDuration() { Value = 8, Unit = DurationUnits.Hour },
                    ProductAmount = new ProductAmount() { Value = 50, Unit = MassUnits.Gram },
                    ReleasedMass = new ProductAmount() { Value = 50, Unit = MassUnits.Gram },
                    WeightFractionSubstance = new Fraction
                    {
                        Value = 0.1,
                        Unit = FractionUnits.Fraction
                    },
                    RoomVolume = new RoomVolume() { Value = 58, Unit = VolumeUnits.CubicMetre },
                    VentilationRate = new Rate() { Value = 0.5, Unit = RateUnits.TimesPerHour },

                    SubmodelType = submodelType,

                    LimitConcentrationToSaturatedAirConcentration = limitConcentrationToSaturatedAirConcentration,
                    VapourPressure = new Pressure() { Value = 0.002, Unit = PressureUnits.MmHg },
                    ApplicationTemperature = new Temperature() { Value = 20, Unit = TemperatureUnits.Celsius },

                    EmissionDuration = new EmissionDuration() { Value = 1, Unit = DurationUnits.Hour }
                },
            };

            scenario.Assessment = ScenarioHelper.GetAssessment(65, 0.1);

            scenario.Assessment.Substance = new SubstanceModel()
            {
                MolecularWeight = new MolecularWeight() { Value = 222, Unit = MolecularWeightUnits.GramPerMol },
                CASNumber = "1181081-51-5"
            };
            return scenario;
        }
    }
}