using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Tests.Helpers;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class InhalationExposureSprayInstantReleaseTests : InhalationExposureSubModelBase
    {
        /// <summary>
        /// Inhalations the exposure vapour constant rate no ventilation test.
        /// </summary>
        [TestMethod]
        public void InhalationExposureSprayInstantReleaseNoVentilationTest()
        {
            var scenario = GetDefaultScenario(InhalationExposureSubmodelTypes.SprayInstantaneousRelease, false);
            scenario.InhalationExposure.VentilationRate.Value = 0;

            TestHelpers.SetupValidateAndTest(scenario, 86.2);
        }

        [TestMethod]
        public void InhalationExposureSprayInstantReleaseExposureFractionTest()
        {
            var scenario = GetDefaultScenario(InhalationExposureSubmodelTypes.SprayInstantaneousRelease, false);
            scenario.InhalationExposure.InhalationRate = new VolumeRate
            {
                Value = 3,
                Unit = VolumeRateUnits.CubicMetrePerDay
            };

            // Value taken from ConsExpo web 1.0.7. This test is only for regression, as the test value has not be checked independently.
            TestInhalationExposureExposureFraction(0.00423, scenario);
        }

        [TestMethod]
        public void InhalationExposureSprayInstantaneousReleaseTest()
        {
            TestInhalationExposure(InhalationExposureSubmodelTypes.SprayInstantaneousRelease, false, 21.2);
        }
    }
}