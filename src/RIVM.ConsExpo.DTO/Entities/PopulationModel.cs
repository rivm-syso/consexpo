using RIVM.ConsExpo.DTO.PhysicalQuantities;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Entities
{
    public class PopulationModel
    {
        public PopulationModel()
        {
        }

        public PopulationModel(bool setDefaults)
        {
            if (setDefaults)
            {
                BodyWeight = new BodyWeight();
            }
        }

        [Key]
        [XmlIgnore]
        public int AssessmentId { get; set; }

        [XmlIgnore]
        public virtual AssessmentModel Assessment { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [Display(Name = "Body weight")]
        public BodyWeight BodyWeight { get; set; }
    }
}