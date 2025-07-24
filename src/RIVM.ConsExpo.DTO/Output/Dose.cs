using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using RIVM.ConsExpo.DTO.Extensions;

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// A result of a calculation, specified as a value and a unit.
    /// </summary>
    public class Dose : IComparable<Dose>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Dose"/> class. Use only for serialization.
        /// </summary>
        [Obsolete("Added to solve \"RIVM.ConsExpo.DTO.Output.Dose cannot be serialized because it does not have a parameterless constructor.\"")]
        public Dose()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Dose"/> class, for the specified value and unit.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="doseUnit">The dose unit.</param>
        public Dose(double? value, DoseUnits doseUnit)
        {
            this.doseValue = value;
            this.Unit = doseUnit;
        }

        /// <summary>
        /// The internal dose value, as expressed in the to the unit.
        /// </summary>
        /// <seealso cref="Unit"/>
        protected double? doseValue;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double? Value
        {
            get => doseValue;
            set
            {
                //Debug.Assert(!value.HasValue || !double.IsNaN(value.Value), "Calculation resulted in NaN (Not a Number), probably as a result of a division of zero by zero.");

                doseValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        [Required]
        public DoseUnits Unit { get; set; }

        /// <summary>
        /// Gets the scientific value. This is the same as value, but with an instruction to the views to use a custom display template for scientific values, like 1.4 × 10³.
        /// </summary>
        /// <value>
        /// The scientific value.
        /// </value>
        [UIHint("ScientificValue")]
        public double? ScientificValue => Value;

        /// <summary>
        /// Implements the IComparable interface for sorting purposes.
        /// </summary>
        /// <param name="dose">The dose to compare the current instance to.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException"></exception>
        public int CompareTo(Dose dose)
        {
            if (Unit != dose.Unit)
                throw new ApplicationException(
                    $"Cannot compare dose '{Value} {Unit}' to dose '{dose.Value} {dose.Unit}', because they have different units.");

            if (Value.HasValue && dose.Value.HasValue)
            {
                return Value.Value.CompareTo(dose.Value.Value);
            }

            if (Value.HasValue)
            {
                return -1;
            }

            return 1;
        }

        public override string ToString()
        {
            string value;
            string unit;

            if (this.Value.HasValue)
            {
                unit = this.Unit.GetType()
                        .GetMember(this.Unit.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>().Name;
                value = this.Value.FormatAsPowerOfTen(1);
            }
            else
            {
                value = "–";
                unit = "";
            }

            return (value + " " + unit).Trim();
        }
    }
}