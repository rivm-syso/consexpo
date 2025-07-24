using RIVM.ConsExpo.DTO.Attributes;
using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.Submodels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Entities
{
    public class InhalationAbsorptionModel
    {
        public InhalationAbsorptionModel()
        {
        }

        public InhalationAbsorptionModel(bool setDefaults)
        {
            SubmodelType = InhalationAbsorptionSubmodelTypes.Fraction;
            AbsorptionFraction = new Fraction();
        }

        [XmlIgnore]
        [BatchLineOverridable]
        public int Id { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        [Display(Name = "Absorption fraction")]
        [BatchLineOverridable]
        public Fraction AbsorptionFraction { get; set; }

        public InhalationAbsorptionSubmodelTypes SubmodelType { get; set; }

        [XmlIgnore]
        [NotMapped]
        public string ModelDescription
        {
            get
            {
                string InhalationAbsorptionRelease = EnumHelper2<InhalationAbsorptionSubmodelTypes>.GetDisplayValue(SubmodelType);
                return InhalationAbsorptionRelease;
            }
        }
    }
}