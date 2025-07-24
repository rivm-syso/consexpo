using RIVM.ConsExpo.DTO.Distributions;

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// An interface for physical quantities, for displaying purposes.
    /// </summary>
    public interface IDistributablePhysicalQuantityBase : IPhysicalQuantityBase
    {
        /// <summary>
        /// Samples a random value from this instance's distribution.
        /// </summary>
        /// <returns></returns>
        void Sample();

        /// <summary>
        /// Gets the type of the distribution.
        /// </summary>
        /// <value>
        /// The type of the distribution.
        /// </value>
        DistributionTypes DistributionType { get; }

        /// <summary>
        /// Gets or sets the distribution.
        /// </summary>
        /// <value>
        /// The distribution.
        /// </value>
        Distribution Distribution { get; set; }
    }
}