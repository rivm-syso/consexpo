using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.Settings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RIVM.ConsExpo.DTO.PhysicalQuantities;

namespace RIVM.ConsExpo.Model.Interfaces.Models
{
    /// <summary>
    /// Interface for route specific simulations.
    /// </summary>
    public interface IRouteSimulation
    {
        /// <summary>
        /// Gets a value indicating whether the model is time dependent. Only if it is, it can be used in time series calculation.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is time dependent; otherwise, <c>false</c>.
        /// </value>
        bool IsTimeDependent(ScenarioModel scenario);

        /// <summary>
        /// The actual duration of the route's exposure scenario.
        /// </summary>
        /// <param name="scenario"></param>
        /// <returns></returns>
        Duration ApplicableExposureDuration(ScenarioModel scenario);

        /// <summary>
        /// Validates the specified scenario for the route that implements this interface.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        IEnumerable<ValidationResult> Validate(ScenarioModel scenario);

        /// <summary>
        /// Indicates whether the specified scenario is valid for the route that implements this interface.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        bool IsValid(ScenarioModel scenario);

        /// <summary>
        /// The end points available for a sensitivity analysis.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        IEnumerable<DoseMeasureType> EndPointsForSensitivityAnalysis(ScenarioModel scenario);

        /// <summary>
        /// The physical quantities available for a sensitivity analysis. The parameters depend on route, selected submodel and sometimes on switches in the submodel, like LimitConcentrationToSaturatedAirConcentration.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <param name="endpointToAnalyse">The endpoint to analyse.</param>
        /// <returns></returns>
        IEnumerable<ModelParameters> ModelParametersForSensitivityAnalysis(ScenarioModel scenario, DoseMeasureType endpointToAnalyse);

        /// <summary>
        /// Returns a list of the parameters relevant for the export to Chesar. The parameters depend on selected submodel and sometimes on switches in the submodel, like LimitConcentrationToSaturatedAirConcentration.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        IEnumerable<ModelParameters> ModelParametersForChesarExport(ScenarioModel scenario);

        /// <summary>
        /// Perform the sensitivity analysis for this route.
        /// </summary>
        /// <param name="scenario">The scenario to perform the analysis on.</param>
        /// <param name="runSettings">The run settings, containing the analysis settings, like parameter and bounds.</param>
        /// <returns>
        /// The result, expressed in the dose measure, corresponding to the end point specified in the run settings.
        /// </returns>
        Dose CalculateSensitivityAnalysis(ScenarioModel scenario, SensitivityAnalysisSettings runSettings);
    }
}