using System;

namespace RIVM.ConsExpo.Model.Computations
{
    /// <summary>
    /// Methods taken from, or based on Press et all., Numerical recipes, 3rd edition.
    /// </summary>
    public class NumericalRecipes
    {
        /// <summary>
        /// Bisects the specified function.
        /// </summary>
        /// <param name="func">The function to find a root for.</param>
        /// <param name="t1">The left side of the initial bracket.</param>
        /// <param name="t2">The right side of the initial bracket.</param>
        /// <param name="maxIter">The maximum number of bisection iterations allowed..</param>
        /// <param name="tol">The tolerance, i.e. the width of the bracket that is small enough to say a root was found.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException"></exception>
        public static double Bisect(Func<double, double> func, double t1, double t2, int maxIter, double tol)
        {
            double dt;
            double tmid;
            double f = func(t1);
            double fmid = func(t2);

            // Root to bracket.
            double rtb;

            if (f < 0.0)
            {
                rtb = t1;
                dt = t2 - t1;
            }
            else
            {
                rtb = t2;
                dt = t1 - t2;
            }

            for (int iteration = 0; iteration < maxIter; iteration++)
            {
                dt = 0.5 * dt;
                fmid = func(tmid = rtb + dt);

                if (fmid <= 0)
                {
                    rtb = tmid;
                }

                if (Math.Abs(dt) < tol || fmid == 0.0)
                {
                    return rtb;
                }
            }

            throw new ApplicationException(string.Format("Could not find a root for the interval {0} to {1} with a precision of {2} within {3} iterations.", t1, t2, tol, maxIter));
        }
    }
}