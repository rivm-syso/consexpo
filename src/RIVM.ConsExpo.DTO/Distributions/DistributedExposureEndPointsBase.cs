namespace RIVM.ConsExpo.DTO.Distributions
{
    /// <summary>
    /// Stores information about the presence of relevant distributed parameters for the exposure end points of this route, in the scenario.
    /// </summary>
    public abstract class DistributedExposureEndPointsBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the external event dose for this route depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [external event dose is distributed]; otherwise, <c>false</c>.
        /// </value>
        public bool ExternalEventDoseIsDistributed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the external day dose for this route depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [external day dose is distributed]; otherwise, <c>false</c>.
        /// </value>
        public bool ExternalDayDoseIsDistributed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the exposure fraction for this route depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [exposure fraction is distributed]; otherwise, <c>false</c>.
        /// </value>
        public bool ExposureFractionIsDistributed { get; set; }

        /// <summary>
        /// Gets a value indicating whether any end point for this exposure route depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [any end point distributed]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool AnyEndPointDistributed =>
            ExternalEventDoseIsDistributed
            || ExternalDayDoseIsDistributed
            || ExposureFractionIsDistributed;

        /// <summary>
        /// Gets a value indicating whether all of the endpoints for the distributed exposure route are distributed.
        /// </summary>
        public virtual bool AllEndPointsDistributed =>
            ExternalEventDoseIsDistributed &&
            ExternalDayDoseIsDistributed &&
            ExposureFractionIsDistributed;
    }
}