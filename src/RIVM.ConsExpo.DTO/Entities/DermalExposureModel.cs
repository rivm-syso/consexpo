using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.Submodels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Attributes;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Entities
{
    public class DermalExposureModel
    {
        public DermalExposureModel()
        {
        }

        public DermalExposureModel(bool setDefaults)
        {
            if (setDefaults)
            {
                ProductAmount = new ProductAmount()
                {
                    Unit = PhysicalUnits.MassUnits.Gram
                };
                ContactDuration = new ExposureDuration()
                {
                    Unit = PhysicalUnits.DurationUnits.Minute
                };
                ExposedArea = new ExposedArea()
                {
                    Unit = PhysicalUnits.AreaUnits.SquareCentimetre
                };

                WeightFractionSubstance = new Fraction();
                ContactRate = new ContactRate();
                ReleaseDuration = new ReleaseDuration();
                LeachableFraction = new Fraction();
                SkinContactFactor = new Fraction();
                TransferCoefficient = new TransferCoefficient();
                DislodgeableAmount = new AreaDensity();
                ContactedSurface = new RubbingContactArea();
                ContactDuration = new ExposureDuration();
                SubstanceConcentration = new SubstanceConcentration()
                {
                    Unit = PhysicalUnits.DensityUnits.GramPerCubicCentimetre
                };
                DiffusionCoefficient = new DiffusionCoefficient();
                LayerThickness = new Thickness();
                ExposureDuration = new ExposureDuration();
                RetentionFactor = new Fraction()
                {
                    Value = 1.0,
                    Unit = FractionUnits.Fraction
                };
            }
        }

        [XmlIgnore]
        [BatchLineOverridable]
        public int Id { get; set; }

        [Display(Name = "Product amount")]
        public ProductAmount ProductAmount { get; set; }

        [Display(Name = "Weight fraction substance")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        [BatchLineOverridable]
        public Fraction WeightFractionSubstance { get; set; }

        private DermalExposureSubmodelTypes submodelType;

        public DermalExposureSubmodelTypes SubmodelType
        {
            get
            {
                return submodelType;
            }
            set
            {
                submodelType = value;
            }
        }

        [XmlIgnore]
        [NotMapped]
        public string ModelDescription
        {
            get
            {
                string dermalExposureRelease = EnumHelper2<DermalExposureSubmodelTypes>.GetDisplayValue(SubmodelType);
                return string.Format("Direct product contact – {0}", dermalExposureRelease);
            }
        }

        [Display(Name = "Contact rate")]
        public ContactRate ContactRate { get; set; }

        [Display(Name = "Release duration")]
        public ReleaseDuration ReleaseDuration { get; set; }

        [Display(Name = "Leachable fraction")]
        [BatchLineOverridable]
        public Fraction LeachableFraction { get; set; }

        [Display(Name = "Skin contact factor")]
        [BatchLineOverridable]
        public Fraction SkinContactFactor { get; set; }

        [Display(Name = "Transfer coefficient")]
        [BatchLineOverridable]
        public TransferCoefficient TransferCoefficient { get; set; }

        [Display(Name = "Dislodgeable amount")]
        public AreaDensity DislodgeableAmount { get; set; }

        [Display(Name = "Contacted surface")]
        public RubbingContactArea ContactedSurface { get; set; }

        [Display(Name = "Contact time")]
        public ExposureDuration ContactDuration { get; set; }

        [Display(Name = "Substance concentration")]
        [BatchLineOverridable]
        public SubstanceConcentration SubstanceConcentration { get; set; }

        [Display(Name = "Diffusion coefficient")]
        [BatchLineOverridable]
        public DiffusionCoefficient DiffusionCoefficient { get; set; }

        [Display(Name = "Layer thickness")]
        public Thickness LayerThickness { get; set; }

        [Display(Name = "Exposure time")]
        public ExposureDuration ExposureDuration { get; set; }

        [Display(Name = "Exposed area")]
        [BatchLineOverridable]
        public ExposedArea ExposedArea { get; set; }

        [Display(Name = "Retention factor")]
        public Fraction RetentionFactor { get; set; } = new Fraction()
        {
            Value = 1.0,
            Unit = FractionUnits.Fraction
        };
    }
}