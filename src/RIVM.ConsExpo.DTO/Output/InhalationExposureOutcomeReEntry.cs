using RIVM.ConsExpo.DTO.PhysicalQuantities;

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// Explanation....
    /// </summary>
    /// <remarks>
    /// Store actual values, not references to PhysicalQuantities, as they will be resampled in a Monte Carlo simulation.
    /// The outcomes are evaluated after the MC simulation completes, which resulted in all outcomes using the last sampled values.
    /// This was caused by <see href="https://rivm.atlassian.net/browse/CONSEXPO-158">CONSEXPO-158</see>.
    /// </remarks>
    public class InhalationExposureOutcomeReEntry : InhalationExposureOutcome
    {
        private readonly double? durationOfExposureInSecondsPerDay;
        private readonly double? durationOfExposureAsFraction;
        private readonly double? emissionDurationInYears;
        private readonly double? frequencyInTimesPerYear;

        public InhalationExposureOutcomeReEntry(
            BodyWeight bodyWeight,
            Frequency scenarioFrequency,
            DailyDuration durationOfExposure,
            EmissionDurationReEntry emissionDuration,
            double? amountOfSubstance,
            VolumeRate inhalationRate,
            Time startOfExposure,
            Time endOfExposure) : base(bodyWeight, scenarioFrequency, amountOfSubstance, inhalationRate, startOfExposure, endOfExposure)
        {
            durationOfExposureInSecondsPerDay = durationOfExposure.InSecondsPerDay();
            durationOfExposureAsFraction = durationOfExposure.AsFraction();
            emissionDurationInYears = emissionDuration.InYears();
            frequencyInTimesPerYear = scenarioFrequency?.InTimesPerYearIfSpecified();
        }

        public override Dose AsMeanEventConcentration => new Dose(MeanAirConcentrationPeak.InMilligramPerCubicMetre(), DoseUnits.MgPerCubicMetre);

        public override Dose AsExternalEventDose
        {
            get
            {
                var value = MeanAirConcentrationPeak.Value * _inhalationRateInCubicMetresPerSecond * durationOfExposureInSecondsPerDay / bodyWeightInKilogram;

                return new Dose(value, DoseUnits.MgPerKgBodyWeight);
            }
        }

        public override Dose AsMeanYearConcentration
        {
            get
            {
                var value = frequencyInTimesPerYear * _meanAirConcentration.Value * emissionDurationInYears;
                return new Dose(value, DoseUnits.MgPerCubicMetre);
            }
        }

        public override Dose AsExposureFraction
        {
            get
            {
                var value = base.AsExposureFraction.Value * durationOfExposureAsFraction;
                return new Dose(value, ExposureFractionDoseUnit);
            }
        }
    }
}