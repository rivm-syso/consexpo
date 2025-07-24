using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Entities
{
    public class SimulationResultsModel
    {
        [Key]
        public int ScenarioId { get; set; }

        public virtual ScenarioModel Scenario { get; set; }

        public string SerializedResults { get; set; }
    }
}