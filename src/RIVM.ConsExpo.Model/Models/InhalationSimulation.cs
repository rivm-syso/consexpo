using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.Settings;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Models;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using RIVM.ConsExpo.Model.Submodels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using RIVM.ConsExpo.DTO.Extensions;

namespace RIVM.ConsExpo.Model.Models
{
    /// <summary>
    /// A class that can start the inhalation simulation, exposure and absorption, as specified in the scenario.
    /// </summary>
    internal class InhalationSimulation : IInhalationSimulation
    {
        /// <summary>
        /// Start the inhalation simulation, exposure and absorption, if in use, as specified in the scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        public RouteOutcomes<InhalationExposureOutcome, InhalationAbsorptionOutcome> CalculatePointValues(ScenarioModel scenario)
        {
            var outputValues = new RouteOutcomes<InhalationExposureOutcome, InhalationAbsorptionOutcome>();

            if (scenario.InhalationExposureRouteInUse)
            {
                IInhalationExposureSubmodel inhalationExposureSimulation = ExposureSubmodel(scenario);

                var meanAirConcentration = inhalationExposureSimulation.MeanAirConcentration();
                var peakAirConcentration = inhalationExposureSimulation.SupportsPeakAirConcentration ? inhalationExposureSimulation.PeakAirConcentration() : null;
                InhalationExposureOutcome inhalationExposureOutcome;

                if (!scenario.InhalationExposure.ReEntry)
                {
                    inhalationExposureOutcome = new InhalationExposureOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, inhalationExposureSimulation.AmountOfSubstance, scenario.InhalationExposure.InhalationRate, inhalationExposureSimulation.StartTimeOfExposure, inhalationExposureSimulation.EndTimeOfExposure);
                }
                else
                {
                    inhalationExposureOutcome = new InhalationExposureOutcomeReEntry(scenario.Assessment.Population.BodyWeight, scenario.Frequency, scenario.InhalationExposure.DailyDuration, scenario.InhalationExposure.EmissionDurationReEntry, inhalationExposureSimulation.AmountOfSubstance, scenario.InhalationExposure.InhalationRate, inhalationExposureSimulation.StartTimeOfExposure, inhalationExposureSimulation.EndTimeOfExposure);
                    var meanAirConcentrationPeak = inhalationExposureSimulation.MeanAirConcentrationPeak();
                    inhalationExposureOutcome.MeanAirConcentrationPeak = meanAirConcentrationPeak;
                }

                inhalationExposureOutcome.SetMeanAirConcentration(meanAirConcentration, inhalationExposureSimulation.DefaultTimeMax);
                inhalationExposureOutcome.SetPeakAirConcentration(peakAirConcentration);

                outputValues.Exposure = inhalationExposureOutcome;

                if (scenario.InhalationAbsorptionRouteInUse)
                {
                    IInhalationAbsorptionSubmodel inhalationAbsorptionSimulation = new InhalationAbsorptionFraction(scenario);
                    outputValues.Absorption = inhalationAbsorptionSimulation.CalculatePointValues(inhalationExposureOutcome);
                }
            }

            return outputValues;
        }

        public Duration ApplicableExposureDuration(ScenarioModel scenario)
        {
            return ExposureSubmodel(scenario).ApplicableExposureDuration;
        }

        private static IInhalationExposureSubmodel ExposureSubmodel(ScenarioModel scenario)
        {
            switch (scenario.InhalationExposure.SubmodelType)
            {
                case InhalationExposureSubmodelTypes.VapourInstantaneousRelease:
                    return new InhalationExposureVapourInstantaniousRelease(scenario);

                case InhalationExposureSubmodelTypes.VapourConstantRate:
                    return new InhalationExposureVapourConstantRate(scenario);

                case InhalationExposureSubmodelTypes.VapourEvaporation:
                    return new InhalationExposureVapourEvaporation(scenario);

                case InhalationExposureSubmodelTypes.SprayInstantaneousRelease:
                    return new InhalationExposureSprayInstantaniousRelease(scenario);

                case InhalationExposureSubmodelTypes.SpraySpraying:
                    return new InhalationExposureSpraySpraying(scenario);

                case InhalationExposureSubmodelTypes.EmissionFromSolidMaterials:
                    return new InhalationExposureEmissionFromSolidMaterials(scenario);

                default:
                    throw new NotSupportedException(
                        $"Unsupported inhalatory exposure submodel '{scenario.InhalationExposure.SubmodelType.ToString()}'");
            }
        }

        private IInhalationAbsorptionSubmodel AbsorptionSubmodel(ScenarioModel scenario)
        {
            return new InhalationAbsorptionFraction(scenario);
        }

        /// <summary>
        /// Calculates a time series.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        public InhalationTimeSeries CalculateTimeSeries(ScenarioModel scenario)
        {
            const int TotalTimeSteps = 101;

            var outputValues = new InhalationTimeSeries();

            if (scenario.InhalationExposureRouteInUse && IsTimeDependent(scenario))
            {
                IInhalationExposureSubmodel exposureSubmodel = ExposureSubmodel(scenario);

                //Currently, the time max is a time at which the simulation logically ends. This depends on the submodel.
                //Later, this may be replaced by a user selection in the run settings.
                Time timeMax = exposureSubmodel.DefaultTimeMax;

                exposureSubmodel.PrepareTimeSeries(timeMax);

                IInhalationAbsorptionSubmodel absorptionSubmodel = null;
                if (scenario.InhalationAbsorptionRouteInUse)
                {
                    absorptionSubmodel = AbsorptionSubmodel(scenario);
                }

                for (int timeStep = 0; timeStep < TotalTimeSteps; timeStep++)
                {
                    Time time = new Time()
                    {
                        Value = timeMax.Value.Value * timeStep / (TotalTimeSteps - 1),
                        Unit = timeMax.Unit
                    };

                    var instantaneousAirConcentration = exposureSubmodel.InstantaneousAirConcentration(time);

                    var exposureOutcome = new InhalationExposureOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, exposureSubmodel.AmountOfSubstance, scenario.InhalationExposure.InhalationRate, exposureSubmodel.StartTimeOfExposure, exposureSubmodel.EndTimeOfExposure)
                    {
                        InstantaneousAirConcentration = instantaneousAirConcentration
                    };

                    AirConcentration meanAirConcentration;
                    if (timeStep == 0)
                    {
                        meanAirConcentration = instantaneousAirConcentration;
                    }
                    else
                    {
                        meanAirConcentration = exposureSubmodel.MeanAirConcentration(time);
                    }

                    exposureOutcome.SetMeanAirConcentration(meanAirConcentration, time);

                    //Integration of the absorption can be replaced by integration of the exposure times the absorption as absorption is a fixed fraction.
                    InhalationAbsorptionOutcome absorptionOutcome = null;
                    if (scenario.InhalationAbsorptionRouteInUse)
                    {
                        absorptionOutcome = absorptionSubmodel.CalculatePointValues(exposureOutcome);
                    }

                    outputValues.Add(new TimeSeriesPoint<InhalationExposureOutcome, InhalationAbsorptionOutcome>()
                    {
                        Time = time,
                        Exposure = exposureOutcome,
                        Absorption = absorptionOutcome
                    }
                    );
                }
            }
            return outputValues;
        }

        public bool IsTimeDependent(ScenarioModel scenario)
        {
            return (scenario.InhalationExposureRouteInUse && ExposureSubmodel(scenario).IsTimeDependent)
                || (scenario.InhalationAbsorptionRouteInUse && AbsorptionSubmodel(scenario).IsTimeDependent);
        }

        public bool SupportsPeakAirConcentration(ScenarioModel scenario)
        {
            return (scenario.InhalationExposureRouteInUse && ExposureSubmodel(scenario).SupportsPeakAirConcentration);
        }

        public bool SupportsMeanDayConcentration(ScenarioModel scenario)
        {
            return (scenario.InhalationExposureRouteInUse && ExposureSubmodel(scenario).SupportsMeanDayConcentration);
        }

        public bool SupportsExternalDayDose(ScenarioModel scenario)
        {
            return (scenario.InhalationExposureRouteInUse && ExposureSubmodel(scenario).SupportsExternalDayDose);
        }

        public bool SupportsInternalDayDose(ScenarioModel scenario)
        {
            return (scenario.InhalationAbsorptionRouteInUse && ExposureSubmodel(scenario).SupportsInternalDayDose);
        }

        public IEnumerable<DoseMeasureType> EndPointsForSensitivityAnalysis(ScenarioModel scenario)
        {
            var endPointsForSensitivityAnalysis = new List<DoseMeasureType>();

            if (scenario.InhalationExposureRouteInUse)
            {
                endPointsForSensitivityAnalysis.AddRange(ExposureSubmodel(scenario).EndPointsForSensitivityAnalysis());
            }

            if (scenario.InhalationAbsorptionRouteInUse)
            {
                endPointsForSensitivityAnalysis.AddRange(AbsorptionSubmodel(scenario).EndPointsForSensitivityAnalysis());
            }

            return endPointsForSensitivityAnalysis;
        }

        public IEnumerable<ModelParameters> ModelParametersForSensitivityAnalysis(ScenarioModel scenario, DoseMeasureType endpointToAnalyse)
        {
            var modelParametersForSensitivityAnalysis = new List<ModelParameters>();

            if (scenario.InhalationExposureRouteInUse)
            {
                modelParametersForSensitivityAnalysis.AddRange(ExposureSubmodel(scenario).ModelParameters());

                switch (endpointToAnalyse)
                {
                    case DoseMeasureType.MeanEventConcentration:
                    case DoseMeasureType.MeanDayConcentration:
                    case DoseMeasureType.MeanYearConcentration:
                    case DoseMeasureType.PeakAirConcentration:
                        break;

                    case DoseMeasureType.ExternalEventDose:
                    case DoseMeasureType.ExternalDayDose:
                    case DoseMeasureType.InternalEventDose:
                    case DoseMeasureType.InternalDayDose:
                    case DoseMeasureType.InternalYearAverageDose:
                        modelParametersForSensitivityAnalysis.Add(ModelParameters.InhalationExposureInhalationRate);
                        break;

                    case DoseMeasureType.DermalLoad:
                        throw new ApplicationException($"The end point '{endpointToAnalyse}' is not valid for the inhalation route.");

                    default:
                        throw new ApplicationException($"Unsupported end point '{endpointToAnalyse}'");
                }
            }

            if (!scenario.InhalationAbsorptionRouteInUse)
            {
                switch (endpointToAnalyse)
                {
                    case DoseMeasureType.DermalLoad:
                    case DoseMeasureType.ExternalEventDose:
                    case DoseMeasureType.ExternalDayDose:
                    case DoseMeasureType.MeanEventConcentration:
                    case DoseMeasureType.MeanDayConcentration:
                    case DoseMeasureType.MeanYearConcentration:
                    case DoseMeasureType.PeakAirConcentration:
                        break;

                    case DoseMeasureType.InternalEventDose:
                    case DoseMeasureType.InternalDayDose:
                    case DoseMeasureType.InternalYearAverageDose:
                        throw new ApplicationException(
                            $"The end point '{endpointToAnalyse}' is not valid for the dermal route.");

                    default:
                        throw new ApplicationException($"Unsupported end point '{endpointToAnalyse}'");
                }
            }
            return modelParametersForSensitivityAnalysis;
        }

        public IEnumerable<ModelParameters> ModelParametersForChesarExport(ScenarioModel scenario)
        {
            var modelParametersForChesarExport = new List<ModelParameters>();

            modelParametersForChesarExport.AddRange(ExposureSubmodel(scenario).ModelParameters());

            return modelParametersForChesarExport;
        }

        /// <summary>
        /// Perform the sensitivity analysis for the inhalation route.
        /// </summary>
        /// <param name="scenario">The scenario to perform the analysis on.</param>
        /// <param name="runSettings">The run settings, containing the analysis settings, like parameter and bounds.</param>
        /// <returns>
        /// The result, expressed in the dose measure, corresponding to the end point specified in the run settings.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// </exception>
        /// <exception cref="ApplicationException">
        /// </exception>
        public Dose CalculateSensitivityAnalysis(ScenarioModel scenario, SensitivityAnalysisSettings runSettings)
        {
            if (scenario.InhalationExposureRouteInUse)
            {
                var routeOutcomes = this.CalculatePointValues(scenario);
                switch (runSettings.EndPointToAnalyse)
                {
                    case DoseMeasureType.MeanEventConcentration:
                        return routeOutcomes.Exposure.AsMeanEventConcentration;

                    case DoseMeasureType.MeanDayConcentration:
                        return routeOutcomes.Exposure.AsMeanDayConcentration;

                    case DoseMeasureType.MeanYearConcentration:
                        return routeOutcomes.Exposure.AsMeanYearConcentration;

                    case DoseMeasureType.PeakAirConcentration:
                        return routeOutcomes.Exposure.AsPeakAirConcentration;

                    case DoseMeasureType.ExternalEventDose:
                        return routeOutcomes.Exposure.AsExternalEventDose;

                    case DoseMeasureType.ExternalDayDose:
                        return routeOutcomes.Exposure.AsExternalDayDose;

                    case DoseMeasureType.InternalEventDose:
                    case DoseMeasureType.InternalDayDose:
                    case DoseMeasureType.InternalYearAverageDose:
                        {
                            if (scenario.InhalationAbsorptionRouteInUse)
                            {
                                switch (runSettings.EndPointToAnalyse)
                                {
                                    case DoseMeasureType.InternalEventDose:
                                        return routeOutcomes.Absorption.AsInternalEventDose;

                                    case DoseMeasureType.InternalDayDose:
                                        return routeOutcomes.Absorption.AsInternalDayDose;

                                    case DoseMeasureType.InternalYearAverageDose:
                                        return routeOutcomes.Absorption.AsInternalYearAverageDose;

                                    default:
                                        throw new NotSupportedException(
                                            $"Unsupported dose measure '{runSettings.EndPointToAnalyse.ToString()}'");
                                }
                            }
                            else
                            {
                                throw new ApplicationException(
                                    $"A sensitivity analysis for end point '{runSettings.EndPointToAnalyse.ToString()}' cannot be performed if the inhalation absorption route is not in use.");
                            }
                        }

                    default:
                        throw new NotSupportedException(
                            $"Unsupported dose measure '{runSettings.EndPointToAnalyse.ToString()}'");
                }
            }
            else
            {
                throw new ApplicationException(
                    $"A sensitivity analysis for end point '{runSettings.EndPointToAnalyse.ToString()}' cannot be performed if the inhalation exposure route is not in use.");
            }
        }

        public DistributedInhalationEndPoints GetDistributedEndPoints(ScenarioModel scenario)
        {
            var inhalationEndpoints = new DistributedInhalationEndPoints();

            if (scenario.InhalationExposureRouteInUse)
            {
                inhalationEndpoints.Exposure = ExposureSubmodel(scenario).DistributedEndPoints;
            }

            if (scenario.InhalationAbsorptionRouteInUse)
            {
                inhalationEndpoints.Absorption = AbsorptionSubmodel(scenario).DistributedEndPoints(inhalationEndpoints.Exposure.ExternalEventDoseIsDistributed);
            }

            return inhalationEndpoints;
        }

        public IEnumerable<ValidationResult> Validate(ScenarioModel scenario)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (scenario.InhalationExposureRouteInUse)
            {
                validationResults.AddRange(ExposureSubmodel(scenario).Validate());
            }

            if (scenario.InhalationAbsorptionRouteInUse)
            {
                validationResults.AddRange(AbsorptionSubmodel(scenario).Validate());
            }

            if (scenario.InhalationExposure.ReEntry && GetDistributedEndPoints(scenario).AnyEndPointDistributed)
            {
                validationResults.Add("Due to long running times, a scenario with re-entry cannot currently be used in a Monte Carlo simulation.");
            }

            return validationResults;
        }

        public bool IsValid(ScenarioModel scenario)
        {
            return !Validate(scenario).Any();
        }
    }
}