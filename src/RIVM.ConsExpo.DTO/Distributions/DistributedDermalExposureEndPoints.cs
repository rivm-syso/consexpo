namespace RIVM.ConsExpo.DTO.Distributions
{
    /// <summary>
    /// Stores information about the presence of relevant distributed parameters for the dermal exposure end points of this route, in the scenario.
    /// </summary>

    public class DistributedDermalExposureEndPoints : DistributedExposureEndPointsBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether dermal load depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [dermal load is distributed]; otherwise, <c>false</c>.
        /// </value>
        public bool DermalLoadIsDistributed { get; set; }

        /// <summary>
        /// Gets a value indicating whether any end point for this exposure route depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [any end point distributed]; otherwise, <c>false</c>.
        /// </value>
        public override bool AnyEndPointDistributed
        {
            get { return DermalLoadIsDistributed || base.AnyEndPointDistributed; }
        }

        /// <summary>
        /// Gets a value indicating whether all of the endpoints for the dermal exposure route are distributed.
        /// </summary>
        public override bool AllEndPointsDistributed
        {
            get { return DermalLoadIsDistributed && base.AllEndPointsDistributed; }
        }
    }
}