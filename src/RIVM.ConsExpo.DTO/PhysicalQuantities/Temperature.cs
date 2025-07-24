using DataAnnotationsExtensions;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// A temperature.
    /// </summary>
    public class Temperature : DistributablePhysicalQuantity<TemperatureUnits>
    {
        protected const double MinTemperatureInCelsius = -100;

        [Display(Name = "Temperature")]
        [Min(MinTemperatureInCelsius, ErrorMessage = "The value of {0} must be greater than or equal to {1}")]
        public override double? Value
        {
            get => base.Value;
            set => base.Value = value;
        }

        protected override double MinForDefaultUnit => ConversionFactors.CelsiusOffset + MinTemperatureInCelsius;

        public override double Min => MinForDefaultUnit - Unit.Offset;

        public override double? Max => MaxForDefaultUnit == null ? null : MaxForDefaultUnit - Unit.Offset;

        public override void Sample()
        {
            if (Unit == TemperatureUnits.Celsius)
            {
                Sample(MinTemperatureInCelsius);
            }
            else if (Unit == TemperatureUnits.Kelvin)
            {
                Sample(MinTemperatureInCelsius + ConversionFactors.CelsiusOffset);
            }
            else
            {
                throw new NotSupportedException(string.Format("Unsupported temperature unit '{0}'", Unit.ToString()));
            }
        }

        public static readonly TemperatureUnits StandardUnit = TemperatureUnits.Kelvin;

        /// <summary>
        /// Returns the value in the unit used in model calculations
        /// </summary>
        protected override double Standardized
        {
            get
            {
                double? internalValue = DerivedValue;

                if (internalValue.HasValue)
                {
                    if (Unit == StandardUnit)
                    {
                        //Done.
                    }
                    else if (Unit == TemperatureUnits.Celsius)
                    {
                        internalValue += ConversionFactors.CelsiusOffset;
                    }
                    else
                    {
                        throw new NotSupportedException(string.Format("Unsupported temperature unit '{0}'", Unit.ToString()));
                    }
                }
                else
                {
#warning To Do: call extension method
                    throw new InvalidOperationException();
                    //throw new InvalidOperationException(ExceptionHelper.NoStandardizedValueMessage(this.GetType().Name, this));
                }
                return internalValue.Value;
            }
        }

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<TemperatureUnits> AllUnits => TemperatureUnits.AllUnits;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<TemperatureUnits> AvailableUnits => TemperatureUnits.AllUnits;

        public double InKelvin()
        {
            return ConvertedValue(TemperatureUnits.Kelvin);
        }

        public double InCelsius()
        {
            return ConvertedValue(TemperatureUnits.Celsius);
        }

        public override double ConvertedValue(TemperatureUnits targetUnit)
        {
            double internalValue = Standardized;

            if (targetUnit.Code == TemperatureUnits.Kelvin.Code)
            {
                return internalValue;
            }
            else if (targetUnit.Code == TemperatureUnits.Celsius.Code)
            {
                return internalValue - ConversionFactors.CelsiusOffset;
            }
            else
            {
                throw new NotSupportedException(string.Format("Cannot convert a temperature from unit '{0}' to unit '{1}'.", this.UnitDisplay, targetUnit.DisplayName));
            }
        }
    }
}