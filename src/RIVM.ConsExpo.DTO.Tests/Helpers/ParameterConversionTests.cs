using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Helpers;

namespace RIVM.ConsExpo.DTO.Tests.Helpers
{
    [TestClass]
    public class ParameterConversionTests
    {
        [TestMethod]
        public void SignificantDigitsTestNegative()
        {
            double rawValue = -123.456;
            int digits = 2;
            double expected = -120;

            double actual = ParameterConversion.SignificantDigits(rawValue, digits);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SignificantDigitsTestZero()
        {
            double rawValue = 0;
            int digits = 2;
            double expected = 0;

            double actual = ParameterConversion.SignificantDigits(rawValue, digits);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SignificantDigitsTestA1()
        {
            double rawValue = 123.456789;
            int digits = 1;
            double expected = 100.0;

            double actual = ParameterConversion.SignificantDigits(rawValue, digits);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SignificantDigitsTestA2()
        {
            double rawValue = 123.456789;
            int digits = 2;
            double expected = 120.0;

            double actual = ParameterConversion.SignificantDigits(rawValue, digits);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SignificantDigitsTestA3()
        {
            double rawValue = 123.456789;
            int digits = 3;
            double expected = 123.0;

            double actual = ParameterConversion.SignificantDigits(rawValue, digits);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SignificantDigitsTestA4()
        {
            double rawValue = 123.456789;
            int digits = 4;
            double expected = 123.5;

            double actual = ParameterConversion.SignificantDigits(rawValue, digits);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SignificantDigitsTestB1()
        {
            double rawValue = 0.0123456789;
            int digits = 2;
            double expected = 0.012;

            double actual = ParameterConversion.SignificantDigits(rawValue, digits);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SignificantDigitsTestB2()
        {
            double rawValue = 0.0123456789;
            int digits = 3;
            double expected = 0.0123;

            double actual = ParameterConversion.SignificantDigits(rawValue, digits);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SignificantDigitsTestB3()
        {
            double rawValue = 0.0123456789;
            int digits = 4;
            double expected = 0.01235;

            double actual = ParameterConversion.SignificantDigits(rawValue, digits);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SignificantDigitsTestAlmost1000()
        {
            double rawValue = 999.999;
            int digits = 2;
            double expected = 1000;

            double actual = ParameterConversion.SignificantDigits(rawValue, digits);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SignificantDigitsTestExact1000()
        {
            double rawValue = 1000;
            int digits = 2;
            double expected = 1000;

            double actual = ParameterConversion.SignificantDigits(rawValue, digits);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SignificantDigitsTestAlmost1000th()
        {
            double rawValue = 0.00999;
            int digits = 2;
            double expected = 0.01;

            double actual = ParameterConversion.SignificantDigits(rawValue, digits);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SignificantDigitsTestExact1000th()
        {
            double rawValue = 0.01;
            int digits = 2;
            double expected = 0.01;

            double actual = ParameterConversion.SignificantDigits(rawValue, digits);
            Assert.AreEqual(expected, actual);
        }
    }
}