using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Output;

namespace RIVM.ConsExpo.Model.Interfaces.Submodels
{
    /// <summary>
    /// The common interface for all absorption submodels that implement calculations for a route.
    /// </summary>
    public interface IAbsorptionSubmodel<TExposureOutcome, TAbsorptionOutcome> : ISubmodel
        where TAbsorptionOutcome : AbsorptionOutcomeBase
        where TExposureOutcome : ExposureOutcomeBase
    {
        /// <summary>
        /// Calculates the point values.
        /// </summary>
        /// <param name="exposure">The exposure.</param>
        /// <returns></returns>
        TAbsorptionOutcome CalculatePointValues(TExposureOutcome exposure);

        ///<summary>
        /// Determines for each end point of this route whether or not it depends on distributed parameters.
        /// </summary>
        DistributedAbsorptionEndPoints DistributedEndPoints(bool externalEventDoseIsDistributed);

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        bool ModelIsDistributed { get; }
    }
}