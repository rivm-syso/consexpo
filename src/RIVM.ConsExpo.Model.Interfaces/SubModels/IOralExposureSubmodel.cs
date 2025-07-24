using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.Submodels;

namespace RIVM.ConsExpo.Model.Interfaces.Submodels
{
    /// <summary>
    /// Interface for models implementing oral exposure.
    /// </summary>
    public interface IOralExposureSubmodel : IExposureSubmodel<OralExposureOutcome>
    {
        /// <summary>
        /// Gets the type of submodel selected by the user for the oral exposure route.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        OralExposureSubmodelTypes Type { get; }

        /// <summary>
        /// Gets the distributed end points.
        /// </summary>
        /// <value>
        /// The distributed end points.
        /// </value>
        DistributedOralExposureEndPoints DistributedEndPoints { get; }

        /// <summary>
        /// Calculates the end point values.
        /// </summary>
        /// <returns></returns>
        OralExposureOutcome CalculatePointValues();

        /// <summary>
        /// Calculates the point values for a specified time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        OralExposureOutcome CalculatePointValues(Time time);
    }
}