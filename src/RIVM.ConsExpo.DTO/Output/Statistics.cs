using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// Data structure for the statistical derivatives of a distribution.
    /// </summary>
    public class Statistics
    {
        private readonly DoseUnits doseUnit;

        [Obsolete("Added to solve \"RIVM.ConsExpo.DTO.Output.Statistics cannot be serialized because it does not have a parameterless constructor.\"")]
        public Statistics()
        { }

        public Statistics(List<double> doseValues, DoseUnits doseUnit)
        {
            this.doseUnit = doseUnit;
            DeriveStatistics(doseValues);
        }

        /// <summary>
        /// Derives the statistics.
        /// </summary>
        /// <param name="doseValues">The dose values.</param>
        /// <exception cref="System.ApplicationException">Cannot calculate statistics for an empty list of outcomes.</exception>
        /// <seeaslo href="//Based on https://en.wikipedia.org/wiki/Algorithms_for_calculating_variance#Online_algorithm"/>
        private void DeriveStatistics(List<double> doseValues)
        {
            int n = 0;
            double runningMean = 0.0;
            double variance = 0.0;
            double delta;

            if (doseValues.Any())
            {
                doseValues.Sort();

                foreach (var doseValue in doseValues)
                {
                    n += 1;
                    delta = doseValue - runningMean;
                    runningMean += delta / n;
                    variance += delta * (doseValue - runningMean);
                }

                Mean = new Dose(runningMean, doseUnit);

                if (n >= 2)
                {
                    StandardDeviation = new Dose(Math.Sqrt(variance / n), doseUnit);
                }

                Median = new Dose(doseValues[(int)(.5 * (n - 1))], doseUnit);
                Percentile95 = new Dose(doseValues[(int)(.95 * (n - 1))], doseUnit);
                Percentile99 = new Dose(doseValues[(int)(.99 * (n - 1))], doseUnit);
            }
            else
            {
                Mean = new Dose(null, doseUnit);
                StandardDeviation = new Dose(null, doseUnit);
                Median = new Dose(null, doseUnit);
                Percentile95 = new Dose(null, doseUnit);
                Percentile99 = new Dose(null, doseUnit);
            }
        }

        public Dose Mean { get; set; }

        public Dose StandardDeviation { get; set; }

        public Dose Median { get; set; }

        public Dose Percentile95 { get; set; }

        public Dose Percentile99 { get; set; }
    }
}