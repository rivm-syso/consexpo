using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Entities
{
    /// <summary>
    /// The properties of the substance that is subject to exposure simulations.
    /// </summary>
    public class SubstanceModel
    {
        [Key]
        [XmlIgnore]
        public int AssessmentId { get; set; }

        [XmlIgnore]
        public virtual AssessmentModel Assessment { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(12)]
        [Display(Name = "CAS number")]
        [RegularExpression("^\\d{2,7}-\\d{2}-\\d$", ErrorMessage = "Invalid CAS number format")]
        [CasNumberChecksum(ErrorMessage = "Invalid CAS number checksum")]
        public string CASNumber { get; set; }

        /// <summary>
        /// The weight of the quantity of one mol of the pure substance.
        /// </summary>
        [UIHint("PhysicalQuantity")]
        [Display(Name = "Molecular weight")]
        public MolecularWeight MolecularWeight { get; set; }

        /// <see cref="Kow"/>
        public Kow Kow { get; set; }

        public SubstanceModel()
        {
        }

        public SubstanceModel(bool setDefaults)
        {
            if (setDefaults)
            {
                MolecularWeight = new MolecularWeight();
                Kow = new Kow(true);
            }
        }
    }
}