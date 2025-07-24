using System;
using System.Diagnostics;

namespace RIVM.ConsExpo.TestFacilities
{
    public class Comparisons
    {
        public static bool AlmostEqualMagnitude(double value1, double value2, double tolerance = 0.05)
        {
            Debug.WriteLine("Testing almost equal magnitude. value 1: {0}, value 2: {1}, relative magnitude: {2}, difference in magnitude: {3}%, tolerance: {4}%.", value1, value2, value1 / value2, 100 * Math.Abs(1 - value1 / value2), 100 * tolerance);
            return Math.Abs(1 - value1 / value2) < tolerance;
        }
    }
}