#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// A class that can contain the outcome of a route.
    /// </summary>
    public class RouteOutcomes<E, A>
        where E : ExposureOutcomeBase
        where A : AbsorptionOutcomeBase
    {
        public E Exposure { get; set; }
        public A Absorption { get; set; }
    }
}