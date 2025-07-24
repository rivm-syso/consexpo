using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.Submodels;

namespace RIVM.ConsExpo.Model.Interfaces.Submodels
{
    /// <summary>
    /// Interface for the oral absorption simulation submodels.
    /// </summary>
    public interface IOralAbsorptionSubmodel : IAbsorptionSubmodel<OralExposureOutcome, OralAbsorptionOutcome>
    {
        /// <summary>
        /// Gets the type of submodel selected by the user for the oral absorption route.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        OralAbsorptionSubmodelTypes Type { get; }
    }
}