using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using RIVM.ConsExpo.Model.SubModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RIVM.ConsExpo.Model.Submodels
{
    /// <summary>
    /// The implementation of the dermal absorption submodel 'Fraction'.
    /// </summary>
    internal class DermalAbsorptionFraction : DermalAbsorptionBase, IDermalAbsorptionSubmodel
    {
        private readonly DermalAbsorptionSubmodelTypes type = DermalAbsorptionSubmodelTypes.Fraction;

        public DermalAbsorptionSubmodelTypes Type => type;

        private ScenarioModel scenario;

        public DermalAbsorptionFraction(ScenarioModel scenario)
        {
            this.scenario = scenario;
        }

        public bool IsTimeDependent => false;

        public override List<DoseMeasureType> EndPointsForSensitivityAnalysis()
        {
            return base.EndPointsForSensitivityAnalysis();
        }

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.DermalAbsorptionAbsorptionFraction
            };
            return modelParameters;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            const string MessageFormat = "'{{0}}' is required for dermal absorption submodel '{0}'.";

            IList<ValidationResult> validationResults = new List<ValidationResult>();

            var route = scenario.DermalAbsorption;

            string FractionMessageFormat = string.Format(MessageFormat, EnumHelper2<DermalAbsorptionSubmodelTypes>.GetDisplayValue(DermalAbsorptionSubmodelTypes.Fraction));
            if (!route.AbsorptionFraction.HasValue)
            {
                validationResults.Add(new ValidationResult(string.Format(FractionMessageFormat, "Fraction")));
            }

            return validationResults;
        }

        public DermalAbsorptionOutcome CalculatePointValues(DermalExposureOutcome exposure)
        {
            var outcome = new DermalAbsorptionOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency);
            outcome.Dose = ExposureToAbsorption(scenario.DermalAbsorption.AbsorptionFraction.AsFraction(), exposure.AsExternalEventDose);
            return outcome;
        }

        /// <summary>
        /// Since absorption itself (in this submodel) is not time dependent, it can be ignored.
        /// </summary>
        /// <param name="exposure">The exposure.</param>
        /// <param name="time">The time at which the absorption must be calculated.</param>
        /// <returns></returns>
        public DermalAbsorptionOutcome CalculatePointValues(DermalExposureOutcome exposure, Time time)
        {
            return CalculatePointValues(exposure);
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

        public bool ModelIsDistributed => scenario.DermalAbsorption.AbsorptionFraction.IsDistributed;
    }
}