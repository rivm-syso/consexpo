using RIVM.ConsExpo.DTO.PhysicalQuantities;
using System;

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// A class that can contain the outcome of a oral calculation. It is capable of transforming the outcome to varying measures.
    /// </summary>
    public class OralExposureOutcome : ExposureOutcomeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OralExposureOutcome" /> class.
        /// </summary>
        /// <param name="bodyWeight">The body weight.</param>
        /// <param name="scenarioFrequency">The scenario frequency.</param>
        /// <param name="amountOfSubstance">The amount of substance released (in mg).</param>
        /// <remarks>
        /// This class does not store a reference to the passed reference types, but derives a value. This is needed to store many outcomes for distributed body weights. So, changing the passed body weight after this constructor has been called, does not change the stored value!
        /// </remarks>
        public OralExposureOutcome(BodyWeight bodyWeight, Frequency scenarioFrequency, double? amountOfSubstance)
            : base(bodyWeight, scenarioFrequency, amountOfSubstance)
        { }

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
                    case DoseUnits.Mg:
                    case DoseUnits.MgPerKgBodyWeight:
                        dose = value;
                        break;

                    default:
                        throw new NotSupportedException(string.Format("An oral exposure outcome cannot be specified by a dose with unit '{0}'.", value.Unit.ToString()));
                }
            }
        }

        /// <summary>
        /// Gets the external event dose.
        /// </summary>
        /// <value>
        /// The external event dose or null if the value is null or one of the parameters needed for the conversion to external event dose is not specified.
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
                        case DoseUnits.Mg:
                        case DoseUnits.MgPerKgBodyWeight:
                            value = ExternalEventDoseValue;
                            break;

                        default:
                            throw new NotSupportedException(
                                $"An oral exposure outcome with dose unit '{dose.Unit}' cannot be converted to an external event dose.");
                    }
                }
                else
                {
                    value = null;
                }

                return new Dose(value, ExternalEventDoseUnit);
            }
        }
    }
}