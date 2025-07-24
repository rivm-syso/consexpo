using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RIVM.ConsExpo.Model.Submodels
{
    internal abstract class ExposureBase
    {
        protected const double FloatingPointZero = 1e-12;

        protected ScenarioModel scenario;

        protected ExposureBase(ScenarioModel scenario)
        {
            this.scenario = scenario;
        }

        public abstract bool IsTimeDependent { get; }

        public abstract Duration ApplicableExposureDuration { get; }

        /// <summary>
        /// Gets the default time maximum for charts.
        /// </summary>
        /// <value>
        /// The default time maximum.
        /// </value>
        public virtual Time DefaultTimeMax
        {
            get
            {
                if (IsTimeDependent)
                {
                    if (ApplicableExposureDuration == null)
                    {
                        throw new NotImplementedException($"The submodel '{this.GetType()}' should implement its own version of 'DefaultTimeMax'.");
                    }
                    return ApplicableExposureDuration.AsTime();
                }

                throw new NotImplementedException($"The submodel '{GetType()}' is not time-dependent, so no default maximum time can be calculated.");
            }
        }

        public virtual Time StartTimeOfExposure => new Time(0, TimeUnits.Second);

        public virtual Time EndTimeOfExposure
        {
            get
            {
                double? value;

                if (this.IsTimeDependent)
                {
                    value = DefaultTimeMax?.InSeconds() ?? 0.0;
                }
                else
                {
                    value = 0.0;
                }

                return new Time(value, TimeUnits.Second);
            }
        }

        /// <remarks>No default implementation. This ensures that each model explicitly defines the calculation needed.</remarks>
        [Obsolete]
        public abstract double? AmountOfSubstance { get; }

        /// <summary>
        /// The amount of substance released: not supported; returns null.
        /// </summary>
        protected double? AmountOfSubstanceNotSupported => null;

        public IList<ValidationResult> ValidateDurationAndFrequency(Duration duration, Frequency frequency)
        {
            IList<ValidationResult> validationResults = new List<ValidationResult>();

            if ((duration?.Value.HasValue ?? false) && duration.InDays() > 0 && scenario.Frequency.Value.HasValue && scenario.Frequency.InTimesPerDay() > (1.0 / duration.InDays()))
            {
                validationResults.Add(new ValidationResult(
                    $"The combination of a duration of {duration.Value.Value} {duration.UnitDisplay} and a frequency of {scenario.Frequency.Value.Value} {scenario.Frequency.UnitDisplay} results in overlapping events, which is not supported."));
            }

            return validationResults;
        }

        public virtual void PrepareTimeSeries(Time maxTime)
        {
            //By default, assume no preparation is needed.
        }
    }
}