using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using RIVM.ConsExpo.Model.Submodels;
using RIVM.ConsExpo.TestFacilities;

namespace RIVM.ConsExpo.Model.Tests.Helpers
{
    /// <summary>
    /// A helper class for generating test scenarios.
    /// </summary>
    /// <remarks>Many test scenarios are now generated in specific test classes, which is fine as long as they are only use in one test class.</remarks>

    public class TestHelpers
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

            return true;
        }

        public static void SetupValidateAndTest(ScenarioModel scenario, double expectedMeanAirConcentration, double? expectedPeakAirConcentration = null, double? expectedMeanAirConcentrationPeak = null)
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
                        throw new NotSupportedException(
                            $"Unsupported inhalatory exposure submodel '{scenario.InhalationExposure.SubmodelType.ToString()}'");
                }

                if (scenario.InhalationExposureRouteInUse)
                {
                    var meanAirConcentration = inhalatoryExposureSimulation.MeanAirConcentration();

                    var meanAirConcentrationValue = meanAirConcentration.InMilligramPerCubicMetre();
                    Assert.IsTrue(
                        Comparisons.AlmostEqualMagnitude(expectedMeanAirConcentration, meanAirConcentrationValue, 0.1),
                        $"The actual mean air concentration {meanAirConcentrationValue} differs from the expected value {expectedMeanAirConcentration} with more than the allowed tolerance.");

                    if (inhalatoryExposureSimulation.SupportsPeakAirConcentration)
                    {
                        var peakAirConcentration = inhalatoryExposureSimulation.PeakAirConcentration();
                        var peakAirConcentrationValue = peakAirConcentration.InMilligramPerCubicMetre();
                        Assert.IsTrue(peakAirConcentrationValue >= meanAirConcentrationValue,
                            $"The peak air concentration {peakAirConcentrationValue} is less than the mean air concentration {meanAirConcentrationValue}.");

                        if (expectedPeakAirConcentration.HasValue)
                        {
                            Assert.IsTrue(
                                Comparisons.AlmostEqualMagnitude(expectedPeakAirConcentration.Value,
                                    peakAirConcentrationValue, 0.1),
                                $"The actual peak air concentration value {meanAirConcentrationValue} differs from the expected value {expectedMeanAirConcentration} with more than the allowed tolerance.");
                        }
                    }

                    if (scenario.InhalationExposure.ReEntry)
                    {
                        var meanAirConcentrationPeakValue = inhalatoryExposureSimulation.MeanAirConcentrationPeak().Value.Value;

                        if (expectedMeanAirConcentrationPeak.HasValue)
                        {
                            Assert.IsTrue(
                                Comparisons.AlmostEqualMagnitude(expectedMeanAirConcentrationPeak.Value,
                                    meanAirConcentrationPeakValue, 0.1),
                                $"The actual mean air concentration peak value {meanAirConcentrationPeakValue} differs from the expected value {expectedMeanAirConcentrationPeak} with more than the allowed tolerance.");
                        }
                    }
                }
            }
        }
    }
}
