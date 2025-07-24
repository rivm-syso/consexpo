using RIVM.ConsExpo.DTO.PhysicalQuantities;
using System;

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// A base class for exposure outcomes. Intended for restriction of generic type specifications.
    /// </summary>
    public abstract class ExposureOutcomeBase
    {
        /// <summary>
        /// The body weight standardized value, if specified.
        /// </summary>
        protected double? bodyWeightInKilogram;

        /// <summary>
        /// The frequency standardized value, if specified.
        /// FrequencyUnits StandardUnit = FrequencyUnits.Daily;
        /// </summary>
        protected double? frequencyInTimesPerDay;

        /// <summary>
        /// The amount of substance available in the scenario.
        /// </summary>
        protected double? amountOfSubstance;

        /// <summary>
        /// The dose.
        /// </summary>
        protected Dose dose;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExposureOutcomeBase" /> class.
        /// </summary>
        /// <param name="bodyWeight">The body weight.</param>
        /// <param name="scenarioFrequency">The scenario Frequency.</param>
        /// <param name="amountOfSubstance">The amount of substance released in mg.</param>
        protected ExposureOutcomeBase(BodyWeight bodyWeight, Frequency scenarioFrequency, double? amountOfSubstance)
        {
            this.bodyWeightInKilogram = bodyWeight?.InKilogramIfSpecified();
            this.frequencyInTimesPerDay = scenarioFrequency?.InTimesPerDayIfSpecified();
            this.amountOfSubstance = amountOfSubstance;
        }

        /// <summary>
        /// Gets or sets the dose.
        /// </summary>
        /// <value>
        /// The dose.
        /// </value>
        public abstract Dose Dose { get; set; }

        /// <summary>
        /// The external event dose unit.
        /// </summary>
        public const DoseUnits ExternalEventDoseUnit = DoseUnits.MgPerKgBodyWeight;

        /// <summary>
        /// The external dose on day of exposure unit.
        /// </summary>
        public const DoseUnits ExternalDayDoseUnit = DoseUnits.MgPerKgBodyWeightPerDay;

        /// <summary>
        /// The unit for exposure fractions.
        /// </summary>
        public const DoseUnits ExposureFractionDoseUnit = DoseUnits.Fraction;

        /// <summary>
        /// Gets the external event dose.
        /// </summary>
        /// <value>
        /// The external event dose or null if the value is null or one of the parameters needed for the conversion to external event dose is not specified.
        /// </value>
        /// <example>If the dose was stored a the amount of substance (mg), the external event dose is the amount of substance divided by the body weight. If the body weight is not specified, null is returned.</example>
        public abstract Dose AsExternalEventDose { get; }

        /// <summary>
        /// Gets the external dose on day of exposure.
        /// If frequency &lt;= 1 per day, ExternalDayDose = ExternalEventDose. Otherwise ExternalDayDose = ExternalEventDose * frequencyInTimesPerDay
        /// </summary>
        /// <value>
        /// The external dose on day of exposure or null if the value is null or one of the parameters needed for the conversion to external dose on day of exposure is not specified.
        /// </value>
        public Dose AsExternalDayDose
        {
            get
            {
                double? value;

                if (dose.Value.HasValue && this.AsExternalEventDose.Value.HasValue && frequencyInTimesPerDay.HasValue)
                {
                    if (frequencyInTimesPerDay.Value > 1)
                    {
                        value = this.AsExternalEventDose.Value * frequencyInTimesPerDay.Value;
                    }
                    else
                    {
                        value = this.AsExternalEventDose.Value;
                    }
                }
                else
                {
                    value = null;
                }

                return new Dose(value, ExternalEventDoseUnit);
            }
        }

        /// <summary>
        /// Gets the get external event dose value.
        /// </summary>
        /// <value>
        /// The get external event dose value.
        /// </value>
        /// <exception cref="System.NotSupportedException"></exception>
        protected double? ExternalEventDoseValue
        {
            get
            {
                double? value;

                if (dose.Value.HasValue)
                {
                    switch (dose.Unit)
                    {
                        case DoseUnits.MgPerKgBodyWeight:
                            value = dose.Value;
                            break;

                        case DoseUnits.Mg:
                            value = bodyWeightInKilogram.HasValue ? dose.Value / bodyWeightInKilogram : null;
                            break;

                        default:
                            throw new NotSupportedException($"An exposure outcome with dose unit '{dose.Unit.ToString()}' cannot be converted to an external event dose.");
                    }
                }
                else
                {
                    value = null;
                }

                return value;
            }
        }

        /// <summary>
        /// Returns the exposure fraction as a dose measure, if the submodel supports exposure fractions. Otherwise, returns null.
        /// </summary>
        public virtual Dose AsExposureFraction
        {
            get
            {
                var value = ExposureFractionValue;

                if (value.HasValue)
                {
                    value = Math.Min(value.Value, Constants.MaxFraction);
                }

                return new Dose(value, ExposureFractionDoseUnit);
            }
        }

        /// <summary>
        /// Gets the exposure fraction. If exposed amount (mg) is the current dose measure, body weight is not required.
        /// </summary>
        /// <exception cref="System.NotSupportedException"></exception>
        protected virtual double? ExposureFractionValue
        {
            get
            {
                double? value;

                if (dose.Value.HasValue)
                {
                    switch (dose.Unit)
                    {
                        case DoseUnits.Mg:
                            value = amountOfSubstance.HasValue ? dose.Value / amountOfSubstance : null;
                            break;

                        case DoseUnits.MgPerKgBodyWeight:
                            value = bodyWeightInKilogram.HasValue && amountOfSubstance.HasValue ? dose.Value * bodyWeightInKilogram / amountOfSubstance : null;
                            break;

                        default:
                            throw new NotSupportedException($"An exposure outcome with dose unit '{dose.Unit}' cannot be converted to an exposure fraction.");
                    }
                }
                else
                {
                    value = null;
                }

                return value;
            }
        }
    }
}