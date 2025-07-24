using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;

namespace RIVM.ConsExpo.Model.Interfaces.Models
{
    /// <summary>
    ///
    /// </summary>
    public interface IInhalationSimulation : IRouteSimulation
    {
        /// <summary>
        /// Indicates whether or not the selected submodel support the calculation of a peak interval.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        bool SupportsPeakAirConcentration(ScenarioModel scenario);

        /// <summary>
        /// Indicates whether or not the selected submodel support the calculation of a mean day concentration.
        /// </summary>
        /// <param name="scenario">The scenario</param>
        /// <returns></returns>
        bool SupportsMeanDayConcentration(ScenarioModel scenario);

        /// <summary>
        /// Indicates whether or not the selected submodel support the calculation of a external dose on day of exposure.
        /// </summary>
        /// <param name="scenario">The scenario</param>
        /// <returns></returns>
        bool SupportsExternalDayDose(ScenarioModel scenario);

        /// <summary>
        /// Indicates whether or not the selected submodel support the calculation of a internal dose on day of exposure.
        /// </summary>
        /// <param name="scenario">The scenario</param>
        /// <returns></returns>
        bool SupportsInternalDayDose(ScenarioModel scenario);

        /// <summary>
        /// Calculates the point values.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        RouteOutcomes<InhalationExposureOutcome, InhalationAbsorptionOutcome> CalculatePointValues(ScenarioModel scenario);

        /// <summary>
        /// Calculates a time series for the inhalation model.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        InhalationTimeSeries CalculateTimeSeries(ScenarioModel scenario);

        /// <summary>
        /// Gets the distributed end points.
        /// </summary>
        /// <value>
        /// The distributed end points.
        /// </value>
        DistributedInhalationEndPoints GetDistributedEndPoints(ScenarioModel scenario);
    }
}