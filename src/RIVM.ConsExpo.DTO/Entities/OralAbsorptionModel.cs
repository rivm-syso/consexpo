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
    public class OralAbsorptionModel
    {
        public OralAbsorptionModel()
        {
        }

        public OralAbsorptionModel(bool setDefaults)
        {
            AbsorptionFraction = new Fraction();
        }

        [XmlIgnore]
        [BatchLineOverridable]
        public int Id { get; set; }

        [Display(Name = "Absorption fraction")]
        [BatchLineOverridable]
        public Fraction AbsorptionFraction { get; set; }

        public OralAbsorptionSubmodelTypes SubmodelType { get; set; }

        [XmlIgnore]
        [NotMapped]
        public string ModelDescription
        {
            get
            {
                string oralAbsorptionRelease = EnumHelper2<OralAbsorptionSubmodelTypes>.GetDisplayValue(SubmodelType);
                return oralAbsorptionRelease;
            }
        }
    }
}