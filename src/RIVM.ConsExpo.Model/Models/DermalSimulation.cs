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
using RIVM.ConsExpo.Model.SubModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RIVM.ConsExpo.Model.Models
{
    /// <summary>
    /// A class that can start the dermal simulation, exposure and absorption, as specified in the scenario.
    /// </summary>
    internal class DermalSimulation : IDermalSimulation
    {
        /// <summary>
        /// Start the dermal simulation, exposure and absorption, if in use, as specified in the scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        public RouteOutcomes<DermalExposureOutcome, DermalAbsorptionOutcome> CalculatePointValues(ScenarioModel scenario)
        {
            var outputValues = new RouteOutcomes<DermalExposureOutcome, DermalAbsorptionOutcome>();

            if (scenario.DermalExposureRouteInUse)
            {
                outputValues.Exposure = ExposureSubmodel(scenario).CalculatePointValues();
                if (scenario.DermalAbsorptionRouteInUse)
                {
                    outputValues.Absorption = AbsorptionSubmodel(scenario).CalculatePointValues(outputValues.Exposure);
                }
            }

            return outputValues;
        }

        private IDermalExposureSubmodel ExposureSubmodel(ScenarioModel scenario)
        {
            switch (scenario.DermalExposure.SubmodelType)
            {
                case DermalExposureSubmodelTypes.InstantApplication:
                    return new DermalExposureInstantApplication(scenario);

                case DermalExposureSubmodelTypes.ConstantRate:
                    return new DermalExposureConstantRate(scenario);

                case DermalExposureSubmodelTypes.Migration:
                    return new DermalExposureMigration(scenario);

                case DermalExposureSubmodelTypes.RubbingOff:
                    return new DermalExposureRubbingOff(scenario);

                case DermalExposureSubmodelTypes.Diffusion:
                    return new DermalExposureDiffusion(scenario);

                default:
                    throw new NotSupportedException(string.Format("Unsupported dermal exposure submodel '{0}'", scenario.DermalExposure.SubmodelType.ToString()));
            }
        }

        private IDermalAbsorptionSubmodel AbsorptionSubmodel(ScenarioModel scenario)
        {
            switch (scenario.DermalAbsorption.SubmodelType)
            {
                case DermalAbsorptionSubmodelTypes.Fraction:
                    return new DermalAbsorptionFraction(scenario);

                case DermalAbsorptionSubmodelTypes.DiffusionThroughSkinForInstantApplication:
                    switch (scenario.DermalExposure.SubmodelType)
                    {
                        case DermalExposureSubmodelTypes.InstantApplication:
                            return new DermalAbsorptionDiffusionThroughSkinForInstantApplication(scenario);

                        default:
                            throw new NotSupportedException(string.Format("The combination of dermal exposure submodel '{0}' with dermal absorption submodel '{1}' is invalid.", scenario.DermalExposure.SubmodelType.ToString(), scenario.DermalAbsorption.SubmodelType.ToString()));
                    }

                case DermalAbsorptionSubmodelTypes.DiffusionThroughSkinForDiffusion:
                    switch (scenario.DermalExposure.SubmodelType)
                    {
                        case DermalExposureSubmodelTypes.Diffusion:
                            return new DermalDiffusionThroughSkinForDiffusion(scenario);

                        default:
                            throw new NotSupportedException(string.Format("The combination of dermal exposure submodel '{0}' with dermal absorption submodel '{1}' is invalid.", scenario.DermalExposure.SubmodelType.ToString(), scenario.DermalAbsorption.SubmodelType.ToString()));
                    }

                default:
                    throw new NotSupportedException(string.Format("Unsupported dermal absorption submodel '{0}'", scenario.DermalAbsorption.SubmodelType.ToString()));
            }
        }

        public Duration ApplicableExposureDuration(ScenarioModel scenario)
        {
            return ExposureSubmodel(scenario).ApplicableExposureDuration;
        }

        /// <summary>
        /// Calculates a time series.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        public DermalTimeSeries CalculateTimeSeries(ScenarioModel scenario)
        {
            const int TotalTimeSteps = 101;

            var outputValues = new DermalTimeSeries();

            if (scenario.DermalExposureRouteInUse && IsTimeDependent(scenario))
            {
                IDermalExposureSubmodel exposureSubmodel = ExposureSubmodel(scenario);

                //Currently, the time max is a time at which the simulation logically ends. This depends on the submodel.
                //Later, this may be replaced by a user selection in the run settings.
                Time timeMax = exposureSubmodel.DefaultTimeMax;

                exposureSubmodel.PrepareTimeSeries(timeMax);

                IDermalAbsorptionSubmodel absorptionSubmodel = null;
                if (scenario.DermalAbsorptionRouteInUse)
                {
                    absorptionSubmodel = AbsorptionSubmodel(scenario);
                    absorptionSubmodel.PrepareTimeSeries(timeMax);
                }

                for (int timeStep = 0; timeStep < TotalTimeSteps; timeStep++)
                {
                    Time time = new Time()
                    {
                        Value = timeMax.Value.Value * timeStep / (TotalTimeSteps - 1),
                        Unit = timeMax.Unit
                    };

                    var exposureOutcome = exposureSubmodel.CalculatePointValues(time);
                    DermalAbsorptionOutcome absorptionOutcome = null;
                    if (absorptionSubmodel != null)
                    {
                        absorptionOutcome = absorptionSubmodel.CalculatePointValues(exposureOutcome, time);
                    }

                    outputValues.Add(new TimeSeriesPoint<DermalExposureOutcome, DermalAbsorptionOutcome>()
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

        /// <summary>
        /// Perform the sensitivity analysis for the dermal route.
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
            if (scenario.DermalExposureRouteInUse)
            {
                var exposureOutcome = ExposureSubmodel(scenario).CalculatePointValues();
                switch (runSettings.EndPointToAnalyse)
                {
                    case DoseMeasureType.DermalLoad:
                        return exposureOutcome.AsDermalLoad;

                    case DoseMeasureType.ExternalEventDose:
                        return exposureOutcome.AsExternalEventDose;

                    case DoseMeasureType.ExternalDayDose:
                        return exposureOutcome.AsExternalDayDose;

                    case DoseMeasureType.InternalEventDose:
                    case DoseMeasureType.InternalDayDose:
                    case DoseMeasureType.InternalYearAverageDose:
                        {
                            //Do not needlessly calculate aborption: only if the route is in use and the end point is related to absorption.
                            DermalAbsorptionOutcome absorptionOutcome = null;
                            if (scenario.DermalAbsorptionRouteInUse)
                            {
                                absorptionOutcome = AbsorptionSubmodel(scenario).CalculatePointValues(exposureOutcome);

                                switch (runSettings.EndPointToAnalyse)
                                {
                                    case DoseMeasureType.InternalEventDose:
                                        return absorptionOutcome.AsInternalEventDose;

                                    case DoseMeasureType.InternalDayDose:
                                        return absorptionOutcome.AsInternalDayDose;

                                    case DoseMeasureType.InternalYearAverageDose:
                                        return absorptionOutcome.AsInternalYearAverageDose;

                                    default:
                                        throw new NotSupportedException(string.Format("Unsupported dose measure '{0}'", runSettings.EndPointToAnalyse.ToString()));
                                }
                            }
                            else
                            {
                                throw new ApplicationException(string.Format("A sensitivity analysis for end point '{0}' cannot be performed if the dermal absorption route is not in use.", runSettings.EndPointToAnalyse.ToString()));
                            }
                        }

                    default:
                        throw new NotSupportedException(string.Format("Unsupported dose measure '{0}'", runSettings.EndPointToAnalyse.ToString()));
                }
            }
            else
            {
                throw new ApplicationException(string.Format("A sensitivity analysis for end point '{0}' cannot be performed if the dermal exposure route is not in use.", runSettings.EndPointToAnalyse.ToString()));
            }
        }

        public IEnumerable<DoseMeasureType> EndPointsForSensitivityAnalysis(ScenarioModel scenario)
        {
            var EndPointsForSensitivityAnalysis = new List<DoseMeasureType>();

            if (scenario.DermalExposureRouteInUse)
            {
                EndPointsForSensitivityAnalysis.AddRange(ExposureSubmodel(scenario).EndPointsForSensitivityAnalysis());
            }

            if (scenario.DermalAbsorptionRouteInUse)
            {
                EndPointsForSensitivityAnalysis.AddRange(AbsorptionSubmodel(scenario).EndPointsForSensitivityAnalysis());
            }

            return EndPointsForSensitivityAnalysis;
        }

        public IEnumerable<ModelParameters> ModelParametersForSensitivityAnalysis(ScenarioModel scenario, DoseMeasureType endpointToAnalyse)
        {
            var modelParametersForSensitivityAnalysis = new List<ModelParameters>();

            if (scenario.DermalExposureRouteInUse)
            {
                modelParametersForSensitivityAnalysis.AddRange(ExposureSubmodel(scenario).ModelParameters());

                switch (endpointToAnalyse)
                {
                    case DoseMeasureType.DermalLoad:
                        modelParametersForSensitivityAnalysis.Add(ModelParameters.DermalExposureExposedArea);
                        break;

                    case DoseMeasureType.ExternalEventDose:
                    case DoseMeasureType.ExternalDayDose:
                    case DoseMeasureType.InternalEventDose:
                    case DoseMeasureType.InternalDayDose:
                    case DoseMeasureType.InternalYearAverageDose:
                        break;

                    case DoseMeasureType.MeanEventConcentration:
                    case DoseMeasureType.MeanDayConcentration:
                    case DoseMeasureType.MeanYearConcentration:
                    case DoseMeasureType.PeakAirConcentration:
                        throw new ApplicationException(string.Format("The end point '{0}' is not valid for the dermal route.", endpointToAnalyse));

                    default:
                        throw new ApplicationException(string.Format("Unsupported end point '{0}'", endpointToAnalyse));
                }

                if (scenario.DermalAbsorptionRouteInUse)
                {
                    switch (endpointToAnalyse)
                    {
                        case DoseMeasureType.DermalLoad:
                        case DoseMeasureType.ExternalEventDose:
                        case DoseMeasureType.ExternalDayDose:
                        case DoseMeasureType.InternalEventDose:
                        case DoseMeasureType.InternalDayDose:
                        case DoseMeasureType.InternalYearAverageDose:
                            break;

                        case DoseMeasureType.MeanEventConcentration:
                        case DoseMeasureType.MeanDayConcentration:
                        case DoseMeasureType.MeanYearConcentration:
                        case DoseMeasureType.PeakAirConcentration:
                            throw new ApplicationException(string.Format("The end point '{0}' is not valid for the dermal route.", endpointToAnalyse));

                        default:
                            throw new ApplicationException(string.Format("Unsupported end point '{0}'", endpointToAnalyse));
                    }
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

        public bool IsTimeDependent(ScenarioModel scenario)
        {
            return (scenario.DermalExposureRouteInUse && ExposureSubmodel(scenario).IsTimeDependent)
                || (scenario.DermalAbsorptionRouteInUse && AbsorptionSubmodel(scenario).IsTimeDependent);
        }

        public DistributedDermalEndPoints GetDistributedEndPoints(ScenarioModel scenario)
        {
            var dermalEndpoints = new DistributedDermalEndPoints();

            if (scenario.DermalExposureRouteInUse)
            {
                dermalEndpoints.Exposure = ExposureSubmodel(scenario).DistributedEndPoints;
            }

            if (scenario.DermalAbsorptionRouteInUse)
            {
                dermalEndpoints.Absorption = AbsorptionSubmodel(scenario).DistributedEndPoints(dermalEndpoints.Exposure.ExternalEventDoseIsDistributed);
            }

            return dermalEndpoints;
        }

        public IEnumerable<ValidationResult> Validate(ScenarioModel scenario)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (scenario.DermalExposureRouteInUse)
            {
                validationResults.AddRange(ExposureSubmodel(scenario).Validate());
            }

            if (scenario.DermalAbsorptionRouteInUse)
            {
                validationResults.AddRange(AbsorptionSubmodel(scenario).Validate());
            }

            return validationResults;
        }

        public bool IsValid(ScenarioModel scenario)
        {
            return !Validate(scenario).Any();
        }
    }
}