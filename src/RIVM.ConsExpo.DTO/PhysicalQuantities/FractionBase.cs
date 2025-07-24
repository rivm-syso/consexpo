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

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public abstract class FractionBase : DistributablePhysicalQuantity<FractionUnits>
    {
        protected override double? MaxForDefaultUnit => Constants.MaxFraction;

        protected override double Standardized => ConvertedValue(FractionUnits.StandardUnit);

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<FractionUnits> AvailableUnits => FractionUnits.AllUnits;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<FractionUnits> AllUnits => FractionUnits.AllUnits;

        /// <summary>
        /// Gets the value as a fraction between 0 and 1.
        /// </summary>
        /// <value>
        /// As fraction.
        /// </value>
        public double AsFraction()
        {
            return Standardized;
        }

        /// <summary>
        /// Gets the fraction as a percentage between 0 and 100.
        /// </summary>
        /// <value>
        /// As percentage.
        /// </value>
        public double AsPercentage()
        {
            return Standardized * ConversionFactors.FractionToPercentage;
        }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            const double lowerBound = 0;

            const double upperBoundFraction = 1;
            const double upperBoundPercentage = 100;

            var validationResults = base.Validate(validationContext).ToList();

            //This validation methods is called from:
            //1) MVC for actions. In that case, the validation context is 'Fraction'.
            //2) EF SaveChanges(). In that case, the validation context is the type with the fraction Instance, e.g. InhalationExposure.
            //Unfortunately, 1) does not present the context of the fraction, so we cannot show the name of the property.
            //We cannot use 2) as it will throw an exception rather than show a validation result.
            if (validationContext.ObjectInstance is Fraction fraction)
            {
                Fraction instance = fraction;

                const string labelFraction = "fraction";
                const string labelPercentage = "percentage";

                if (instance.IsDistributed)
                {
                    if (instance.Unit == FractionUnits.Fraction)
                    {
                        CheckDistributionBounds(validationContext, validationResults, instance, lowerBound, upperBoundFraction, labelFraction);
                    }
                    else if (instance.Unit == FractionUnits.Percentage)
                    {
                        CheckDistributionBounds(validationContext, validationResults, instance, lowerBound, upperBoundPercentage, labelPercentage);
                    }
                }
                else if (instance.Value.HasValue)
                {
                    if (instance.Unit == FractionUnits.Fraction)
                    {
                        CheckValueBounds(validationContext, validationResults, instance, lowerBound, upperBoundFraction, labelFraction);
                    }
                    else if (instance.Unit == FractionUnits.Percentage)
                    {
                        CheckValueBounds(validationContext, validationResults, instance, lowerBound, upperBoundPercentage, labelPercentage);
                    }
                }
            }

            return validationResults;
        }

        private static void CheckValueBounds(ValidationContext validationContext, List<ValidationResult> validationResults, Fraction instance, double lowerBound, double upperBound, string unitLabel)
        {
            const string lowerBoundFormat = "A {0} must not be less than {1}.";
            const string upperBoundFormat = "A {0} must not be greater than {1}.";

            if (instance.Value < lowerBound)
            {
                validationResults.Add(String.Format(lowerBoundFormat, unitLabel, lowerBound), validationContext);
            }

            if (instance.Value > upperBound)
            {
                validationResults.Add(String.Format(upperBoundFormat, unitLabel, upperBound), validationContext);
            }
        }

        [Obsolete("Should use the bounds validation in DistributablePhysicalQuantity<t>.")]
        private static void CheckDistributionBounds(ValidationContext validationContext, List<ValidationResult> validationResults, Fraction instance, double lowerBound, double upperBound, string unitLabel)
        {
            const string lowerBoundFormat = "The {0} of a {1} with distribution type '{2}' must not be less than {3}.";
            const string upperBoundFormat = "The {0} of a {1} with distribution type '{2}' must not be greater than {3}.";

            switch (instance.Distribution.DistributionType)
            {
                case DistributionTypes.Uniform:
                    if (instance.Distribution.LowerBound.HasValue && instance.Distribution.LowerBound.Value < lowerBound)
                    {
                        validationResults.Add(String.Format(lowerBoundFormat, ModelHelpers.GetDisplayName<Distribution>(d => d.LowerBound), unitLabel, EnumHelper2<DistributionTypes>.GetDisplayValue(DistributionTypes.Uniform), lowerBound), validationContext);
                    }

                    if (instance.Distribution.UpperBound.HasValue && instance.Distribution.UpperBound.Value > upperBound)
                    {
                        validationResults.Add(String.Format(upperBoundFormat, ModelHelpers.GetDisplayName<Distribution>(d => d.UpperBound), unitLabel, EnumHelper2<DistributionTypes>.GetDisplayValue(DistributionTypes.Uniform), upperBound), validationContext);
                    }

                    break;

                case DistributionTypes.Triangular:
                    if (instance.Distribution.Location.HasValue && instance.Distribution.Location.Value < lowerBound)
                    {
                        validationResults.Add(String.Format(lowerBoundFormat, ModelHelpers.GetDisplayName<Distribution>(d => d.Location), unitLabel, EnumHelper2<DistributionTypes>.GetDisplayValue(DistributionTypes.Triangular), lowerBound), validationContext);
                    }

                    if (instance.Distribution.Scale.HasValue && instance.Distribution.Scale.Value > upperBound)
                    {
                        validationResults.Add(String.Format(upperBoundFormat, ModelHelpers.GetDisplayName<Distribution>(d => d.Scale), unitLabel, EnumHelper2<DistributionTypes>.GetDisplayValue(DistributionTypes.Triangular), upperBound), validationContext);
                    }

                    break;

                default:
                    // No validation required.
                    break;
            }
        }

        public override void Sample()
        {
            Distribution.BetaScalingFactor = Unit.ConversionFactor;
            base.Sample();
        }

        public override void Sample(double min)
        {
            Distribution.BetaScalingFactor = Unit.ConversionFactor;
            base.Sample(min);
        }

        public override double? DerivedValue
        {
            get
            {
                Distribution.BetaScalingFactor = Unit.ConversionFactor;
                return base.DerivedValue;
            }
        }
    }
}