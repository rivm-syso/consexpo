using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using RIVM.ConsExpo.Model.SubModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RIVM.ConsExpo.Model.Submodels
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="RIVM.ConsExpo.Model.Interfaces.Submodels.IDermalAbsorptionSubmodel" />
    internal class DermalAbsorptionDiffusionThroughSkinForInstantApplication : DermalAbsorptionBase, IDermalAbsorptionSubmodel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DermalAbsorptionDiffusionThroughSkinForInstantApplication"/> class.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        public DermalAbsorptionDiffusionThroughSkinForInstantApplication(ScenarioModel scenario)
        {
            this.scenario = scenario;
        }

        private ScenarioModel scenario;

        private const DermalAbsorptionSubmodelTypes type = DermalAbsorptionSubmodelTypes.DiffusionThroughSkinForInstantApplication;

        /// <summary>
        /// Gets the type of submodel selected by the user for the dermal absorption route.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public DermalAbsorptionSubmodelTypes Type => type;

        /// <summary>
        /// Calculates the point values.
        /// </summary>
        /// <param name="exposure">The exposure.</param>
        /// <returns></returns>
        public DermalAbsorptionOutcome CalculatePointValues(DermalExposureOutcome exposure)
        {
            return CalculatePointValues(exposure, scenario.DermalAbsorption.ExposureDuration.AsTime());
        }

        /// <summary>
        /// Calculates the permeation of the skin by dU/dt = PxC(t)xA.
        /// </summary>
        /// <param name="exposure">The exposure.</param>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException"></exception>
        /// <exception cref="System.NotSupportedException"></exception>
        public DermalAbsorptionOutcome CalculatePointValues(DermalExposureOutcome exposure, Time time)
        {
            {
                DermalAbsorptionOutcome outcome;

                if (!scenario.DermalExposure.ExposedArea.HasValue)
                {
                    throw new ApplicationException(string.Format("Exposed area must be specified for calculation of dermal absorption with sub model '{0}'.", EnumHelper2<DermalAbsorptionSubmodelTypes>.GetDisplayValue(DermalAbsorptionSubmodelTypes.DiffusionThroughSkinForInstantApplication)));
                }

                // Local units: milligram, minute, cm.
                double C = scenario.DermalAbsorption.ConcentrationInMatrix.InMilligramPerCubicCentimetre();
                double P = scenario.DermalAbsorption.SkinPermeability.InCentimetrePerMinute();
                double A = scenario.DermalExposure.ExposedArea.InSquareCentimetre();
                double t = Math.Min(time.InMinutes(), scenario.DermalAbsorption.ExposureDuration.InMinutes());

                double Ao = scenario.DermalExposure.WeightFractionSubstance.AsFraction() * scenario.DermalExposure.ProductAmount.InMilligram();
                double dose;

                // Volume V in cm3
                double Vo;
                if (C > 0)
                {
                    Vo = Ao / C;
                }
                else
                {
                    Vo = 0;
                }

                if (Vo > 0)
                {
                    dose = Ao * (1 - Math.Exp(-(P * A / Vo) * t));
                }
                else
                {
                    dose = 0;
                }

                outcome = new DermalAbsorptionOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency);
                outcome.Dose = new Dose(dose, DoseUnits.Mg);

                return outcome;
            }
        }

        /// <summary>
        /// Determines for each end point of this route whether or not it depends on distributed parameters.
        /// </summary>
        /// <returns></returns>
        public DistributedAbsorptionEndPoints DistributedEndPoints(bool externalEventDoseIsDistributed)
        {
            var distributedAbsorptionEndPoints = new DistributedAbsorptionEndPoints();

            distributedAbsorptionEndPoints.InternalEventDoseIsDistributed =
                externalEventDoseIsDistributed
                || ModelIsDistributed
                || scenario.Assessment.Population.BodyWeight.IsDistributed;

            distributedAbsorptionEndPoints.InternalDayDoseIsDistributed =
                distributedAbsorptionEndPoints.InternalEventDoseIsDistributed
                || scenario.Frequency.IsDistributed;

            distributedAbsorptionEndPoints.InternalYearAverageDoseIsDistributed =
                distributedAbsorptionEndPoints.InternalEventDoseIsDistributed
                || scenario.Frequency.IsDistributed;

            return distributedAbsorptionEndPoints;
        }

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        public bool ModelIsDistributed =>
            scenario.DermalAbsorption.ConcentrationInMatrix.IsDistributed
            || scenario.DermalAbsorption.SkinPermeability.IsDistributed
            || scenario.DermalAbsorption.ExposureDuration.IsDistributed;

        /// <summary>
        /// Gets a value indicating whether the model is time dependent. Only if it is, it can be used in time series calculation.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is time dependent; otherwise, <c>false</c>.
        /// </value>
        public bool IsTimeDependent => true;

        public override List<DoseMeasureType> EndPointsForSensitivityAnalysis()
        {
            return base.EndPointsForSensitivityAnalysis();
        }

        /// <summary>
        /// The physical quantities that are available for sensitivity analysis when this submodel is in use.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
				// This model needs the skin area in its calculations.
				// modelParameters.Add(DTO.Models.ModelParameters.DermalExposureExposedArea);

				DTO.Models.ModelParameters.DermalAbsorptionConcentrationInMatrix,
                DTO.Models.ModelParameters.DermalAbsorptionSkinPermeability,
                DTO.Models.ModelParameters.DermalAbsorptionExposureDuration
            };
            return modelParameters;
        }

        /// <summary>
        /// Validates the specified scenario on completeness and consistency of the input parameters.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate()
        {
            const string MessageFormat = "'{{0}}' is required for dermal absorption submodel '{0}'.";

            IList<ValidationResult> validationResults = new List<ValidationResult>();

            var route = scenario.DermalAbsorption;

            Duration duration = route.ExposureDuration;

            if ((duration?.Value.HasValue ?? false) && duration.InDays() > 0 && scenario.Frequency.Value.HasValue && scenario.Frequency.InTimesPerDay() > (1.0 / duration.InDays()))
            {
                validationResults.Add(new ValidationResult(
                    $"The combination of a duration of {duration.Value.Value} {duration.UnitDisplay} and a frequency of {scenario.Frequency.Value.Value} {scenario.Frequency.UnitDisplay} results in overlapping events, which is not supported."));
            }

            string diffusionThroughSkinForInstantApplicationMessageFormat = string.Format(MessageFormat, EnumHelper2<DermalAbsorptionSubmodelTypes>.GetDisplayValue(DermalAbsorptionSubmodelTypes.DiffusionThroughSkinForInstantApplication));
            if (!scenario.DermalExposure.ExposedArea.HasValue)
            {
                validationResults.Add(new ValidationResult(string.Format(diffusionThroughSkinForInstantApplicationMessageFormat, "Exposed area")));
            }

            if (!route.ConcentrationInMatrix.HasValue)
            {
                validationResults.Add(new ValidationResult(string.Format(diffusionThroughSkinForInstantApplicationMessageFormat, "Concentration")));
            }

            if (!route.SkinPermeability.HasValue)
            {
                validationResults.Add(new ValidationResult(string.Format(diffusionThroughSkinForInstantApplicationMessageFormat, "SkinPermeability")));
            }

            if (!route.ExposureDuration.HasValue)
            {
                validationResults.Add(new ValidationResult(string.Format(diffusionThroughSkinForInstantApplicationMessageFormat, "ExposureDuration")));
            }

            return validationResults;
        }
    }
}