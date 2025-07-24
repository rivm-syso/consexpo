using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class LengthTests
    {
        [TestMethod]
        public void LengthUnitConversionTest()
        {
            //All instances are 1 millimetre, expressed in different units.
            Length x = new Thickness()
            {
                Value = 1,
                Unit = LengthUnits.Millimetre
            };

            Length y = new Thickness()
            {
                Value = 0.1,
                Unit = LengthUnits.Centimetre
            };

            Length z = new Thickness()
            {
                Value = 1E3,
                Unit = LengthUnits.Micrometre
            };

            Length u = new Height()
            {
                Value = 1E-3,
                Unit = LengthUnits.Metre
            };

            TestHelpers.AreEqual(x.InCentimetre(), y.InCentimetre());
            TestHelpers.AreEqual(x.InCentimetre(), z.InCentimetre());
            TestHelpers.AreEqual(x.InCentimetre(), u.InCentimetre());
        }

        [TestMethod]
        public void LengthInMetresTest()
        {
            Length x = new Height()
            {
                Value = 1,
                Unit = LengthUnits.Metre
            };

            TestHelpers.AreEqual(1, x.InMetre());
        }

        [TestMethod]
        public void LengthInCentimetreTest()
        {
            Length x = new Thickness()
            {
                Value = 1,
                Unit = LengthUnits.Centimetre
            };

            TestHelpers.AreEqual(1, x.InCentimetre());
        }

        [TestMethod]
        public void LengthInMilliMetreTest()
        {
            Length x = new Thickness()
            {
                Value = 1,
                Unit = LengthUnits.Millimetre
            };

            TestHelpers.AreEqual(1, x.InMillimetre());
        }

        [TestMethod]
        public void LengthInMicroMetreTest()
        {
            Length x = new Thickness()
            {
                Value = 1,
                Unit = LengthUnits.Micrometre
            };

            TestHelpers.AreEqual(1, x.InMicrometre());
        }
    }
}