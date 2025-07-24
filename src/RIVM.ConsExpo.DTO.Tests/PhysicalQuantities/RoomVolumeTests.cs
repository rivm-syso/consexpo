using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class RoomVolumeTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void RoomVolumeCompleteUnitSupportTest()
        {
            var x = new RoomVolume();
            foreach (var unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InCubicMetres();
            }
        }

        //Litres are currently not in use with any physical quantity.
        //[TestMethod]
        //public void RoomVolumeUnitConversionTest()
        //{
        //    var x = new RoomVolume()
        //    {
        //        Value = 1000,
        //        Unit = VolumeUnits.Litre
        //    };

        //    TestHelpers.AreEqual(1, x.InCubicMetres());
        //}
    }
}