using RIVM.ConsExpo.DTO.PhysicalQuantities;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Entities
{
    public class ProductModel
    {
        public const int MaxNameLength = 40;

        public ProductModel()
        {
        }

        public ProductModel(bool setDefaults)
        {
            if (setDefaults)
            {
                WeightFractionSubstanceDefault = new Fraction();
            }
        }

        [Key]
        [XmlIgnore]
        public int AssessmentId { get; set; }

        [XmlIgnore]
        public virtual AssessmentModel Assessment { get; set; }

        [MaxLength(MaxNameLength)]
        public string Name { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        [Display(Name = "Weight fraction substance")]
        public Fraction WeightFractionSubstanceDefault { get; set; }
    }
}