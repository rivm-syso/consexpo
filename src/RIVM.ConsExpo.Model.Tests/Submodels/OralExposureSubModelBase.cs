using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.Model.Interfaces.Models;
using RIVM.ConsExpo.Model.Models;
using RIVM.ConsExpo.Model.Tests.Helpers;
using RIVM.ConsExpo.TestFacilities;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    public class OralExposureSubModelBase
    {
        protected AssessmentModel GetAssessment()
        {
            return new AssessmentModel()
            {
                Substance = new SubstanceModel()
                {
                    Name = "Oral intakable substance",
                    CASNumber = "1181081-51-5"
                },
                Population = new PopulationModel()
                {
                    Name = "Volwassenen",
                    BodyWeight = new BodyWeight() { Value = 65, Unit = MassUnits.Kilogram }
                },
                Product = new ProductModel()
                {
                    WeightFractionSubstanceDefault = new Fraction
                    {
                        Value = 0.01,
                        Unit = FractionUnits.Fraction
                    }
                }
            };
        }

        protected static void TestOralExposureExternalEventDose(double expectedExternalEventDose, ScenarioModel scenario)
        {
            if (TestHelpers.ValidateInput(scenario))
            {
                IOralSimulation oralExposureSimulation = new OralSimulation();

                if (scenario.OralExposureRouteInUse)
                {
                    var exposureOutputValues = oralExposureSimulation.CalculatePointValues(scenario);

                    var actualExternalEventDose = exposureOutputValues.Exposure.AsExternalEventDose;
                    Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedExternalEventDose, actualExternalEventDose.Value.Value),
                        $"The actual external event dose value {actualExternalEventDose.Value} differs from the expected value {expectedExternalEventDose} with more than the allowed tolerance.");
                }
            }
        }

        protected static void TestOralExposureExposureFraction(double? expectedExposureFraction, ScenarioModel scenario, double tolerance = 0.05)
        {
            if (TestHelpers.ValidateInput(scenario))
            {
                IOralSimulation oralExposureSimulation = new OralSimulation();

                if (scenario.OralExposureRouteInUse)
                {
                    var exposureOutputValues = oralExposureSimulation.CalculatePointValues(scenario);

                    var actualExposureFraction = exposureOutputValues.Exposure.AsExposureFraction;

                    if (expectedExposureFraction == null)
                    {
                        Assert.IsNull(actualExposureFraction.Value);
                    }
                    else
                    {
                        Assert.IsNotNull(actualExposureFraction.Value, "The actual exposure fraction should be null, as this model does not support exposure fractions.");
                        Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedExposureFraction.Value,
                                actualExposureFraction.Value.Value, tolerance),
                            $"The actual exposure fraction {actualExposureFraction.Value} differs from the expected value {expectedExposureFraction} with more than the allowed tolerance.");
                    }
                }
                else
                {
                    Assert.Inconclusive("The specified scenario does not specify oral exposure.");
                }
            }
        }
    }
}