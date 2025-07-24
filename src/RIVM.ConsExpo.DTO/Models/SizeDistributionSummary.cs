using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Models
{
    public class SizeDistributionSummary
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Number of scenarios using this distribution")]
        public int NumberOfScenarios { get; set; }
    }
}