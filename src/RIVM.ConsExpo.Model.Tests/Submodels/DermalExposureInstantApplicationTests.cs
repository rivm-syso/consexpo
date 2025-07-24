using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.Model.Interfaces.Models;
using RIVM.ConsExpo.Model.Models;
using System;
using System.Diagnostics;
using RIVM.ConsExpo.Model.Tests.Helpers;
using RIVM.ConsExpo.TestFacilities;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class DermalExposureInstantApplicationTests : DermalExposureSubModelBase
    {
        [TestMethod]
        public void DermalExposureInstantApplicationHandCreamTest()
        {
            var scenarioName = "Application";
            var frequencyValue = 7.3E2;
            var frequencyUnit = FrequencyUnits.Yearly;
            var weightFraction = 0.1;
            var productWeightValue = 1.7;
            var productWeightUnit = MassUnits.Gram;

            var scenario = ScenarioHelper.GetScenarioDermalExposureInstantApplication(scenarioName, frequencyValue, frequencyUnit, weightFraction, productWeightValue, productWeightUnit);

            scenario.Assessment = ScenarioHelper.GetAssessment(60, 0.1);

            TestDermalExposureExternalEventDose(2.8, scenario);
        }

        [TestMethod]
        public void DermalExposureInstantApplicationExposureFractionTest()
        {
            var scenarioName = "Application";
            var frequencyValue = 2;
            var frequencyUnit = FrequencyUnits.Weekly;
            var weightFraction = 0.1;
            var productWeightValue = 1.7;
            var productWeightUnit = MassUnits.Gram;

            var scenario = ScenarioHelper.GetScenarioDermalExposureInstantApplication(scenarioName, frequencyValue, frequencyUnit, weightFraction, productWeightValue, productWeightUnit);

            scenario.Assessment = ScenarioHelper.GetAssessment(60, 0.1);

            TestDermalExposureExposureFraction(1.0, scenario);
        }

        [TestMethod]
        public void DermalExposureMonteCarloSimulationTest()
        {
            var scenarioName = "Application";
            var frequencyValue = 1.0;
            var frequencyUnit = FrequencyUnits.Daily;
            var weightFraction = 1;
            var productAmountValue = 0.0;
            var productAmountUnit = MassUnits.Gram;

            var expectedMean = 7.67;
            var expectedSD = 4.53;

            var scenario = ScenarioHelper.GetScenarioDermalExposureInstantApplication(scenarioName, frequencyValue, frequencyUnit, weightFraction, productAmountValue, productAmountUnit);

            scenario.Assessment = ScenarioHelper.GetAssessment(60, 0.1);

            if (TestHelpers.ValidateInput(scenario))
            {
                scenario.Assessment.Population.BodyWeight = new BodyWeight()
                {
                    Unit = MassUnits.Kilogram,
                    Distribution = new Distribution
                    {
                        DistributionType = DistributionTypes.Uniform,
                        LowerBound = 55,
                        UpperBound = 75
                    }
                };

                scenario.DermalExposure.ProductAmount = new ProductAmount()
                {
                    Unit = MassUnits.Gram,
                    Distribution = new Distribution
                    {
                        DistributionType = DistributionTypes.Uniform,
                        LowerBound = 0,
                        UpperBound = 1
                    }
                };

                IDermalSimulation dermalExposureSimulation = new DermalSimulation();

                if (scenario.DermalExposureRouteInUse)
                {
                    const int Iterations = 10000;
                    var endPoints = new EndPoint<DermalExposureOutcome, DermalAbsorptionOutcome>(Iterations);

                    for (int iteration = 0; iteration < Iterations; iteration++)
                    {
                        scenario.DermalExposure.ProductAmount.Sample();
                        scenario.Assessment.Population.BodyWeight.Sample();
                        endPoints.Points.Add(iteration, dermalExposureSimulation.CalculatePointValues(scenario));
                    }

                    double s1 = 0.0;
                    double s2 = 0.0;

#warning To Do: This code can be moved to the model class, as the mean and sd must be displayed eventually.
#warning To Do: https://en.wikipedia.org/wiki/Standard_deviation#Rapid_calculation_methods describes a better algorithm with reduced rouding errors.
                    foreach (var outcome in endPoints.Points)
                    {
                        double value = outcome.Value.Exposure.AsExternalEventDose.Value.Value;
                        s1 += value;
                        s2 += value * value;
                    }

                    double sampleCount = (double)endPoints.Points.Count;

                    double actualMean = s1 / sampleCount;
                    double actualSD = Math.Sqrt((sampleCount * s2 - s1 * s1) / (sampleCount * (sampleCount - 1.0)));

                    Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedMean, actualMean),
                        $"The actual mean of the distribution {actualMean} differs from the expected value {expectedMean} with more than the allowed tolerance.");

                    Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedSD, actualSD),
                        $"The actual mean of the distribution {actualSD} differs from the expected value {expectedSD} with more than the allowed tolerance.");

                    Debug.WriteLine("Sample size: {2}, Mean: {0}, Standard deviation {1}.", actualMean, actualSD, sampleCount);
                }
                else
                {
                    Assert.Inconclusive("The tested scenario does not specify dermal exposure.");
                }
            }
        }
    }
}