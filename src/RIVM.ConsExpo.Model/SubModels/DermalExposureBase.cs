using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.Submodels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace RIVM.ConsExpo.Model.Submodels
{
    /// <summary>
    /// Base class for all oral dermal submodels. Provides basic functionality which is used by the submodels.
    /// </summary>
    internal abstract class DermalExposureBase : ExposureBase
    {
        private const string MessageFormatTemplate = "'{{0}}' is required for dermal exposure submodel '{0}'.";

        private readonly string messageFormat;

        protected DermalExposureModel route;

        protected DermalExposureBase(ScenarioModel scenario, DermalExposureSubmodelTypes type)
            : base(scenario)
        {
            this.messageFormat = string.Format(MessageFormatTemplate, EnumHelper2<DermalExposureSubmodelTypes>.GetDisplayValue(type));
            this.route = scenario.DermalExposure;
        }

        /// <summary>
        /// The amount of substance (in mg) released: [Product amount] x [weight fraction]
        /// </summary>
        protected double AmountOfSubstanceByProductAmount
        {
            get
            {
                double productAmount = scenario.DermalExposure.ProductAmount.InMilligram();
                double weightFractionSubstance = scenario.DermalExposure.WeightFractionSubstance.AsFraction();

                return productAmount * weightFractionSubstance;
            }
        }

        protected void RequireProductAmount(IList<ValidationResult> validationResults)
        {
            if (!route.ProductAmount.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ProductAmount));
            }
        }

        protected void RequireWeightFractionSubstance(IList<ValidationResult> validationResults)
        {
            if (!route.WeightFractionSubstance.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.WeightFractionSubstance));
            }
        }

        protected void RequireLeachableFraction(IList<ValidationResult> validationResults)
        {
            if (!route.LeachableFraction.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.LeachableFraction));
            }
        }

        protected void RequireReleaseDuration(IList<ValidationResult> validationResults)
        {
            if (!route.ReleaseDuration.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ReleaseDuration));
            }
        }

        protected void RequireContactRate(IList<ValidationResult> validationResults)
        {
            if (!route.ContactRate.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ContactRate));
            }
        }

        protected void RequireSkinContactFactor(IList<ValidationResult> validationResults)
        {
            if (!route.SkinContactFactor.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.SkinContactFactor));
            }
        }

        protected void RequireTransferCoefficient(IList<ValidationResult> validationResults)
        {
            if (!route.TransferCoefficient.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.TransferCoefficient));
            }
        }

        protected void RequireDislodgeableAmount(IList<ValidationResult> validationResults)
        {
            if (!route.DislodgeableAmount.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.DislodgeableAmount));
            }
        }

        protected void RequireContactDuration(IList<ValidationResult> validationResults)
        {
            if (!route.ContactDuration.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ContactDuration));
            }
        }

        protected void RequireExposedArea(IList<ValidationResult> validationResults)
        {
            if (!route.ExposedArea.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ExposedArea));
            }
        }

        protected void RequireContactedSurface(IList<ValidationResult> validationResults)
        {
            if (!route.ContactedSurface.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ContactedSurface));
            }
        }

        protected void RequireSubstanceConcentration(IList<ValidationResult> validationResults)
        {
            if (!route.SubstanceConcentration.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.SubstanceConcentration));
            }
        }

        protected void RequireDiffusionCoefficient(IList<ValidationResult> validationResults)
        {
            if (!route.DiffusionCoefficient.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.DiffusionCoefficient));
            }
        }

        protected void RequireLayerThickness(IList<ValidationResult> validationResults)
        {
            if (!route.LayerThickness.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.LayerThickness));
            }
        }

        protected void RequireExposureTime(IList<ValidationResult> validationResults)
        {
            if (!route.ExposureDuration.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.ExposureDuration));
            }
        }

        protected void RequireRetentionFactor(IList<ValidationResult> validationResults)
        {
            if (!route.RetentionFactor.HasValue)
            {
                validationResults.Add(GetValidationMessage(p => p.RetentionFactor));
            }
        }

        /// <summary>
        /// Gets the validation message. Wraps the logic needed to selecte the property display name.
        /// </summary>
        /// <param name="modelProperty">The model property.</param>
        /// <returns></returns>
        private ValidationResult GetValidationMessage(Expression<Func<DermalExposureModel, object>> modelProperty)
        {
            return new ValidationResult(string.Format(messageFormat, ModelHelpers.GetDisplayName<DermalExposureModel>(modelProperty)));
        }

        public virtual List<DoseMeasureType> EndPointsForSensitivityAnalysis()
        {
#warning To Do: check relevant parameters values to see if the end point is really available.
            var endPoints = new List<DoseMeasureType>
            {
                DoseMeasureType.DermalLoad,
                DoseMeasureType.ExternalEventDose
            };

            return endPoints;
        }
    }
}