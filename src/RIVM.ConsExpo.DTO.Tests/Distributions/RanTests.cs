using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RIVM.ConsExpo.DTO.Tests.Distributions
{
    /// <summary>
    /// Some test to check whether a signed 32 bit int is properly converted to a unsigned 64 bit int , without overflow.
    /// </summary>
    [TestClass]
    public class RanTests
    {
        [TestMethod]
        public void ZeroSeed()
        {
            Ran ran = Ran.RandomProvider.GetThreadRandom();
            ran.Seed(0);

            var x = ran.NextInt64();
        }

        [TestMethod]
        public void PositiveSeed()
        {
            Ran ran = Ran.RandomProvider.GetThreadRandom();
            ran.Seed(1);

            var x = ran.NextInt64();
        }

        [TestMethod]
        public void NegativeSeed()
        {
            Ran ran = Ran.RandomProvider.GetThreadRandom();
            ran.Seed(-1);

            var x = ran.NextInt64();
        }

        [TestMethod]
        public void MaxSeed()
        {
            Ran ran = Ran.RandomProvider.GetThreadRandom();
            ran.Seed(Int32.MaxValue);

            var x = ran.NextInt64();
        }

        [TestMethod]
        public void MinSeed()
        {
            Ran ran = Ran.RandomProvider.GetThreadRandom();
            ran.Seed(Int32.MinValue);

            var x = ran.NextInt64();
        }
    }
}