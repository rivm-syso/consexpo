using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace RIVM.ConsExpo.Model.Submodels
{
    /// <summary>
    /// Base class for all inhalation exposure submodels. Provides basic functionality which is used by the submodels.
    /// </summary>
    internal abstract class InhalationExposureBase : ExposureBase
    {
        private const string MessageFormatTemplate = "'{{0}}' is required for inhalation exposure submodel '{0}'.";

        // Indication whether or not the solution is analytic.
        private readonly bool _analytic;

        private readonly string _messageFormat;

        protected readonly InhalationExposureModel route;

        protected InhalationExposureBase(ScenarioModel scenario, InhalationExposureSubmodelTypes type, bool analytic)
            : base(scenario)
        {
            _messageFormat = string.Format(MessageFormatTemplate, EnumHelper2<InhalationExposureSubmodelTypes>.GetDisplayValue(type));
            this.route = scenario.InhalationExposure;
            _analytic = analytic;
        }

        protected void RequireExposureDuration(IList<ValidationResult> validationResults)
        {
            if (!route.ExposureDuration.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ExposureDuration));
            }
        }

        protected void RequireProductAmount(IList<ValidationResult> validationResults)
        {
            if (!route.ProductAmount.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ProductAmount));
            }
        }

        protected void RequireProductSurfaceArea(IList<ValidationResult> validationResults)
        {
            if (!route.ProductSurfaceArea.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ProductSurfaceArea));
            }
        }

        protected void RequireWeightFractionSubstance(IList<ValidationResult> validationResults)
        {
            if (!route.WeightFractionSubstance.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.WeightFractionSubstance));
            }
        }

        protected void RequireWeightFractionSubstanceForEmission(IList<ValidationResult> validationResults)
        {
            if (!route.WeightFractionSubstanceForEmission.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.WeightFractionSubstanceForEmission));
            }
        }

        protected void RequireRoomVolume(IList<ValidationResult> validationResults)
        {
            if (!route.RoomVolume.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.RoomVolume));
            }
        }

        protected void RequireVentilationRate(IList<ValidationResult> validationResults)
        {
            if (!route.VentilationRate.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.VentilationRate));
            }
        }

        protected void RequireInhalationRate(IList<ValidationResult> validationResults)
        {
            if (!route.InhalationRate.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.InhalationRate));
            }
        }

        protected void RequireVapourPressure(IList<ValidationResult> validationResults)
        {
            if (!route.VapourPressure.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.VapourPressure));
            }
        }

        protected void RequireReleaseArea(IList<ValidationResult> validationResults)
        {
            if (!route.ReleaseArea.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ReleaseArea));
            }
        }

        protected void RequireApplicationDuration(IList<ValidationResult> validationResults)
        {
            if (!route.ApplicationDuration.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ApplicationDuration));
            }
        }

        protected void RequireApplicationTemperature(IList<ValidationResult> validationResults)
        {
            if (!route.ApplicationTemperature.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ApplicationTemperature));
            }
        }

        protected void RequireMolecularWeight(IList<ValidationResult> validationResults, SubstanceModel substance)
        {
            if (!(substance?.MolecularWeight?.HasValue ?? false))
            {
                validationResults.Add(new ValidationResult(string.Format(_messageFormat, ModelHelpers.GetDisplayName<SubstanceModel>(p => p.MolecularWeight))));
            }
        }

        protected void RequireMolecularWeightMatrix(IList<ValidationResult> validationResults)
        {
            if (!route.MolecularWeightMatrix.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.MolecularWeightMatrix));
            }
        }

        protected void RequireDilution(IList<ValidationResult> validationResults)
        {
            if (!route.Dilution.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.Dilution));
            }
        }

        protected void RequireEmissionDuration(IList<ValidationResult> validationResults)
        {
            if (!route.EmissionDuration.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.EmissionDuration));
            }
        }

        protected void RequireEmissionDurationReEntry(IList<ValidationResult> validationResults)
        {
            if (!route.EmissionDurationReEntry.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.EmissionDurationReEntry));
            }
        }

        protected void RequireDailyDuration(IList<ValidationResult> validationResults)
        {
            if (!route.DailyDuration.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.DailyDuration));
            }
        }

        protected void RequireEmissionDurationEvaporation(IList<ValidationResult> validationResults)
        {
            if (!route.EmissionDurationEvaporation.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.EmissionDurationEvaporation));
            }
        }

        protected void RequireReleasedMass(IList<ValidationResult> validationResults)
        {
            if (!route.ReleasedMass.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ReleasedMass));
            }
        }

        protected void RequireSprayDuration(IList<ValidationResult> validationResults)
        {
            if (!route.SprayDuration.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.SprayDuration));
            }
        }

        protected void RequireRoomHeight(IList<ValidationResult> validationResults)
        {
            if (!route.RoomHeight.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.RoomHeight));
            }
        }

        protected void RequireCloudVolume(IList<ValidationResult> validationResults)
        {
            if (!route.CloudVolume.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.CloudVolume));
            }
        }

        protected void RequireMassGenerationRate(IList<ValidationResult> validationResults)
        {
            if (!route.MassGenerationRate.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.MassGenerationRate));
            }
        }

        protected void RequireAirborneFraction(IList<ValidationResult> validationResults)
        {
            if (!route.AirborneFraction.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.AirborneFraction));
            }
        }

        protected void RequireDensityNonVolatile(IList<ValidationResult> validationResults)
        {
            if (!route.DensityNonVolatile.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.DensityNonVolatile));
            }
        }

        protected void RequireInhalationCutOffDiameter(IList<ValidationResult> validationResults)
        {
            if (!route.InhalationCutOffDiameter.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.InhalationCutOffDiameter));
            }
        }

        protected void RequireMedianDiameter(IList<ValidationResult> validationResults)
        {
            if (!route.MedianDiameter.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.MedianDiameter));
            }
        }

        protected void RequireArithmicCoefficientOfVariation(IList<ValidationResult> validationResults)
        {
            if (!route.ArithmicCoefficientOfVariation.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ArithmicCoefficientOfVariation));
            }
        }

        protected void RequireMaximumDiameter(IList<ValidationResult> validationResults)
        {
            if (!route.MaximumDiameter.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.MaximumDiameter));
            }
        }

        protected void RequireMeanDiameter(IList<ValidationResult> validationResults)
        {
            if (!route.MeanDiameter.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.MeanDiameter));
            }
        }

        protected void RequireStandardDeviation(IList<ValidationResult> validationResults)
        {
            if (!route.StandardDeviation.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.StandardDeviation));
            }
        }

        protected void RequireNonParametricSizeDistribution(IList<ValidationResult> validationResults)
        {
            if (!route.NonParametricSizeDistributionId.HasValue)
            {
                validationResults.Add((GetValidationMessage((p => p.NonParametricSizeDistributionId))));
            }
        }

        protected void RequireStartExposure(IList<ValidationResult> validationResults)
        {
            if (!route.StartExposure.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.StartExposure));
            }
        }

        protected void RequireExposureDurationForEmissionModel(IList<ValidationResult> validationResults)
        {
            if (!route.ExposureDurationForEmissionModel.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ExposureDurationForEmissionModel));
            }
        }

        protected void RequireMassTransferCoefficient(IList<ValidationResult> validationResults)
        {
            if (!route.MassTransferCoefficient.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.MassTransferCoefficient));
            }
        }

        protected void RequireProductAirPartitionCoefficient(IList<ValidationResult> validationResults)
        {
            if (!route.ProductAirPartitionCoefficient.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ProductAirPartitionCoefficient));
            }
        }

        protected void RequireDiffusionCoefficient(IList<ValidationResult> validationResults)
        {
            if (!route.DiffusionCoefficientForEmission.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.DiffusionCoefficientForEmission));
            }
        }

        protected void RequireProductDensity(IList<ValidationResult> validationResults)
        {
            if (!route.ProductDensity.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ProductDensity));
            }
        }

        protected void RequireProductThickness(IList<ValidationResult> validationResults)
        {
            if (!route.ProductThickness.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ProductThickness));
            }
        }

        /// <summary>
        /// Gets the validation message. Wraps the logic needed to select the property display name.
        /// </summary>
        /// <param name="modelProperty">The model property.</param>
        /// <returns></returns>
        protected ValidationResult GetValidationMessage(Expression<Func<InhalationExposureModel, object>> modelProperty)
        {
            return new ValidationResult(string.Format(_messageFormat, ModelHelpers.GetDisplayName<InhalationExposureModel>(modelProperty)));
        }

        public abstract bool SupportsPeakAirConcentration { get; }

        public virtual bool SupportsMeanDayConcentration => true;

        public virtual bool SupportsExternalDayDose => true;

        public virtual bool SupportsInternalDayDose => true;

        public virtual List<DoseMeasureType> EndPointsForSensitivityAnalysis()
        {
#warning check if re-entry requires modifications.

#warning To Do: check relevant parameters values to see if the end point is really available.
            var endPoints = new List<DoseMeasureType>
            {
                DoseMeasureType.MeanEventConcentration,
                DoseMeasureType.ExternalEventDose
            };

            if (SupportsPeakAirConcentration)
            {
                endPoints.Add(DoseMeasureType.PeakAirConcentration);
            }

            return endPoints;
        }

        public DistributedInhalationExposureEndPoints DistributedEndPoints
        {
            get
            {
                var modelIsDistributed = ModelIsDistributed;

                var endPoints = new DistributedInhalationExposureEndPoints
                    {
                        MeanEventConcentrationIsDistributed = modelIsDistributed,
                        MeanDayConcentrationIsDistributed = modelIsDistributed || scenario.Frequency.IsDistributed,
                        MeanYearConcentrationIsDistributed = modelIsDistributed || scenario.Frequency.IsDistributed,
                        PeakConcentrationIsDistributed = modelIsDistributed,
                        PeakExternaldoseIsDistributed = modelIsDistributed || route.InhalationRate.IsDistributed || scenario.Assessment.Population.BodyWeight.IsDistributed,
                        ExternalEventDoseIsDistributed = modelIsDistributed || route.InhalationRate.IsDistributed || scenario.Assessment.Population.BodyWeight.IsDistributed
                    };

                endPoints.ExternalDayDoseIsDistributed = endPoints.ExternalEventDoseIsDistributed || scenario.Frequency.IsDistributed;

                endPoints.ExposureFractionIsDistributed = modelIsDistributed;

                return endPoints;
            }
        }

        public abstract bool ModelIsDistributed { get; }

        /// <summary>
        /// Gets an instance of time with the correct duration for a TWA 15 minutes interval.
        /// </summary>
        /// <value>
        /// The twa15 time interval.
        /// </value>
        protected static Time Twa15TimeInterval => new Time(15, TimeUnits.Minute);

        public abstract TimeInterval PeakInterval(Time time);

        public virtual TimeInterval PeakInterval() => PeakInterval(Twa15TimeInterval);

        /// <summary>
        /// Calculated the peak air concentration.
        /// </summary>
        /// <returns>The average air concentration over the peak interval.</returns>
        protected double PeakAirConcentration(TimeInterval peakInterval, double meanAirConcentrationAtStart, double meanAirConcentrationAtEnd)
        {
            double peakAirConcentration;
            if (peakInterval.DurationInSeconds <= 0)
            {
                // The peak interval is 0 (start and end are at the same discrete sample point of the numerical solution).
                // The average over the interval is simply the value at this point.
                peakAirConcentration = meanAirConcentrationAtStart;
            }
            else
            {
                // The average is (C2 * t2 - C1 * t1)  / (t2 - t1).
                var integratedAirConcentrationAtPeakStart = meanAirConcentrationAtStart * peakInterval.StartTime.InSeconds();
                var integratedAirConcentrationAtPeakEnd = meanAirConcentrationAtEnd * peakInterval.EndTime.InSeconds();

                peakAirConcentration = (integratedAirConcentrationAtPeakEnd - integratedAirConcentrationAtPeakStart) / peakInterval.DurationInSeconds;
            }

            return peakAirConcentration;
        }

        /// <summary>
        /// The amount of substance (in mg) released: [Product amount] x [weight fraction]
        /// </summary>
        protected double AmountOfSubstanceByProductAmount
        {
            get
            {
                double productAmount = scenario.InhalationExposure.ProductAmount.InMilligram();
                double weightFractionSubstance = scenario.InhalationExposure.WeightFractionSubstance.AsFraction();

                return productAmount * weightFractionSubstance;
            }
        }

        public abstract AirConcentration MeanAirConcentration(Time time);

        /// <summary>
        /// In this model the air concentration is at its maximum at t=0. The peak interval is therefore easily determined.
        /// </summary>
        /// <returns></returns>
        public virtual AirConcentration PeakAirConcentration()
        {
            return PeakAirConcentration(Twa15TimeInterval);
        }

        public virtual AirConcentration PeakAirConcentration(Time time)
        {
            if (!_analytic)
                throw new NotImplementedException("Cannot determine the peak concentration, because the submodel '{nameof(this)}' is not deterministic and therefore cannot use the default implementation for determining the peak concentration. This submodel must override the default implementation.");

            TimeInterval peakInterval = this.PeakInterval(time);

            var integratedAirConcentrationAtPeakStart = this.MeanAirConcentration(peakInterval.StartTime).Value.Value * peakInterval.StartTime.InSeconds();

            var integratedAirConcentrationAtPeakEnd = this.MeanAirConcentration(peakInterval.EndTime).Value.Value * peakInterval.EndTime.InSeconds();

            double? peakAirConcentration = (integratedAirConcentrationAtPeakEnd - integratedAirConcentrationAtPeakStart) / peakInterval.DurationInSeconds;

            return new AirConcentration
            {
                Value = peakAirConcentration,
                Unit = DensityUnits.MilligramPerCubicMetre
            };
        }

        /// <summary>
        /// Finds an interval using the overall solution, that is wide enough to contain the peak interval.
        /// </summary>
        /// <param name="solution">The solution.</param>
        /// <param name="timeMax">The maximum time and which the peak interval must end.</param>
        /// <param name="peakInterval">The required length of the interval around the peak value.</param>
        /// <param name="numberOfTimeSteps">The total number of time steps in the provided solution.</param>
        /// <param name="indexOfValueToMaximize">The index in the solution of the value to maximize.</param>
        /// <returns></returns>
        public static TimeInterval InitialBracket(double[,] solution, Time timeMax, Time peakInterval, double numberOfTimeSteps,
            int indexOfValueToMaximize)
        {
            int indexOfMaxValue = -1;

            // Find the maximum value.
            double maxValueFound = double.MinValue;

            for (int step = 0; step <= numberOfTimeSteps; step++)
            {
                double value = solution[step, indexOfValueToMaximize];
                if (value > maxValueFound)
                {
                    maxValueFound = value;
                    indexOfMaxValue = step;
                }
            }

            /*

            Below are time lines.

            The requested peak interval length: {----------}

            Sampled point in the discrete solution: +

            [+    +    +    +    +    M    +    +    +    +    +]   M: the solution value with the highest value.

            [                    <--------->                    ]   The interval the actual max is in. In can be anywhere between the sampled point left of M and the sampled point right of M.

            [        {----------}<--------->{----------}        ]   The initial bracket consists of the requested peak interval length + the interval the actual max +  requested peak interval length.

            [        [---------------------------------]        ]   The resulting initial bracket.

             */

            var indexOfStepBeforeMaxValue = indexOfMaxValue <= 0 ? indexOfMaxValue : indexOfMaxValue - 1;
            var indexOfStepAfterMaxValue = indexOfMaxValue >= numberOfTimeSteps ? indexOfMaxValue : indexOfMaxValue + 1;

            var startIntervalValue = Math.Max(0, timeMax.InSeconds() * indexOfStepBeforeMaxValue / numberOfTimeSteps - peakInterval.InSeconds());

            var endIntervalValue = Math.Min(timeMax.InSeconds(), timeMax.InSeconds() * indexOfStepAfterMaxValue / numberOfTimeSteps + peakInterval.InSeconds());

            return new TimeInterval(new Time { Value = startIntervalValue, Unit = TimeUnits.Second }, new Time { Value = endIntervalValue, Unit = TimeUnits.Second });
        }

        protected static bool FindPeakInterval(Time intervalLength, int numberOfTimeStepsInInitialBracket,
            TimeInterval initialBracketTimeInterval, double[,] helperSolution, int indexOfTimeSeries,
            int indexOfInstantAirConcentrationSeries, TimeUnits timeUnit, out TimeInterval peakInterval)
        {
            // The solution is a (two-dimensional) array. This number is to be added to find the solution <intervalLength> minutes later.
            // We use Floor() to find the sampled point before the intervalLength is passed (indexBeforePeakIntervalEnd). Later, we also look at the next point (indexAfterPeakIntervalEnd).
            int indexOffsetPeakIntervalWidth = (int)Math.Floor(numberOfTimeStepsInInitialBracket
                                                               * intervalLength.InMinutes()
                                                               / initialBracketTimeInterval.EndTime.InMinutes());

            int indexAtPeakIntervalStart;

            if (initialBracketTimeInterval.StartTime.InMinutes() <= 0)
            {
                indexAtPeakIntervalStart = 0;
            }
            else
            {
                indexAtPeakIntervalStart = (int)Math.Floor(numberOfTimeStepsInInitialBracket
                                                           * initialBracketTimeInterval.StartTime.InMinutes()
                                                           / initialBracketTimeInterval.EndTime.InMinutes());
            }

            for (int timeIndex = indexAtPeakIntervalStart;
                 timeIndex <= numberOfTimeStepsInInitialBracket;
                 timeIndex++)
            {
                // Loop through all solution points, start at the bracket interval start, to find solution values that are (almost) equal in y-value and (almost) <intervalLength> apart.
                var solutionAtPeakIntervalStart = helperSolution[timeIndex, indexOfInstantAirConcentrationSeries];
                int indexBeforePeakIntervalEnd = timeIndex + indexOffsetPeakIntervalWidth;
                int indexAfterPeakIntervalEnd = indexBeforePeakIntervalEnd + 1;

                if (indexAfterPeakIntervalEnd >= numberOfTimeStepsInInitialBracket)
                {
                    // Passed the end of the initial bracket, without finding points that are (almost) equal in y-value and (almost) <intervalLength> apart.
                    // Assume the last <intervalLength> must be the peak interval.
                    // Sometimes, the initial solution has a peak, but the helper solution does not have that peak in the bracketed interval. Assume that this is due to numerical noise and the differences in air concentration are so small, we can safely use the end of the bracketed interval.
                    Time timeAtPeakIntervalStart = new Time(initialBracketTimeInterval.EndTime.InMinutes() - intervalLength.InMinutes(), TimeUnits.Minute);
                    {
                        peakInterval = new TimeInterval(timeAtPeakIntervalStart, initialBracketTimeInterval.EndTime);
                        return true;
                    }
                }

                var solutionBeforePeakIntervalEnd = helperSolution[indexBeforePeakIntervalEnd, indexOfInstantAirConcentrationSeries];
                var solutionAfterPeakIntervalEnd = helperSolution[indexAfterPeakIntervalEnd, indexOfInstantAirConcentrationSeries];

                if (solutionAfterPeakIntervalEnd < solutionAtPeakIntervalStart)
                {
                    // The increasing slope has now a higher y-value than the decreasing slope.
                    // We have found the correct interval.

                    int indexAtPeakIntervalStartToUse;
                    var prevSolutionAtPeakIntervalStart = helperSolution[timeIndex - 1, indexOfInstantAirConcentrationSeries];

                    if (solutionBeforePeakIntervalEnd > (prevSolutionAtPeakIntervalStart + solutionAtPeakIntervalStart) / 2)
                    {
                        // The y-value at the end of the interval is closer to the higher y-value at the start of the interval.
                        indexAtPeakIntervalStartToUse = timeIndex;
                    }
                    else
                    {
                        // The y-value at the end of the interval is closer to the lower y-value at the start of the interval.
                        indexAtPeakIntervalStartToUse = timeIndex - 1;
                    }

                    double startTimeTime = helperSolution[indexAtPeakIntervalStartToUse, indexOfTimeSeries];
#if DEBUG
                    // Useful for validating that the peak interval is almost of the correct length.
                    var intervalLengthFromSolution = helperSolution[indexBeforePeakIntervalEnd, indexOfTimeSeries] - startTimeTime;
#endif
                    double endTimeTime = helperSolution[indexBeforePeakIntervalEnd, indexOfTimeSeries];
                    {
                        peakInterval = new TimeInterval(startTimeTime, endTimeTime, timeUnit);
                        return true;
                    }
                }
            }

            peakInterval = null;
            return false;
        }

        public virtual AirConcentration MeanAirConcentrationPeak() => null;
    }
}