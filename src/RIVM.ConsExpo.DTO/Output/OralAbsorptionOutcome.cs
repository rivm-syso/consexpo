using RIVM.ConsExpo.DTO.PhysicalQuantities;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// A class that can contain the outcome of an oral calculation. It is capable of transforming the outcome to varying measures.
    /// </summary>
    public class OralAbsorptionOutcome : AbsorptionOutcomeBase
    {
        public OralAbsorptionOutcome(BodyWeight bodyWeight, Frequency frequency)
            : base(bodyWeight, frequency)
        { }
    }
}