using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.Submodels;

namespace RIVM.ConsExpo.Model.Interfaces.Submodels
{
    /// <summary>
    /// Interface for the inhalation absorption simulation submodels.
    /// </summary>
    public interface IInhalationAbsorptionSubmodel : IAbsorptionSubmodel<InhalationExposureOutcome, InhalationAbsorptionOutcome>
    {
        /// <summary>
        /// Gets the type of submodel selected by the user for the inhalation absorption route.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        InhalationAbsorptionSubmodelTypes Type { get; }
    }
}