using System;
using System.Collections.Generic;
using System.Linq;

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// Represents a histogram with bins containing the number of outcomes per bin.
    /// </summary>
    /// <remarks>Currently, only exposure is supported.</remarks>
    public class Histogram
    {
        /// <summary>
        /// The minimum width of the histogram, as a fraction of the average value.
        /// </summary>
        /// <remarks>This is done to prevent artifacts of narrow distributions to confuse the user.</remarks>
        const double MinHistogramWidthFraction = 0.01;

        /// <summary>
        /// Gets or sets the bins for the histogram
        /// </summary>
        /// <value>
        /// The bins.
        /// </value>
        /// <remarks>This is no longer a SortedList, as a SortedList cannot be serialized</remarks>
        /// <seealso href="http://stackoverflow.com/questions/495647/serialize-class-containing-dictionary-member/495718#495718">Serialize Class containing Dictionary member</seealso>
        protected List<Bin> bins;

        /// <summary>
        /// Gets the bins of the histogram.
        /// </summary>
        /// <value>
        /// The bins.
        /// </value>
        public List<Bin> Bins
        { get { return bins; } }

        /// <summary>
        /// Gets or sets the dose unit for the histogram.
        /// </summary>
        /// <value>
        /// The dose unit.
        /// </value>
        public DoseUnits DoseUnit { get; set; }

        public ScaleType XAxisType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Histogram"/> class. Use only for serialization.
        /// </summary>
        [Obsolete("Added to solve \"RIVM.ConsExpo.DTO.Output.Histogram cannot be serialized because it does not have a parameterless constructor.\"")]
        public Histogram()
        {
            bins = new List<Bin>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Histogram"/> class.
        /// </summary>
        /// <param name="doses">The doses.</param>
        /// <param name="numberOfBins">The number of bins.</param>
        /// <param name="xAxisType">The xAxis scale.</param>
        /// <param name="doseUnit">The dose unit.</param>
        /// <exception cref="System.ApplicationException">Cannot create a histogram for a simulation result without any points.</exception>
        public Histogram(List<double> doses, int numberOfBins, ScaleType xAxisType, DoseUnits doseUnit)
        {
            // The creation of bins and filling them, depends on having a sorted list.
            doses.Sort();

            bins = new List<Bin>(numberOfBins);
            XAxisType = xAxisType;

            if (numberOfBins > 0 && doses.Any() && !doses.Any(d => d.Equals(double.NaN)))
            {
                DoseUnit = doseUnit;

                if (xAxisType == ScaleType.Linear)
                {
                    CreateLinearBins(numberOfBins, doses, out double highestUpperbound);
                    FillLinearBins(doses, numberOfBins, highestUpperbound);
                }
                else if (xAxisType == ScaleType.Logarithmic && doses.All(d => d <= 0))
                {
                    // no valid values
                    CreateLogarithmicBins(1, doses, out double highestUpperbound);
                    FillLogarithmicBins(doses, numberOfBins, highestUpperbound);
                }
                else if (xAxisType == ScaleType.Logarithmic)
                {
                    CreateLogarithmicBins(numberOfBins, doses, out double highestUpperbound);
                    FillLogarithmicBins(doses, numberOfBins, highestUpperbound);
                }

                DeriveCumulativeFractions(doses.Count());
            }
            else
            {
                //throw new ApplicationException("Cannot create a histogram for a simulation result without any points.");
            }
        }

        /// <summary>
        /// Create the specified number of equally sized bins.
        /// </summary>
        protected void CreateLinearBins(int numberOfBins, List<double> doses, out double highestUpperbound)
        {
            double lowestValue = doses.Min();
            double highestValue = doses.Max();

            double lowestLowerbound;

            if (lowestValue > (1 - MinHistogramWidthFraction) * highestValue)
            {
                double meanValue = (lowestValue + highestValue) / 2;
                lowestLowerbound = (1 - MinHistogramWidthFraction / 2) * meanValue;
                highestUpperbound = (1 + MinHistogramWidthFraction / 2) * meanValue;
            }
            else
            {
                lowestLowerbound = lowestValue;
                highestUpperbound = highestValue;
            }

            double binSize = (highestUpperbound - lowestLowerbound) / numberOfBins;

            for (var binIndex = 0; binIndex < numberOfBins; binIndex++)
            {
                double lowerBound = lowestLowerbound + binSize * binIndex;
                double upperBound = lowestLowerbound + binSize * (binIndex + 1);

                bins.Add(new Bin
                {
                    LowerBound = lowerBound,
                    UpperBound = upperBound,
                    Mean = (lowerBound + upperBound) / 2,
                    NumberOfOutcomes = 0,
                    CumulativeFraction = 0
                });
            }
        }

        /// <summary>
        /// Create the specified number of logarithmicaly equally sized bins.
        /// </summary>
        protected void CreateLogarithmicBins(int numberOfBins, List<double> doses, out double highestUpperbound)
        {
            double lowestValue = doses.Where(d => d > 0).Min();
            double highestValue = doses.Max();

            double lowestLowerbound;

            if (lowestValue > (1 - MinHistogramWidthFraction) * highestValue)
            {
                double meanValue = (lowestValue + highestValue) / 2;
                lowestLowerbound = (1 - MinHistogramWidthFraction / 2) * meanValue;
                highestUpperbound = (1 + MinHistogramWidthFraction / 2) * meanValue;
            }
            else
            {
                lowestLowerbound = lowestValue;
                highestUpperbound = highestValue;
            }

            double logBinSize = (Math.Log10(highestUpperbound) - Math.Log10(lowestLowerbound)) / numberOfBins;

            for (var binIndex = 0; binIndex < numberOfBins; binIndex++)
            {
                double logLowerBound = Math.Log10(lowestLowerbound) + logBinSize * binIndex;
                double logUpperBound = Math.Log10(lowestLowerbound) + logBinSize * (binIndex + 1);

                double lowerBound = Math.Pow(10, logLowerBound);
                double upperBound = Math.Pow(10, logUpperBound);

                bins.Add(new Bin
                {
                    LowerBound = lowerBound,
                    UpperBound = upperBound,
                    Mean = Math.Sqrt(lowerBound * upperBound),
                    NumberOfOutcomes = 0,
                    CumulativeFraction = 0
                });
            }
        }

        /// <summary>
        /// Put each result in the correct bin.
        /// </summary>
        /// <param name="doses">The doses.</param>
        /// <param name="numberOfBins">The number of bins.</param>
        /// <param name="highestUpperbound">The upper bound of the last bin.</param>
        protected void FillLinearBins(List<double> doses, int numberOfBins, double highestUpperbound)
        {
            int binIndex = 0;

            foreach (var dose in doses)
            {
                if (dose >= highestUpperbound)
                {
                    //Iteration outcomes must go into exactly one bin. Limits of bins are lower bound inclusive and upper bound exclusive. Only the last bin has upper bound inclusive to fit in the largest value(s).
                    binIndex = numberOfBins - 1;
                }
                else
                {
                    while (dose >= bins[binIndex].UpperBound)
                    {
                        binIndex++;
                    }
                }

                bins[binIndex].NumberOfOutcomes++;
            }
        }

        /// <summary>
        /// Put each result in the correct bin.
        /// </summary>
        /// <param name="doses">The doses.</param>
        /// <param name="numberOfBins">The number of bins.</param>
        /// <param name="highestUpperbound">The upper bound of the last bin.</param>
        protected void FillLogarithmicBins(List<double> doses, int numberOfBins, double highestUpperbound)
        {
            int binIndex = 0;

            foreach (var dose in doses)
            {
                if (dose == 0)
                {
                    bins[binIndex].LowerBound = 0;
                }
                else if (dose >= highestUpperbound)
                {
                    //Iteration outcomes must go into exactly one bin. Limits of bins are lower bound inclusive and upper bound exclusive. Only the last bin has upper bound inclusive to fit in the largest value(s).
                    binIndex = numberOfBins - 1;
                }
                else
                {
                    while (dose >= bins[binIndex].UpperBound)
                    {
                        binIndex++;
                    }
                }

                bins[binIndex].NumberOfOutcomes++;
            }
        }

        /// <summary>
        /// Derives the cumulative fractions by summing the number of outcomes in the bins for values up to the current value.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        /// <seealso href="http://www.dotnetperls.com/sort-dictionary">Sort Dictionary</seealso>
        protected void DeriveCumulativeFractions(int numberOfIterations)
        {
            //Do not assume the bins are sorted. Acquire keys and sort them.
            int runningTotal = 0;

            foreach (Bin bin in bins.OrderBy(b => b.LowerBound))
            {
                runningTotal += bin.NumberOfOutcomes;
                bin.CumulativeFraction = (double)runningTotal / numberOfIterations;
            }
        }
    }
}