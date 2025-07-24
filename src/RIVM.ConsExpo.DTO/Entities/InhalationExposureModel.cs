using DataAnnotationsExtensions;
using RIVM.ConsExpo.DTO.Attributes;
using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.Parameters;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Entities
{
    public class InhalationExposureModel
    {
        public InhalationExposureModel()
        {
        }

        public InhalationExposureModel(bool setDefaults)
        {
            if (setDefaults)
            {
                ExposureDuration = new ExposureDuration();
                EmissionDurationReEntry = new EmissionDurationReEntry();
                DailyDuration = new DailyDuration();
                ReleasedMass = new ProductAmount();
                WeightFractionSubstance = new Fraction();
                WeightFractionSubstanceForEmission = new WeightFractionSubstanceForEmission();
                RoomVolume = new RoomVolume();
                VentilationRate = new Rate();
                InhalationRate = new VolumeRate();
                ProductThickness = new ThicknessForEmission();
                ProductDensity = new DensitySolid();
                ProductAirPartitionCoefficient = new ProductAirPartitionCoefficient(setDefaults);
                ExposureDurationForEmissionModel = new IntermediateDuration();
                StartExposure = new IntermediateDuration();
                VapourPressure = new Pressure();
                ApplicationTemperature = new Temperature();
                AirborneFraction = new Fraction();
                RoomHeight = new Height();
                CloudVolume = new CloudVolume();
                DensityNonVolatile = new DensityNonVolatile();

                MassGenerationRate = new MassGenerationRate();
                SprayDuration = new SprayDuration();
                ReleaseArea = new ReleaseArea();
                ProductSurfaceArea = new ReleaseArea();
                EmissionDurationEvaporation = new EmissionDurationEvaporation();
                ApplicationDuration = new ApplicationDuration();
                MolecularWeightMatrix = new MolecularWeight();
                MedianDiameter = new FixedDiameter();
                MeanDiameter = new FixedDiameter();
                StandardDeviation = new DiameterStandardDeviation();

                ProductAmount = new ProductAmount()
                {
                    Unit = MassUnits.Gram
                };

                ApplicationTemperature = new Temperature()
                {
                    Unit = TemperatureUnits.Celsius
                };

                VapourPressure = new Pressure()
                {
                    Unit = PressureUnits.Pascal
                };

                EmissionDuration = new EmissionDuration()
                {
                    Unit = DurationUnits.Minute
                };

                DailyDuration = new DailyDuration()
                {
                    Unit = DailyDurationUnits.MinutesPerDay
                };

                InhalationRate = new VolumeRate()
                {
                    Unit = VolumeRateUnits.CubicMetrePerHour
                };

                ProductSurfaceArea = new ReleaseArea()
                {
                    Unit = AreaUnits.SquareMetre
                };

                ProductDensity = new DensitySolid()
                {
                    Unit = DensityUnits.GramPerCubicCentimetre
                };

                DiffusionCoefficientForEmission = new DiffusionCoefficientForEmission()
                {
                    Unit = SurfaceRateUnits.SquareMetrePerSecond
                };

                MassTransferCoefficient = new MassTransferCoefficient();
                InhalationCutOffDiameter = new Diameter();
                MaximumDiameter = new FixedDiameter();
                Dilution = new Dilution();

                ReplaceValuesWithDefaults(false);
            }
        }

        /// <summary>
        /// Method to reset values to their default values and units. Most notably used when the scenario is created from a fact sheet and some values are not specified in the fact sheet.
        /// </summary>
        /// <param name="onlyIfValueNull">if set to <c>true</c> only replace the defaults if their value is null.</param>
        public void ReplaceValuesWithDefaults(bool onlyIfValueNull)
        {
            if (MassTransferCoefficient.Value == null || !onlyIfValueNull)
            {
                MassTransferCoefficient.Value = 10;
                MassTransferCoefficient.Unit = VelocityUnits.MetrePerHour;
            }

            if (InhalationCutOffDiameter.Value == null || !onlyIfValueNull)
            {
                InhalationCutOffDiameter.Value = 15;
                InhalationCutOffDiameter.Unit = LengthUnits.Micrometre;
            }

            if (MaximumDiameter.Value == null || !onlyIfValueNull)
            {
                MaximumDiameter.Value = 50;
                MaximumDiameter.Unit = LengthUnits.Micrometre;
            }

            if (Dilution.Value == null || !onlyIfValueNull)
            {
                Dilution.Value = Dilution.DefaultValue;
                Dilution.Unit = FactorUnits.Times;
            }
        }

        public InhalationExposureModelTypes ModelType { get; set; }

        public InhalationExposureSubmodelTypes SubmodelType { get; set; }

        public static InhalationExposureModelTypes GetModelType(InhalationExposureSubmodelTypes inhalationExposureSubmodelType)
        {
            switch (inhalationExposureSubmodelType)
            {
                case InhalationExposureSubmodelTypes.VapourConstantRate:
                case InhalationExposureSubmodelTypes.VapourInstantaneousRelease:
                case InhalationExposureSubmodelTypes.VapourEvaporation:
                    return InhalationExposureModelTypes.Vapour;

                case InhalationExposureSubmodelTypes.SprayInstantaneousRelease:
                case InhalationExposureSubmodelTypes.SpraySpraying:
                    return InhalationExposureModelTypes.Spray;

                case InhalationExposureSubmodelTypes.EmissionFromSolidMaterials:
                    return InhalationExposureModelTypes.Emission;

                default:
                    throw new NotSupportedException(string.Format("Unsupported inhalatory exposure submodel '{0}'", inhalationExposureSubmodelType.ToString()));
            }
        }

        public static List<InhalationExposureSubmodelTypes> AvailableSubmodels(InhalationExposureModelTypes modelType)
        {
            var submodels = new List<InhalationExposureSubmodelTypes>();

            switch (modelType)
            {
                case InhalationExposureModelTypes.Vapour:
                    submodels.Add(InhalationExposureSubmodelTypes.VapourInstantaneousRelease);
                    submodels.Add(InhalationExposureSubmodelTypes.VapourConstantRate);
                    submodels.Add(InhalationExposureSubmodelTypes.VapourEvaporation);

                    break;

                case InhalationExposureModelTypes.Spray:
                    submodels.Add(InhalationExposureSubmodelTypes.SprayInstantaneousRelease);
                    submodels.Add(InhalationExposureSubmodelTypes.SpraySpraying);
                    break;

                case InhalationExposureModelTypes.Emission:
                    submodels.Add(InhalationExposureSubmodelTypes.EmissionFromSolidMaterials);
                    break;

                default:
                    throw new NotSupportedException(string.Format("Unsupported modeltype '{0}'", modelType.ToString()));
            }
            return submodels;
        }

        [XmlIgnore]
        [NotMapped]
        public string ModelDescription
        {
            get
            {
                string inhalationExposure = EnumHelper2<InhalationExposureModelTypes>.GetDisplayValue(ModelType);

                if (ModelType != InhalationExposureModelTypes.Emission)
                {
                    inhalationExposure += " – " + EnumHelper2<InhalationExposureSubmodelTypes>.GetDisplayValue(SubmodelType);
                }

                return inhalationExposure;
            }
        }

        [XmlIgnore]
        [BatchLineOverridable]
        public int Id { get; set; }

        [UIHint("PhysicalQuantity")]
        [Display(Name = "Emission duration")]
        public EmissionDuration EmissionDuration { get; set; }

        private bool _reEntry;

        [Display(Name = "Re-entry")]
        public bool ReEntry
        {
            get
            {
                return _reEntry && SupportsReEntry(SubmodelType);
            }
            set
            {
                _reEntry = value;
            }
        }

        private bool SupportsReEntry(InhalationExposureSubmodelTypes submodelType)
        {
            switch (submodelType)
            {
                case InhalationExposureSubmodelTypes.VapourConstantRate:
                case InhalationExposureSubmodelTypes.VapourEvaporation:
                case InhalationExposureSubmodelTypes.EmissionFromSolidMaterials:
                    return true;
            }

            return false;
        }

        [Display(Name = "Emission duration")]
        public EmissionDurationReEntry EmissionDurationReEntry { get; set; }

        [Display(Name = "Daily exposure duration")]
        public DailyDuration DailyDuration { get; set; }

        [Display(Name = "Exposure duration")]
        public ExposureDuration ExposureDuration { get; set; }

        /// <summary>
        /// In most of the scenario's, this is the amount of product used, but when the product is used in a dilution (which currently is possible for evaporation scenario's), this is the amount of solution used.
        /// </summary>
        [Display(Name = "Product amount")]
        public ProductAmount ProductAmount { get; set; }

        [Display(Name = "Released mass")]
        public ProductAmount ReleasedMass { get; set; }

        [Display(Name = "Weight fraction substance")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        [BatchLineOverridable]
        public Fraction WeightFractionSubstance { get; set; }

        [Display(Name = "Weight fraction substance")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:G3}")]
        public WeightFractionSubstanceForEmission WeightFractionSubstanceForEmission { get; set; }

        [Display(Name = "Room volume")]
        public RoomVolume RoomVolume { get; set; }

        [Display(Name = "Ventilation rate")]
        public Rate VentilationRate { get; set; }

        [Display(Name = "Inhalation rate")]
        [BatchLineOverridable]
        public VolumeRate InhalationRate { get; set; }

        [Display(Name = "Limit concentration to saturated air concentration")]
        public bool LimitConcentrationToSaturatedAirConcentration { get; set; }

        /// <summary>
        /// Similar to Layer thickness, but with another name.
        /// </summary>
        /// <value>
        /// The product thickness.
        /// </value>
        [Display(Name = "Product thickness")]
        public ThicknessForEmission ProductThickness { get; set; }

        [Display(Name = "Product density")]
        public DensitySolid ProductDensity { get; set; } //Product Density (g/cm3, mg/cm3, kg/l, g/m3, kg/m3)

        [Display(Name = "Diffusion coefficient")]
        [BatchLineOverridable]
        public DiffusionCoefficientForEmission DiffusionCoefficientForEmission { get; set; } //Diffusion coefficient (m2/hr, cm2/hr, m2/s, cm2/s)

        /// <summary>
        /// Gets or sets the product air partition coefficient.
        /// </summary>
        /// <value>
        /// The product air partition coefficient. This is not a fraction. It can be larger than 1.
        /// </value>
        [Display(Name = "Product/air partition coefficient")]
        [BatchLineOverridable]
        public ProductAirPartitionCoefficient ProductAirPartitionCoefficient { get; set; }

        /// <summary>
        /// Exposure duration, specific for the emission model, as a duration of minutes is not useful in that model.
        /// </summary>
        /// <value>
        /// The duration of the exposure.
        /// </value>
        [Display(Name = "Exposure duration")]
        public IntermediateDuration ExposureDurationForEmissionModel { get; set; }

        [Display(Name = "Start exposure")] // (hr, days)
        public IntermediateDuration StartExposure { get; set; }

        /// <summary>
        /// The vapour pressure of the substance at the application temperature.
        /// </summary>
        [Display(Name = "Vapour pressure")]
        [BatchLineOverridable]
        public Pressure VapourPressure { get; set; }

        [Display(Name = "Application temperature")]
        public Temperature ApplicationTemperature { get; set; }

        #region Parameters specific for submodel Inhalation - exposure to spray - spraying.

        [Display(Name = "Airborne fraction")]
        public Fraction AirborneFraction { get; set; }

        [Display(Name = "Room height")]
        public Height RoomHeight { get; set; }

        [Display(Name = "Cloud volume")]
        public CloudVolume CloudVolume { get; set; }

        [Display(Name = "Density non volatile")]
        [BatchLineOverridable]
        public DensityNonVolatile DensityNonVolatile { get; set; }

        [Display(Name = "Inhalation cut off diameter")]
        public Diameter InhalationCutOffDiameter { get; set; }

        [Display(Name = "Mass generation rate")]
        public MassGenerationRate MassGenerationRate { get; set; }

        [Display(Name = "Spray duration")]
        public SprayDuration SprayDuration { get; set; }

        [Display(Name = "Spraying towards person")]
        public bool SprayingTowardsPerson { get; set; }

        /// <summary>
        /// The type of distribution specified in this instance.
        /// </summary>
        /// <remarks>The value of this type implies which parameters must be used for the distribution</remarks>
        [Display(Name = "Aerosol diameter distribution type")]
        [XmlIgnore]
        public SizeDistributionTypes AerosolDiameterDistributionType { get; set; }

        [NotMapped]
        public int AerosolDiameterDistribution
        {
            get
            {
                return (int)AerosolDiameterDistributionType;
            }
            set
            {
                AerosolDiameterDistributionType = (SizeDistributionTypes)value;
            }
        }

        [Display(Name = "Maximum diameter")]
        public FixedDiameter MaximumDiameter { get; set; }

        [Display(Name = "Mean diameter")]
        public FixedDiameter MeanDiameter { get; set; }

        [Display(Name = "Median diameter")]
        public FixedDiameter MedianDiameter { get; set; }

        [Display(Name = "Standard deviation")]
        public DiameterStandardDeviation StandardDeviation { get; set; }

        [Display(Name = "Arithmetic coefficient of variation")]
        [Min(1E-3)] //Prevent a 'spike' distribution, because sampling it might miss the spike. 1E-3 = Min(StandardDeviation) / 10 (10 µm is a reasonable mean/median size).
        public double? ArithmicCoefficientOfVariation { get; set; }

        [XmlIgnore]
        [Display(Name = "Distribution")]
        public int? NonParametricSizeDistributionId { get; set; }

        public virtual NonParametricSizeDistribution NonParametricSizeDistribution { get; set; }

        [Display(Name = "Release area")]
        public ReleaseArea ReleaseArea { get; set; }

        [Display(Name = "Release area type")]
        public InhalationExposureReleaseAreaTypes ReleaseAreaType { get; set; }

        /// <summary>
        /// Gets or sets the product surface area.
        /// </summary>
        /// <value>
        /// The product surface area. Similar to release area, but with a different name.
        /// </value>
        [Display(Name = "Product surface area")]
        public ReleaseArea ProductSurfaceArea { get; set; }

        [Display(Name = "Emission duration")]
        public EmissionDurationEvaporation EmissionDurationEvaporation { get; set; }

        [Display(Name = "Application duration")]
        public ApplicationDuration ApplicationDuration { get; set; }

        [Display(Name = "Mass transfer coefficient")]
        public MassTransferCoefficient MassTransferCoefficient { get; set; }

        [Display(Name = "Pure form")]
        [BatchLineOverridable]
        public bool PureForm { get; set; }

        [Display(Name = "Molecular weight matrix")]
        public MolecularWeight MolecularWeightMatrix { get; set; }

        [Display(Name = "Include oral non-respirable material exposure")]
        public bool IncludeOralNonRespirableMaterialExposure { get; set; }

        [Display(Name = "The product is used in dilution")]
        public bool ProductInDilution { get; set; }

        public Dilution Dilution { get; set; }

        #endregion Parameters specific for submodel Inhalation - exposure to spray - spraying.
    }
}