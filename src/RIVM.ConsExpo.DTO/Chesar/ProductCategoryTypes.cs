#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

using RIVM.ConsExpo.DTO.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RIVM.ConsExpo.DTO.Chesar
{
    public class ProductCategoryModel
    {
        [Key]
        [MaxLength(10)]
        public string Code { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        public int Sort { get; set; }

        [NotMapped]
        public string Description
        {
            get
            {
                return string.Format("{0}: {1}", Code, Name);
            }
        }

        public virtual List<ScenarioModel> Scenarios { get; set; }
    }
}