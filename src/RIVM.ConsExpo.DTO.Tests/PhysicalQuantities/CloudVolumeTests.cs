using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class CloudVolumeTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void CloudVolumeCompleteUnitSupportTest()
        {
            var x = new CloudVolume();
            foreach (var unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InCubicMetres();
            }
        }

        [TestMethod]
        public void CloudVolumeUnitConversionTest()
        {
            var x = new CloudVolume()
            {
                Value = 1,
                Unit = VolumeUnits.CubicMetre
            };

            TestHelpers.AreEqual(1, x.InCubicMetres());
            TestHelpers.AreEqual(1000, x.InLitres());
        }
    }
}