using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Extensions;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using System.ComponentModel;
using System.Text;

namespace RIVM.ConsExpo.DTO.Tests
{
    internal class TestHelpers
    {
        public static void AreEqual(IPhysicalQuantityBase x, IPhysicalQuantityBase y, double tolerance = 1E-9)
        {
            Assert.IsTrue(x.Value.Value.AlmostEqualMagnitude(y.Value.Value, tolerance), string.Format("The compared physical quantities of type '{0}' are not equal in value: {1}; {2}.", x.GetType().Name, x.Value, y.Value));
            Assert.AreEqual(x.UnitDisplay, y.UnitDisplay, string.Format("The compared physical quantities are not equal in unit: {0}; {1}.", x.UnitDisplay, y.UnitDisplay));
        }

        public static void AreEqual(double? x, double? y, double tolerance = 1E-9)
        {
            if (x == null ^ y == null)
            {
                Assert.Fail("One of the compared physical quantities is null: {0}; {1}", x, y);
            }
            else if (x != null && y != null)
            {
                AreEqualDoubles(x.Value, y.Value, tolerance);
            }
        }

        public static void AreEqualDoubles(double x, double y, double tolerance = 1E-9)
        {
            Assert.IsTrue(x.AlmostEqualMagnitude(y, tolerance), string.Format("The compared values are not equal in value: {0}; {1}.", x, y));
        }

        /// <summary>
        /// Dumps the object to a string representation.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        /// <see href="http://stackoverflow.com/questions/852181/c-printing-all-properties-of-an-object/852291#852291">C#: Printing all properties of an object [duplicate]</see>
        public static string DumpObject(object obj)
        {
            var dump = new StringBuilder();
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(obj))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(obj);
                dump.AppendLine(string.Format("{0}={1}", name, value));
            }
            return dump.ToString();
        }
    }
}