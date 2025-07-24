using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Settings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RIVM.ConsExpo.Model.Interfaces.Models
{
    /// <summary>
    /// The interface that specifies the features of the complete simulation.
    /// </summary>
    public interface ISimulation
    {
        /// <summary>
        /// Validates the specified scenario model.
        /// </summary>
        /// <param name="scenarioModel">The scenario model.</param>
        /// <returns></returns>
        List<ValidationResult> Validate(ScenarioModel scenarioModel);

        /// <summary>
        /// Indicates whether or not end points can be calculated.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        bool EndPointsEnabled(ScenarioModel scenario);

        /// <summary>
        /// Calculates the point values, only if the scenario is not distributed.
        /// </summary>
        /// <returns></returns>
        SimulationResults CalculateResults(ScenarioModel scenario);

        /// <summary>
        /// Calculates the point values.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <param name="calculateDistributions">if set to <c>true</c> calculate distributed end points. Otherwise, only calculate deterministic endpoints.</param>
        /// <param name="numberOfIterations">The number of iterations.</param>
        /// <param name="numberOfBins">The number of bins.</param>
        /// <returns></returns>
        SimulationResults CalculateResults(ScenarioModel scenario, bool calculateDistributions, int numberOfIterations, int numberOfBins, ScaleType outputScale);

        /// <summary>
        /// Determines whether the specified scenario is time dependent.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        bool IsTimeDependent(ScenarioModel scenario);

        /// <summary>
        /// Calculates a time series for the specified scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        TimeSeries CalculateTimeSeries(ScenarioModel scenario);

        /// <summary>
        /// Gets the distributed end points.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        DistributedEndPoints GetDistributedEndPoints(ScenarioModel scenario);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool AnyEndPointDistributed(ScenarioModel scenario);

        /// <summary>
        /// Indicates whether or not the specified scenario can be used for a sensitivity analysis.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        bool SensitivityAnalysisEnabled(ScenarioModel scenario);

        /// <summary>
        /// Returns the routes available for a sensitivity analysis, based on the scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        List<RouteTypes> RoutesForSensitivityAnalysis(ScenarioModel scenario);

        /// <summary>
        /// Returns the end points available for a sensitivity analysis, based on the scenario and the selected route.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <param name="routeToAnalyse">The route to analyse.</param>
        /// <returns></returns>
        List<DoseMeasureType> EndPointsForSensitivityAnalysis(ScenarioModel scenario, RouteTypes routeToAnalyse);

        /// <summary>
        /// Returns the model parameters that can be used for a sensitivity analysis for the specified scenario and the selected route and endpoint.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <param name="routeToAnalyse">The route to analyse.</param>
        /// <param name="endpointToAnalyse">The endpoint to analyse.</param>
        /// <returns></returns>
        IEnumerable<ModelParameters> ModelParametersForSensitivityAnalysis(ScenarioModel scenario, RouteTypes routeToAnalyse, DoseMeasureType endpointToAnalyse);

        /// <summary>
        /// A list of the model parameters for an export to Chesar.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        IEnumerable<ModelParameters> ModelParametersForChesarExport(ScenarioModel scenario);

        /// <summary>
        /// Returns the units for the specified model parameter.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <param name="modelParameterToAnalyse">The model parameter to analyse.</param>
        /// <returns></returns>
        List<UnitBase> UnitsForSensitivityAnalysis(ScenarioModel scenario, ModelParameters modelParameterToAnalyse);

        /// <summary>
        /// Calculates the sensitivity analysis.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <param name="runSettings">The run settings.</param>
        /// <returns></returns>
        SensitivityAnalysis CalculateSensitivityAnalysis(ScenarioModel scenario, SensitivityAnalysisSettings runSettings);
    }
}