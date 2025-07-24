#if DEBUG

using System;
using System.Globalization;

namespace RIVM.ConsExpo.Model.Helpers
{
    /// <summary>
    /// A helper class for writing ODE solutions to a file.
    /// </summary>
    /// <remarks>Only for debugging purposes.</remarks>
    [Obsolete("Only to be used while debugging, e.g. in the Immediate Window.", true)]
    public class TestHelpers
    {
        public static void SaveOdeSolution(double[,] sol)
        {
            string fileName = $"C:\\Temp\\OdeSolution{DateTime.Now:yyyyMMddTHHmmss}.csv";

            using (var file = new System.IO.StreamWriter(fileName))
            {
                for (int x = sol.GetLowerBound(0); x < sol.GetUpperBound(0); x++)
                {
                    for (int y = sol.GetLowerBound(1); y < sol.GetUpperBound(1); y++)
                    {
                        file.Write(sol[x, y].ToString(CultureInfo.InvariantCulture) + ";");
                    }
                    file.WriteLine("");
                }
            }
        }
    }
}
#endif
