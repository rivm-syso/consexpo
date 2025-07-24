using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Tests.Helpers;

namespace RIVM.ConsExpo.Model.Tests.Submodels
{
    [TestClass]
    public class InhalationExposureVapourConstantRateTests : InhalationExposureSubModelBase
    {
        [TestMethod]
        public void InhalationExposureVapourConstantRateIgnoreSaturationLimitTest()
        {
            TestInhalationExposure(InhalationExposureSubmodelTypes.VapourConstantRate, false, 21.0);
        }

        /// <summary>
        /// Tests the inhalation exposure for model constant rate.
        /// </summary>
        [TestMethod]
        public void InhalationExposureVapourConstantRateApplySaturationLimitTest()
        {
            TestInhalationExposure(InhalationExposureSubmodelTypes.VapourConstantRate, true, 14.2);
        }

        /// <summary>
        /// Inhalations the exposure vapour constant rate no ventilation test.
        /// </summary>
        /// <seealso>inhalation vapour constant rate no ventilation.Ce4</seealso>
        [TestMethod]
        public void InhalationExposureVapourConstantRateNoVentilationTest()
        {
            var scenario = GetDefaultScenario(InhalationExposureSubmodelTypes.VapourConstantRate, false);
            scenario.InhalationExposure.VentilationRate.Value = 0;

            TestHelpers.SetupValidateAndTest(scenario, 80.7);
        }

        [TestMethod]
        public void InhalationExposureVapourConstantRateExposureFractionTest()
        {
            var scenario = GetDefaultScenario(InhalationExposureSubmodelTypes.VapourConstantRate, false);

            scenario.InhalationExposure.InhalationRate = new VolumeRate
            {
                Value = 5,
                Unit = VolumeRateUnits.CubicMetrePerDay
            };

            // Value taken from ConsExpo web 1.0.7. This test is only for regression, as the test value has not be checked independently.
            TestInhalationExposureExposureFraction(0.00671, scenario);
        }
    }
}
