#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Distributions
{
    /// <summary>
    /// Stores information about the presence of relevant distributed parameters for the end points in the scenario.
    /// </summary>
    public class DistributedEndPoints
    {
        public DistributedInhalationEndPoints Inhalation { get; set; }
        public DistributedDermalEndPoints Dermal { get; set; }
        public DistributedOralEndPoints Oral { get; set; }
        public DistributedIntegratedEndPoints Integrated { get; set; }

        public DistributedEndPoints()
        {
            Inhalation = new DistributedInhalationEndPoints();
            Dermal = new DistributedDermalEndPoints();
            Oral = new DistributedOralEndPoints();
            Integrated = new DistributedIntegratedEndPoints();
        }

        /// <summary>
        /// Determines which of the integrated end points are distributed and stores this result in this object.
        /// </summary>
        /// <remarks>This only works correctly if the distributed end points of all routes have been specified correctly.</remarks>
        public void ApplyIntegratedEndPoints()
        {
            Integrated.Exposure.ExternalEventDoseIsDistributed =
                Dermal.Exposure.ExternalEventDoseIsDistributed
                || Inhalation.Exposure.ExternalEventDoseIsDistributed
                || Oral.Exposure.ExternalEventDoseIsDistributed;

            Integrated.Exposure.ExternalDayDoseIsDistributed =
                Dermal.Exposure.ExternalDayDoseIsDistributed
                || Inhalation.Exposure.ExternalDayDoseIsDistributed
                || Oral.Exposure.ExternalDayDoseIsDistributed;

            Integrated.Absorption.InternalEventDoseIsDistributed =
                Dermal.Absorption.InternalEventDoseIsDistributed
                || Inhalation.Absorption.InternalEventDoseIsDistributed
                || Oral.Absorption.InternalEventDoseIsDistributed;

            Integrated.Absorption.InternalDayDoseIsDistributed =
                Dermal.Absorption.InternalDayDoseIsDistributed
                || Inhalation.Absorption.InternalDayDoseIsDistributed
                || Oral.Absorption.InternalDayDoseIsDistributed;

            Integrated.Absorption.InternalYearAverageDoseIsDistributed =
                Dermal.Absorption.InternalYearAverageDoseIsDistributed
                || Inhalation.Absorption.InternalYearAverageDoseIsDistributed
                || Oral.Absorption.InternalYearAverageDoseIsDistributed;
        }

        /// <summary>
        /// Gets a value indicating whether any end point in the scenario depends on a distributed parameter.
        /// </summary>
        /// <value>
        /// <c>true</c> if [any end point distributed]; otherwise, <c>false</c>.
        /// </value>
        public bool AnyEndPointDistributed => Inhalation.AnyEndPointDistributed || Dermal.AnyEndPointDistributed || Oral.AnyEndPointDistributed;
    }
}