using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.Submodels;

namespace RIVM.ConsExpo.Model.Interfaces.Submodels
{
    /// <summary>
    /// Interface for the dermal exposure simulation submodels.
    /// </summary>
    public interface IDermalExposureSubmodel : IExposureSubmodel<DermalExposureOutcome>
    {
        /// <summary>
        /// Gets the type of submodel selected by the user for the dermal exposure route.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        DermalExposureSubmodelTypes Type { get; }

        ///<summary>
        /// Determines for each end point of this route whether or not it depends on distributed parameters.
        /// </summary>
        DistributedDermalExposureEndPoints DistributedEndPoints { get; }

        /// <summary>
        /// Calculates the end point values.
        /// </summary>
        /// <returns></returns>
        DermalExposureOutcome CalculatePointValues();

        /// <summary>
        /// Calculates the point values for a specified time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        DermalExposureOutcome CalculatePointValues(Time time);
    }
}