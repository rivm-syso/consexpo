using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Output;

namespace RIVM.ConsExpo.Model.Interfaces.Models
{
    /// <summary>
    ///
    /// </summary>
    public interface IDermalSimulation : IRouteSimulation
    {
        /// <summary>
        /// Calculates the point values.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        RouteOutcomes<DermalExposureOutcome, DermalAbsorptionOutcome> CalculatePointValues(ScenarioModel scenario);

        /// <summary>
        /// Calculates a time series for the dermal model.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        DermalTimeSeries CalculateTimeSeries(ScenarioModel scenario);

        /// <summary>
        /// Gets the distributed end points.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        DistributedDermalEndPoints GetDistributedEndPoints(ScenarioModel scenario);
    }
}