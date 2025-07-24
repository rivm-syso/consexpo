using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Tests.Helpers;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class InhalationExposureVapourInstantaneousReleaseTests : InhalationExposureSubModelBase
    {
        [TestMethod]
        public void InhalationExposureVapourInstantaneousReleaseIgnoreSaturationLimitTest()
        {
            TestInhalationExposure(InhalationExposureSubmodelTypes.VapourInstantaneousRelease, false, 21.2);
        }

        [TestMethod]
        public void InhalationExposureVapourInstantaneousReleaseApplySaturationLimitTest()
        {
            TestInhalationExposure(InhalationExposureSubmodelTypes.VapourInstantaneousRelease, true, 20.0);
        }

        /// <summary>
        /// Inhalations the exposure vapour constant rate no ventilation test.
        /// </summary>
        /// <seealso>inhalation vapour constant rate - no ventilation.Ce4</seealso>
        [TestMethod]
        public void InhalationExposureVapourInstantReleaseNoVentilationTest()
        {
            var scenario = GetDefaultScenario(InhalationExposureSubmodelTypes.VapourInstantaneousRelease, true);
            scenario.InhalationExposure.VentilationRate.Value = 0;

            TestHelpers.SetupValidateAndTest(scenario, 23.7);
        }

        [TestMethod]
        public void InhalationExposureVapourInstantReleaseExposureFractionTest()
        {
            var scenario = GetDefaultScenario(InhalationExposureSubmodelTypes.VapourInstantaneousRelease, true);
            scenario.InhalationExposure.InhalationRate = new VolumeRate
            {
                Value = 5,
                Unit = VolumeRateUnits.CubicMetrePerDay
            };

            // Value taken from ConsExpo web 1.0.7. This test is only for regression, as the test value has not be checked independently.
            TestInhalationExposureExposureFraction(0.00701, scenario);
        }
    }
}