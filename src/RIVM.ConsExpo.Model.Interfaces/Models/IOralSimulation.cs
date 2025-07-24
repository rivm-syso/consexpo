using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Output;

namespace RIVM.ConsExpo.Model.Interfaces.Models
{
    /// <summary>
    ///
    /// </summary>
    public interface IOralSimulation : IRouteSimulation
    {
        /// <summary>
        /// Calculates the point values.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        RouteOutcomes<OralExposureOutcome, OralAbsorptionOutcome> CalculatePointValues(ScenarioModel scenario);

        /// <summary>
        /// Calculates a time series for the oral model.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        OralTimeSeries CalculateTimeSeries(ScenarioModel scenario);

        /// <summary>
        /// Gets the distributed end points.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        DistributedOralEndPoints GetDistributedEndPoints(ScenarioModel scenario);
    }
}