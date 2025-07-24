using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Extensions;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Settings;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Models;
using RIVM.ConsExpo.Model.Models;
using RIVM.ConsExpo.Model.Tests.Helpers;
using System;
using System.Collections.Generic;

namespace RIVM.ConsExpo.Model.Tests.Models
{
    [TestClass]
    public class SimulationTests
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void SensitivityAnalysisTest()
        {
            var runSettings = new SensitivityAnalysisSettings
            {
                RouteToAnalyse = RouteTypes.Dermal,
                EndPointToAnalyse = DoseMeasureType.ExternalEventDose,
                ModelParameterToAnalyse = ModelParameters.DermalExposureProductAmount,
                LowerBound = 0,
                UpperBound = 1,
                UnitCode = MassUnits.Gram.Code
            };

            var scenario = new ScenarioModel(true)
            {
                Assessment = new AssessmentModel(true),
                DermalExposureRouteInUse = true,
                DermalExposure =
                {
                    SubmodelType = DermalExposureSubmodelTypes.InstantApplication,
                    ProductAmount = new DTO.PhysicalQuantities.ProductAmount {Value = 1, Unit = MassUnits.Gram},
                    WeightFractionSubstance = new DTO.PhysicalQuantities.Fraction
                    {
                        Value = 1, Unit = FractionUnits.Fraction
                    }
                }
            };

            IInhalationSimulation inhalationSimulation = new InhalationSimulation();
            IDermalSimulation dermalSimulation = new DermalSimulation();
            IOralSimulation oralSimulation = new OralSimulation();

            ISimulation simulation = new Simulation(inhalationSimulation, dermalSimulation, oralSimulation);

            var outcome = simulation.CalculateSensitivityAnalysis(scenario, runSettings);
        }

        /// <summary>
        ///
        /// </summary>
        /// Based on test case in <see href="https://gemini.rivm.nl/workspace/0/item/40500">CE2015-40500  US157 - Toevoegen Beta-distributie</see>
        [TestMethod()]
        public void CalculateResultsTest()
        {
            const int numberOfIterations = 100000;
            var scenario = ScenarioHelper.GetScenarioDermalExposureInstantApplication("MCWithBetaDist", 1,
                FrequencyUnits.Daily, 0, 1, MassUnits.Milligram);

            scenario.Assessment = ScenarioHelper.GetAssessment(1, 1);
            scenario.DermalExposure.WeightFractionSubstance.Distribution = new Distribution { DistributionType = DistributionTypes.Beta };

            var testSet = new Dictionary<Tuple<double, double>, Percentiles>
            {
                {
                    new Tuple<double, double>(1, 1),
                    new Percentiles
                    {
                        P10 = 0.1003,
                        P25 = 0.2502,
                        P50 = 0.5000,
                        P75 = 0.7501,
                        P90 = 0.9003,
                        P99 = 0.9900,
                        Average = 0.4995
                    }
                },
                {
                    new Tuple<double, double>(0.1, 1),
                    new Percentiles{
                        P10 = 1.003e-10,
                        P25 = 9.503e-07,
                        P50 = 9.702e-04,
                        P75 = 5.613e-02,
                        P90 = 3.491e-01,
                        P99 = 9.050e-01,
                        Average = 0.09064
                    }
                },
                {
                    new Tuple<double, double>(10, 1),
                    new Percentiles{
                        P10 = 0.7942,
                        P25 = 0.8705,
                        P50 = 0.9331,
                        P75 = 0.9717,
                        P90 = 0.9896,
                        P99 = 0.9990,
                        Average = 0.9092
                    }
                },
                {
                    new Tuple<double, double>(0.5, 5),
                    new Percentiles{
                        P10 = 0.001644,
                        P25 = 0.010520,
                        P50 = 0.046620,
                        P75 = 0.129600,
                        P90 = 0.247800,
                        P99 = 0.501400,
                        Average = 0.09083
                    }
                },
                {
                    new Tuple<double, double>(10, 20),
                    new Percentiles{
                        P10 = 0.2266,
                        P25 = 0.2734,
                        P50 = 0.3297,
                        P75 = 0.3894,
                        P90 = 0.4450,
                        P99 = 0.5417,
                        Average = 0.3333
                    }
                }
            };

            IInhalationSimulation inhalationSimulation = new InhalationSimulation();
            IDermalSimulation dermalSimulation = new DermalSimulation();
            IOralSimulation oralSimulation = new OralSimulation();

            ISimulation simulation = new Simulation(inhalationSimulation, dermalSimulation, oralSimulation);

            TestContext.WriteLine($"alpha\tbeta\t10th\trel\t25th\trel\t50th\trel\t75th\trel\t90th\trel\t99th\trel\taverage\trel");

            foreach (var testCase in testSet)
            {
                var outcomes = new List<double>(numberOfIterations);

                for (int simulationIteration = 0; simulationIteration < numberOfIterations; simulationIteration++)
                {
                    scenario.DermalExposure.WeightFractionSubstance.Distribution.Alpha = testCase.Key.Item1;
                    scenario.DermalExposure.WeightFractionSubstance.Distribution.Beta = testCase.Key.Item2;

                    scenario.SampleAll();
                    outcomes.Add(dermalSimulation.CalculatePointValues(scenario).Exposure.AsExternalEventDose.Value.Value);
                }

                outcomes.Sort();

                double p10 = outcomes[(int)(0.10 * numberOfIterations)];
                double p25 = outcomes[(int)(0.25 * numberOfIterations)];
                double p50 = outcomes[(int)(0.50 * numberOfIterations)];
                double p75 = outcomes[(int)(0.75 * numberOfIterations)];
                double p90 = outcomes[(int)(0.90 * numberOfIterations)];
                double p99 = outcomes[(int)(0.99 * numberOfIterations)];
                double sum = 0;
                for (int i = 0; i < numberOfIterations; i++)
                {
                    sum += outcomes[i];
                }
                double average = sum / (double)numberOfIterations;
                //TestContext.WriteLine(
                //    $"The calculated average {average} has a relative magnitude of {average.RelativeDifference(testCase.Value.Average)} as compared to the reference percentile {testCase.Value.Average} for alpha = {testCase.Key.Item1} and beta = {testCase.Key.Item2}.");
                TestContext.WriteLine($"{testCase.Key.Item1}\t{testCase.Key.Item2}\t{p10:g2}\t{p10.RelativeDifference(testCase.Value.P10):g2}\t{p25:g2}\t{p25.RelativeDifference(testCase.Value.P25):g2}\t{p50:g2}\t{p50.RelativeDifference(testCase.Value.P50):g2}\t{p75:g2}\t{p75.RelativeDifference(testCase.Value.P75):g2}\t{p90:g2}\t{p90.RelativeDifference(testCase.Value.P90):g2}\t{p99:g2}\t{p99.RelativeDifference(testCase.Value.P99):g2}\t{average:g2}\t{average.RelativeDifference(testCase.Value.Average):g2}");
            }
        }

        private void CheckPercentile(Tuple<double, double> parameters, double reference, string percentileName,
            double outcome)
        {
            TestContext.WriteLine(
              $"The calculated {percentileName} percentile {outcome} has a relative magnitude of {outcome.RelativeDifference(reference)} as compared to the reference percentile {reference} for alpha = {parameters.Item1} and beta = {parameters.Item2}.");
        }
    }

    internal class Percentiles
    {
        public double P10 { get; set; }
        public double P25 { get; set; }
        public double P50 { get; set; }
        public double P75 { get; set; }
        public double P90 { get; set; }
        public double P99 { get; set; }
        public double Average { get; set; }
    }
}