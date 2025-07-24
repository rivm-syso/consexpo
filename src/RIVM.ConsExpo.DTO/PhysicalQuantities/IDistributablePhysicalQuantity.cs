namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// An interface for physical quantities, for displaying purposes.
    /// </summary>
    public interface IDistributablePhysicalQuantity<T> : IDistributablePhysicalQuantityBase, IPhysicalQuantity<T>
    {
    }
}