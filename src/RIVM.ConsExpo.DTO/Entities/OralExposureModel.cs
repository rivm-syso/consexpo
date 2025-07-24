using RIVM.ConsExpo.DTO.Attributes;
using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.Submodels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Entities
{
    public class OralExposureModel
    {
        [XmlIgnore]
        [BatchLineOverridable]
        public int Id { get; set; }

        [Display(Name = "Amount ingested")]
        [BatchLineOverridable]
        public ProductAmount IngestedAmountMouthing { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        [Display(Name = "Weight fraction substance")]
        [BatchLineOverridable]
        public Fraction WeightFractionSubstance { get; set; }

        private OralExposureModelTypes modelType;

        public OralExposureModelTypes ModelType
        {
            get
            {
                return modelType;
            }
            set
            {
                modelType = value;
            }
        }

        private OralExposureSubmodelTypes submodelType;

        public OralExposureSubmodelTypes SubmodelType
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
                string oralExposure = EnumHelper2<OralExposureModelTypes>.GetDisplayValue(ModelType);

                if (SubmodelType != OralExposureSubmodelTypes.SprayingNonRespirableMaterial)
                {
                    oralExposure += " – " + EnumHelper2<OralExposureSubmodelTypes>.GetDisplayValue(SubmodelType);
                }

                return oralExposure;
            }
        }

        public static OralExposureModelTypes GetModelType(OralExposureSubmodelTypes oralExposureSubmodelType)
        {
            switch (oralExposureSubmodelType)
            {
                case OralExposureSubmodelTypes.DirectIntake:
                case OralExposureSubmodelTypes.ConstantRate:
                case OralExposureSubmodelTypes.ProductMouthing:
                    return OralExposureModelTypes.Direct;

                case OralExposureSubmodelTypes.MigrationFromPackagingInstantRelease:
                case OralExposureSubmodelTypes.MigrationFromPackagingConstantRate:
                    return OralExposureModelTypes.Packaging;

                case OralExposureSubmodelTypes.SprayingNonRespirableMaterial:
                    return OralExposureModelTypes.Spray;

                default:
                    throw new NotSupportedException(string.Format("Unsupported inhalatory exposure submodel '{0}'", oralExposureSubmodelType.ToString()));
            }
        }

        public static List<OralExposureSubmodelTypes> AvailableSubmodels(OralExposureModelTypes modelType)
        {
            var submodels = new List<OralExposureSubmodelTypes>();

            switch (modelType)
            {
                case OralExposureModelTypes.Direct:
                    submodels.Add(OralExposureSubmodelTypes.DirectIntake);
                    submodels.Add(OralExposureSubmodelTypes.ConstantRate);
                    submodels.Add(OralExposureSubmodelTypes.ProductMouthing);

                    break;

                case OralExposureModelTypes.Packaging:
                    submodels.Add(OralExposureSubmodelTypes.MigrationFromPackagingInstantRelease);
                    submodels.Add(OralExposureSubmodelTypes.MigrationFromPackagingConstantRate);
                    break;

                case OralExposureModelTypes.Spray:
                    submodels.Add(OralExposureSubmodelTypes.SprayingNonRespirableMaterial);
                    break;

                default:
                    throw new NotSupportedException(string.Format("Unsupported modeltype '{0}'", modelType.ToString()));
            }
            return submodels;
        }

        public OralExposureModel()
        {
        }

        public OralExposureModel(bool setDefaults)
        {
            if (setDefaults)
            {
                IngestedAmountMouthing = new ProductAmount()
                {
                    Unit = PhysicalUnits.MassUnits.Gram
                };
                WeightFractionSubstance = new Fraction();
                ProductAmount = new ProductAmount();
                ExposureDuration = new ExposureDuration();
                IngestionRate = new IngestionRate();
                ContactAreaMouthing = new ContactAreaMouthing();
                InitialMigrationRate = new MigrationRate();
                ProductAmount = new ProductAmount();
                SubstanceConcentration = new SubstanceConcentrationPackaging()
                {
                    Unit = PhysicalUnits.DensityUnits.GramPerCubicCentimetre
                };
                ThicknessPackaging = new Thickness();
                ContactAreaPackaging = new ContactAreaPackaging();
                PackagedAmount = new ProductAmountPackaging()
                {
                    Unit = PhysicalUnits.MassUnits.Gram
                };
                IngestedAmountPackaging = new ProductAmountPackaging()
                {
                    Unit = PhysicalUnits.MassUnits.Gram
                };
                MigrationRatePackaging = new MassRatePackaging();
                StorageTime = new StorageTime()
                {
                    Unit = PhysicalUnits.DurationUnits.Day
                };
            }
        }

        [Display(Name = "Product amount")]
        public ProductAmount ProductAmount { get; set; }

        [Display(Name = "Exposure duration")]
        public ExposureDuration ExposureDuration { get; set; }

        [Display(Name = "Ingestion rate")]
        [BatchLineOverridable]
        public IngestionRate IngestionRate { get; set; }

        [Display(Name = "Contact area")]
        [BatchLineOverridable]
        public ContactAreaMouthing ContactAreaMouthing { get; set; }

        [Display(Name = "Initial migration rate")]
        [BatchLineOverridable]
        public MigrationRate InitialMigrationRate { get; set; }

        [Display(Name = "Substance concentration")]
        [BatchLineOverridable]
        public SubstanceConcentrationPackaging SubstanceConcentration { get; set; }

        [Display(Name = "Thickness packaging")]
        public Thickness ThicknessPackaging { get; set; }

        [Display(Name = "Contact area")]
        public ContactAreaPackaging ContactAreaPackaging { get; set; }

        [Display(Name = "Packaged amount")]
        public ProductAmountPackaging PackagedAmount { get; set; }

        [Display(Name = "Ingested amount")]
        public ProductAmountPackaging IngestedAmountPackaging { get; set; }

        [Display(Name = "Migration rate")]
        [BatchLineOverridable]
        public MassRatePackaging MigrationRatePackaging { get; set; }

        [Display(Name = "Storage time")]
        public StorageTime StorageTime { get; set; }
    }
}