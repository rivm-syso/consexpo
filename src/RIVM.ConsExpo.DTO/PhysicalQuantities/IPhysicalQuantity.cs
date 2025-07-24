namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// An interface for physical quantities, for displaying purposes.
    /// </summary>
    public interface IPhysicalQuantity<T> : IPhysicalQuantityBase
    {
        /// <summary>
        /// Gets or sets the type specific unit of a physical quantity.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        T Unit { get; set; }
    }
}