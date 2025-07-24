using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using DataAnnotationsExtensions;
using RIVM.ConsExpo.DTO.Extensions;

namespace RIVM.ConsExpo.DTO.Distributions
{
    /// <summary>
    /// This class stores the distribution settings of a physical quantity.
    /// </summary>
    [ComplexType]
    public class Distribution : IValidatableObject
    {
        /// <summary>
        /// The type of distribution selected by the user.
        /// </summary>
        [Display(Name = "Distribution type")]
        public DistributionTypes DistributionType { get; set; }

        /// <summary>
        /// Gets or sets the lower bound for a uniform distribution.
        /// </summary>
        [Display(Name = "Lower bound")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        public double? LowerBound { get; set; }

        /// <summary>
        /// Instruction inspected by the XML-serializer to determine if the lower bound must be serialized.
        /// </summary>
        public bool ShouldSerializeLowerBound()
        {
            return LowerBound != null;
        }

        /// <summary>
        /// Gets or sets the upper bound for a uniform distribution.
        /// </summary>
        [Display(Name = "Upper bound")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        public double? UpperBound { get; set; }

        /// <summary>
        /// Instruction inspected by the XML-serializer to determine if the UpperBound must be serialized.
        /// </summary>
        public bool ShouldSerializeUpperBound()
        {
            return UpperBound != null;
        }

        /// <summary>
        /// Gets or sets the mean of a normal distribution.
        /// </summary>
        /// <value>
        /// The mean.
        /// </value>
        [Display(Name = "Mean")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        public double? Mean { get; set; }

        /// <summary>
        /// Instruction inspected by the XML-serializer to determine if the Mean must be serialized.
        /// </summary>
        public bool ShouldSerializeMean()
        {
            return Mean != null;
        }

        /// <summary>
        /// Gets or sets the standard deviation of a normal distribution.
        /// </summary>
        /// <value>
        /// The standard deviation.
        /// </value>
        [Display(Name = "Standard deviation")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        public double? StandardDeviation { get; set; }

        /// <summary>
        /// Instruction inspected by the XML-serializer to determine if the StandardDeviation must be serialized.
        /// </summary>
        public bool ShouldSerializeStandardDeviation()
        {
            return StandardDeviation != null;
        }

        /// <summary>
        /// Gets or sets the median of a log normal distribution.
        /// </summary>
        /// <value>
        /// The median.
        /// </value>
        [Display(Name = "Median")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        public double? Median { get; set; }

        /// <summary>
        /// Instruction inspected by the XML-serializer to determine if the Median must be serialized.
        /// </summary>
        public bool ShouldSerializeMedian()
        {
            return Median != null;
        }

        /// <summary>
        /// Gets or sets the coefficient of variation of a log normal distribution.
        /// </summary>
        /// <value>
        /// The coefficient of variation.
        /// </value>
        [Display(Name = "Arithmetic coefficient of variation")]
        [Min(1E-3)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        public double? CoefficientOfVariation { get; set; }

        /// <summary>
        /// Instruction inspected by the XML-serializer to determine if the CoefficientOfVariation must be serialized.
        /// </summary>
        public bool ShouldSerializeCoefficientOfVariation()
        {
            return CoefficientOfVariation != null;
        }

        /// <summary>
        /// The lower bound of a triangular distribution.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        [Display(Name = "Location")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        public double? Location { get; set; }

        /// <summary>
        /// Instruction inspected by the XML-serializer to determine if the Location must be serialized.
        /// </summary>
        public bool ShouldSerializeLocation()
        {
            return Location != null;
        }

        /// <summary>
        /// The upper bound of a triangular distribution.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        [Display(Name = "Scale")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        public double? Scale { get; set; }

        /// <summary>
        /// Instruction inspected by the XML-serializer to determine if the Scale must be serialized.
        /// </summary>
        public bool ShouldSerializeScale()
        {
            return Scale != null;
        }

        /// <summary>
        /// The value of the maximum probability, i.e. the value for the top of the triangle.
        /// </summary>
        /// <value>
        /// The shape.
        /// </value>
        [Display(Name = "Shape")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        public double? Shape { get; set; }

        /// <summary>
        /// Instruction inspected by the XML-serializer to determine if the Shape must be serialized.
        /// </summary>
        public bool ShouldSerializeShape()
        {
            return Shape != null;
        }

        /// <summary>
        /// Instruction inspected by the XML-serializer to determine if the Alpha must be serialized.
        /// </summary>
        public bool ShouldSerializeAlpha()
        {
            return Alpha != null;
        }

        private double? _alpha;

        /// <summary>
        /// parameters a and b for the beta distribution
        /// </summary>
        [Display(Name = "Alpha")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        [Range(1E-10, Constants.BetaDistributionMaxParameterValue)]
        public double? Alpha
        {
            get => _alpha;
            set
            {
                _derivedBetaMedian = null;
                _alpha = value;
            }
        }

        /// <summary>
        /// Instruction inspected by the XML-serializer to determine if the Beta must be serialized.
        /// </summary>
        public bool ShouldSerializeBeta()
        {
            return Beta != null;
        }

        private double? _beta;

        /// <summary>
        /// parameters a and b for the beta distribution
        /// </summary>
        [Display(Name = "Beta")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        [Range(1E-10, Constants.BetaDistributionMaxParameterValue)]
        public double? Beta
        {
            get => _beta;
            set
            {
                _derivedBetaMedian = null;
                _beta = value;
            }
        }

        /// <summary>
        /// Gets the previously sampled value.
        /// </summary>
        /// <value>
        /// The sampled value, if the value has been sampled, null otherwise.
        /// </value>
        [NotMapped]
        [XmlIgnore]
        public double? SampledValue { get; private set; }

        /// <summary>
        /// Take a pseudo-random number from the distribution.
        /// </summary>
        public void Sample()
        {
            SampledValue = GetSample();
        }

        /// <summary>
        /// Take a pseudo-random number from the distribution, with the additional restriction that the generated number is not smaller than the specified minimum.
        /// </summary>
        /// <param name="min">The minimum.</param>
        public void Sample(double min)
        {
            double rand;

            do
            {
                rand = GetSample();
            } while (rand < min);

            SampledValue = rand;
        }

        /// <summary>
        /// Take a pseudo-random number from the distribution, with the additional restrictions that the generated number is not smaller than the specified minimum and not larger than the specified maximum.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        public void Sample(double min, double max)
        {
            double rand;

            do
            {
                rand = GetSample();
            } while (rand < min || rand > max);

            SampledValue = rand;
        }

        /// <summary>
        /// Return a pseudo-random number from the distribution.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <exception cref="System.ApplicationException"></exception>
        private double GetSample()
        {
#warning Tech Debt: Should switching be avoided by implementing derived classes?
            double rand;
            switch (DistributionType)
            {
                case DistributionTypes.Uniform:
                    rand = LowerBound.Value + (UpperBound.Value - LowerBound.Value) *
                           Ran.RandomProvider.GetThreadRandom().NextDouble();
                    break;

                case DistributionTypes.Normal:
                    rand = NormalDeviate(Mean.Value, StandardDeviation.Value);
                    break;

                case DistributionTypes.LogNormal:
                    double mean = Math.Log(Median.Value);
                    double standardDeviation =
                        Math.Sqrt(Math.Log(CoefficientOfVariation.Value * CoefficientOfVariation.Value + 1));
                    rand = Math.Exp(NormalDeviate(mean, standardDeviation));
                    break;

                case DistributionTypes.Triangular:
                    // The triangular distribution is generated from a uniform distribution (see: Numerical recipes in C++ (Press) par 7.2 page 291)
                    // Draw random number in range [0,1]
                    // By inverting the cumulative triangular distribution:
                    // Cumulative triangular distribution goes from 0 to 1, inverse goes from A to B

                    double uniformRand = Ran.RandomProvider.GetThreadRandom().NextDouble();

                    // Get the branch of the cumulative distribution (inverse below or above the mode)
                    double theBranch = (Shape.Value - Location.Value) / (Scale.Value - Location.Value);
                    if (uniformRand < theBranch)
                    {
                        // Lower triangle
                        rand = Location.Value +
                               Math.Sqrt((Scale.Value - Location.Value) * (Shape.Value - Location.Value) * uniformRand);
                    }
                    else
                    {
                        // Upper triangle
                        rand = Scale.Value - Math.Sqrt((Scale.Value - Location.Value) * (Scale.Value - Shape.Value) *
                                                       (1 - uniformRand));
                    }

                    break;

                case DistributionTypes.Beta:
                    var beta = new MathNet.Numerics.Distributions.Beta(Alpha.Value, Beta.Value);
                    rand = beta.Sample() / BetaScalingFactor;

                    break;

                default:
                    throw new NotSupportedException(string.Format("Unsupported distribution type '{0}'",
                        DistributionType.ToString()));
            }

            return rand;
        }

        /// <summary>
        /// Generates a normal deviate.
        /// </summary>
        /// <param name="mean">The mean.</param>
        /// <param name="standardDeviation">The standard deviation.</param>
        /// <returns></returns>
        /// <see>Press, William H.; Teukolsky, Saul A.; Vetterling, William T.; Flannery, Brian P. (2007). Numerical Recipes: The Art of Scientific Computing (3rd ed.). New York: Cambridge University Press. ISBN 978-0-521-88068-8, section 7.3.9. Normal Deviates by Ratio-of-Uniforms, page 369.</see>
        private double NormalDeviate(double mean, double standardDeviation)
        {
            double u, v, x, y, q;
            do
            {
                u = Ran.RandomProvider.GetThreadRandom().NextDouble();
                v = 1.7156 * (Ran.RandomProvider.GetThreadRandom().NextDouble() - 0.5);
                x = u - 0.449871;
#if DEBUG
                //Only activate when really testing this:
                //Only for testing thread safety.
                //Debug.WriteLine(string.Format("A) u: {0}, v: {1}, x: {2}.", u, v, x));
                //Thread.Sleep(1000);
                //Debug.WriteLine(string.Format("B) u: {0}, v: {1}, x: {2}.", u, v, x));
#endif
                y = Math.Abs(v) + 0.386595;
                q = x * x + y * (0.19600 * y - 0.25472 * x);
            } while (q > 0.27597 && (q > 0.27846 || v * v > -4 * Math.Log(u) * Math.Sqrt(u)));

            return mean + standardDeviation * v / u;
        }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        /// <exception cref="System.ApplicationException"></exception>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            var distribution = this;

#warning Tech Debt: Should switching be avoided by implementing derived classes?
            switch (DistributionType)
            {
                case DistributionTypes.PointValue:
                    //Point values are not required.
                    break;

                case DistributionTypes.Uniform:
                    if (distribution.LowerBound.HasValue && distribution.UpperBound.HasValue)
                    {
                        if (distribution.UpperBound <= distribution.LowerBound)
                        {
                            validationResults.Add(new ValidationResult(
                                "The upper bound must be greater than the lower bound.",
                                new List<string>() { "UpperBound" }));
                        }
                    }
                    else
                    {
                        if (!distribution.LowerBound.HasValue)
                        {
                            validationResults.Add(new ValidationResult("The lower bound must be specified.",
                                new List<string>() { "LowerBound" }));
                        }

                        if (!distribution.UpperBound.HasValue)
                        {
                            validationResults.Add(new ValidationResult("The upper bound must be specified.",
                                new List<string>() { "UpperBound" }));
                        }
                    }

                    break;

                case DistributionTypes.Normal:
                    if (!distribution.Mean.HasValue)
                    {
                        validationResults.Add(new ValidationResult("The mean must be specified.",
                            new List<string>() { "Mean" }));
                    }

                    if (!distribution.StandardDeviation.HasValue)
                    {
                        validationResults.Add(new ValidationResult("The standard deviation must be specified.",
                            new List<string>() { "StandardDeviation" }));
                    }

                    break;

                case DistributionTypes.LogNormal:

                    if (!distribution.Median.HasValue)
                    {
                        validationResults.Add(new ValidationResult("The median must be specified.",
                            new List<string>() { "Median" }));
                    }

                    if (!distribution.CoefficientOfVariation.HasValue)
                    {
                        validationResults.Add(new ValidationResult(
                            "The arithmetic coefficient of variation must be specified.",
                            new List<string>() { "CoefficientOfVariation" }));
                    }

                    break;

                case DistributionTypes.Triangular:
                    if (distribution.Location.HasValue && distribution.Scale.HasValue && distribution.Shape.HasValue)
                    {
                        if (distribution.Scale <= distribution.Location)
                        {
                            validationResults.Add(new ValidationResult("The scale must be greater than the location.",
                                new List<string>() { "Scale" }));
                        }

                        if (distribution.Shape < distribution.Location)
                        {
                            validationResults.Add(new ValidationResult(
                                "The shape must be greater than or equal to the location.",
                                new List<string>() { "Shape" }));
                        }

                        if (distribution.Shape > distribution.Scale)
                        {
                            validationResults.Add(new ValidationResult(
                                "The shape must be smaller than or equal to the scale.", new List<string>() { "Shape" }));
                        }
                    }
                    else
                    {
                        if (!distribution.Location.HasValue)
                        {
                            validationResults.Add(new ValidationResult("The location must be specified.",
                                new List<string>() { "Location" }));
                        }

                        if (!distribution.Scale.HasValue)
                        {
                            validationResults.Add(new ValidationResult("The scale must be specified.",
                                new List<string>() { "Scale" }));
                        }

                        if (!distribution.Shape.HasValue)
                        {
                            validationResults.Add(new ValidationResult("The shape must be specified.",
                                new List<string>() { "Shape" }));
                        }
                    }

                    break;

                case DistributionTypes.Beta:
                    if (!distribution.Alpha.HasValue)
                    {
                        validationResults.Add(new ValidationResult("The parameter alpha must be specified.",
                            new List<string>() { "Alpha" }));
                    }

                    if (!distribution.Beta.HasValue)
                    {
                        validationResults.Add(new ValidationResult("The parmater beta must be specified.",
                            new List<string>() { "Beta" }));
                    }

                    break;

                default:
                    throw new NotSupportedException($"Unsupported distribution type '{DistributionType.ToString()}'");
            }

            return validationResults;
        }

        /// <summary>
        /// Gets or sets the derived median.
        /// </summary>
        /// <value>
        /// The derived median.
        /// </value>
        /// <seealso href="https://en.wikipedia.org/wiki/Triangular_distribution">Triangular distribution</seealso>
        [NotMapped]
        public double? DerivedMedian
        {
            get
            {
                double? derivedMedian;

                switch (DistributionType)
                {
                    case DistributionTypes.PointValue:
                        derivedMedian = null;
                        break;

                    case DistributionTypes.Uniform:
                        if (UpperBound == null || LowerBound == null)
                            derivedMedian = null;
                        else
                            derivedMedian = (UpperBound.Value + LowerBound.Value) / 2;
                        break;

                    case DistributionTypes.Normal:
                        if (Mean == null)
                            derivedMedian = null;
                        else
                            derivedMedian = Mean.Value;
                        break;

                    case DistributionTypes.LogNormal:
                        if (Median == null)
                            derivedMedian = null;
                        else
                            derivedMedian = Median.Value;
                        break;

                    case DistributionTypes.Triangular:
                        if (Location == null || Scale == null || Shape == null)
                            derivedMedian = null;
                        else
                        {
                            double a = Location.Value;
                            double b = Scale.Value;
                            double c = Shape.Value;

                            if (c >= (a + b) / 2)
                            {
                                derivedMedian = a + Math.Sqrt((b - a) * (c - a) / 2);
                            }
                            else
                            {
                                derivedMedian = b - Math.Sqrt((b - a) * (b - c) / 2);
                            }
                        }

                        break;

                    case DistributionTypes.Beta:
                        // var beta = new MathNet.Numerics.Distributions.Beta(Alpha.Value, Beta.Value);
                        // Throws 'Specified method is not supported.':
                        // derivedMedian = beta.Median;

                        // See CE2015-40500  US157 - Toevoegen Beta-distributie https://gemini.rivm.nl/workspace/0/item/40500

                        derivedMedian = DeriveBetaMedian();
                        break;

                    default:
                        throw new NotSupportedException(string.Format("Unsupported distribution type '{0}'",
                            DistributionType.ToString()));
                }

                return derivedMedian;
            }
        }

        /// <summary>
        /// This fields stores the derived beta median
        /// </summary>
        private double? _derivedBetaMedian = null;

        /// <summary>
        /// A factor to use when beta distributions are sampled, or when a derived median is needed. Beta is only valid on [0 - 1]. When the unit is percentage, it must be rescaled to [0 - 100] after sampling. This factor can be used for that.
        /// <remarks>Only valid for beta distributions and thus only for FractionBase and derived classes. Do not set this for other types.</remarks>
        /// </summary>
        [XmlIgnore]
        [NotMapped]
        public double BetaScalingFactor { get; set; } = 1;

        /// <seealso href="https://en.wikipedia.org/wiki/Beta_distribution#Median">Beta distribution</seealso>
        private double? DeriveBetaMedian()
        {
            if (Alpha == null || Beta == null)
            {
                return null;
            }

            if (_derivedBetaMedian.HasValue)
            {
                return _derivedBetaMedian.Value;
            }

            double alpha = Alpha.Value;
            double beta = Beta.Value;
            double? unscaledDerivedBetaMedian;

            if (alpha.AlmostEqualMagnitude(beta))
            {
                // For symmetric cases α = β, median = 1/2.
                unscaledDerivedBetaMedian = 0.5;
            }
            else if (alpha.AlmostEqualMagnitude(1.0) && Beta > 0)
            {
                // For α = 1 and β > 0, median = 1 − 2 − 1 β
                unscaledDerivedBetaMedian = 1 - Math.Pow(2, -1 / beta);
            }
            else if (alpha.AlmostEqualMagnitude(3.0) && beta.AlmostEqualMagnitude(2.0))
            {
                // For α > 0 and β = 1, median = 2 − 1
                // For α = 3 and β = 2, median = 0.6142724318676105..., the real solution to the quartic equation 1 − 8x3 + 6x4 = 0, which lies in [0,1].
                unscaledDerivedBetaMedian = Constants.MedianOfBeta2_3;
            }
            else if (alpha.AlmostEqualMagnitude(2.0) && beta.AlmostEqualMagnitude(3.0))
            {
                // For α = 2 and β = 3, median = 0.38572756813238945... = 1−median(Beta(3, 2))
                unscaledDerivedBetaMedian = Constants.MedianOfBeta3_2;
            }
            else if (alpha >= 2.0 && beta >= 2.0)
            {
                // A reasonable approximation of the value of the median of the beta distribution, for both α and β greater or equal to one, is given by the formula below.
                // When α, β ≥ 1, the relative error (the absolute error divided by the median) in this approximation is less than 4% and for both α ≥ 2 and β ≥ 2 it is less than 1%.
                unscaledDerivedBetaMedian = (alpha - 1.0 / 3.0) / (alpha + beta - 2.0 / 3.0);
            }
            else
            {
                unscaledDerivedBetaMedian = EstimateBetaMedian(alpha, beta);
            }

            _derivedBetaMedian = unscaledDerivedBetaMedian / BetaScalingFactor;
            return _derivedBetaMedian;
        }

        /// <summary>
        /// Estimates a median for the beta distribution, by getting the middle value of many samples.
        /// </summary>
        /// <returns>The median of the sampled values</returns>
        /// <remarks>Quick Median could speed up this estimation. <see href="https://www.i-programmer.info/babbages-bag/505-quick-median.html?start=1"></see></remarks>
        private double EstimateBetaMedian(double alpha, double beta)
        {
            const int maxSamples = 10001; //Needs to be odd to have a middle value.
            const int middleValue = (maxSamples - 1) / 2;
            var samples = new List<double>(maxSamples);
            for (int i = 0; i < maxSamples; i++)
            {
                samples.Add(GetSample());
            }
            samples.Sort();
            return samples[middleValue];
        }

        /// <summary>
        /// Rescales the distribution with the specified correction factor.
        /// </summary>
        /// <param name="conversionFactor">The value correction factor.</param>
        public Distribution Rescale(double conversionFactor)
        {
            if (conversionFactor != 1.0)
            {
                switch (DistributionType)
                {
                    case DistributionTypes.Uniform:
                        LowerBound *= conversionFactor;
                        UpperBound *= conversionFactor;
                        break;

                    case DistributionTypes.Normal:
                        Mean *= conversionFactor;
                        StandardDeviation *= conversionFactor;
                        break;

                    case DistributionTypes.LogNormal:

                        Median *= conversionFactor;
                        break;

                    case DistributionTypes.Triangular:
                        Location *= conversionFactor;
                        Scale *= conversionFactor;
                        Shape *= conversionFactor;
                        break;

                    case DistributionTypes.PointValue:
                    default:
                        throw new NotSupportedException(string.Format("Unsupported distribution type '{0}'", DistributionType.ToString()));
                }
            }

            return this;
        }
    }
}