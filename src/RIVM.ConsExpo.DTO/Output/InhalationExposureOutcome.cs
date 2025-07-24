using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using System;

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// A class that can contain the outcome of a inhalation calculation. It is capable of transforming the outcome to varying measures.
    /// </summary>
    public class InhalationExposureOutcome : ExposureOutcomeBase
    {
        protected double? _inhalationRateInCubicMetresPerSecond;
        private readonly double _startOfExposureInSeconds;
        private readonly double _endOfExposureInSeconds;

        private double? ExposureDurationInDays => (_endOfExposureInSeconds - _startOfExposureInSeconds) / ConversionFactors.SecondsPerDay;

        /// <summary>
        /// Initializes a new instance of the <see cref="InhalationExposureOutcome" /> class.
        /// </summary>
        /// <param name="bodyWeight">The body weight.</param>
        /// <param name="scenarioFrequency">The scenario Frequency.</param>
        /// <param name="amountOfSubstance">The amount of substance released (in mg).</param>
        /// <param name="inhalationRate">The inhalation rate.</param>
        /// <param name="startOfExposure">The start of exposure.</param>
        /// <param name="endOfExposure">The end of exposure.</param>
        /// <remarks>This class does not store a reference to the passed reference types, but derives a value. This is needed to store many outcomes for distributed body weights etc. So, changing the passed body weight, inhalation rate or start/end of exposure after this constructor has been called, does not change the stored values!</remarks>
        public InhalationExposureOutcome(BodyWeight bodyWeight, Frequency scenarioFrequency, double? amountOfSubstance, VolumeRate inhalationRate, Time startOfExposure, Time endOfExposure)
            : base(bodyWeight, scenarioFrequency, amountOfSubstance)
        {
            this._inhalationRateInCubicMetresPerSecond = inhalationRate?.InCubicMetresPerSecondIfSpecified();
            this._startOfExposureInSeconds = startOfExposure.InSeconds();
            this._endOfExposureInSeconds = endOfExposure.InSeconds();
        }

        /// <summary>
        /// Gets or sets the dose.
        /// </summary>
        /// <value>
        /// The dose.
        /// </value>
        /// <exception cref="System.NotSupportedException">Specify a dose with a valid unit.</exception>
        public override Dose Dose
        {
            get => dose;
            set
            {
                switch (value.Unit)
                {
                    case DoseUnits.MgPerCubicMetre:
                        dose = value;
                        break;

                    default:
                        throw new NotSupportedException($"An inhalation exposure outcome cannot be specified by a dose with unit '{value.Unit}'.");
                }
            }
        }

        /// <summary>
        /// Gets the air concentration at the point in time to which this outcome is linked.
        /// </summary>
        public AirConcentration InstantaneousAirConcentration { get; set; }

        protected AirConcentration _meanAirConcentration;

        /// <summary>
        /// Sets the mean air concentration.  Also sets the amount of substance exposed to, by using the exposure duration and the inhalation rate.
        /// </summary>
        /// <param name="meanAirConcentration">The mean air concentration.</param>
        public void SetMeanAirConcentration(AirConcentration meanAirConcentration)
        {
            ApplyMeanAirConcentration(meanAirConcentration, _endOfExposureInSeconds - _startOfExposureInSeconds);
        }

        /// <summary>
        /// Sets the mean air concentration at the specified sample time. Also sets the amount of substance exposed to, by using the exposure duration and the inhalation rate.
        /// </summary>
        /// <param name="meanAirConcentration">The mean air concentration.</param>
        /// <param name="sampleTime">The sample time.</param>
        public void SetMeanAirConcentration(AirConcentration meanAirConcentration, Time sampleTime)
        {
            double? exposureDurationValue;

            if (sampleTime.InSeconds() < _startOfExposureInSeconds)
            {
                exposureDurationValue = 0;
            }
            else if (sampleTime.InSeconds() > _endOfExposureInSeconds)
            {
                exposureDurationValue = _endOfExposureInSeconds - _startOfExposureInSeconds;
            }
            else
            {
                exposureDurationValue = sampleTime.InSeconds() - _startOfExposureInSeconds;
            }

            ApplyMeanAirConcentration(meanAirConcentration, exposureDurationValue);
        }

        /// <summary>
        /// Sets the mean air concentration.
        /// </summary>
        /// <param name="meanAirConcentration">The mean air concentration.</param>
        /// <param name="exposureDurationInSeconds">The exposure duration in seconds.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        protected void ApplyMeanAirConcentration(AirConcentration meanAirConcentration, double? exposureDurationInSeconds)
        {
            this._meanAirConcentration = meanAirConcentration;

            double? value;

            if (_inhalationRateInCubicMetresPerSecond.HasValue)
            {
                value = meanAirConcentration.InMilligramPerCubicMetre() * _inhalationRateInCubicMetresPerSecond * exposureDurationInSeconds;
            }
            else
            {
                value = null;
            }

            Dose amount = new Dose(value, DoseUnits.Mg);

            dose = amount;
        }

        /// <summary>
        /// The dose unit that for the end point <see cref="AsMeanEventConcentration">'mean event concentration'</see>.
        /// </summary>
        public const DoseUnits MeanEventConcentrationDoseUnit = DoseUnits.MgPerCubicMetre;

        /// <summary>
        /// Gets the average air concentration for the time interval from start time of the simulation up to the point in time to which this outcome is linked.
        /// </summary>
        /// <remarks>
        /// Assume that this outcome instance has always been set by SetMeanAirConcentration.
        /// Therefore, there is no need to support converting back from amount or external event dose.
        /// </remarks>
        public virtual Dose AsMeanEventConcentration =>
            new Dose(_meanAirConcentration.InMilligramPerCubicMetre(), MeanEventConcentrationDoseUnit);

        /// <summary>
        /// The dose unit that for the end point <see cref="AsMeanDayConcentration">'Mean concentration on day of exposure'</see>.
        /// </summary>
        public const DoseUnits MeanDayConcentrationDoseUnit = DoseUnits.MgPerCubicMetre;

        /// <summary>
        /// Gets the average air concentration over the day (accounts for the number of events on one day).
        /// </summary>
        public Dose AsMeanDayConcentration
        {
            get
            {
                double? valueMeanDayConcentration = null;

                //Als [exposure duration] >                24
                //[MCDE] = [mean event concentration]

                //Als [exposure duration] x [exposure frequency per day] > 24
                // [MCDE] = [mean event concentration]

                //Als [exposure duration < 24] en [exposure frequency] <= 1 per day
                //[MCDE] =[mean event concentration] x [exposure duration] / 24

                //Als [exposure duration] x [exposure frequency per day] < 24 en [exposure frequency] > 1 per day:
                //[MCDE]=[exposure frequency per day] x [mean event concentration] x [exposure duration] / 24
                if (frequencyInTimesPerDay.HasValue)
                {
                    if (ExposureDurationInDays > 1 || ExposureDurationInDays * frequencyInTimesPerDay > 1)
                    {
                        valueMeanDayConcentration = _meanAirConcentration.InMilligramPerCubicMetre();
                    }
                    else
                    {
                        if (ExposureDurationInDays <= 1 && frequencyInTimesPerDay <= 1)
                        {
                            valueMeanDayConcentration = _meanAirConcentration.InMilligramPerCubicMetre() * ExposureDurationInDays;
                        }
                        else
                        {
                            valueMeanDayConcentration = frequencyInTimesPerDay * _meanAirConcentration.InMilligramPerCubicMetre() * ExposureDurationInDays;
                        }
                    }
                }

                return new Dose(valueMeanDayConcentration, MeanDayConcentrationDoseUnit);
            }
        }

        /// <summary>
        /// The dose unit that for the end point <see cref="AsMeanYearConcentration">'Mean daily air concentration averaged over a year'</see>.
        /// </summary>
        public const DoseUnits MeanYearConcentrationDoseUnit = DoseUnits.MgPerCubicMetre;

        /// <summary>
        /// Gets the mean daily air concentration averaged over a year.
        /// </summary>
        public virtual Dose AsMeanYearConcentration
        {
            get
            {
                double? valueMeanYearConcentration = null;

                // Als frequentie < 1/day , maar de exposure duration> 1 day
                // [YAC] = [MCDE] x [exposure duration in days] x [frequency]/365

                // Als [frequency per day] < 1, dan :
                // [YAC] = [MCDE] x [frequency per year]/365

                // Als [frequency per day] > 1, dan :
                // [YAC] = [MCDE]

                if (frequencyInTimesPerDay.HasValue)
                {
                    if (frequencyInTimesPerDay < 1)
                    {
                        if (ExposureDurationInDays > 1)
                        {
                            valueMeanYearConcentration = AsMeanDayConcentration.Value * ExposureDurationInDays * frequencyInTimesPerDay;
                        }
                        else
                        {
                            valueMeanYearConcentration = AsMeanDayConcentration.Value * frequencyInTimesPerDay;
                        }
                    }
                    else
                    {
                        valueMeanYearConcentration = AsMeanDayConcentration.Value;
                    }
                }

                return new Dose(valueMeanYearConcentration, MeanYearConcentrationDoseUnit);
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

                        //case DoseUnits.MgPerCubicMetre: should not be needed, as ApplyMeanAirConcentration will save in Mg.

                        default:
                            throw new NotSupportedException(
                                $"An inhalation exposure outcome with dose unit '{dose.Unit.ToString()}' cannot be converted to an external event dose.");
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
        /// The time duration of a 15 minute interval in seconds, i.e. 15 * 60 = 900 seconds.
        /// </summary>
        protected const double Twa15MinDurationInSeconds = 15 * ConversionFactors.SecondsPerMinute;

        /// <summary>
        /// The inhalation outcomes now have additional end points for the 15 min peak in air concentration. These are stored alongside the instantaneous and mean concentrations.
        /// </summary>
        protected AirConcentration peakAirConcentration;

        /// <summary>
        /// The peak external dose.
        /// </summary>
        protected Dose peakDose;

        /// <summary>
        /// The dose unit that for the end point <see cref="AsPeakAirConcentration">'peak air concentration'</see>.
        /// </summary>
        public const DoseUnits PeakAirConcentrationDoseUnit = DoseUnits.MgPerCubicMetre;

        /// <summary>
        /// Sets the peak air concentration around the maximum. Also sets the amount of substance exposed to, by using the exposure duration and the inhalation rate.
        /// </summary>
        /// <param name="peakAirConcentration">The peak air concentration.</param>
        public void SetPeakAirConcentration(AirConcentration peakAirConcentration)
        {
            this.peakAirConcentration = peakAirConcentration;

            double? value;

            if (peakAirConcentration != null && peakAirConcentration.HasValue && _inhalationRateInCubicMetresPerSecond.HasValue)
            {
                value = peakAirConcentration.InMilligramPerCubicMetre() * _inhalationRateInCubicMetresPerSecond * Twa15MinDurationInSeconds;
            }
            else
            {
                value = null;
            }

            Dose amount = new Dose(value, DoseUnits.Mg);

            peakDose = amount;
        }

        public AirConcentration MeanAirConcentrationPeak { get; set; }
        
        /// <summary>
        /// Gets the average air concentration during the 15 minutes of the highest air concentration during the simulation.
        /// </summary>
        public Dose AsPeakAirConcentration
        {
            get
            {
                double? peakAirConcentrationValue;
                if (peakAirConcentration != null && peakAirConcentration.HasValue)
                {
                    // Assume that this outcome instance has always been set by SetPeakAirConcentration.
                    // Therefore, there is no need to support converting back from amount or external event dose.
                    peakAirConcentrationValue = peakAirConcentration.InMilligramPerCubicMetre();
                }
                else
                {
                    peakAirConcentrationValue = null;
                }

                return new Dose(peakAirConcentrationValue, PeakAirConcentrationDoseUnit);
            }
        }
    }
}