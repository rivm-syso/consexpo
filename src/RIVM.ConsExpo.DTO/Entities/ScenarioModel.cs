using RIVM.ConsExpo.DTO.Chesar;
using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Entities
{
    [Serializable]
    public class ScenarioModel
    {
        public const int MaxNameLength = 100;

        [XmlIgnore]
        public int Id { get; set; }

        [Required]
        [XmlIgnore]
        public int AssessmentId { get; set; }

        [XmlIgnore]
        public virtual AssessmentModel Assessment { get; set; }

        [Required(ErrorMessage = "A scenario name is required", AllowEmptyStrings = false)]
        [MaxLength(MaxNameLength)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        public Frequency Frequency { get; set; }

        //Dermal Routes & Models
        public bool DermalExposureRouteInUse { get; set; }

        [Display(Name = "Annotation")]
        [MaxLength(4000)]
        public string DermalAnnotation { get; set; }

        [XmlIgnore]
        public int DermalExposureId { get; set; }

        public virtual DermalExposureModel DermalExposure { get; set; }

        public bool DermalAbsorptionRouteInUse { get; set; }

        [XmlIgnore]
        public int DermalAbsorptionId { get; set; }

        public virtual DermalAbsorptionModel DermalAbsorption { get; set; }

        //Inhalation Routes & Models
        public bool InhalationExposureRouteInUse { get; set; }

        [Display(Name = "Annotation")]
        [MaxLength(4000)]
        public string InhalationAnnotation { get; set; }

        [Obsolete("Spray and Vapour models are only distinguished in the UI. Need not be stored in the database.")]
        public int InhalationExposureRouteModelSelected { get; set; }

        [XmlIgnore]
        public int InhalationExposureId { get; set; }

        public virtual InhalationExposureModel InhalationExposure { get; set; }

        public bool InhalationAbsorptionRouteInUse { get; set; }

        [XmlIgnore]
        public int InhalationAbsorptionId { get; set; }

        public virtual InhalationAbsorptionModel InhalationAbsorption { get; set; }

        //Oral Routes & Models
        public bool OralExposureRouteInUse { get; set; }

        [Display(Name = "Annotation")]
        [MaxLength(4000)]
        public string OralAnnotation { get; set; }

        [XmlIgnore]
        public int OralExposureId { get; set; }

        public virtual OralExposureModel OralExposure { get; set; }

        public bool OralAbsorptionRouteInUse { get; set; }

        [XmlIgnore]
        public int OralAbsorptionId { get; set; }

        public virtual OralAbsorptionModel OralAbsorption { get; set; }

        [NotMapped]
        public bool HasSimulationResults
        { get { return SimulationResults != null; } }

        [XmlIgnore]
        public virtual SimulationResultsModel SimulationResults { get; set; }

        [NotMapped]
        public SimulationResults Results { get; set; }

        [XmlIgnore]
        public string ChesarProductCategoryCode { get; set; }

        [XmlIgnore]
        public virtual ProductCategoryModel ChesarProductCategory { get; set; }

        public ScenarioModel()
        { }

        public ScenarioModel(bool setDefaults)
        {
            if (setDefaults)
            {
                Frequency = new Frequency();
                DermalExposure = new DermalExposureModel(setDefaults);
                DermalAbsorption = new DermalAbsorptionModel(setDefaults);
                InhalationExposure = new InhalationExposureModel(setDefaults);
                InhalationAbsorption = new InhalationAbsorptionModel(setDefaults);
                OralExposure = new OralExposureModel(setDefaults);
                OralAbsorption = new OralAbsorptionModel(setDefaults);
            }
        }

        /// <summary>
        /// Assign a sample value from the distribution for all relevant parameters in the scenario.
        /// Parameters are sampled if they are in a route that is in use and if they are specified as a distribution.
        /// </summary>
        /// <remarks>
        /// Compare this with the ModelParameters enumeration.
        /// </remarks>
        public void SampleAll()
        {
            Assessment.Population.BodyWeight?.Sample();
            Assessment.Product.WeightFractionSubstanceDefault?.Sample(); //Not sure if this must be sampled at all.
            Assessment.Substance.Kow?.Sample();
            Frequency?.Sample();

            if (DermalExposureRouteInUse)
            {
                SampleHelper.SampleAll(DermalExposure);

                if (DermalAbsorptionRouteInUse)
                {
                    SampleHelper.SampleAll(DermalAbsorption);
                }
            }

            if (InhalationExposureRouteInUse)
            {
                SampleHelper.SampleAll(InhalationExposure);

                if (InhalationAbsorptionRouteInUse)
                {
                    SampleHelper.SampleAll(InhalationAbsorption);
                }
            }

            if (OralExposureRouteInUse)
            {
                SampleHelper.SampleAll(OralExposure);

                if (OralAbsorptionRouteInUse)
                {
                    SampleHelper.SampleAll(OralAbsorption);
                }
            }
        }

        public ScenarioModel ShallowCopy()
        {
            return (ScenarioModel)this.MemberwiseClone();
        }
    }
}