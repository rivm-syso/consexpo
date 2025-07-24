using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.Submodels;

namespace RIVM.ConsExpo.Model.Interfaces.Submodels
{
    /// <summary>
    /// Interface for models implementing inhalation exposure.
    /// </summary>
    public interface IInhalationExposureSubmodel : IExposureSubmodel<InhalationExposureOutcome>
    {
        /// <summary>
        /// Gets the type of submodel selected by the user for the inhalation exposure route.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        InhalationExposureSubmodelTypes Type { get; }

        /// <summary>
        /// Gets the start of exposure.
        /// </summary>
        /// <value>
        /// The start of exposure.
        /// </value>
        Time StartTimeOfExposure { get; }

        /// <summary>
        /// Gets the end of exposure.
        /// </summary>
        /// <value>
        /// The end of exposure.
        /// </value>
        Time EndTimeOfExposure { get; }

        /// <summary>
        /// Calculates the air concentration at the specified time, for the specified scenario.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        AirConcentration InstantaneousAirConcentration(Time time);

        /// <summary>
        /// Calculates the time-averaged air concentration for the specified scenario, up to the specified time.
        /// </summary>
        /// <param name="time">The end time for calculation of the mean</param>
        /// <returns></returns>
        AirConcentration MeanAirConcentration(Time time);

        /// <summary>
        /// Gets a value indicating whether the model [supports peak air concentration].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [supports peak air concentration]; otherwise, <c>false</c>.
        /// </value>
        bool SupportsPeakAirConcentration { get; }

        /// <summary>
        /// Gets a value indicating whether the model [supports mean day concentration].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [supports mean day concentration]; otherwise, <c>false</c>.
        /// </value>
        bool SupportsMeanDayConcentration { get; }

        /// <summary>
        /// Gets a value indicating whether the model [supports external day dose].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [supports external day dose]; otherwise, <c>false</c>.
        /// </value>
        bool SupportsExternalDayDose { get; }

        /// <summary>
        /// Gets a value indicating whether the model [supports internal day dose].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [supports internal day dose]; otherwise, <c>false</c>.
        /// </value>
        bool SupportsInternalDayDose { get; }

        /// <summary>
        /// Calculates the average air concentration during the peak interval of 15 min.
        /// </summary>
        /// <returns></returns>
        AirConcentration PeakAirConcentration();

        /// <summary>
        /// Calculates the time-averaged air concentration for the specified scenario, up to the exposure duration.
        /// </summary>
        /// <returns></returns>
        AirConcentration MeanAirConcentration();

        /// <summary>
        /// Gets the distributed end points.
        /// </summary>
        /// <value>
        /// The distributed end points.
        /// </value>
        DistributedInhalationExposureEndPoints DistributedEndPoints { get; }

        /// <summary>
        /// Gets the mean air concentration on the peak time of exposure for Re-entry.
        /// </summary>
        /// <value>
        /// The mean air concentration on the peak time of exposure for Re-entry.
        /// </value>
        AirConcentration MeanAirConcentrationPeak();
    }
}
