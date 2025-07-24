using RIVM.ConsExpo.DTO.Output;

namespace RIVM.ConsExpo.DTO.Settings
{
    /// <summary>
    /// Settings that are used for the time series graphs, which are needed in addition to the data in the scenario model.
    /// </summary>
    public class TimeSeriesGraphSettings
    {
        /// <summary>
        /// Gets or sets the type of the dose measure.
        /// </summary>
        /// <value>
        /// The type of the dose measure.
        /// </value>
        public DoseMeasureType DoseMeasureType { get; set; }
    }
}