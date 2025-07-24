using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RIVM.ConsExpo.Model.Submodels
{
    /// <summary>
    /// In this model, the substance is released from the packaging during storage and transferred to the contents.
    /// </summary>
    internal class OralExposureMigrationFromPackagingConstantRate : OralExposureBase, IOralExposureSubmodel
    {
        private const OralExposureSubmodelTypes type = OralExposureSubmodelTypes.MigrationFromPackagingConstantRate;

        public OralExposureSubmodelTypes Type => type;

        public OralExposureMigrationFromPackagingConstantRate(ScenarioModel scenario)
            : base(scenario, type)
        {
            this.scenario = scenario;
        }

        /// <summary>
        /// amount of compound ingested, independent of inTime
        /// it is assumed that the migrated compound is homogeneously dispersed in the product
        /// the amount of compound ingested then depends only on the amount of product (food) ingested
        /// and the total amount of compound migrated during storage from packaging material to food
        /// </summary>
        /// <returns></returns>
        /// <seealso>Borland C++ code: 'classOralScenarios.cpp' lines 663-738</seealso>
        private Dose InputToExposure()
        {
            // units (local): cm, gram, days
            double Ain;
            double Ap;
            double Cpack;
            double Vpack;
            double Atot;
            double Amigr;
            double Rmigr;
            double T;

            Ain = scenario.OralExposure.IngestedAmountPackaging.InGram();
            Ap = scenario.OralExposure.PackagedAmount.InGram();
            Cpack = scenario.OralExposure.SubstanceConcentration.InGramPerCubicCentimetre();
            Vpack = scenario.OralExposure.ThicknessPackaging.InCentimetre() * scenario.OralExposure.ContactAreaPackaging.InSquareCentimetre();
            Rmigr = scenario.OralExposure.MigrationRatePackaging.InGramPerDay();
            T = scenario.OralExposure.StorageTime.InDays();

            Atot = Vpack * Cpack; // total amount of substance available in system

            if (Rmigr * T > Atot)
            {
                Amigr = Atot;
            }
            else
            {
                Amigr = Rmigr * T;
            }

            double Dp;

            if (Ap > 0)
            {
                if (Ain >= Ap)
                {
                    Dp = Amigr;
                }
                else
                {
                    Dp = (Ain / Ap) * Amigr;
                }
            }
            else
            {
                Dp = 0;
            }

            Dose dose = new Dose(Dp * ConversionFactors.One2Milli, DoseUnits.Mg);

            return dose;
        }

        /// <summary>
        /// The amount of substance cannot be inferred. Null is returned.
        /// </summary>
        public override double? AmountOfSubstance => AmountOfSubstanceNotSupported;

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.OralExposureSubstanceConcentration,
                DTO.Models.ModelParameters.OralExposureThicknessPackaging,
                DTO.Models.ModelParameters.OralExposureContactAreaPackaging,
                DTO.Models.ModelParameters.OralExposurePackagedAmount,
                DTO.Models.ModelParameters.OralExposureIngestedAmountPackaging,
                DTO.Models.ModelParameters.OralExposureMigrationRatePackaging,
                DTO.Models.ModelParameters.OralExposureStorageTime
            };
            return modelParameters;
        }

        public override bool IsTimeDependent => false;

        public override Duration ApplicableExposureDuration => null;

        public IEnumerable<ValidationResult> Validate()
        {
            IList<ValidationResult> validationResults = new List<ValidationResult>();

            RequireIngestedAmount(validationResults);
            RequirePackagedAmount(validationResults);
            RequireSubstanceConcentration(validationResults);
            RequireThicknessPackaging(validationResults);
            RequireContactAreaPackaging(validationResults);
            RequireMigrationRatePackaging(validationResults);
            RequireStorageTime(validationResults);

            if (route.IngestedAmountPackaging.HasValue && route.PackagedAmount.HasValue)
            {
                if (route.IngestedAmountPackaging.InGram() > route.PackagedAmount.InGram())
                {
                    string packagedAmountDisplayName = ModelHelpers.GetDisplayName<OralExposureModel>(p => p.PackagedAmount);
                    string ingestedAmountDisplayName = ModelHelpers.GetDisplayName<OralExposureModel>(p => p.IngestedAmountPackaging);
                    validationResults.Add(new ValidationResult($"The {ingestedAmountDisplayName} must not be more than the {packagedAmountDisplayName}"));
                }
            }

            return validationResults;
        }

        public OralExposureOutcome CalculatePointValues()
        {
            var outcome = new OralExposureOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, AmountOfSubstance);
            outcome.Dose = InputToExposure();
            return outcome;
        }

        public OralExposureOutcome CalculatePointValues(Time time)
        {
            return CalculatePointValues();
        }

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        public override bool ModelIsDistributed =>
            route.IngestedAmountPackaging.IsDistributed
            || route.PackagedAmount.IsDistributed
            || route.SubstanceConcentration.IsDistributed
            || route.ThicknessPackaging.IsDistributed
            || route.ContactAreaPackaging.IsDistributed
            || route.MigrationRatePackaging.IsDistributed
            || route.StorageTime.IsDistributed;
    }
}