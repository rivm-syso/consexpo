using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Entities
{
    public class DefaultPopulation
    {
        public int Id { get; set; }
        
        public int DefaultPopulationDatabaseId { get; set; }

        [MaxLength(45)]
        public string Name { get; set; }

        /// <summary>
        /// In Kg.
        /// </summary>
        [Display(Name = "Body weight")]
        public double? BodyWeightValue { get; set; }

        /// <summary>
        /// In cubic metre per day.
        /// </summary>
        [Display(Name = "Inhalation rate")]
        public double? InhalationRateValue { get; set; }

        public int Sort { get; set; }
    }
}