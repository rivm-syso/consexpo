#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Distributions
{
    /// <summary>
    /// Stores information about the presence of relevant distributed parameters for the integrated end points in the scenario.
    /// </summary>
    public class DistributedIntegratedEndPoints
    {
        public DistributedIntegratedExposureEndPoints Exposure { get; set; }
        public DistributedAbsorptionEndPoints Absorption { get; set; }

        public DistributedIntegratedEndPoints()
        {
            this.Exposure = new DistributedIntegratedExposureEndPoints();
            this.Absorption = new DistributedAbsorptionEndPoints();
        }

        /// <summary>
        /// Gets a value indicating whether any integrated end point depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [any end point distributed]; otherwise, <c>false</c>.
        /// </value>
        public bool AnyEndPointDistributed
        {
            get { return Exposure.AnyEndPointDistributed || Absorption.AnyEndPointDistributed; }
        }

        public bool AllEndPointsDistributed
        {
            get { return Exposure.AllEndPointsDistributed && Absorption.AllEndPointsDistributed; }
        }
    }
}