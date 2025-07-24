using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;

namespace RIVM.ConsExpo.Model.Tests.Helpers
{
    internal class ScenarioHelper
    {
        public static ScenarioModel GetScenarioDermalExposureInstantApplication(string scenarioName, double frequencyValue, FrequencyUnits frequencyUnit, double weightFraction, double productAmountValue, MassUnits productAmountUnit)
        {
            var scenario = new ScenarioModel
            {
                Name = scenarioName,
                Frequency = new Frequency
                {
                    Value = frequencyValue,
                    Unit = frequencyUnit
                },
                DermalExposureRouteInUse = true,
                DermalExposure = new DermalExposureModel
                {
                    SubmodelType = DermalExposureSubmodelTypes.InstantApplication,
                    WeightFractionSubstance = new Fraction
                    {
                        Value = weightFraction,
                        Unit = FractionUnits.Fraction
                    },
                    ProductAmount = new ProductAmount
                    {
                        Value = productAmountValue,
                        Unit = productAmountUnit
                    },
                    RetentionFactor = new Fraction
                    {
                        Value = 1,
                        Unit = FractionUnits.Fraction
                    }
                },
                DermalAbsorptionRouteInUse = false,
            };
            return scenario;
        }

        public static AssessmentModel GetAssessment(double bodyWeightValue, double weightFractionSubstance)
        {
            return new AssessmentModel
            {
                Substance = new SubstanceModel
                {
                    Name = "Substance",
                    Kow = new Kow(true)
                },
                Population = new PopulationModel
                {
                    Name = "People",
                    BodyWeight = new BodyWeight { Value = bodyWeightValue, Unit = MassUnits.Kilogram }
                },
                Product = new ProductModel
                {
                    WeightFractionSubstanceDefault = new Fraction
                    {
                        Value = weightFractionSubstance,
                        Unit = FractionUnits.Fraction
                    }
                }
            };
        }
    }
}