using RIVM.ConsExpo.DTO.PhysicalQuantities;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// A class that can contain the outcome of an inhalation calculation. It is capable of transforming the outcome to varying measures.
    /// </summary>
    public class InhalationAbsorptionOutcome : AbsorptionOutcomeBase
    {
        private readonly bool _reEntry;

        public InhalationAbsorptionOutcome(BodyWeight bodyWeight, Frequency frequency, bool reEntry = false)
            : base(bodyWeight, frequency)
        {
            _reEntry = reEntry;
        }

        /// <summary>
        /// This value is to be set explicitly, in case of re-entry, as many additional parameters (inhalation rate, average year concentration) are needed to transform in from another dose measure.
        /// </summary>
        public Dose InternalYearAverageDoseReEntry { get; set; }

        public override Dose AsInternalYearAverageDose
        {
            get
            {
                if (_reEntry)
                {
                    return InternalYearAverageDoseReEntry;
                }
                else
                {
                    return base.AsInternalYearAverageDose;
                }
            }
        }
    }
}