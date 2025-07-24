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
    /// The implementation of the oral absorption submodel 'Fraction'.
    /// </summary>
    internal class OralAbsorptionFraction : IOralAbsorptionSubmodel
    {
        private const OralAbsorptionSubmodelTypes type = OralAbsorptionSubmodelTypes.Fraction;

        public OralAbsorptionSubmodelTypes Type => type;

        private ScenarioModel scenario;

        public OralAbsorptionFraction(ScenarioModel scenario)
        {
            this.scenario = scenario;
        }

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
                DTO.Models.ModelParameters.OralAbsorptionAbsorptionFraction
            };
            return modelParameters;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            const string MessageFormat = "'{{0}}' is required for oral absorption submodel '{0}'.";

            IList<ValidationResult> validationResults = new List<ValidationResult>();

            var route = scenario.OralAbsorption;

            string fractionMessageFormat = string.Format(MessageFormat, EnumHelper2<OralAbsorptionSubmodelTypes>.GetDisplayValue(OralAbsorptionSubmodelTypes.Fraction));
            if (!route.AbsorptionFraction.HasValue)
            {
                validationResults.Add(new ValidationResult(string.Format(fractionMessageFormat, ModelHelpers.GetDisplayName<OralAbsorptionModel>(p => p.AbsorptionFraction))));
            }

            return validationResults;
        }

        public OralAbsorptionOutcome CalculatePointValues(OralExposureOutcome exposure)
        {
            var outcome = new OralAbsorptionOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency);
            outcome.Dose = ExposureToAbsorption(scenario.OralAbsorption.AbsorptionFraction.AsFraction(), exposure.AsExternalEventDose);
            return outcome;
        }

        public OralAbsorptionOutcome CalculatePointValues(OralExposureOutcome exposureOutcome, Time time)
        {
            return CalculatePointValues(exposureOutcome);
        }

        private static Dose ExposureToAbsorption(double absorptionFraction, Dose exposureDoseValue)
        {
            double? absorptionDoseValue = exposureDoseValue.Value * absorptionFraction;

            return new Dose(absorptionDoseValue, exposureDoseValue.Unit);
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

            return distributedAbsorptionEndPoints;
        }

        public bool ModelIsDistributed => scenario.OralAbsorption.AbsorptionFraction.IsDistributed;
    }
}