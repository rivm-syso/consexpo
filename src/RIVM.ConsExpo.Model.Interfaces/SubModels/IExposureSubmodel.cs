using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;

namespace RIVM.ConsExpo.Model.Interfaces.Submodels
{
    /// <summary>
    /// The common interface for all exposure submodels that implement calculations for a route.
    /// </summary>
    public interface IExposureSubmodel<TExposureOutcome> : ISubmodel
        where TExposureOutcome : ExposureOutcomeBase
    {
        /// <summary>
        /// Gets the default time maximum for time series. A suitable value depends on the exposure submodel being used. E.g. for constant rate models, this is the exposure duration, whereas it is contact time for the dermal exposure model 'Rubbing off'.
        /// </summary>
        /// <value>
        /// The default time maximum.
        /// </value>
        /// <remarks>This property is used for the automatic plotting maximum for time series, for now. Later, this may be used to set the default on the user selection option for chart settings.</remarks>
        Time DefaultTimeMax { get; }

        /// <summary>
        /// The effective exposure duration, depending on the submodel and settings like re-entry.
        /// </summary>
        Duration ApplicableExposureDuration { get; }

        /// <summary>
        /// Prepares the time series. Typically needed for numerical solution methods.
        /// </summary>
        /// <param name="timeMax">The maximum time.</param>
        void PrepareTimeSeries(Time timeMax);

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        bool ModelIsDistributed { get; }

        /// <summary>
        /// The amount of substance release. Needed to calculate exposure fractions.
        /// </summary>
        double? AmountOfSubstance { get; }
    }
}