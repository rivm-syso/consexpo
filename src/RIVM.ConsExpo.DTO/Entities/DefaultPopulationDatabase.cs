using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Entities
{
    public class DefaultPopulationDatabase
    {
        public DefaultPopulationDatabase()
        {
        }

        public int Id { get; set; }

        [MaxLength(45)]
        public string Name { get; set; }

        public virtual List<DefaultPopulation> DefaultPopulations { get; set; }

        public bool HasBodyWeightDefaults { get; set; }

        public bool HasInhalationRateDefaults { get; set; }

        public int Sort { get; set; }
    }
}