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

namespace RIVM.ConsExpo.Model.Models
{
    /// <summary>
    /// A class that can start the oral simulation, exposure and absorption, as specified in the scenario.
    /// </summary>
    internal class OralSimulation : IOralSimulation
    {
        /// <summary>
        /// Start the oral simulation, exposure and absorption, if in use, as specified in the scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">The oral route does not have an absorption model, yet.</exception>
        public RouteOutcomes<OralExposureOutcome, OralAbsorptionOutcome> CalculatePointValues(ScenarioModel scenario)
        {
            var outputValues = new RouteOutcomes<OralExposureOutcome, OralAbsorptionOutcome>();

            if (scenario.OralExposureRouteInUse)
            {
                var oralExposureOutcome = ExposureSubmodel(scenario).CalculatePointValues();

                outputValues.Exposure = oralExposureOutcome;

                if (scenario.OralAbsorptionRouteInUse)
                {
                    outputValues.Absorption = AbsorptionSubmodel(scenario).CalculatePointValues(outputValues.Exposure);
                }
            }

            return outputValues;
        }

        private IOralExposureSubmodel ExposureSubmodel(ScenarioModel scenario)
        {
            switch (scenario.OralExposure.SubmodelType)
            {
                case OralExposureSubmodelTypes.DirectIntake:
                    return new OralExposureDirectIntake(scenario);

                case OralExposureSubmodelTypes.ConstantRate:
                    return new OralExposureConstantRate(scenario);

                case OralExposureSubmodelTypes.ProductMouthing:
                    return new OralExposureProductMouthing(scenario);

                case OralExposureSubmodelTypes.SprayingNonRespirableMaterial:
                    return new OralExposureSprayingNonRespirableMaterial(scenario);

                case OralExposureSubmodelTypes.MigrationFromPackagingInstantRelease:
                    return new OralExposureMigrationFromPackagingInstantRelease(scenario);

                case OralExposureSubmodelTypes.MigrationFromPackagingConstantRate:
                    return new OralExposureMigrationFromPackagingConstantRate(scenario);

                default:
                    throw new NotSupportedException($"Unsupported oral exposure submodel '{scenario.OralExposure.SubmodelType}'");
            }
        }

        private IOralAbsorptionSubmodel AbsorptionSubmodel(ScenarioModel scenario)
        {
            switch (scenario.OralAbsorption.SubmodelType)
            {
                case OralAbsorptionSubmodelTypes.Fraction:
                    return new OralAbsorptionFraction(scenario);

                default:
                    throw new NotSupportedException($"Unsupported oral absorption submodel '{scenario.OralAbsorption.SubmodelType}'");
            }
        }

        public bool IsTimeDependent(ScenarioModel scenario)
        {
            return (scenario.OralExposureRouteInUse && ExposureSubmodel(scenario).IsTimeDependent)
                || (scenario.OralAbsorptionRouteInUse && AbsorptionSubmodel(scenario).IsTimeDependent);
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
        public OralTimeSeries CalculateTimeSeries(ScenarioModel scenario)
        {
            const int TotalTimeSteps = 101;

            var outputValues = new OralTimeSeries();

            if (scenario.OralExposureRouteInUse && IsTimeDependent(scenario))
            {
                IOralExposureSubmodel exposureSubmodel = ExposureSubmodel(scenario);

                //Currently, the time max is a time at which the simulation logically ends. This depends on the submodel.
                //Later, this may be replaced by a user selection in the run settings.
                Time timeMax = exposureSubmodel.DefaultTimeMax;

                exposureSubmodel.PrepareTimeSeries(timeMax);

                if (scenario.OralAbsorptionRouteInUse)
                {
                    AbsorptionSubmodel(scenario);
                }

                for (int timeStep = 0; timeStep < TotalTimeSteps; timeStep++)
                {
                    Time time = new Time()
                    {
                        Value = timeMax.Value.Value * timeStep / (TotalTimeSteps - 1),
                        Unit = timeMax.Unit
                    };

                    var exposureOutcome = exposureSubmodel.CalculatePointValues(time);

                    OralAbsorptionOutcome absorptionOutcome = null;

                    if (scenario.OralAbsorptionRouteInUse)
                    {
                        absorptionOutcome = AbsorptionSubmodel(scenario).CalculatePointValues(exposureOutcome);
                    }

                    outputValues.Add(new TimeSeriesPoint<OralExposureOutcome, OralAbsorptionOutcome>()
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

        public IEnumerable<DoseMeasureType> EndPointsForSensitivityAnalysis(ScenarioModel scenario)
        {
            var EndPointsForSensitivityAnalysis = new List<DoseMeasureType>();

            if (scenario.OralExposureRouteInUse)
            {
                EndPointsForSensitivityAnalysis.AddRange(ExposureSubmodel(scenario).EndPointsForSensitivityAnalysis());
            }

            if (scenario.OralAbsorptionRouteInUse)
            {
                EndPointsForSensitivityAnalysis.AddRange(AbsorptionSubmodel(scenario).EndPointsForSensitivityAnalysis());
            }

            return EndPointsForSensitivityAnalysis;
        }

        public IEnumerable<ModelParameters> ModelParametersForSensitivityAnalysis(ScenarioModel scenario, DoseMeasureType endpointToAnalyse)
        {
            var modelParametersForSensitivityAnalysis = new List<ModelParameters>();

            modelParametersForSensitivityAnalysis.AddRange(ExposureSubmodel(scenario).ModelParameters());

            if (scenario.OralExposureRouteInUse)
            {
                switch (endpointToAnalyse)
                {
                    case DoseMeasureType.ExternalEventDose:
                    case DoseMeasureType.ExternalDayDose:
                    case DoseMeasureType.InternalEventDose:
                    case DoseMeasureType.InternalDayDose:
                    case DoseMeasureType.InternalYearAverageDose:
                        break;

                    case DoseMeasureType.DermalLoad:
                    case DoseMeasureType.MeanEventConcentration:
                    case DoseMeasureType.MeanDayConcentration:
                    case DoseMeasureType.MeanYearConcentration:
                    case DoseMeasureType.PeakAirConcentration:
                        throw new ApplicationException(
                            $"The end point '{endpointToAnalyse}' is not valid for the oral route.");

                    default:
                        throw new ApplicationException($"Unsupported end point '{endpointToAnalyse}'");
                }
            }

            if (scenario.OralAbsorptionRouteInUse)
            {
                switch (endpointToAnalyse)
                {
                    case DoseMeasureType.ExternalEventDose:
                    case DoseMeasureType.ExternalDayDose:
                        break;

                    case DoseMeasureType.InternalEventDose:
                    case DoseMeasureType.InternalDayDose:
                    case DoseMeasureType.InternalYearAverageDose:
                        modelParametersForSensitivityAnalysis.AddRange(AbsorptionSubmodel(scenario).ModelParameters());
                        break;

                    case DoseMeasureType.DermalLoad:
                    case DoseMeasureType.MeanEventConcentration:
                    case DoseMeasureType.MeanDayConcentration:
                    case DoseMeasureType.MeanYearConcentration:
                    case DoseMeasureType.PeakAirConcentration:
                        throw new ApplicationException($"The end point '{endpointToAnalyse}' is not valid for the dermal route.");

                    default:
                        throw new ApplicationException($"Unsupported end point '{endpointToAnalyse}'");
                }
            }
            return modelParametersForSensitivityAnalysis;
        }

        public IEnumerable<ModelParameters> ModelParametersForChesarExport(ScenarioModel scenario)
        {
            var modelParametersForChesarExport = new List<ModelParameters>();

            if (scenario.OralExposure.SubmodelType != OralExposureSubmodelTypes.SprayingNonRespirableMaterial)
            {
                // This model depends on the Inhalation - Exposure to spray - Spraying model and uses the same parameters.
                // For the export to Chesar, they can be ignored here because they are already included with the inhalation model
                modelParametersForChesarExport.AddRange(ExposureSubmodel(scenario).ModelParameters());
            }

            return modelParametersForChesarExport;
        }

        /// <summary>
        /// Perform the sensitivity analysis for the oral route.
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
            if (scenario.OralExposureRouteInUse)
            {
                var exposureOutcome = ExposureSubmodel(scenario).CalculatePointValues();
                switch (runSettings.EndPointToAnalyse)
                {
                    case DoseMeasureType.ExternalEventDose:
                        return exposureOutcome.AsExternalEventDose;

                    case DoseMeasureType.ExternalDayDose:
                        return exposureOutcome.AsExternalDayDose;

                    case DoseMeasureType.InternalEventDose:
                    case DoseMeasureType.InternalDayDose:
                    case DoseMeasureType.InternalYearAverageDose:
                        {
                            //Do not needlessly calculate absorption: only if the route is in use and the end point is related to absorption.
                            if (scenario.OralAbsorptionRouteInUse)
                            {
                                var absorptionOutcome = AbsorptionSubmodel(scenario).CalculatePointValues(exposureOutcome);

                                switch (runSettings.EndPointToAnalyse)
                                {
                                    case DoseMeasureType.InternalEventDose:
                                        return absorptionOutcome.AsInternalEventDose;

                                    case DoseMeasureType.InternalDayDose:
                                        return absorptionOutcome.AsInternalDayDose;

                                    case DoseMeasureType.InternalYearAverageDose:
                                        return absorptionOutcome.AsInternalYearAverageDose;

                                    default:
                                        throw new NotSupportedException($"Unsupported dose measure '{runSettings.EndPointToAnalyse.ToString()}'");
                                }
                            }
                            else
                            {
                                throw new ApplicationException($"A sensitivity analysis for end point '{runSettings.EndPointToAnalyse}' cannot be performed if the oral absorption route is not in use.");
                            }
                        }

                    default:
                        throw new NotSupportedException($"Unsupported dose measure '{runSettings.EndPointToAnalyse}'");
                }
            }
            else
            {
                throw new ApplicationException($"A sensitivity analysis for end point '{runSettings.EndPointToAnalyse}' cannot be performed if the oral exposure route is not in use.");
            }
        }

        public DistributedOralEndPoints GetDistributedEndPoints(ScenarioModel scenario)
        {
            var oralEndpoints = new DistributedOralEndPoints();

            if (scenario.OralExposureRouteInUse)
            {
                oralEndpoints.Exposure = ExposureSubmodel(scenario).DistributedEndPoints;
            }

            if (scenario.OralAbsorptionRouteInUse)
            {
                oralEndpoints.Absorption = AbsorptionSubmodel(scenario).DistributedEndPoints(oralEndpoints.Exposure.ExternalEventDoseIsDistributed);
            }

            return oralEndpoints;
        }

        public IEnumerable<ValidationResult> Validate(ScenarioModel scenario)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (scenario.OralExposureRouteInUse)
            {
                validationResults.AddRange(ExposureSubmodel(scenario).Validate());
            }

            if (scenario.OralAbsorptionRouteInUse)
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