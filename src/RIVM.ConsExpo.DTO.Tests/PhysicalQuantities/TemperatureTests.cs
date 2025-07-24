using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class TemperatureTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void TemperatureCompleteUnitSupportTest()
        {
            var x = new Temperature();
            foreach (var unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InKelvin();
            }
        }

        [TestMethod]
        public void TemperatureUnitConversionTest()
        {
            var x = new Temperature()
            {
                Value = 273.15,
                Unit = TemperatureUnits.Kelvin
            };

            var y = new Temperature()
            {
                Value = 0,
                Unit = TemperatureUnits.Celsius
            };

            TestHelpers.AreEqual(x.InKelvin(), y.InKelvin());
        }

        [TestMethod]
        public void TemperatureConvertedValueFromCelsiusToCelsiusTest()
        {
            var x = new Temperature()
            {
                Value = 20.0,
                Unit = TemperatureUnits.Celsius
            };

            TestHelpers.AreEqual(x.ConvertedValue(TemperatureUnits.Celsius), 20.0);
        }

        [TestMethod]
        public void TemperatureConvertedValueFromCelsiusToKelvinTest()
        {
            var x = new Temperature()
            {
                Value = 20.0,
                Unit = TemperatureUnits.Celsius
            };

            TestHelpers.AreEqual(x.ConvertedValue(TemperatureUnits.Kelvin), 293.15);
        }

        [TestMethod]
        public void TemperatureConvertedValueFromKelvinToCelsiusTest()
        {
            var x = new Temperature()
            {
                Value = 263.15,
                Unit = TemperatureUnits.Kelvin
            };

            TestHelpers.AreEqual(x.ConvertedValue(TemperatureUnits.Celsius), -10.0);
        }

        [TestMethod]
        public void TemperatureConvertedValueFromKelvinToKelvinTest()
        {
            var x = new Temperature()
            {
                Value = 285.0,
                Unit = TemperatureUnits.Kelvin
            };

            TestHelpers.AreEqual(x.ConvertedValue(TemperatureUnits.Kelvin), 285.0);
        }
    }
}