using DotNumerics.ODE;
using RIVM.ConsExpo.DTO.Exceptions;
using System;

namespace RIVM.ConsExpo.Model.Computations
{
    /// <summary>
    /// A wrapper around the DotNumerics ODE implicit Runge Kutta solver.
    /// </summary>
    internal class OdeSolver
    {
        protected const double FloatingPointZero = 1e-12;

        public static double[,] Solve(OdeFunction model, OdeJacobian jacobian, double x0, double xf, int numberOfEquations, double[] y0, double relTol, int numberOfTimeSteps, bool applyStepSizeFix)
        {
            double[,] sol;
            //Fix: reduce the time steps slightly to avoid rounding errors that cause the ODE integrator to skip the last time step.
            double dx = (xf - x0) / (numberOfTimeSteps * (1 + FloatingPointZero));

            // OdeImplicitRungeKutta5 class is used to solve the diffusion-emission equations:
            // If a new version of the radau5 library is imported, existing code will break, because the parameter 'applyStepSizeFix' was added by the ConsExpo team. This is intentional. If a new version is provided, carefully assess the necessity of fix and reapply in the radau5 code base.
            OdeImplicitRungeKutta5 rungeKutta = new OdeImplicitRungeKutta5(model, jacobian, numberOfEquations, applyStepSizeFix)
            {
                ErrorToleranceType = ErrorToleranceEnum.Scalar,
                RelTol = relTol
            };

            try
            {
                sol = rungeKutta.Solve(y0, x0, dx, xf);
            }
            catch (Exception exc)
            {
                throw new ODEIntegrationException("ConsExpo Web could not evaluate the scenario. The combination of certain input parameter values cause the problem to occur (e.g. long exposure duration, short application duration, low product amount, and fast release). Please consider changing the input values to a less extreme setting.", exc);
            }

            return sol;
        }
    }
}