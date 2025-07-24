namespace RIVM.ConsExpo.DTO.Distributions
{
    /// <summary>
    /// Stores information about the presence of relevant distributed parameters for the absorption end points of this route, in the scenario.
    /// </summary>
    public class DistributedAbsorptionEndPoints
    {
        /// <summary>
        /// Gets or sets a value indicating whether the internal event dose depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [internal event dose is distributed]; otherwise, <c>false</c>.
        /// </value>
        public bool InternalEventDoseIsDistributed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the internal day dose depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [internal day dose is distributed]; otherwise, <c>false</c>.
        /// </value>
        public bool InternalDayDoseIsDistributed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the internal year average dose depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [internal year average dose is distributed]; otherwise, <c>false</c>.
        /// </value>
        public bool InternalYearAverageDoseIsDistributed { get; set; }

        public bool PeakInternaldoseIsDistributed { get; set; }

        /// <summary>
        /// Gets a value indicating whether any absorption end point of this route depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [any end point distributed]; otherwise, <c>false</c>.
        /// </value>
        public bool AnyEndPointDistributed
        {
            get
            { return InternalEventDoseIsDistributed || InternalDayDoseIsDistributed || InternalYearAverageDoseIsDistributed; }
        }

        /// <summary>
        /// Gets a value indicating whether all of the endpoints for the distributed absorption route are distributed.
        /// </summary>
        public bool AllEndPointsDistributed
        {
            get { return InternalEventDoseIsDistributed && InternalDayDoseIsDistributed && InternalYearAverageDoseIsDistributed; }
        }
    }
}