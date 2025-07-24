using RIVM.ConsExpo.DTO.PhysicalQuantities;
using System;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// A base class for absorption outcomes. Intended for restriction of generic type specifications.
    /// </summary>
    public abstract class AbsorptionOutcomeBase
    {
        protected double? bodyweightInKilogram;
        protected double? frequencyInTimesPerDay;

        public AbsorptionOutcomeBase(BodyWeight bodyWeight, Frequency frequency)
        {
            this.bodyweightInKilogram = bodyWeight != null ? bodyWeight.InKilogramIfSpecified() : null;
            this.frequencyInTimesPerDay = frequency != null ? frequency.InTimesPerDayIfSpecified() : null;
        }

        protected Dose dose;

        /// <summary>
        /// Gets or sets the dose.
        /// </summary>
        /// <value>
        /// The dose.
        /// </value>
        /// <exception cref="System.NotSupportedException">Specify a dose with a valid unit.</exception>
        public Dose Dose
        {
            get
            {
                return dose;
            }
            set
            {
                switch (value.Unit)
                {
                    case DoseUnits.Mg:
                    case DoseUnits.MgPerKgBodyWeight:
                    case DoseUnits.MgPerKgBodyWeightPerDay:
                        dose = value;
                        break;

                    default:
                        throw new NotSupportedException(string.Format("An absorption outcome cannot be specified by a dose with unit '{0}'", value.Unit.ToString()));
                }
            }
        }

        public const DoseUnits InternalEventDoseUnit = DoseUnits.MgPerKgBodyWeight;

        public virtual Dose AsInternalEventDose
        {
            get
            {
                double? value;

                if (dose.Value.HasValue)
                {
                    switch (dose.Unit)
                    {
                        case DoseUnits.Mg:
                            value = bodyweightInKilogram.HasValue ? dose.Value / bodyweightInKilogram : null;
                            break;

                        case DoseUnits.MgPerKgBodyWeight:
                            value = dose.Value;
                            break;

                        case DoseUnits.MgPerKgBodyWeightPerDay:
                            value = frequencyInTimesPerDay.HasValue ? dose.Value / frequencyInTimesPerDay : null;
                            break;

                        default:
                            throw new NotSupportedException(string.Format("An absorption outcome with dose unit '{0}' cannot be converted to an internal event dose.", dose.Unit.ToString()));
                    }
                }
                else
                {
                    value = null;
                }

                return new Dose(value, InternalEventDoseUnit);
            }
        }

        public const DoseUnits InternalDayDoseUnit = DoseUnits.MgPerKgBodyWeightPerDay;

        public virtual Dose AsInternalDayDose
        {
            get
            {
                double? value;

                if (dose.Value.HasValue && this.AsInternalEventDose.Value.HasValue && frequencyInTimesPerDay.HasValue)
                {
                    if (frequencyInTimesPerDay.Value > 1)
                    {
                        value = this.AsInternalEventDose.Value * frequencyInTimesPerDay.Value;
                    }
                    else
                    {
                        value = this.AsInternalEventDose.Value;
                    }
                }
                else
                {
                    value = null;
                }

                return new Dose(value, InternalDayDoseUnit);
            }
        }

        public const DoseUnits InternalYearAverageDoseUnit = DoseUnits.MgPerKgBodyWeightPerDay;

        public virtual Dose AsInternalYearAverageDose
        {
            get
            {
                double? value;

                if (dose.Value.HasValue)
                {
                    switch (dose.Unit)
                    {
                        case DoseUnits.Mg:
                            value = bodyweightInKilogram.HasValue && frequencyInTimesPerDay.HasValue ? dose.Value * frequencyInTimesPerDay / bodyweightInKilogram : null;
                            break;

                        case DoseUnits.MgPerKgBodyWeight:
                            value = frequencyInTimesPerDay.HasValue ? dose.Value * frequencyInTimesPerDay : null;
                            break;

                        case DoseUnits.MgPerKgBodyWeightPerDay:
                            value = dose.Value;
                            break;

                        default:
                            throw new NotSupportedException(string.Format("An absorption outcome with dose unit '{0}' cannot be converted to an internal event dose.", dose.Unit.ToString()));
                    }
                }
                else
                {
                    value = null;
                }

                return new Dose(value, InternalYearAverageDoseUnit);
            }
        }

        public const DoseUnits StandardUnit = DoseUnits.Mg;
    }
}