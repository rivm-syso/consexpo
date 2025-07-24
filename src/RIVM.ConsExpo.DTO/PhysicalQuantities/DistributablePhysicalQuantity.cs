using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Extensions;
using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Serialization;

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// A base class for physical quantities that can be specified by a distribution. Enforces a common interface to, e.g. views.
    /// </summary>
    /// <typeparam name="T">The enumeration type for the unit.</typeparam>
    /// <remarks>If you create a new parameters of a type that derives from this abstract class, you must add sampling of this parameter to ScenarioModel.SampleAll()!</remarks>
    [ComplexType]
    public abstract class DistributablePhysicalQuantity<T> : PhysicalQuantity<T>, IDistributablePhysicalQuantity<T> where T : UnitBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DistributablePhysicalQuantity{T}"/> class.
        /// </summary>
        protected DistributablePhysicalQuantity()
        {
            Init();
        }

        /// <summary>
        /// Creates a new instance with some explicit value for properties.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="minMaxUnit">The unit which must be used to interpret {min} and {max}.</param>
        /// <param name="min">The minimum allowed physical value for this instance.</param>
        /// <param name="max">The maximum allowed physical value for this instance.</param>
        protected DistributablePhysicalQuantity(string displayName, T minMaxUnit, double min, double? max) : base(displayName, minMaxUnit, min, max)
        {
            Init();
        }

        private void Init()
        {
            // Note: Distribution is a complex type en EF will often initialize a Distribution without actually using this instance.
            // This does not seem to be an issue, but when I tried to pass data to the distribution instance from a property setter of this class, the values where lost, because the Distribution instance was replaced with another instance.

            distribution = new Distribution();
        }

        /// <summary>
        /// Validates the specified validation context.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns></returns>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            validationResults.AddRange(distribution.Validate(validationContext));
            validationResults.AddRange(CheckDistributionBounds(validationContext));
            validationResults.AddRange(base.Validate(validationContext));

            return validationResults;
        }

#warning HACK: disable to see why this is needed.
        ///// <summary>
        ///// Gets a value indicating whether this instance is valid.
        ///// </summary>
        ///// <value>
        /////   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        ///// </value>
        //[NotMapped]
        //public bool IsValid
        //{
        //    get
        //    {
        //        var context = new ValidationContext(this, serviceProvider: null, items: null);
        //        return !Validate(context).Any();
        //    }
        //}

        /// <summary>
        /// Gets a value indicating whether this instance has a value. For a distributable physical quantity, inspect the parameters for the specified distribution type.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has value; otherwise, <c>false</c>.
        /// </value>
        public override bool HasValue
        {
            get
            {
                if (IsDistributed)
                {
                    var context = new ValidationContext(this, serviceProvider: null, items: null);
                    return !distribution.Validate(context).Any();
                }
                else
                {
                    return base.HasValue;
                }
            }
        }

        /// <summary>
        /// Gets the value. If a distribution has been specified, this property will return the sampled value, if a sample has been taken, or the median of the distribution. If is not distributed, it will return the value specified by the user.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public virtual double? DerivedValue
        {
            get
            {
                if (distribution.DistributionType == DistributionTypes.PointValue)
                {
                    return base.Value;
                }
                else
                {
#warning Tech Debt: This function may have tricky results, as parameters in a scenario used for Monte Carlo simulations may give a distribution's median in the first iteration and give the sampled value for other iterations.
                    return distribution.SampledValue ?? distribution.DerivedMedian;
                }
            }
        }

        /// <summary>
        /// Returns the derived value of the physical quantity, converted to the specified target unit. The derived value is the specified value for non-distributed parameter values, or the median value of distributed parameter values.
        /// </summary>
        /// <param name="targetUnit">The target unit.</param>
        /// <returns></returns>
        public override double ConvertedValue(T targetUnit)
        {
            return ConvertedValue(targetUnit, DerivedValue.Value);
        }

        /// <summary>
        /// Gets a value indicating whether the physical quantity supports specification of a distribution.
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports distributions]; otherwise, <c>false</c>.
        /// </value>
        public override sealed bool SupportsDistributions => true;

        /// <summary>
        /// Gets a value indicating whether this instance is distributed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is distributed; otherwise, <c>false</c>.
        /// </value>
        public override bool IsDistributed => (SupportsDistributions && distribution != null && distribution.DistributionType != DistributionTypes.PointValue);

        /// <summary>
        /// Gets the type of the distribution.
        /// </summary>
        /// <value>
        /// The type of the distribution.
        /// </value>
        public DistributionTypes DistributionType => distribution.DistributionType;

        /// <summary>
        /// The distribution to sample random values for the instance from.
        /// </summary>
        protected Distribution distribution;

        /// <summary>
        /// Gets or sets the distribution (specified by type and parameters) for the physical quantity.
        /// </summary>
        /// <value>
        /// The distribution.
        /// </value>
        [XmlElement]
        public Distribution Distribution
        {
            get => distribution;
            set => distribution = value;
        }

        /// <summary>
        /// A value for a dynamic test whether or not the Distribution should be serialized. This must be done if any parameter of the distribution is different than the default value.
        /// </summary>
        /// <returns>true if the distribution should be serialized, false otherwise.</returns>
        public bool ShouldSerializeDistribution()
        {
            return distribution != null
                && (
                    DistributionType != DistributionTypes.PointValue
                    || distribution.LowerBound != null
                    || distribution.UpperBound != null
                    || distribution.Mean != null
                    || distribution.StandardDeviation != null
                    || distribution.Median != null
                    || distribution.CoefficientOfVariation != null
                    || distribution.Location != null
                    || distribution.Scale != null
                    || distribution.Shape != null
                    || distribution.Alpha != null
                    || distribution.Beta != null
                );
        }

        /// <summary>
        /// Generates a sample from the distribution specified for the instance.
        /// </summary>
        /// <returns>A pseudo-random sample from the distribution, or the specified value, if not distribution is used.</returns>
        /// <remarks>Uses reflection to read the lower and upper bounds allowed by the instance to restrict the sampled values.</remarks>
        public virtual void Sample()
        {
            if (IsDistributed)
            {
                if (this.Max.HasValue)
                {
                    Distribution.Sample(this.Min, this.Max.Value);
                }
                else
                {
                    Distribution.Sample(this.Min);
                }
            }
        }

        /// <summary>
        /// Return a pseudo-random number from the distribution, with the additional restriction that the generated number is not smaller than the specified minimum.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <returns></returns>
        public virtual void Sample(double min)
        {
            if (IsDistributed)
            {
                distribution.Sample(min);
            }
        }

        /// <summary>
        /// Checks if the specified values are valid the distribution bounds, compared to the minimum and maximum values of the quantity and taking the unit into account.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns></returns>
        protected virtual List<ValidationResult> CheckDistributionBounds(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            const string lowerBoundFormat = "The {0} of a '{1}' with distribution type '{2}' must not be less than {3} {4}.";
            const string upperBoundFormat = "The {0} of a '{1}' with distribution type '{2}' must not be greater than {3} {4}.";

            switch (this.Distribution.DistributionType)
            {
                case DistributionTypes.Uniform:
                    if (this.Distribution.LowerBound.HasValue && this.Distribution.LowerBound.Value < Min)
                    {
                        validationResults.Add(String.Format(lowerBoundFormat, ModelHelpers.GetDisplayName<Distribution>(d => d.LowerBound), validationContext.DisplayName.ToFriendly(), EnumHelper2<DistributionTypes>.GetDisplayValue(DistributionTypes.Uniform), Min, UnitDisplay), validationContext);
                    }

                    if (this.Distribution.UpperBound.HasValue && this.Distribution.UpperBound.Value > Max)
                    {
                        validationResults.Add(String.Format(upperBoundFormat, ModelHelpers.GetDisplayName<Distribution>(d => d.UpperBound), validationContext.DisplayName.ToFriendly(), EnumHelper2<DistributionTypes>.GetDisplayValue(DistributionTypes.Uniform), Max, UnitDisplay), validationContext);
                    }

                    break;

                case Distributions.DistributionTypes.Triangular:
                    if (this.Distribution.Location.HasValue && this.Distribution.Location.Value < Min)
                    {
                        validationResults.Add(String.Format(lowerBoundFormat, ModelHelpers.GetDisplayName<Distribution>(d => d.Location), validationContext.DisplayName.ToFriendly(), EnumHelper2<DistributionTypes>.GetDisplayValue(DistributionTypes.Triangular), Min, UnitDisplay), validationContext);
                    }

                    if (this.Distribution.Scale.HasValue && this.Distribution.Scale.Value > Max)
                    {
                        validationResults.Add(String.Format(upperBoundFormat, ModelHelpers.GetDisplayName<Distribution>(d => d.Scale), validationContext.DisplayName.ToFriendly(), EnumHelper2<DistributionTypes>.GetDisplayValue(DistributionTypes.Triangular), Max, UnitDisplay), validationContext);
                    }

                    break;

                default:
                    // No validation required.
                    break;
            }

            return validationResults;
        }
    }
}