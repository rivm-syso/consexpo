using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using RIVM.ConsExpo.Model.Submodels;
using RIVM.ConsExpo.TestFacilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RIVM.ConsExpo.Model.Tests
{
    internal class TestHelpers
    {

        public static bool ValidateInput(ScenarioModel scenario)
        {
            var context = new ValidationContext(scenario, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(scenario, context, results, true))
            {
                StringBuilder messageDetails = new StringBuilder("The constructed scenario, to be used in the unit test, fails validation. The test cannot be performed.");
                foreach (var result in results)
                {
                    foreach (var memberName in result.MemberNames)
                    {
                        messageDetails.Append(Environment.NewLine).AppendFormat("Field: '{0}'", memberName);
                    }
                    messageDetails.Append(Environment.NewLine).Append(result.ErrorMessage);
                }
                Assert.Inconclusive(messageDetails.ToString());
                return false;
            }
            else
            {
                return true;
            }
        }

        public static AssessmentModel GetAssessment(double bodyWeightValue, double weightFractionSubstance)
        {
            return new AssessmentModel()
            {
                Substance = new SubstanceModel()
                {
                    Name = "Substance"
                },
                Population = new PopulationModel()
                {
                    Name = "People",
                    BodyWeight = new BodyWeight() { Value = bodyWeightValue, Unit = MassUnits.Kilogram }
                },
                Product = new ProductModel()
                {
                    WeightFractionSubstanceDefault = new Fraction
                    {
                        Value = weightFractionSubstance,
                        Unit = FractionUnits.Fraction
                    }
                }
            };
        }

        public static void SaveOdeSolution(double[,] sol)
        {
            const string FileName = "OdeSolution.csv";

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(FileName))
            {
                for (int x = 0; x < sol.GetLength(0); x++)
                {
                    for (int y = 0; y < sol.GetLength(1); y++)
                    {
                        file.Write(sol[x, y].ToString() + ";");
                    }
                    file.WriteLine("");
                }
            }
        }

        public static void SetupValidateAndTest(ScenarioModel scenario, double expectedMeanAirConcentration, double? expectedPeakAirConcentration = null)
        {
            if (TestHelpers.ValidateInput(scenario))
            {
                IInhalationExposureSubmodel inhalatoryExposureSimulation;

                switch (scenario.InhalationExposure.SubmodelType)
                {
                    case InhalationExposureSubmodelTypes.VapourConstantRate:
                        inhalatoryExposureSimulation = new InhalationExposureVapourConstantRate(scenario);
                        break;

                    case InhalationExposureSubmodelTypes.VapourInstantaneousRelease:
                        inhalatoryExposureSimulation = new InhalationExposureVapourInstantaniousRelease(scenario);
                        break;

                    case InhalationExposureSubmodelTypes.SprayInstantaneousRelease:
                        inhalatoryExposureSimulation = new InhalationExposureSprayInstantaniousRelease(scenario);
                        break;

                    case InhalationExposureSubmodelTypes.SpraySpraying:
                        inhalatoryExposureSimulation = new InhalationExposureSpraySpraying(scenario);
                        break;

                    case InhalationExposureSubmodelTypes.EmissionFromSolidMaterials:
                        inhalatoryExposureSimulation = new InhalationExposureEmissionFromSolidMaterials(scenario);
                        break;

                    case InhalationExposureSubmodelTypes.VapourEvaporation: //To Do
                    default:
                        throw new NotSupportedException(string.Format("Unsupported inhalatory exposure submodel '{0}'", scenario.InhalationExposure.SubmodelType.ToString()));
                }

                if (scenario.InhalationExposureRouteInUse)
                {
                    var meanAirConcentration = inhalatoryExposureSimulation.MeanAirConcentration();
                    var peakAirConcentration = inhalatoryExposureSimulation.PeakAirConcentration();

                    var meanAirConcentrationValue = meanAirConcentration.InMilligramPerCubicMetre();
                    Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedMeanAirConcentration, meanAirConcentrationValue, 0.1), string.Format("The actual mean air concentration {0} differs from the expected value {1} with more than the allowed tolerance.", meanAirConcentrationValue, expectedMeanAirConcentration));

                    if (peakAirConcentration.Value.HasValue)
                    {
                        var peakAirConcentrationValue = peakAirConcentration.InMilligramPerCubicMetre();
                        Assert.IsTrue(peakAirConcentrationValue >= meanAirConcentrationValue, string.Format("The peak air concentration {0} is less than the mean air concentration {1}.", peakAirConcentrationValue, meanAirConcentrationValue));

                        if (expectedPeakAirConcentration.HasValue)
                        {
                            Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedPeakAirConcentration.Value, peakAirConcentrationValue, 0.1), string.Format("The actual peak air concentration value {0} differs from the expected value {1} with more than the allowed tolerance.", meanAirConcentrationValue, expectedMeanAirConcentration));
                        }
                    }
                }
            }
        }
    }
}