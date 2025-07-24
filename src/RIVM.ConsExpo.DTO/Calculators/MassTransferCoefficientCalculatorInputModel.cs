using RIVM.ConsExpo.DTO.PhysicalQuantities;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Calculators
{
    /// <summary>
    /// A model that will store the information entered by the user, while using the calculator from the scenario view.
    /// </summary>
    public class MassTransferCoefficientCalculatorInputModel
    {
        public Temperature ApplicationTemperature { get; set; }

        public MolecularWeight MolecularWeight { get; set; }

        public MassTransferCoefficient MassTransferCoefficient { get; set; }

        public string MassTransferCoefficientEstimateMethod { get; set; }

        public int AssessmentId { get; set; }
    }
}