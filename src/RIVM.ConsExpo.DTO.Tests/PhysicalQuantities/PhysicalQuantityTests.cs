using DataAnnotationsExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Linq;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class PhysicalQuantityTests
    {
        [TestMethod]
        public void ReadSubstanceConcentrationMinAttributeTest()
        {
            var attrib = typeof(SubstanceConcentration)
              .GetProperty("Value")
              .GetCustomAttributes(typeof(MinAttribute), false)
              .Cast<MinAttribute>()
              .FirstOrDefault();

            Assert.IsTrue(((double)attrib.Min) == 0.0);
        }

        [TestMethod]
        public void ReadSubstanceTemperatureMinAttributeTest()
        {
            var attrib = typeof(Temperature)
              .GetProperty("Value")
              .GetCustomAttributes(typeof(MinAttribute), false)
              .Cast<MinAttribute>()
              .FirstOrDefault();

            Assert.IsTrue(((double)attrib.Min) == -100.0);
        }

        [TestMethod]
        public void EqualValuesTestBothNull()
        {
            bool expected = true;

            Length x = null, y = null;

            Assert.AreEqual<bool>(expected, Length.EqualValues(x, y));
        }

        [TestMethod]
        public void EqualValuesTestOneNull()
        {
            bool expected = true;

            Length x = null, y = new Height() { Value = 1, Unit = LengthUnits.Metre };

            Assert.AreNotEqual<bool>(expected, Length.EqualValues(x, y));
        }

        [TestMethod]
        public void EqualValuesTestOneNullOneNoValue()
        {
            bool expected = true;

            Length x = null, y = new Height() { Unit = LengthUnits.Metre };

            Assert.AreNotEqual<bool>(expected, Length.EqualValues(x, y));
        }

        [TestMethod]
        public void EqualValuesTestDifferentUnits()
        {
            bool expected = true;

            Length x = new Height() { Value = 1, Unit = LengthUnits.Metre }
                , y = new Thickness() { Value = 1, Unit = LengthUnits.Centimetre };

            Assert.AreNotEqual<bool>(expected, Length.EqualValues(x, y));
        }

        [TestMethod]
        public void EqualValuesTestDifferentValues()
        {
            bool expected = true;

            Length x = new Height() { Value = 10, Unit = LengthUnits.Metre }
                , y = new Height() { Value = 1, Unit = LengthUnits.Metre };

            Assert.AreNotEqual<bool>(expected, Length.EqualValues(x, y));
        }

        [TestMethod]
        public void EqualValuesTestDifferentUnitsAllowConversion()
        {
            bool expected = true;

            Length x = new Height() { Value = 1, Unit = LengthUnits.Metre }
                , y = new Thickness() { Value = 1, Unit = LengthUnits.Centimetre };

            Assert.AreNotEqual<bool>(expected, Length.EqualValues(x, y, true));
        }

        [TestMethod]
        public void EqualValuesTestDifferentValuesAllowConversion()
        {
            bool expected = true;

            Length x = new Height() { Value = 1, Unit = LengthUnits.Metre }
                , y = new Thickness() { Value = 100, Unit = LengthUnits.Centimetre };

            Assert.AreEqual<bool>(expected, Length.EqualValues(x, y, true));
        }
    }
}