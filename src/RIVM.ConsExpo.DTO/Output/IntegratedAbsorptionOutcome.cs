using RIVM.ConsExpo.DTO.PhysicalQuantities;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// A class that can contain the outcome of an integrated calculation. It is capable of transforming the outcome to varying measures.
    /// </summary>
    public class IntegratedAbsorptionOutcome : AbsorptionOutcomeBase
    {
        public IntegratedAbsorptionOutcome(BodyWeight bodyWeight, Frequency frequency)
            : base(bodyWeight, frequency)
        { }

        public AbsorptionOutcomeBase DermalAbsorptionOutcome { get; set; }
        public InhalationAbsorptionOutcome InhalationAbsorptionOutcome { get; set; }
        public AbsorptionOutcomeBase OralAbsorptionOutcome { get; set; }

        public override Dose AsInternalYearAverageDose
        {
            get
            {
                var totalValue = (DermalAbsorptionOutcome?.AsInternalYearAverageDose?.Value ?? 0) + (InhalationAbsorptionOutcome?.AsInternalYearAverageDose?.Value ?? 0) + (OralAbsorptionOutcome?.AsInternalYearAverageDose?.Value ?? 0);
                return new Dose(totalValue, InternalYearAverageDoseUnit);
            }
        }

        public override Dose AsInternalEventDose
        {
            get
            {
                var totalValue = (DermalAbsorptionOutcome?.AsInternalEventDose?.Value ?? 0) + (InhalationAbsorptionOutcome?.AsInternalEventDose?.Value ?? 0) + (OralAbsorptionOutcome?.AsInternalEventDose?.Value ?? 0);
                return new Dose(totalValue, InternalEventDoseUnit);
            }
        }

        public override Dose AsInternalDayDose
        {
            get
            {
                var totalValue = (DermalAbsorptionOutcome?.AsInternalDayDose?.Value ?? 0) + (InhalationAbsorptionOutcome?.AsInternalDayDose?.Value ?? 0) + (OralAbsorptionOutcome?.AsInternalDayDose?.Value ?? 0);
                return new Dose(totalValue, InternalDayDoseUnit);
            }
        }
    }
}