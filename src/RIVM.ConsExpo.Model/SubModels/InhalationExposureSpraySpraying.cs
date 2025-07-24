using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Computations;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RIVM.ConsExpo.Model.Submodels
{
    /// <summary>
    /// In this model, all of the substance is released as spray at once, subsequently removed by ventilation.
    /// </summary>
    internal class InhalationExposureSpraySpraying : InhalationExposureBase, IInhalationExposureSubmodel
    {
        private const InhalationExposureSubmodelTypes type = InhalationExposureSubmodelTypes.SpraySpraying;

        public InhalationExposureSubmodelTypes Type => type;

        private readonly ExposureSpraySprayingComputations _exposureSpraySprayingComputations;

        public InhalationExposureSpraySpraying(ScenarioModel scenario)
            : base(scenario, type, true)
        {
            _exposureSpraySprayingComputations = new ExposureSpraySprayingComputations(scenario, true);
        }

        public override Duration ApplicableExposureDuration => scenario.InhalationExposure.ExposureDuration;

        public override Time DefaultTimeMax => ApplicableExposureDuration.AsTime();

        public override void PrepareTimeSeries(Time timeMax)
        {
            _exposureSpraySprayingComputations.PrepareSolution(timeMax);
        }

        /// <summary>
        /// The amount of substance released (in mg): [Spray duration] x [Mass generation rate] x [weight fraction]
        /// </summary>
        public override double? AmountOfSubstance
        {
            get
            {
                double sprayDuration = scenario.InhalationExposure.SprayDuration.InMinutes();
                double massGenerationRate = scenario.InhalationExposure.MassGenerationRate.InMilligramPerMinute();
                double weightFractionSubstance = scenario.InhalationExposure.WeightFractionSubstance.AsFraction();

                return sprayDuration * massGenerationRate * weightFractionSubstance;
            }
        }

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.InhalationExposureSprayDuration,
                DTO.Models.ModelParameters.InhalationExposureExposureDuration,
                DTO.Models.ModelParameters.InhalationExposureWeightFractionSubstance,
                DTO.Models.ModelParameters.InhalationExposureRoomVolume,
                DTO.Models.ModelParameters.InhalationExposureRoomHeight,
                DTO.Models.ModelParameters.InhalationExposureVentilationRate
            };
            if (route.SprayingTowardsPerson)
            {
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureCloudVolume);
            }
            modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureMassGenerationRate);
            modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureAirborneFraction);
            modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureDensityNonVolatile);
            modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureInhalationCutOffDiameter);

            switch (route.AerosolDiameterDistributionType)
            {
                case SizeDistributionTypes.LogNormal:
                    modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureMedianDiameter);
#warning ToDo: this is not a physical quantity. Should we make it one (e.g. Dimensionless)?
                    //modelParameters.Add(DTO.Models.ModelParameters.ArithmicCoefficientOfVariation);
                    modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureMaximumDiameter);
                    break;

                case SizeDistributionTypes.Normal:
                    modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureMeanDiameter);
                    modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureStandardDeviation);
                    modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureMaximumDiameter);
                    break;

                case SizeDistributionTypes.NonParametric:
                    // None. The selected non-parametric size distribution is not a true model parameter. It is just a reference Id.
                    break;

                default:
                    throw new NotSupportedException(string.Format("Unsupported distribution type '{0}'", scenario.InhalationExposure.AerosolDiameterDistributionType.ToString()));
            }
            return modelParameters;
        }

        public override bool IsTimeDependent => true;

        public override bool SupportsPeakAirConcentration => true;

        public IEnumerable<ValidationResult> Validate()
        {
            IList<ValidationResult> validationResults = ValidateDurationAndFrequency(base.route.ExposureDuration, scenario.Frequency);

            RequireSprayDuration(validationResults);
            RequireExposureDuration(validationResults);
            RequireWeightFractionSubstance(validationResults);
            RequireRoomVolume(validationResults);
            RequireRoomHeight(validationResults);
            RequireVentilationRate(validationResults);

            if (route.SprayingTowardsPerson)
            {
                RequireCloudVolume(validationResults);
            }

            RequireMassGenerationRate(validationResults);
            RequireAirborneFraction(validationResults);

            RequireDensityNonVolatile(validationResults);
            RequireInhalationCutOffDiameter(validationResults);

            switch (route.AerosolDiameterDistributionType)
            {
                case 0: //not specified in UI
                    validationResults.Add(GetValidationMessage(p => p.AerosolDiameterDistributionType));
                    break;

                case SizeDistributionTypes.LogNormal:
                    RequireMedianDiameter(validationResults);
                    RequireArithmicCoefficientOfVariation(validationResults);
                    RequireMaximumDiameter(validationResults);
                    break;

                case SizeDistributionTypes.Normal:
                    RequireMeanDiameter(validationResults);
                    RequireStandardDeviation(validationResults);
                    RequireMaximumDiameter(validationResults);
                    break;

                case SizeDistributionTypes.NonParametric:
                    RequireNonParametricSizeDistribution(validationResults);
                    break;

                default:
                    throw new NotSupportedException(string.Format("Unsupported distribution type '{0}'", scenario.InhalationExposure.AerosolDiameterDistributionType.ToString()));
            }

            if (route.InhalationCutOffDiameter.HasValue && route.MaximumDiameter.HasValue && route.InhalationCutOffDiameter.InMicrometre() > route.MaximumDiameter.InMicroMetre())
            {
                validationResults.Add(new ValidationResult(string.Format("The {0} must not be larger than the {1}.", ModelHelpers.GetDisplayName<InhalationExposureModel>(r => r.InhalationCutOffDiameter), ModelHelpers.GetDisplayName<InhalationExposureModel>(r => r.MaximumDiameter))));
            }

            return validationResults;
        }

        public AirConcentration InstantaneousAirConcentration(Time time)
        {
            return _exposureSpraySprayingComputations.InstantaneousAirConcentration(time);
        }

        public override AirConcentration MeanAirConcentration(Time time)
        {
            if (time.InSeconds() == 0)
            {
                return _exposureSpraySprayingComputations.InstantaneousAirConcentration(time);
            }
            else
            {
                return _exposureSpraySprayingComputations.MeanAirConcentration(time);
            }
        }

        public AirConcentration MeanAirConcentration()
        {
            if (scenario.InhalationExposure.InhalationCutOffDiameter.InMicrometre() <= 0.0)
            {
                return new AirConcentration() { Value = 0.0, Unit = DensityUnits.MilligramPerCubicMetre };
            }
            else
            {
                return _exposureSpraySprayingComputations.MeanAirConcentration();
            }
        }

        public override TimeInterval PeakInterval(Time time)
        {
            TimeInterval peakInterval;

            if (route.ExposureDuration.AsTime() <= time)
            {
                peakInterval = new TimeInterval(0, route.ExposureDuration.InSeconds(), TimeUnits.Second);
            }
            else if (route.SprayDuration.AsTime() >= route.ExposureDuration.AsTime())
            {
                peakInterval = new TimeInterval(route.ExposureDuration.InSeconds() - time.InSeconds(), route.ExposureDuration.InSeconds(), TimeUnits.Second);
            }
            else
            {
                if (scenario.InhalationExposure.SprayingTowardsPerson)
                {
                    // CloudVolume is presented as a volume, but is actually a volume rate: a cloud with a volume increasing at the specifed number of cubic metres per second!
                    if (route.SprayDuration.InSeconds() * route.CloudVolume.InCubicMetres() < route.RoomVolume.InCubicMetres())
                    {
                        peakInterval = new TimeInterval(0, time.InSeconds(), TimeUnits.Second);
                    }
                    else
                    {
                        peakInterval = FindPeakIntervalNumerically(time);
                    }
                }
                else
                {
                    peakInterval = FindPeakIntervalNumerically(time);
                }
            }

            return peakInterval;
        }

        private TimeInterval FindPeakIntervalNumerically(Time time)
        {
            double initialBracketStartTimeMin = Math.Max(0, route.SprayDuration.InSeconds() - time.InSeconds());

            double initialBracketStartTimeMax = route.SprayDuration.InSeconds();

            return BisectStartTimeOfPeakInterval(initialBracketStartTimeMin, initialBracketStartTimeMax);
        }

        /// <summary>
        /// Find the switch of sign for A(t) - A(t+15').
        /// </summary>
        /// <param name="time1">The min time in seconds.</param>
        /// <param name="time2">The max time in seconds.</param>
        /// <returns></returns>
        private TimeInterval BisectStartTimeOfPeakInterval(double time1, double time2)
        {
            const double AllowedTolerance = 0.1; // (second)
            const int MaxIterations = 20;

            double startTime = NumericalRecipes.Bisect(DeltaAirConcentration, time1, time2, MaxIterations, AllowedTolerance);

            // The end time must not be later than the end of exposure.
            double endTime = Math.Min(startTime + Twa15TimeInterval.InSeconds(), route.ExposureDuration.InSeconds());

            // The start time must be 15 minutes before the end time, with a minimum of 0.
            startTime = Math.Max(0, Math.Min(startTime, endTime - Twa15TimeInterval.InSeconds()));

            return new TimeInterval(startTime, endTime, TimeUnits.Second);
        }

        /// <summary>
        /// Helper function that calculated the air concentration at t minus the air concentration at t+15 minutes. If we find t with f(t) = 0, we have found the peak interval.
        /// </summary>
        /// <param name="startTimeInSeconds">The start time in seconds.</param>
        /// <returns></returns>
        private double DeltaAirConcentration(double startTimeInSeconds)
        {
            Time startTime = new Time(startTimeInSeconds, TimeUnits.Second);
            return InstantaneousAirConcentration(startTime).InMilligramPerCubicMetre() - InstantaneousAirConcentration(startTime + Twa15TimeInterval).InMilligramPerCubicMetre();
        }

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        public override bool ModelIsDistributed => _exposureSpraySprayingComputations.ModelIsDistributed;
    }
}