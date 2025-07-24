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
    public class DermalAbsorptionModel
    {
        public DermalAbsorptionModel()
        { }

        public DermalAbsorptionModel(bool setDefaults)
        {
            SubmodelType = DermalAbsorptionSubmodelTypes.Fraction;
            AbsorptionFraction = new Fraction();
            ConcentrationInMatrix = new SubstanceConcentration();
            SkinPermeability = new SkinPermeability();
            ExposureDuration = new ExposureDuration();
        }

        [XmlIgnore]
        [BatchLineOverridable]
        public int Id { get; set; }

        [Display(Name = "Absorption fraction")]
        [BatchLineOverridable]
        public Fraction AbsorptionFraction { get; set; }

        [Display(Name = "Model")]
        public DermalAbsorptionSubmodelTypes SubmodelType { get; set; }

        [XmlIgnore]
        [NotMapped]
        public string ModelDescription
        {
            get
            {
                string dermalAbsorptionRelease = EnumHelper2<DermalAbsorptionSubmodelTypes>.GetDisplayValue(SubmodelType);
                return dermalAbsorptionRelease;
            }
        }

        [Display(Name = "Concentration in matrix")]
        public SubstanceConcentration ConcentrationInMatrix { get; set; }

        [Display(Name = "Skin permeability")]
        public SkinPermeability SkinPermeability { get; set; }

        [Display(Name = "Exposure duration")]
        public ExposureDuration ExposureDuration { get; set; }
    }
}