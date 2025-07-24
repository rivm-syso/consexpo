using RIVM.ConsExpo.DTO.PhysicalQuantities;
using System;

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// A class that can contain the outcome of a dermal exposure calculation. It is capable of transforming the outcome to varying measures.
    /// </summary>
    public class DermalExposureOutcome : ExposureOutcomeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DermalExposureOutcome"/> class.
        /// </summary>
        /// <param name="bodyWeight">The body weight.</param>
        /// <param name="scenarioFrequency">The scenario Frequency.</param>
        /// <param name="amountOfSubstance">The amount of substance released (in mg).</param>
        /// <param name="exposedArea">The exposed area.</param>
        /// <remarks>This class does not store a reference to the passed reference types, but derives a value. This is needed to store many outcomes for distributed body weights or exposed areas. So, changing the passed body weight or exposed area after this constructor has been called, does not change the stored values!</remarks>
        public DermalExposureOutcome(BodyWeight bodyWeight, Frequency scenarioFrequency, double? amountOfSubstance, ExposedArea exposedArea)
            : base(bodyWeight, scenarioFrequency, amountOfSubstance)
        {
            exposedAreaInSquareCentimetre = exposedArea?.InSquareCentimetreIfSpecified;
        }

        /// <summary>
        /// The exposed area, if specified.
        /// </summary>
        protected double? exposedAreaInSquareCentimetre;

        /// <summary>
        /// Gets or sets the dose.
        /// </summary>
        /// <value>
        /// The dose.
        /// </value>
        /// <exception cref="System.NotSupportedException">Specify a dose with a valid unit.</exception>
        public override Dose Dose
        {
            get
            {
                return dose;
            }
            set
            {
                switch (value.Unit)
                {
                    case DoseUnits.MgPerSquareCentimetre:
                    case DoseUnits.Mg:
                    case DoseUnits.MgPerKgBodyWeight:
                        dose = value;
                        break;

                    default:
                        throw new NotSupportedException(
                            $"A dermal exposure outcome cannot be specified by a dose with unit '{value.Unit.ToString()}'.");
                }
            }
        }

        /// <summary>
        /// The dermal load unit
        /// </summary>
        public const DoseUnits DermalLoadUnit = DoseUnits.MgPerSquareCentimetre;

        /// <summary>
        /// Gets as dermal load.
        /// </summary>
        /// <value>
        /// As dermal load.
        /// </value>
        public Dose AsDermalLoad
        {
            get
            {
                double? value;

                if (dose.Value.HasValue)
                {
                    switch (dose.Unit)
                    {
                        case DoseUnits.Mg:      //GetExternalEventDose will test if body weight has been specified.
                            value = exposedAreaInSquareCentimetre.HasValue ? dose.Value / exposedAreaInSquareCentimetre.Value : null;
                            break;

                        case DoseUnits.MgPerKgBodyWeight:
                            value = exposedAreaInSquareCentimetre.HasValue && bodyWeightInKilogram.HasValue ? dose.Value * bodyWeightInKilogram.Value / exposedAreaInSquareCentimetre : null;
                            break;

                        case DoseUnits.MgPerSquareCentimetre:
                            value = dose.Value;
                            break;

                        default:
                            throw new NotSupportedException(
                                $"A dermal exposure outcome with dose unit '{dose.Unit.ToString()}' cannot be converted to an external event dose.");
                    }
                }
                else
                {
                    value = null;
                }

                return new Dose(value, DermalLoadUnit);
            }
        }

        /// <summary>
        /// Gets the external event dose.
        /// </summary>
        /// <value>
        /// The external event dose, or null if the value is null or one of the parameters needed for the conversion to external event dose is not specified.
        /// </value>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <example>If the dose was stored a the amount of substance (mg), the external event dose is the amount of substance divided by the body weight. If the body weight is not specified, null is returned.</example>
        public override Dose AsExternalEventDose
        {
            get
            {
                double? value;

                if (dose.Value.HasValue)
                {
                    switch (dose.Unit)
                    {
                        case DoseUnits.Mg:      //GetExternalEventDose will test if body weight has been specified.
                        case DoseUnits.MgPerKgBodyWeight:
                            value = ExternalEventDoseValue;
                            break;

                        case DoseUnits.MgPerSquareCentimetre:
                            value = exposedAreaInSquareCentimetre.HasValue && bodyWeightInKilogram.HasValue ? exposedAreaInSquareCentimetre * dose.Value / bodyWeightInKilogram.Value : null;
                            break;

                        default:
                            throw new NotSupportedException($"A dermal exposure outcome with dose unit '{dose.Unit.ToString()}' cannot be converted to an external event dose.");
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
        /// Gets the exposure fraction.
        /// </summary>
        /// <value>
        /// The exposure fraction, or null if the submodel does not support exposure fractions, the value is null or one of the parameters needed for the conversion to exposure fraction is not specified.
        /// </value>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <example>If the dose was stored a the amount of substance (mg), the external event dose is the amount of substance divided by the body weight. If the body weight is not specified, null is returned.</example>
        protected override double? ExposureFractionValue
        {
            get
            {
                double? value;

                if (dose.Value.HasValue)
                {
                    switch (dose.Unit)
                    {
                        case DoseUnits.Mg:
                        case DoseUnits.MgPerKgBodyWeight:
                            value = base.ExposureFractionValue;
                            break;

                        case DoseUnits.MgPerSquareCentimetre:
                            value = exposedAreaInSquareCentimetre.HasValue && amountOfSubstance.HasValue
                                ? exposedAreaInSquareCentimetre * dose.Value / amountOfSubstance
                                : null;
                            break;

                        default:
                            throw new NotSupportedException($"A dermal exposure outcome with dose unit '{dose.Unit.ToString()}' cannot be converted to an external event dose.");
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