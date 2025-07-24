using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.PhysicalQuantities;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Entities
{
    /// <summary>
    /// A entry for a substance, in the list of substances for one user, to be used in batch assessments.
    /// </summary>
    public class BatchSubstanceModel
    {
        public BatchSubstanceModel()
        {
        }

        public BatchSubstanceModel(bool setDefaults)
        {
            if (setDefaults)
            {
                MolecularWeight = new MolecularWeight();
                Kow = new Kow();
                VapourPressure = new Pressure();
                SkinPermeability = new SkinPermeability();
                MassTransferCoefficient = new MassTransferCoefficient();
                WeightFractionSubstance = new Fraction();
                // Not in use?
                // WeightFractionNonVolatile
                DensityNonVolatile = new DensityNonVolatile();
                InhalationAbsorptionFraction = new Fraction();
                DermalAbsorptionFraction = new Fraction();
                OralAbsorptionFraction = new Fraction();
            }
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public virtual UserModel User { get; set; }

        public const int MaxNameLength = 200;

        [Required(AllowEmptyStrings = false)]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        [Display(Name = "CAS number")]
        [RegularExpression("^\\d{2,7}-\\d{2}-\\d$", ErrorMessage = "Invalid CAS number format")]
        [CasNumberChecksum(ErrorMessage = "Invalid CAS number checksum")]
        public string CASNumber { get; set; }

        [UIHint("PhysicalQuantity")]
        [Display(Name = "Molecular weight")]
        public MolecularWeight MolecularWeight { get; set; }

        public Kow Kow { get; set; }

        [Display(Name = "Vapour pressure")]
        public Pressure VapourPressure { get; set; }

        [Display(Name = "Skin permeability")]
        public SkinPermeability SkinPermeability { get; set; }

        [Display(Name = "Mass transfer coefficient")]
        public MassTransferCoefficient MassTransferCoefficient { get; set; }

        [Display(Name = "Weight fraction substance")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        public Fraction WeightFractionSubstance { get; set; }

#warning Not supported?
        // Weight fraction non-volatile
        // public Fraction WeightFractionNonVolatile { get; set; }

        [Display(Name = "Density non volatile")]
        public DensityNonVolatile DensityNonVolatile { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        [Display(Name = "Inhalation absorption fraction")]
        public Fraction InhalationAbsorptionFraction { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        [Display(Name = "Dermal absorption fraction")]
        public Fraction DermalAbsorptionFraction { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        [Display(Name = "Oral absorption fraction")]
        public Fraction OralAbsorptionFraction { get; set; }
    }
}