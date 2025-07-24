using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using System.Diagnostics;

namespace RIVM.ConsExpo.DTO.Tests.Helpers
{
    public static class AssertHelpers
    {
        [DebuggerHidden]
        public static void AreEqualPhysicalQuantities(IPhysicalQuantityBase expected, IPhysicalQuantityBase actual)
        {
            Assert.AreEqual<double?>(expected.Value, actual.Value, string.Format("The actual value {1} of the physical quantity differs from the expected value {0}.", expected.Value, actual.Value));
            Assert.AreEqual<string>(expected.UnitDisplay, actual.UnitDisplay, string.Format("The actual unit '{1}' of the physical quantity differs from the expected unit '{0}'.", expected.UnitDisplay, actual.UnitDisplay));
            Assert.AreEqual<bool>(expected.IsDistributed, actual.IsDistributed, string.Format("The actual physical quantity has distribution setting {1}, while the expected setting is {0}.", expected.IsDistributed, actual.IsDistributed));
        }

        [DebuggerHidden]
        public static void AreEqualDistributablePhysicalQuantities(IPhysicalQuantityBase expected, IPhysicalQuantityBase actual)
        {
            AreEqualPhysicalQuantities(expected, actual);
        }
    }
}