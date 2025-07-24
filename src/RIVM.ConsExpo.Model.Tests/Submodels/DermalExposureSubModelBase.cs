using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.Model.Interfaces.Models;
using RIVM.ConsExpo.Model.Models;
using RIVM.ConsExpo.Model.Tests.Helpers;
using RIVM.ConsExpo.TestFacilities;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    public class DermalExposureSubModelBase
    {
        protected static void TestDermalExposureExternalEventDose(double expectedExternalEventDose, ScenarioModel scenario)
        {
            if (TestHelpers.ValidateInput(scenario))
            {
                IDermalSimulation dermalExposureSimulation = new DermalSimulation();

                if (scenario.DermalExposureRouteInUse)
                {
                    var exposureOutputValues = dermalExposureSimulation.CalculatePointValues(scenario);

                    var actualExternalEventDose = exposureOutputValues.Exposure.AsExternalEventDose.Value;
                    Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedExternalEventDose, actualExternalEventDose.Value),
                        $"The actual external event dose value {actualExternalEventDose} differs from the expected value {expectedExternalEventDose} with more than the allowed tolerance.");
                }
            }
        }

        protected static void TestDermalExposureExposureFraction(double? expectedExposureFraction, ScenarioModel scenario)
        {
            if (TestHelpers.ValidateInput(scenario))
            {
                IDermalSimulation dermalExposureSimulation = new DermalSimulation();

                if (scenario.DermalExposureRouteInUse)
                {
                    var exposureOutputValues = dermalExposureSimulation.CalculatePointValues(scenario);

                    var actualExposureFraction = exposureOutputValues.Exposure.AsExposureFraction;

                    if (expectedExposureFraction == null)
                    {
                        Assert.IsNull(actualExposureFraction.Value);
                    }
                    else
                    {
                        Assert.IsNotNull(actualExposureFraction.Value, "The actual exposure fraction should be null, as this model does not support exposure fractions.");
                        Assert.IsTrue(Comparisons.AlmostEqualMagnitude(expectedExposureFraction.Value,
                                actualExposureFraction.Value.Value),
                            $"The actual exposure fraction {actualExposureFraction.Value} differs from the expected value {expectedExposureFraction} with more than the allowed tolerance.");
                    }
                }
            }
        }
    }
}