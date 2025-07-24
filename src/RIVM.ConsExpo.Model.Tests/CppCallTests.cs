using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

/// <summary>
/// Some tests to see if calls to C++ classes work.
/// </summary>
[TestClass]
public class CppCallTests
{
    [TestMethod]
    public void MathFuncsWrapperTest()
    {
        double sum;
        using (var mathFuncs = new MathFuncsWrapper())
        {
            sum = mathFuncs.Add(1, 2);
        }

        Assert.AreEqual<double>(3.0, sum);
    }
}