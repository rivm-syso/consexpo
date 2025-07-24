using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Extensions;

namespace RIVM.ConsExpo.DTO.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void ToFriendlyTestSpace()
        {
            Assert.AreEqual("", "".ToFriendly(), false);
        }

        [TestMethod]
        public void ToFriendlyTest1()
        {
            Assert.AreEqual("1", "1".ToFriendly(), false);
        }

        [TestMethod]
        public void ToFriendlyTestThisIsSomethingTest()
        {
            Assert.AreEqual("This is something", "ThisIsSomething".ToFriendly(), false);
        }

        [TestMethod]
        public void ToFriendlyTestthisIsSomethingTest()
        {
            Assert.AreEqual("this is something", "thisIsSomething".ToFriendly(), false);
        }

        [TestMethod]
        public void ToFriendlyTestTheAbbreviationNAMeansNotApplicableTest()
        {
            Assert.AreEqual("The abbreviation NA means not applicable", "TheAbbreviationNAMeansNotApplicable".ToFriendly(), false);
        }

        [TestMethod()]
        public void GenerateSlugTestEmptyString()
        {
            GenerateSlugTest("", "");
        }

        [TestMethod()]
        public void GenerateSlugTestAlreadyValid()
        {
            GenerateSlugTest("justavalidfilename", "justavalidfilename");
        }

        [TestMethod()]
        public void GenerateSlugTestTooLong()
        {
            GenerateSlugTest("thisisaverylongfilenameofmorethanfiftycharactersificountedcorrectly", "thisisaverylongfilenameofmorethanfiftycharactersif");
        }

        [TestMethod()]
        public void GenerateSlugTestWithNonAscii()
        {
            GenerateSlugTest(@"ЅЈТКАЕВНОРХС ЯфШЫЧЙЗЛ", "");
        }

        [TestMethod()]
        public void GenerateSlugTestUnwantedInFilenames()
        {
            GenerateSlugTest(@"some invalid characters*.""/\[]:;|=,", "some invalid characters");
        }

        [TestMethod()]
        public void GenerateSlugTestWithWithAccents()
        {
            GenerateSlugTest("Vous êtes très français", "Vous etes tres francais");
        }

        private static void GenerateSlugTest(string input, string expected)
        {
            string actual = input.GenerateSlug();

            Assert.AreEqual(expected, actual);
        }

    }
}