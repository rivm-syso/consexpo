#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Distributions
{
    /// <summary>
    /// Stores information about the presence of relevant distributed parameters for the inhalation end points in the scenario.
    /// </summary>
    public class DistributedInhalationEndPoints
    {
        public DistributedInhalationExposureEndPoints Exposure { get; set; }
        public DistributedAbsorptionEndPoints Absorption { get; set; }

        public DistributedInhalationEndPoints()
        {
            this.Exposure = new DistributedInhalationExposureEndPoints();
            this.Absorption = new DistributedAbsorptionEndPoints();
        }

        /// <summary>
        /// Gets a value indicating whether any inhalation end point depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [any end point distributed]; otherwise, <c>false</c>.
        /// </value>
        public virtual bool AnyEndPointDistributed => Exposure.AnyEndPointDistributed || Absorption.AnyEndPointDistributed;

        public bool AllEndPointsDistributed
        {
#warning Tech Debt: this test ignores the possibility that a route is not in use. In that case it will not be distributed. But all end points in use are distributed.
            get { return Exposure.AllEndPointsDistributed && Absorption.AllEndPointsDistributed; }
        }
    }
}