using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.Submodels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using RIVM.ConsExpo.DTO.Distributions;

namespace RIVM.ConsExpo.Model.Submodels
{
    /// <summary>
    /// Base class for all oral exposure submodels. Provides basic functionality which is used by the submodels.
    /// </summary>
    internal abstract class OralExposureBase : ExposureBase
    {
        private const string MessageFormatTemplate = "'{{0}}' is required for oral exposure submodel '{0}'.";

        private readonly string messageFormat;
        protected OralExposureModel route;

        protected OralExposureBase(ScenarioModel scenario, OralExposureSubmodelTypes type)
            : base(scenario)
        {
            this.messageFormat = string.Format(MessageFormatTemplate, EnumHelper2<OralExposureSubmodelTypes>.GetDisplayValue(type));
            this.route = scenario.OralExposure;
        }

        /// <summary>
        /// The amount of substance released (in mg): [Product amount] x [weight fraction]
        /// </summary>
        protected double AmountOfSubstanceByProductAmount
        {
            get
            {
                double productAmount = scenario.OralExposure.ProductAmount.InMilligram();
                double weightFractionSubstance = scenario.OralExposure.WeightFractionSubstance.AsFraction();

                return productAmount * weightFractionSubstance;
            }
        }

        protected void RequireWeightFractionSubstance(IList<ValidationResult> validationResults)
        {
            if (!route.WeightFractionSubstance.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.WeightFractionSubstance));
            }
        }

        protected void RequireAmountIngested(IList<ValidationResult> validationResults)
        {
            if (!route.IngestedAmountMouthing.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.IngestedAmountMouthing));
            }
        }

        protected void RequireProductAmount(IList<ValidationResult> validationResults)
        {
            if (!route.ProductAmount.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ProductAmount));
            }
        }

        protected void RequireExposureDuration(IList<ValidationResult> validationResults)
        {
            if (!route.ExposureDuration.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ExposureDuration));
            }
        }

        protected void RequireIngestionRate(IList<ValidationResult> validationResults)
        {
            if (!route.IngestionRate.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.IngestionRate));
            }
        }

        protected void RequireContactArea(IList<ValidationResult> validationResults)
        {
            if (!route.ContactAreaMouthing.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ContactAreaMouthing));
            }
        }

        protected void RequireInitialMigrationRate(IList<ValidationResult> validationResults)
        {
            if (!route.InitialMigrationRate.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.InitialMigrationRate));
            }
        }

        protected void RequireIngestedAmount(IList<ValidationResult> validationResults)
        {
            if (!route.IngestedAmountPackaging.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.IngestedAmountPackaging));
            }
        }

        protected void RequirePackagedAmount(IList<ValidationResult> validationResults)
        {
            if (!route.PackagedAmount.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.PackagedAmount));
            }
        }

        protected void RequireSubstanceConcentration(IList<ValidationResult> validationResults)
        {
            if (!route.SubstanceConcentration.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.SubstanceConcentration));
            }
        }

        protected void RequireThicknessPackaging(IList<ValidationResult> validationResults)
        {
            if (!route.ThicknessPackaging.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ThicknessPackaging));
            }
        }

        protected void RequireContactAreaPackaging(IList<ValidationResult> validationResults)
        {
            if (!route.ContactAreaPackaging.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ContactAreaPackaging));
            }
        }

        protected void RequireMigrationRatePackaging(IList<ValidationResult> validationResults)
        {
            if (!route.MigrationRatePackaging.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.MigrationRatePackaging));
            }
        }

        protected void RequireStorageTime(IList<ValidationResult> validationResults)
        {
            if (!route.StorageTime.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.StorageTime));
            }
        }

        /// <summary>
        /// Gets the validation message. Wraps the logic needed to select the property display name.
        /// </summary>
        /// <param name="modelProperty">The model property.</param>
        /// <returns></returns>
        private ValidationResult GetValidationMessage(Expression<Func<OralExposureModel, object>> modelProperty)
        {
            return new ValidationResult(string.Format(messageFormat, ModelHelpers.GetDisplayName<OralExposureModel>(modelProperty)));
        }

        public virtual List<DoseMeasureType> EndPointsForSensitivityAnalysis()
        {
#warning To Do: check relevant parameters values to see if the end point is really available.
            var endPoints = new List<DoseMeasureType>
            {
                DoseMeasureType.ExternalEventDose
            };

            return endPoints;
        }

        public abstract bool ModelIsDistributed { get; }

        public DistributedOralExposureEndPoints DistributedEndPoints
        {
            get
            {
                bool modelIsDistributed = ModelIsDistributed;

                DistributedOralExposureEndPoints endPoints = new DistributedOralExposureEndPoints();

                endPoints.ExternalEventDoseIsDistributed = modelIsDistributed || scenario.Assessment.Population.BodyWeight.IsDistributed;
                endPoints.ExternalDayDoseIsDistributed = endPoints.ExternalEventDoseIsDistributed || scenario.Frequency.IsDistributed;
                endPoints.ExposureFractionIsDistributed = modelIsDistributed;

                return endPoints;
            }
        }
    }
}