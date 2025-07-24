using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Helpers;

namespace RIVM.ConsExpo.DTO.Tests.Helpers
{
    [TestClass()]
    public class UniqueNameHelperTests
    {
        [TestMethod()]
        public void GetUniqueNameImmediateSuccessTest()
        {
            string start = "name";
            string expected = start;
            var uniqueNameChecker = new UniqueNameHelper.CheckUniqueNameForUser(CheckUniqueNameImmediateSuccess);
            string actual = UniqueNameHelper.GetUniqueNameForUser(start, uniqueNameChecker, -1, 10);
            Assert.AreEqual<string>(expected, actual);
        }

        private bool CheckUniqueNameImmediateSuccess(string name, int userId)
        {
            return true;
        }

        [TestMethod()]
        public void GetUniqueNameSecondSuccessTest()
        {
            string start = "name";
            string expected = start + "_2";
            var uniqueNameChecker = new UniqueNameHelper.CheckUniqueNameForUser(CheckUniqueNameSecondSuccess);
            string actual = UniqueNameHelper.GetUniqueNameForUser(start, uniqueNameChecker, 10, -1, "_{0}", 2);
            Assert.AreEqual<string>(expected, actual);
        }

        private bool CheckUniqueNameSecondSuccess(string name, int userId)
        {
            return name == "name_2";
        }

        [TestMethod()]
        public void GetUniqueNameThirdSuccessTest()
        {
            string start = "name";
            string expected = start + " (5)";
            var uniqueNameChecker = new UniqueNameHelper.CheckUniqueNameForUser(CheckUniqueNameThirdSuccess);
            string actual = UniqueNameHelper.GetUniqueNameForUser(start, uniqueNameChecker, 10, -1, " ({0})", 4);
            Assert.AreEqual<string>(expected, actual);
        }

        private bool CheckUniqueNameThirdSuccess(string name, int userId)
        {
            return name == "name (5)";
        }

        [TestMethod()]
        public void GetUniqueNameEllipsisTest()
        {
            string start = "name";
            string expected = "na…_10";
            var uniqueNameChecker = new UniqueNameHelper.CheckUniqueNameForUser(CheckUniqueNameEllipsis);
            string actual = UniqueNameHelper.GetUniqueNameForUser(start, uniqueNameChecker, 6, -1, "_{0}", 1);
            Assert.AreEqual<string>(expected, actual);
        }

        private bool CheckUniqueNameEllipsis(string name, int userId)
        {
            return name.EndsWith("_10");
        }

        [TestMethod()]
        [ExpectedException(typeof(System.ApplicationException))]
        public void GetUniqueNameFailureTest()
        {
            string start = "nm";
            var uniqueNameChecker = new UniqueNameHelper.CheckUniqueNameForUser(CheckAlwaysFails);
            string actual = UniqueNameHelper.GetUniqueNameForUser(start, uniqueNameChecker, 4, -1, "_{0}", 1);
        }

        private bool CheckAlwaysFails(string name, int usedId)
        {
            return false;
        }
    }
}