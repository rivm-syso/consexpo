using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RIVM.ConsExpo.DTO.Distributions
{
    /// <summary>
    /// A representation of a size distribution of particles.
    /// </summary>
    public class SizeDistribution
    {
        /// <summary>
        /// The type of distribution selected by the user.
        /// </summary>
        public SizeDistributionTypes SizeDistributionType { get; set; }

        /// <summary>
        /// Gets or sets the bins for the distribution.
        /// </summary>
        /// <value>
        /// The bins.
        /// </value>
        public List<SizeBin> Bins { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeDistribution"/> class.
        /// </summary>
        public SizeDistribution()
        {
            Bins = new List<SizeBin>();
        }

        /// <summary>
        /// Initializes a normal distribution, specified by mean and sd, but truncated below min and above max
        /// and the amplitude, which is an overall multiplicative constant to scale the total distribution volume (e.g. to normalize to total mass or volume)
        /// </summary>
        /// <param name="mean">The mean.</param>
        /// <param name="sd">The standard deviation</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="numberOfBins">The number of bins.</param>
        /// <param name="amplitude">The amplitude of the distribution.</param>
        public void InitNormal(double mean, double sd, double min, double max, double numberOfBins, double amplitude)
        {
            if (max > min)
            {
                double deltaD = (max - min) / numberOfBins; // evenly spaced bins
                double nD = min + deltaD / 2;

                SizeDistributionType = SizeDistributionTypes.Normal;

                // fill distribution bins, probability mass given by P(x) dx with P(x, GM, GSD) the probability density function of the log-normal distribution
                for (int i = 1; i <= numberOfBins; i++)
                {
                    SizeBin sizeBin = new SizeBin
                    {
                        Variable = nD,
                        Delta = deltaD,
                        ProbabilityMass = deltaD * amplitude * Math.Exp(-Math.Pow(nD - mean, 2) / (2 * sd * sd)) / (sd * Math.Sqrt(2 * Math.PI))
                    };
                    Bins.Add(sizeBin);
                    nD += deltaD;
                }
            }
        }

        /// <summary>
        /// Initializes a log normal distribution, specified by median and aCoV, but truncated below min and above max
        /// and the amplitude, which is an overall multiplicative constant to scale the total distribution volume (e.g. to normalize to total mass or volume)
        /// </summary>
        /// <param name="median">The median.</param>
        /// <param name="aCoV">The arithmetic coefficient of variation.</param>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <param name="numberOfBins">The number of bins aerosol distribution.</param>
        /// <param name="amplitude">The in amplitude.</param>
        public void InitLogNormal(double median, double aCoV, double min, double max, double numberOfBins, double amplitude)
        {
            if (max > min)
            {
                double deltaD = (max - min) / numberOfBins; // evenly spaced bins between min and max.
                double nD = min + deltaD / 2;
                double logGM = Math.Log(median);

                // calculation GSD from aCoV:
                // GSD = Exp (sqrt(log(aCoV^2 + 1)))
                double logGMSD = Math.Pow(Math.Log(aCoV * aCoV + 1), 0.5);

                SizeDistributionType = SizeDistributionTypes.LogNormal;

                // fill distribution bins, probability mass given by P(x) dx with P(x, GM, GSD) the probability density function of the lognormal distribution
                for (int i = 1; i <= numberOfBins; i++)
                {
                    double expProb = (Math.Log(nD) - logGM) / logGMSD;

                    SizeBin sizeBin = new SizeBin
                    {
                        Variable = nD,
                        Delta = deltaD,
                        ProbabilityMass = deltaD * amplitude * Math.Exp(-expProb * expProb / 2) / nD / logGMSD / Math.Sqrt(2 * Math.PI)
                    };

                    Bins.Add(sizeBin);
                    nD += deltaD;
                }
            }
        }

        /// <summary>
        /// Initialize the particle size distribution from the saved data.
        /// </summary>
        public void InitNonParametric(NonParametricSizeDistribution nonParametricSizeDistribution, bool belowCutOff, double cutOffDiameter, double amplitude)
        {
            double lowerBound = 0.0;

            foreach (var nonParametricSizeBin in nonParametricSizeDistribution.Bins.OrderBy(sb => sb.UpperBound))
            {
                var upperBound = nonParametricSizeBin.UpperBound * ConversionFactors.Micro2One;

                if (belowCutOff && lowerBound >= cutOffDiameter)
                {
                    // Skip
                }

                else if (!belowCutOff && upperBound <= cutOffDiameter)
                {
                    // Skip
                }
                else if (nonParametricSizeBin.RelativeMass > 0.0)
                {
                    double fractionOfBin = 1.0;

                    if (belowCutOff && cutOffDiameter < upperBound)
                    {
                        fractionOfBin = (cutOffDiameter - lowerBound) / (upperBound - lowerBound);
                        upperBound = cutOffDiameter;
                    }

                    if (!belowCutOff && cutOffDiameter > lowerBound)
                    {
                        fractionOfBin = (upperBound - cutOffDiameter) / (upperBound - lowerBound);
                        lowerBound = cutOffDiameter;
                    }
                    // Empty bins need not be calculated as they contribute nothing to the exposure.
                    var sizeBin = new SizeBin
                    {
                        Variable = (lowerBound + upperBound) / 2,
                        Delta = upperBound - lowerBound,
                        ProbabilityMass = amplitude * fractionOfBin * nonParametricSizeBin.RelativeMass
                    };

                    Bins.Add(sizeBin);
                }

                // Set lower bound for the next iteration.
                lowerBound = upperBound;
            }
        }

        /// <summary>
        /// Can be used to implement a check to validate that no bins overlap.
        /// </summary>
        /// <returns></returns>
        [Obsolete("To Do")]
        public bool Validate()
        {
            return false;
        }

        /// <summary>
        /// Recalculate the numbers in all bins to sum up to 1, by dividing all numbers by the same constant.
        /// </summary>
        [Obsolete("To Do")]
        public void Normalize()
        {
        }
    }
}