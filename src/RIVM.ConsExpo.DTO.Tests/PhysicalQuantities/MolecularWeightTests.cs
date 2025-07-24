using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.PhysicalQuantities
{
    [TestClass]
    public class MolecularWeightTests
    {
        /// <summary>
        /// Tests whether all available units have support for conversion to another unit. It does not test if the conversion is correct. At units test for the correct numerical conversion, if you discover missing units.
        /// </summary>
        [TestMethod]
        public void MolecularWeightCompleteUnitSupportTest()
        {
            var x = new MolecularWeight();
            foreach (MolecularWeightUnits unit in x.AvailableUnits)
            {
                x.Value = 1;
                x.Unit = unit;

                x.InGramPerMol();
            }
        }

        [TestMethod]
        public void MolecularWeightUnitConversionTest()
        {
            //All instances are 1 GramPerMol, expressed in different units.
            var x = new MolecularWeight()
            {
                Value = 1,
                Unit = MolecularWeightUnits.GramPerMol
            };

            var y = new MolecularWeight()
            {
                Value = 1000,
                Unit = MolecularWeightUnits.MilliGramPerMol
            };

            TestHelpers.AreEqual(x.InGramPerMol(), y.InGramPerMol());
        }

        [TestMethod]
        public void MolecularWeightInMgPerMolTest()
        {
            var x = new MolecularWeight()
            {
                Value = 1,
                Unit = MolecularWeightUnits.MilliGramPerMol
            };

            TestHelpers.AreEqual(1, x.InMgPerMol());
        }
    }
}