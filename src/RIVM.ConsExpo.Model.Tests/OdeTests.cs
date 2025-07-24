using DotNumerics.ODE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace RIVM.ConsExpo.Model.Tests
{
    /// <summary>
    /// Test for the ODE integrator of DotNumerics.
    /// </summary>
    [TestClass]
    public class OdeTests
    {
        private const int NumberOfProductLayers = 0;
        private const int NumberOfIntervals = 100;

        /// <summary>
        /// The array of derivatives.
        /// </summary>
        /// <remarks>Declared once, for better performance and reduced garbage collection.</remarks>
        private readonly double[] dydt = new double[NumberOfProductLayers + 1];

        /// <summary>
        /// The jacobian of the set of differential equations.
        /// </summary>
        /// <remarks>Declared once, for better performance and reduced garbage collection.</remarks>
        private readonly double[,] jacobian = new double[NumberOfProductLayers + 1, NumberOfProductLayers + 1];

        /// <summary>
        /// The solution of the model.
        /// </summary>
        private double[,] solution;

        [TestMethod]
        public void SolveODEWithExplicitDependencyOnIndependentVariable()
        {
            OdeFunction YDot = new OdeFunction(DerivativeWithExplicitDependencyOnIndependentVariable);
            OdeJacobian Jac = new OdeJacobian(JacobianWithExplicitDependencyOnIndependentVariable);

            OdeImplicitRungeKutta5 rungeKutta = new OdeImplicitRungeKutta5(YDot, Jac, NumberOfProductLayers + 1, false);

            double[] y0 = new double[NumberOfProductLayers + 1];

            double x0 = -2;
            y0[0] = 1 + Math.Exp(-2);

            double xf = 4;

            double dx = (xf - x0) / (NumberOfIntervals + 1e-12);

            rungeKutta.RelTolArray[0] = 0.000001;

            solution = rungeKutta.Solve(y0, x0, dx, xf);

            for (int i = 0; i <= NumberOfIntervals; i++)
            {
                Debug.WriteLine("{0:G}\t{1:G}", solution[i, 0], solution[i, 1]);
            }
        }

        /// <summary>
        /// The differential equations describing the system.
        /// </summary>
        /// <param name="T">The t.</param>
        /// <param name="Y">The y.</param>
        /// <returns></returns>
        private double[] DerivativeWithExplicitDependencyOnIndependentVariable(double T, double[] Y)
        {
            dydt[0] = -Y[0] * T + T;
            return dydt;
        }

        /// <summary>
        /// The jacobian of the system.
        /// </summary>
        /// <param name="T">The t.</param>
        /// <param name="Y">The y.</param>
        /// <returns></returns>
        public double[,] JacobianWithExplicitDependencyOnIndependentVariable(double T, double[] Y)
        {
            jacobian[0, 0] = -T;

            return jacobian;
        }
    }
}