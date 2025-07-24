using RIVM.ConsExpo.DTO.PhysicalUnits;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// A physical quantity that describes a ratio between the concentrations of some substance in two compartments.
    /// </summary>
    public abstract class PartitionCoefficient : DistributablePhysicalQuantity<Dimensionless>
    {
        protected static readonly Dimensionless DefaultUnit = Dimensionless.Log10;

        public PartitionCoefficient()
        {
        }

        public PartitionCoefficient(bool setDefaults)
        {
            if (setDefaults)
            {
                Unit = DefaultUnit;
            }
        }

        public override double Min
        {
            get
            {
                if (Unit == Dimensionless.Linear)
                {
                    return this.MinForDefaultUnit;
                }
                else if (Unit == Dimensionless.Log10)
                {
                    return Math.Log10(this.MinForDefaultUnit);
                }
                else
                {
                    throw new NotSupportedException(string.Format("Unsupported partition coefficient unit '{0}'", Unit.ToString()));
                }
            }
        }

        public override double? Max
        {
            get
            {
                if (this.MaxForDefaultUnit == null)
                {
                    return null;
                }
                else
                {
                    if (Unit == Dimensionless.Linear)
                    {
                        return this.MaxForDefaultUnit;
                    }
                    else if (Unit == Dimensionless.Log10)
                    {
                        return Math.Log10(this.MaxForDefaultUnit.Value);
                    }
                    else
                    {
                        throw new NotSupportedException(string.Format("Unsupported partition coefficient unit '{0}'", Unit.ToString()));
                    }
                }
            }
        }

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
                    if (Unit == Dimensionless.Linear)
                    {
                        //Done. This is assumed to be the standard unit.
                    }
                    else if (Unit == Dimensionless.Log10)
                    {
                        internalValue = Math.Pow(10, internalValue.Value);
                    }
                    else
                    {
                        throw new NotSupportedException(string.Format("Unsupported partition coefficient unit '{0}'", Unit.ToString()));
                    }
                }
                else
                {
#warning To Do: call extension method
                    throw new InvalidOperationException();
                    // throw new InvalidOperationException(ExceptionHelper.NoStandardizedValueMessage(this.GetType().Name, this));
                }
                return internalValue.Value;
            }
        }

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<Dimensionless> AllUnits => Dimensionless.AllUnits;

        [NotMapped]
        [XmlIgnore]
        public override IEnumerable<Dimensionless> AvailableUnits => Dimensionless.AllUnits;

        public double AsLinear()
        {
            return ConvertedValue(Dimensionless.Linear);
        }

        public double As10Log()
        {
            return ConvertedValue(Dimensionless.Log10);
        }

        public override double ConvertedValue(Dimensionless targetUnit)
        {
#warning To Do: check if the derived value should be used. currently, calling this when the partition coefficient is distributed, a null reference exception will occur.
            double? derivedValue = DerivedValue;
            if (derivedValue.HasValue)
            {
                if (this.UnitCode == targetUnit.Code)
                {
                    return derivedValue.Value;
                }
                else if (this.UnitCode == Dimensionless.Linear.Code && targetUnit.Code == Dimensionless.Log10.Code)
                {
                    return Math.Log10(derivedValue.Value);
                }
                else if (this.UnitCode == Dimensionless.Log10.Code && targetUnit.Code == Dimensionless.Linear.Code)
                {
                    return Math.Pow(10, derivedValue.Value);
                }
                else
                {
                    throw new NotSupportedException(string.Format("Cannot convert a partition coefficient from unit '{0}' to unit '{1}'.", this.UnitDisplay, targetUnit.DisplayName));
                }
            }
            else
            {
                throw new NullReferenceException("");
            }
        }
    }
}