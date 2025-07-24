using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class MassRateTests
    {
        [ClassInitialize]
        public static void Init(TestContext context)
        {
        }

        [TestMethod]
        public void ConversionTests1()
        {
            //All instances are 1 gram per second, expressed in different units.
            MassRate x = new ContactRate()
            {
                Value = 1E6 * 60,
                Unit = MassRateUnits.MicrogramPerMinute
            };

            MassRate y = new MassGenerationRate()
            {
                Value = 1E3 * 60,
                Unit = MassRateUnits.MilligramPerMinute
            };

            MassRate z = new MassGenerationRate()
            {
                Value = 1,
                Unit = MassRateUnits.GramPerSecond
            };

            MassRate u = new MassGenerationRate()
            {
                Value = 60,
                Unit = MassRateUnits.GramPerMinute
            };

            MassRate v = new IngestionRate()
            {
                Value = 60 * 60,
                Unit = MassRateUnits.GramPerHour
            };

            MassRate w = new IngestionRate()
            {
                Value = 60 * 60 * 24,
                Unit = MassRateUnits.GramPerDay
            };

            TestHelpers.AreEqual(x.InMilligramPerMinute(), y.InMilligramPerMinute());
            TestHelpers.AreEqual(x.InMilligramPerMinute(), z.InMilligramPerMinute());
            TestHelpers.AreEqual(x.InMilligramPerMinute(), u.InMilligramPerMinute());
            TestHelpers.AreEqual(x.InMilligramPerMinute(), v.InMilligramPerMinute());
            TestHelpers.AreEqual(x.InMilligramPerMinute(), w.InMilligramPerMinute());

            TestHelpers.AreEqual(x.InGramPerSecond(), y.InGramPerSecond());
        }

        [TestMethod]
        public void ConversionTests2()
        {
            MassRate q = new MassRatePackaging
            {
                Value = 1E3,
                Unit = MassRateUnits.MicrogramPerHour
            };

            MassRate r = new MassRatePackaging
            {
                Value = 2.4E4,
                Unit = MassRateUnits.MicrogramPerDay
            };

            MassRate s = new MassRatePackaging
            {
                Value = 1,
                Unit = MassRateUnits.MilligramPerHour
            };

            MassRate t = new MassRatePackaging
            {
                Value = 24,
                Unit = MassRateUnits.MilligramPerDay
            };

            TestHelpers.AreEqual(q.InMilligramPerMinute(), r.InMilligramPerMinute());
            TestHelpers.AreEqual(q.InMilligramPerMinute(), s.InMilligramPerMinute());
            TestHelpers.AreEqual(q.InMilligramPerMinute(), t.InMilligramPerMinute());

            TestHelpers.AreEqual(q.InGramPerDay(), r.InGramPerDay());
        }

        [TestMethod]
        public void MassInGramPerHourTest()
        {
            //All instances are 1 gram per second, expressed in different units.
            MassRate x = new ContactRate()
            {
                Value = 1,
                Unit = MassRateUnits.GramPerHour
            };
            TestHelpers.AreEqual(1, x.InGramPerHour());
        }

        [TestMethod]
        public void MassInGramPerSecondTest()
        {
            //All instances are 1 gram per second, expressed in different units.
            MassRate x = new MassGenerationRate()
            {
                Value = 1,
                Unit = MassRateUnits.GramPerSecond
            };
            TestHelpers.AreEqual(1, x.InGramPerSecond());
        }

        [TestMethod]
        public void MassInMilligramPerMinuteTest()
        {
            //All instances are 1 gram per second, expressed in different units.
            MassRate x = new MassGenerationRate()
            {
                Value = 1,
                Unit = MassRateUnits.MilligramPerMinute
            };
            TestHelpers.AreEqual(1, x.InMilligramPerMinute());
        }
    }
}