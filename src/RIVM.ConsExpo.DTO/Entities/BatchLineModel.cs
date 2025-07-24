using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Entities
{
    /// <summary>
    /// This model uses the same route/mode models (e.g. InhalationExposure), to avoid code duplication. However, only some of the parameters will be assignable in the UI for the Batch line. A benefit is, that if another parameter must be editable in the batch line, the entities already support it.
    /// </summary>
    public class BatchLineModel
    {
        public BatchLineModel()
        { }

        public BatchLineModel(bool setDefaults = false)
        {
            if (setDefaults)
            {
                InhalationExposure = new InhalationExposureModel(true);
                InhalationAbsorption = new InhalationAbsorptionModel(true);
                DermalExposure = new DermalExposureModel(true);
                DermalAbsorption = new DermalAbsorptionModel(true);
                OralExposure = new OralExposureModel(true);
                OralAbsorption = new OralAbsorptionModel(true);
            }
        }

        public int Id { get; set; }

        public int BatchAssessmentId { get; set; }

        public BatchAssessmentModel BatchAssessment { get; set; }

        /// <summary>
        /// Sort order. 1 based, increment 1.
        /// </summary>
        [Display(Name = "Sort order")]
        public int Sort { get; set; }

        
        [NotMapped]
        [Display(Name = "Assessment")]
        /// <remarks>Must reference an assessment of the user who owns the batch assessment.</remarks>
        public int AssessmentId { get; set; }

        /// <remarks>Must reference a scenario of the user who owns the batch assessment.</remarks>
        [Display(Name = "Scenario")]
        public int ScenarioId { get; set; }

        public virtual ScenarioModel Scenario { get; set; }

        [Display(Name = "Batch substance")]
        public int BatchSubstanceModelId { get; set; }

        public virtual BatchSubstanceModel BatchSubstance { get; set; }

        [Display(Name = "Default population")]
        public int DefaultPopulationId { get; set; }

        public virtual DefaultPopulation DefaultPopulation { get; set; }

        public int DermalExposureId { get; set; }

        public virtual DermalExposureModel DermalExposure { get; set; }

        public int DermalAbsorptionId { get; set; }

        public virtual DermalAbsorptionModel DermalAbsorption { get; set; }

        public int InhalationExposureId { get; set; }

        public virtual InhalationExposureModel InhalationExposure { get; set; }

        public int InhalationAbsorptionId { get; set; }

        public virtual InhalationAbsorptionModel InhalationAbsorption { get; set; }

        public int OralExposureId { get; set; }

        public virtual OralExposureModel OralExposure { get; set; }

        public int OralAbsorptionId { get; set; }

        public virtual OralAbsorptionModel OralAbsorption { get; set; }
    }
}