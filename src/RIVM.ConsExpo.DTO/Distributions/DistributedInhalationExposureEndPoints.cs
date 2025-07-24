namespace RIVM.ConsExpo.DTO.Distributions
{
    /// <summary>
    /// Stores information about the presence of relevant distributed parameters for the inhalation exposure end points, in the scenario.
    /// </summary>
    public class DistributedInhalationExposureEndPoints : DistributedExposureEndPointsBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the mean event concentration depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [mean event concentration is distributed]; otherwise, <c>false</c>.
        /// </value>
        public bool MeanEventConcentrationIsDistributed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mean day concentration depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [mean day concentration is distributed]; otherwise, <c>false</c>.
        /// </value>
        public bool MeanDayConcentrationIsDistributed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mean year concentration depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [mean year concentration is distributed]; otherwise, <c>false</c>.
        /// </value>
        public bool MeanYearConcentrationIsDistributed { get; set; }

        public bool PeakConcentrationIsDistributed { get; set; }
        public bool PeakExternaldoseIsDistributed { get; set; }

        /// <summary>
        /// Gets a value indicating whether any end point for the inhalation exposure route depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [any end point distributed]; otherwise, <c>false</c>.
        /// </value>
        public override bool AnyEndPointDistributed =>
            MeanEventConcentrationIsDistributed ||
            MeanDayConcentrationIsDistributed ||
            MeanYearConcentrationIsDistributed ||
            base.AnyEndPointDistributed;

        /// <summary>
        /// Gets a value indicating whether all of the endpoints for the inhalation exposure route are distributed.
        /// </summary>
        public override bool AllEndPointsDistributed =>
            MeanEventConcentrationIsDistributed &&
            MeanDayConcentrationIsDistributed &&
            MeanYearConcentrationIsDistributed &&
            base.AllEndPointsDistributed;
    }
}