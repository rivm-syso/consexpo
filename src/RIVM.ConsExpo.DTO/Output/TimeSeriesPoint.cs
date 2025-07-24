using RIVM.ConsExpo.DTO.PhysicalQuantities;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// A class that can contain the data for one point in time of a time series.
    /// </summary>
    public class TimeSeriesPoint<E, A>
        where E : ExposureOutcomeBase
        where A : AbsorptionOutcomeBase
    {
        public Time Time { get; set; }
        public E Exposure { get; set; }
        public A Absorption { get; set; }
    }
}