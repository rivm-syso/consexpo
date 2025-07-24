using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class ExposureDurationTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. Add units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void ExposureDurationCompleteUnitSupportTest()
        {
            var x = new ExposureDuration();
            foreach (DurationUnits unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InSeconds();
            }
        }
    }
}