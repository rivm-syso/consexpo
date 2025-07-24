#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// The time series calculations return time series for all routes, which are stored in this class.
    /// </summary>
    public class TimeSeries
    {
        public InhalationTimeSeries Inhalation { get; set; }

        public DermalTimeSeries Dermal { get; set; }

        public OralTimeSeries Oral { get; set; }
    }
}