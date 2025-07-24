using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RIVM.ConsExpo.Model.Submodels
{
    /// <summary>
    /// The implementation of the inhalation absorption submodel 'Fraction'.
    /// </summary>
    internal class InhalationAbsorptionFraction : IInhalationAbsorptionSubmodel
    {
        private const InhalationAbsorptionSubmodelTypes type = InhalationAbsorptionSubmodelTypes.Fraction;

        private ScenarioModel scenario;

        public InhalationAbsorptionFraction(ScenarioModel scenario)
        {
            this.scenario = scenario;
        }

        /// <summary>
        /// Calculates the dose measures of a model simulation.
        /// </summary>
        /// <param name="exposureOutcome">The exposure outcome.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException"></exception>
        public InhalationAbsorptionOutcome CalculatePointValues(InhalationExposureOutcome exposureOutcome)
        {
            var outputValues = new InhalationAbsorptionOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, scenario.InhalationExposure.ReEntry);

            outputValues.Dose = ExposureToAbsorption(exposureOutcome);

            if (scenario.InhalationExposure.ReEntry)
            {
                var value = exposureOutcome.AsMeanYearConcentration.Value *
                    scenario.InhalationExposure.DailyDuration.InSecondsPerDay() *
                    scenario.InhalationExposure.InhalationRate.InCubicMetresPerSecond() *
                    scenario.InhalationAbsorption.AbsorptionFraction.AsFraction() /
                    scenario.Assessment.Population.BodyWeight.InKilogram();

                outputValues.InternalYearAverageDoseReEntry = new Dose(value, DoseUnits.MgPerKgBodyWeightPerDay);
            }

            return outputValues;
        }

        public InhalationAbsorptionOutcome CalculatePointValues(InhalationExposureOutcome exposureOutcome, Time time)
        {
            var outputValues = new InhalationAbsorptionOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency);

            outputValues.Dose = ExposureToAbsorption(exposureOutcome);

            return outputValues;
        }

        public InhalationAbsorptionSubmodelTypes Type => type;

        public bool IsTimeDependent => false;

        public List<DoseMeasureType> EndPointsForSensitivityAnalysis()
        {
#warning To Do: check relevant parameters values to see if the end point is really available.
            var endPoints = new List<DoseMeasureType>
            {
                DoseMeasureType.InternalEventDose
            };

            return endPoints;
        }

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.InhalationAbsorptionAbsorptionFraction
            };
            return modelParameters;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            const string MessageFormat = "'{{0}}' is required for inhalation absorption submodel '{0}'.";

            IList<ValidationResult> validationResults = new List<ValidationResult>();

            var route = scenario.InhalationAbsorption;

            string fractionMessageFormat = string.Format(MessageFormat, EnumHelper2<InhalationAbsorptionSubmodelTypes>.GetDisplayValue(InhalationAbsorptionSubmodelTypes.Fraction));

            if (!route.AbsorptionFraction.HasValue)
            {
                validationResults.Add(new ValidationResult(string.Format(fractionMessageFormat, ModelHelpers.GetDisplayName<InhalationAbsorptionModel>(p => p.AbsorptionFraction))));
            }

            return validationResults;
        }

        /// <summary>
        /// Implementation of the inhalation absorption submodel 'Fixed fraction'.
        /// </summary>
        /// <param name="exposureOutcome">The exposure outcome.</param>
        /// <returns></returns>
        /// Devent  = C x fabs x Qinh x Texp/body weight
        /// met
        /// C = exposure mean event concentration
        /// fabs = absorption fraction
        /// Qinh = inhalation rate
        /// Texp = exposure duration (uit het exposure model)
        public Dose ExposureToAbsorption(InhalationExposureOutcome exposureOutcome)
        {
            double absorptionFraction = scenario.InhalationAbsorption.AbsorptionFraction.AsFraction();

            Dose exposureDose = exposureOutcome.AsExternalEventDose;

            double? absorptionDoseValue = exposureDose.Value * absorptionFraction;

            return new Dose(absorptionDoseValue, exposureDose.Unit);
        }

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

            distributedAbsorptionEndPoints.PeakInternaldoseIsDistributed = externalEventDoseIsDistributed
                || ModelIsDistributed
                || scenario.Assessment.Population.BodyWeight.IsDistributed;

            return distributedAbsorptionEndPoints;
        }

        public bool ModelIsDistributed => scenario.InhalationAbsorption.AbsorptionFraction.IsDistributed;
    }
}