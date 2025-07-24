using RIVM.ConsExpo.DTO.Distributions;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// The end point calculations end points for each active route. These are stored in this data structure. They can be either single points or distributions.
    /// </summary>
    public class Outcomes
    {
        public Outcomes()
        {
            Dermal = new EndPoint<DermalExposureOutcome, DermalAbsorptionOutcome>();
            Inhalation = new EndPoint<InhalationExposureOutcome, InhalationAbsorptionOutcome>();
            Oral = new EndPoint<OralExposureOutcome, OralAbsorptionOutcome>();
            Integrated = new EndPoint<IntegratedExposureOutcome, IntegratedAbsorptionOutcome>();
        }

        public DistributedEndPoints DistributedEndPoints { get; set; }

        public EndPoint<DermalExposureOutcome, DermalAbsorptionOutcome> Dermal { get; set; }

        public EndPoint<InhalationExposureOutcome, InhalationAbsorptionOutcome> Inhalation { get; set; }

        public EndPoint<OralExposureOutcome, OralAbsorptionOutcome> Oral { get; set; }

        public EndPoint<IntegratedExposureOutcome, IntegratedAbsorptionOutcome> Integrated { get; set; }
    }
}