using RIVM.ConsExpo.DTO.Extensions;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Serialization;
using DataAnnotationsExtensions;

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T">The enumeration type for the unit.</typeparam>
    [ComplexType]
    public abstract class PhysicalQuantity<T> : IPhysicalQuantity<T>, IValidatableObject where T : UnitBase
    {
        private int unitCode;
        private readonly string displayName;

        /// <summary>
        /// Constructor for creating an instance with a min and a max value.
        /// </summary>
        protected PhysicalQuantity(string displayName, T minMaxUnit, double min, double? max) : this(displayName, minMaxUnit, min)
        {
            this.MaxForDefaultUnit = max * minMaxUnit.ConversionFactor;
        }

        /// <summary>
        /// Constructor for creating an instance with a min value.
        /// </summary>
        protected PhysicalQuantity(string displayName, T minMaxUnit, double min) : this()
        {
            this.displayName = displayName;
            this.MinForDefaultUnit = min * minMaxUnit.ConversionFactor;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalQuantity{T}"/> class.
        /// </summary>
        protected PhysicalQuantity()
        {
            unitCode = AvailableUnits.First().Code;
        }

        /// <summary>
        /// Gets or sets the unit code. This is the numerical representation of the unit, which is stored in the database.
        /// </summary>
        /// <value>
        /// The unit code.
        /// </value>
        [Required]
        public int UnitCode
        {
            get => unitCode;
            set
            {
                bool any = false;
                foreach (var unit in AvailableUnits)
                {
                    if (unit.Code == unitCode)
                    {
                        any = true;
                        break;
                    }
                }

                if (!any)
                {
                    throw new ApplicationException($"The physical quantity '{this.GetType().Name}' does not support a unit with code '{value}'.");
                }

                unitCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the unit. This is the instance of the unit, corresponding to the unit code.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        [NotMapped]
        [XmlIgnore]
        public T Unit
        {
            get
            {
                return AllUnits.FirstOrDefault(u => u.Code == UnitCode);
            }
            set => UnitCode = value.Code;
        }

        /// <summary>
        /// Gets the available units. This is a subset of the units of the physical quantity, specified in base quantities and possibly altered in derived quantities.
        /// </summary>
        /// <value>
        /// The available units.
        /// </value>
        /// <see href="http://stackoverflow.com/questions/730401/upcasting-and-generic-lists"/>
        public abstract IEnumerable<T> AvailableUnits { get; }

        /// <summary>
        /// Gets all supported units.
        /// </summary>
        public abstract IEnumerable<T> AllUnits { get; }

        /// <summary>
        /// Gets a user-friendly description for the unit.
        /// </summary>
        public string UnitDisplay => Unit.DisplayName;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        [Min(0.0, ErrorMessage = "Enter a non-negative number.")]
        public virtual double? Value { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has a value. For a non-distributable physical quantity, just inspect the Value property.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has value; otherwise, <c>false</c>.
        /// </value>
        public virtual bool HasValue => Value.HasValue;

        /// <summary>
        /// Gets the minimum for the default unit.
        /// </summary>
        /// <value>
        /// The minimum for default unit.
        /// </value>
        /// <remarks>The default unit is the unit that has conversion factor 1.0 (or conversion offset 0.0, for Temperature).</remarks>
        protected virtual double MinForDefaultUnit { get; }

        /// <summary>
        /// Gets the dynamic minimum allowed value for the instance, based on the current unit.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        /// <exception cref="NullReferenceException">A null reference exception may occur when the unit is not properly initialized.</exception>
        public virtual double Min => this.MinForDefaultUnit / Unit.ConversionFactor;

        /// <summary>
        /// Gets the maximum for the default unit.
        /// </summary>
        /// <value>
        /// The maximum for default unit.
        /// </value>
        protected virtual double? MaxForDefaultUnit { get; }

        /// <summary>
        /// Gets the dynamic maximum allowed value for the instance, based on the current unit.
        /// </summary>
        /// <value>
        /// The maximum.
        /// </value>
        public virtual double? Max
        {
            get
            {
                if (this.MaxForDefaultUnit == null)
                {
                    return null;
                }

                return this.MaxForDefaultUnit.Value / Unit.ConversionFactor;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the physical quantity supports specification of a distribution.
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports distributions]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool SupportsDistributions => false; //Unless overridden in DistributablePhysicalQuantity<T>.

        /// <summary>
        /// Returns true if both instances are specified by the same value and unit or if both are null.
        /// </summary>
        /// <param name="x">First instance.</param>
        /// <param name="y">Second instance</param>
        /// <param name="allowUnitConversion">If set to <c>true</c>, values are considered equal if they are equal when converted to the same unit. E.g. 100 cm == 1 m.</param>
        /// <returns></returns>
        public static bool EqualValues(PhysicalQuantity<T> x, PhysicalQuantity<T> y, bool allowUnitConversion = false)
        {
            if (x == null && y == null)
            {
                return true;
            }

            if (x == null || y == null)
            {
                return false;
            }

            if (!x.HasValue && !y.HasValue && x.UnitCode == 0 && y.UnitCode == 0)
            {
                return true;
            }

            if (!allowUnitConversion && !x.Unit.Equals(y.Unit))
            {
                return false;
            }

            if (x.HasValue ^ y.HasValue)
            {
                return false;
            }

            if (!allowUnitConversion)
            {
                return x.Value == y.Value;
            }

            return x.Standardized == y.Standardized;
        }

        /// <summary>
        /// Returns the value of the physical quantity, expressed in the standard unit.
        /// </summary>
        /// <value>
        /// The standardized.
        /// </value>
        protected abstract double Standardized { get; }

        /// <summary>
        /// Returns the value of the physical quantity, converted to the specified target unit.
        /// </summary>
        /// <param name="targetUnit">The target unit.</param>
        /// <returns></returns>
        public virtual double ConvertedValue(T targetUnit)
        {
            return ConvertedValue(targetUnit, this.Value.Value);
        }

        /// <summary>
        /// Returns the specified value, converted to the specified target unit.
        /// </summary>
        /// <param name="targetUnit">The target unit.</param>
        /// <param name="calculationValue">The calculation value.</param>
        /// <returns></returns>
        protected virtual double ConvertedValue(T targetUnit, double calculationValue)
        {
#warning HACK, removed this.
            double conversionFactor = Unit.ConversionFactor / targetUnit.ConversionFactor;
            return calculationValue * conversionFactor;
        }

        /// <summary>
        /// Instruction for the XML serializer. Only serialize Value if it has a value.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeValue()
        {
            return Value != null;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is distributed.
        /// </summary>
        /// <value>
        /// false, as instances of this base type cannot be distributed.
        /// </value>
        [NotMapped]
        public virtual bool IsDistributed => false;

        /// <summary>
        /// Gets the available units as unit base instances.
        /// </summary>
        /// <value>
        /// The available base units.
        /// </value>
        public IEnumerable<UnitBase> AvailableBaseUnits => this.AvailableUnits.Cast<UnitBase>();

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>
        /// A collection that holds failed-validation information.
        /// </returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            const string LowerBoundFormat = "{0} must not be less than {1} {2}.";
            const string UpperBoundFormat = "{0} must not be greater than {1} {2}.";

            string displayName = this.displayName ?? validationContext.DisplayName.ToFriendly();

#warning validationContext.DisplayName in this method returns the name of the physical quantity, rather than the name of the property of that type.
            if (this.Value.HasValue && this.Value < Min)
            {
                validationResults.Add(String.Format(LowerBoundFormat, displayName, Min, UnitDisplay), validationContext);
            }

            if (this.Value.HasValue && Max != null && this.Value > Max)
            {
                validationResults.Add(String.Format(UpperBoundFormat, displayName, Max, UnitDisplay), validationContext);
            }

            return validationResults;
        }
    }
}