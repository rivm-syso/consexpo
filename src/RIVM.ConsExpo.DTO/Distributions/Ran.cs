using System;
using System.Threading;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

/// <summary>
/// Based on Numerical Recipes 3rd Edition: The Art of Scientific Computing
/// </summary>
public class Ran
{
    private UInt64 u;
    private UInt64 v;
    private UInt64 w;

    /// <summary>
    ///
    /// </summary>
    /// <see href="http://csharpindepth.com/Articles/Chapter12/Random.aspx">C# in Depth - Random numbers</see>
    public static class RandomProvider
    {
        private static Int32 seed = Environment.TickCount;

        private static ThreadLocal<Ran> randomWrapper = new ThreadLocal<Ran>(() =>
        {
            return new Ran(Interlocked.Increment(ref seed));
        }
        );

        /// <summary>
        /// Gets the random generator for the current thread.
        /// </summary>
        /// <remarks>Since the random generator is not thread-safe, make sure one is kept for each thread.</remarks>
        /// <returns></returns>
        public static Ran GetThreadRandom()
        {
            return randomWrapper.Value;
        }
    }

    private Ran(int j)
        : this(Int32ToUInt64(j))
    { }

    /// <summary>
    /// Seeds the random generator with the specified value.
    /// </summary>
    /// <param name="j">The j.</param>
    [Obsolete("No need to seed the random generator, except for unit testing.")]
    public void Seed(Int32 j)
    {
        Seed(Int32ToUInt64(j));
    }

    private Ran(UInt64 j)
    {
        Seed(j);
    }

    /// <summary>
    /// Seeds the random generator with the specified value.
    /// </summary>
    /// <param name="j">The j.</param>
    [Obsolete("No need to seed the random generator, except for unit testing.")]
    public void Seed(UInt64 j)
    {
        v = 4101842887655102017;
        w = 1;

        u = j ^ v;
        NextInt64();
        v = u;
        NextInt64();
        w = v;
        NextInt64();
    }

    public UInt64 NextInt64()
    {
        u = u * 2862933555777941757 + 7046029254386353087;

        v ^= v >> 17;
        v ^= v << 31;
        v ^= v >> 8;

        UInt64 x = u ^ (u << 21);
        x ^= x >> 35;
        x ^= x << 4;

        return (x + v) ^ w;
    }

    public UInt32 NextInt32()
    {
        return (UInt32)NextInt64();
    }

    public double NextDouble()
    {
        return 5.4101086242752217E-20 * NextInt64();
    }

    /// <summary>
    /// TickCount cycles between Int32.MinValue, which is a negative
    /// number, and Int32.MaxValue once every 49.8 days. This sample
    /// removes the sign bit to yield a nonnegative number that cycles
    /// between zero and Int32.MaxValue once every 24.9 days.
    /// </summary>
    /// <see href="https://msdn.microsoft.com/en-us/library/system.environment.tickcount%28v=vs.110%29.aspx">Environment.TickCount Property</see>
    private static UInt64 Int32ToUInt64(Int32 seed)
    {
        UInt64 unsignedSeed = Convert.ToUInt64(seed & Int32.MaxValue);
        return unsignedSeed;
    }
}