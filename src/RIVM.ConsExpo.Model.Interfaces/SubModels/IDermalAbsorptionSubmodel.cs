using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.Submodels;

namespace RIVM.ConsExpo.Model.Interfaces.Submodels
{
    /// <summary>
    /// Interface for the dermal absorption simulation submodels.
    /// </summary>
    public interface IDermalAbsorptionSubmodel : IAbsorptionSubmodel<DermalExposureOutcome, DermalAbsorptionOutcome>
    {
        /// <summary>
        /// Gets the type of submodel selected by the user for the dermal absorption route.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        DermalAbsorptionSubmodelTypes Type { get; }

        /// <summary>
        /// Calculates the point values.
        /// </summary>
        /// <param name="exposure">The exposure.</param>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        DermalAbsorptionOutcome CalculatePointValues(DermalExposureOutcome exposure, Time time);

        /// <summary>
        /// Some models need to prepare a time series, for better performance. This method requests the model to do this preparation.
        /// These are models that use numerical time integration. The models are called for 100 time steps, but it would be very inefficient if the integration was done 100 times.
        /// </summary>
        /// <param name="maxTime">The maximum time.</param>
        void PrepareTimeSeries(Time maxTime);
    }
}