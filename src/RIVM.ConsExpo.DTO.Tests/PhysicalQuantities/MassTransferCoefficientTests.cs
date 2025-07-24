using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class MassTransferCoefficientTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void MassTransferCoefficientCompleteUnitSupportTest()
        {
            var x = new MassTransferCoefficient();
            foreach (var unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InMetresPerHour();
            }
        }
    }
}