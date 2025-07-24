using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RIVM.ConsExpo.Model.Submodels
{
    /// <summary>
    /// Implementation of the dermal exposure submodel 'Rubbing Off'.
    /// </summary>
    internal class DermalExposureRubbingOff : DermalExposureBase, IDermalExposureSubmodel
    {
        private const DermalExposureSubmodelTypes type = DermalExposureSubmodelTypes.RubbingOff;

        public DermalExposureSubmodelTypes Type => type;

        public DermalExposureRubbingOff(ScenarioModel scenario)
            : base(scenario, type)
        {
            this.scenario = scenario;
        }

        /* B2B code  rubbing off:
        * - external event dose: rr 509-528
        * - internal event dose: rr 741-758
        * if (!inTime.IsParameterSet ()) {
         outDose.UnSet ();
         break;
       }

       if (inTime.GetDistributedNumber (kMinuteString) < GetReleaseDuration ().GetDistributedNumber (kMinuteString)) {
         theContactDuration = inTime.GetDistributedNumber (kMinuteString);
       }
       else {
         theContactDuration = GetReleaseDuration ().GetDistributedNumber (kMinuteString);
       }
       theWeightFraction = GetWeightFractionCompound ().GetDistributedNumber (kFractionString);
       theTransferCoefficient  = mTransferCoefficient.GetDistributedNumber (kCm2PerMinuteString);
       theDislodgeableFormulation = mDislodgeableFormulation.GetDistributedNumber (kMiligramPerCm2String);
       theRubbedSurface = mRubbedSurface.GetDistributedNumber (kCentimeterSquaredString);
       // not more rubbing than is in total on the rubbed surface
       theTotalSurface = min (theRubbedSurface, theTransferCoefficient * theContactDuration);
       outDose.SetInMiligrams (theTotalSurface * theWeightFraction * theDislodgeableFormulation);
       */

        /// <summary>
        /// Gets a value indicating whether this instance is time dependent.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is time dependent; otherwise, <c>false</c>.
        /// </value>
        public override bool IsTimeDependent => true;

        public override Duration ApplicableExposureDuration => route.ContactDuration;

        /// <summary>
        /// Gets the default time maximum for charts.
        /// </summary>
        /// <value>
        /// The default time maximum.
        /// </value>
        public override Time DefaultTimeMax => ApplicableExposureDuration.AsTime();

        /// <summary>
        /// The amount of substance cannot be inferred. Null is returned.
        /// </summary>
        public override double? AmountOfSubstance => AmountOfSubstanceNotSupported;

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.DermalExposureWeightFractionSubstance,
                DTO.Models.ModelParameters.DermalExposureTransferCoefficient,
                DTO.Models.ModelParameters.DermalExposureDislodgeableAmount,
                DTO.Models.ModelParameters.DermalExposureContactDuration,
                DTO.Models.ModelParameters.DermalExposureContactedSurface
            };
            return modelParameters;
        }

        public IEnumerable<ValidationResult> Validate()
        {
            IList<ValidationResult> validationResults = ValidateDurationAndFrequency(route.ContactDuration, scenario.Frequency);

            RequireWeightFractionSubstance(validationResults);
            RequireTransferCoefficient(validationResults);
            RequireDislodgeableAmount(validationResults);
            RequireContactDuration(validationResults);
            RequireContactedSurface(validationResults);

            return validationResults;
        }

        public DermalExposureOutcome CalculatePointValues()
        {
            return CalculatePointValues(route.ContactDuration.AsTime());
        }

        public DermalExposureOutcome CalculatePointValues(Time time)
        {
            var outcome = new DermalExposureOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, AmountOfSubstance, scenario.DermalExposure.ExposedArea);
            outcome.Dose = InputToExposure(time);
            return outcome;
        }

        private Dose InputToExposure(Time time)
        {
            double exposureDoseValue;
            double contactDuration;

            //Contact time in Second
            contactDuration = Math.Min(route.ContactDuration.InSeconds(), time.InSeconds());

            //Not more rubbing than is in total on the rubbed surface
            //In Metres.
            double totalSurface = Math.Min(route.ContactedSurface.InSquareMetre(),
                (route.TransferCoefficient.InSquareMetresPerSecond() * contactDuration));

            //ConsExpo sample code.
            //theWeightFraction = GetWeightFraction().GetInFraction();
            //theContactRate = mContactRate.GetInMiligramPerMinute() * theWeightFraction;
            //outDose.SetInMiligrams(theContactRate * theContactDuration);

            exposureDoseValue = totalSurface * route.WeightFractionSubstance.AsFraction() * route.DislodgeableAmount.InMilligramPerSquareMetre();

            var dose = new Dose(exposureDoseValue, DoseUnits.Mg);

            return dose;
        }

        public DistributedDermalExposureEndPoints DistributedEndPoints
        {
            get
            {
                bool modelIsDistributed = ModelIsDistributed;

                DistributedDermalExposureEndPoints endPoints = new DistributedDermalExposureEndPoints();

                endPoints.DermalLoadIsDistributed = modelIsDistributed || route.ExposedArea.IsDistributed;
                endPoints.ExternalEventDoseIsDistributed = modelIsDistributed || scenario.Assessment.Population.BodyWeight.IsDistributed;
                endPoints.ExternalDayDoseIsDistributed = endPoints.ExternalEventDoseIsDistributed || scenario.Frequency.IsDistributed;
                endPoints.ExposureFractionIsDistributed = modelIsDistributed;

                return endPoints;
            }
        }

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        public bool ModelIsDistributed =>
            route.WeightFractionSubstance.IsDistributed
            || route.TransferCoefficient.IsDistributed
            || route.DislodgeableAmount.IsDistributed
            || route.ContactDuration.IsDistributed
            || route.ContactedSurface.IsDistributed;
    }
}