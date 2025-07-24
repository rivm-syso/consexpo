using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class MassGenerationRateTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void MassGenerationRateCompleteUnitSupportTest()
        {
            var x = new MassGenerationRate();
            foreach (var unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InGramPerSecond();
            }
        }

        [ClassInitialize]
        public static void Init(TestContext context)
        {
        }

        [TestMethod]
        public void ConversionTests()
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

            MassRate v = new ContactRate()
            {
                Value = 60 * 60,
                Unit = MassRateUnits.GramPerHour
            };

            MassRate w = new ContactRate()
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
        public void MassRateInGramPerSecondTest()
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
        public void MassRateInMilligramPerMinuteTest()
        {
            //All instances are 1 gram per second, expressed in different units.
            MassRate x = new MassGenerationRate()
            {
                Value = 1,
                Unit = MassRateUnits.MilligramPerMinute
            };
            TestHelpers.AreEqual(1, x.InMilligramPerMinute());
        }

        [TestMethod]
        public void MassRateInGramPerHourTest()
        {
            //All instances are 1 gram per second, expressed in different units.
            MassRate x = new IngestionRate()
            {
                Value = 1,
                Unit = MassRateUnits.GramPerHour
            };
            TestHelpers.AreEqual(1, x.InGramPerHour());
        }

        [TestMethod]
        public void MassRateInGramPerDayTest()
        {
            //All instances are 1 gram per second, expressed in different units.
            MassRate x = new IngestionRate()
            {
                Value = 1,
                Unit = MassRateUnits.GramPerDay
            };
            TestHelpers.AreEqual(1, x.InGramPerDay());
        }
    }
}