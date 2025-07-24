using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Interfaces.Submodels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RIVM.ConsExpo.Model.Submodels
{
    /// <summary>
    /// Implementation of the inhalation exposure model 'Constant rate'.
    /// </summary>
    internal class InhalationExposureVapourConstantRate : InhalationExposureBase, IInhalationExposureSubmodel
    {
        private const InhalationExposureSubmodelTypes type = InhalationExposureSubmodelTypes.VapourConstantRate;

        public InhalationExposureSubmodelTypes Type => type;

        /// <summary>
        /// Initializes a new instance of the <see cref="InhalationExposureVapourConstantRate"/> class.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        public InhalationExposureVapourConstantRate(ScenarioModel scenario)
            : base(scenario, type, true)
        { }

        /// <summary>
        /// The amount of substance released (in mg): [Product amount] x [weight fraction]
        /// </summary>
        public override double? AmountOfSubstance => AmountOfSubstanceByProductAmount;

        public List<ModelParameters> ModelParameters()
        {
            var modelParameters = new List<ModelParameters>
            {
                DTO.Models.ModelParameters.InhalationExposureProductAmount,
                DTO.Models.ModelParameters.InhalationExposureWeightFractionSubstance,
                DTO.Models.ModelParameters.InhalationExposureRoomVolume,
                DTO.Models.ModelParameters.InhalationExposureVentilationRate,
            };

            if (scenario.InhalationExposure.ReEntry)
            {
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureEmissionDurationReEntry);
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureDailyDuration);
            }
            else
            {
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureExposureDuration);
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureEmissionDuration);
            }

            if (scenario.InhalationExposure.LimitConcentrationToSaturatedAirConcentration)
            {
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureVapourPressure);
                modelParameters.Add(DTO.Models.ModelParameters.InhalationExposureApplicationTemperature);
                modelParameters.Add(DTO.Models.ModelParameters.AssessmentMolecularWeight);
            }

            return modelParameters;
        }

        public override bool IsTimeDependent => true;

        public override Duration ApplicableExposureDuration
        {
            get
            {
                if (route.ReEntry)
                {
                    return route.EmissionDurationReEntry;
                }

                return route.ExposureDuration;
            }
        }

        public override bool SupportsPeakAirConcentration => true;

        public override bool SupportsMeanDayConcentration
        {
            get
            {
                if (scenario.InhalationExposure.ReEntry)
                {
                    return false;
                }
                return true;
            }
        }

        public override bool SupportsExternalDayDose
        {
            get
            {
                if (scenario.InhalationExposure.ReEntry)
                {
                    return false;
                }
                return true;
            }
        }

        public override bool SupportsInternalDayDose
        {
            get
            {
                if (scenario.InhalationExposure.ReEntry)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Ideal gas constant
        /// </summary>
        private const double R = PhysicalConstants.GasConstant;

        /// <summary>
        /// Total available product amount
        /// </summary>
        private double P;

        /// <summary>
        /// Weight fraction of substance in released product
        /// </summary>
        private double f;

        /// <summary>
        /// The release duration
        /// </summary>
        private double t_R;

        /// <summary>
        /// The exposure duration
        /// </summary>
        private double t_E;	    //

        /// <summary>
        /// The time at which saturation occurs
        /// </summary>
        private double t_sat;

        /// <summary>
        /// Time at which the concentration would have dropped back to the level of saturation, if no saturation occurred.
        /// </summary>
        private double t_F;

        /// <summary>
        /// The mass release rate of substance
        /// </summary>
        private double E;

        /// <summary>
        /// The ventilation rate, the frequency of air changes
        /// </summary>
        private double q;

        /// <summary>
        /// The room volume
        /// </summary>
        private double V;

        /// <summary>
        /// The air concentration of the evaporated substance at the end of the release duration
        /// </summary>
        private double C_R;

        /// <summary>
        /// The air concentration of the evaporated substance at saturation
        /// </summary>
        private double C_sat;

        /// <summary>
        /// Molecular weight of the substance
        /// </summary>
        private double M;

        /// <summary>
        /// Vapour pressure of the substance at saturation
        /// </summary>
        private double p_sat;

        /// <summary>
        /// Room temperature
        /// </summary>
        private double T;

        private bool limitConcentrationToSaturatedAirConcentration;

        private void ParseScenario()
        {
            if (scenario.InhalationExposure.ReEntry)
            {
                t_R = scenario.InhalationExposure.EmissionDurationReEntry.InSeconds();
                t_E = t_R;
            }
            else
            {
                t_R = scenario.InhalationExposure.EmissionDuration.InSeconds();
                t_E = scenario.InhalationExposure.ExposureDuration.InSeconds();
            }
            P = scenario.InhalationExposure.ProductAmount.InMilligram();
            f = scenario.InhalationExposure.WeightFractionSubstance.AsFraction();
            q = scenario.InhalationExposure.VentilationRate.InTimesPerSecond();
            V = scenario.InhalationExposure.RoomVolume.InCubicMetres();

            limitConcentrationToSaturatedAirConcentration = scenario.InhalationExposure.LimitConcentrationToSaturatedAirConcentration;
            if (limitConcentrationToSaturatedAirConcentration)
            {
                M = scenario.Assessment.Substance.MolecularWeight.InMgPerMol();
                p_sat = scenario.InhalationExposure.VapourPressure.InPascal();
                T = scenario.InhalationExposure.ApplicationTemperature.InKelvin();
            }

            E = P * f / t_R;                                                            // (A.1)

            if (q > FloatingPointZero)
            {
                C_R = (E / (q * V)) * (1 - Math.Exp(-(q * t_R)));                       // (A.4) at t = t_R
            }
            else
            {
                C_R = E * t_R / V;
            }
        }

        /// <summary>
        /// Calculates the time-averaged air concentration for the specified scenario.
        /// </summary>
        /// <param name="time">The end time for calculation of the mean</param>
        /// <returns></returns>
        public override AirConcentration MeanAirConcentration(Time time)
        {
            ParseScenario();

            double t = time.InSeconds();
            double C_avg;

            if (t == 0)
            {
                return InstantaneousAirConcentration(time);
            }
            else if (limitConcentrationToSaturatedAirConcentration)
            {
                C_sat = M * p_sat / (R * T);                                    // (B.1)

                if (C_R < C_sat)
                {
                    C_avg = MeanAirConcentrationWithoutSaturation(t_R, t, E, q, V, C_R);
                }
                else
                {
                    // Saturation occurs.
                    if (q > FloatingPointZero)
                    {
                        t_sat = -(Math.Log(1 - q * V * C_sat / E)) / q;         // (B.2)
                    }
                    else
                    {
                        t_sat = V * C_sat / E;                                  // (B.2); lim q-> 0
                    }

                    if (t < t_sat)
                    {
                        // Saturation occurs after the point in time we are looking at.
                        C_avg = MeanAirConcentrationExposureLessThanReleaseAndNoSaturation(t, E, q, V);
                    }
                    else
                    {
                        if (q > FloatingPointZero)
                        {
                            t_F = GetT_F();

                            if (t < t_F)
                            {
                                C_avg = ((E / (q * V) * (t_sat + (Math.Exp(-q * t_sat) - 1) / q)) + C_sat * (t - t_sat)) / t;
                                // (B.6)
                            }
                            else
                            {
                                C_avg = ((E / (q * V) * (t_sat + (Math.Exp(-q * t_sat) - 1) / q)) + C_sat * (t_F - t_sat) + (C_sat / q) * (1 - Math.Exp(-q * (t - t_F)))) / t;
                                // (B.8)
                            }
                        }
                        else
                        {
                            // Saturation occurs and ventilation is zero or low so the air concentration will not drop below saturation.
                            C_avg = C_sat * (1 - t_sat / t) + E * (t_sat * t_sat / (2 * t * V)); // (B.6); lim q->0.
                        }
                    }
                }
            }
            else
            {
                C_avg = MeanAirConcentrationWithoutSaturation(t_R, t, E, q, V, C_R);
            }

            return new AirConcentration()
            {
                Value = C_avg,
                Unit = DensityUnits.MilligramPerCubicMetre
            };
        }

        public override TimeInterval PeakInterval(Time time)
        {
            double startIntervalValue;
            double endIntervalValue;

            if (q > FloatingPointZero)
            {
                startIntervalValue = 1 / q * Math.Log(1 + (1 - Math.Exp(-q * t_R)) * Math.Exp(-q * (time.InSeconds() - t_R)));
            }
            else
            {
                startIntervalValue = t_R;
            }

            endIntervalValue = startIntervalValue + time.InSeconds();

            if (endIntervalValue > t_E)
            {
                endIntervalValue = t_E;
                startIntervalValue = endIntervalValue - time.InSeconds();
            };

            if (startIntervalValue < 0)
            {
                startIntervalValue = 0.0;
            }

            return new TimeInterval(startIntervalValue, endIntervalValue, TimeUnits.Second);
        }

        /// <summary>
        /// Gets the time at which concentration drops below saturation after having been at saturation for a while.
        /// </summary>
        /// <returns></returns>
        /// <remarks>q must be larger than 0.</remarks>
        private double GetT_F()
        {
            double tF;
            tF = -Math.Log(C_sat / C_R) / q + t_R;
            return tF;
        }

        protected static double MeanAirConcentrationWithoutSaturation(double t_R, double t_E, double E, double q, double V, double C_R)
        {
            double C_avg;
            if (t_E < t_R)
            {
                C_avg = MeanAirConcentrationExposureLessThanReleaseAndNoSaturation(t_E, E, q, V);
            }
            else
            {
                if (q > FloatingPointZero)
                {
                    //Exposure duration more than release duration
                    C_avg = (E / (q * V * t_E)) * (t_R + ((Math.Exp(-q * t_R) - 1) / (q))) +
                        (C_R / (q * t_E)) * (1 - Math.Exp(-q * (t_E - t_R)));               // (A.12)
                }
                else
                {
                    C_avg = C_R - (C_R * t_R) / t_E + E * t_R * t_R / (2 * t_E * V);        // limit A.12 for q -> 0
                }
            }
            return C_avg;
        }

        protected static double MeanAirConcentrationExposureLessThanReleaseAndNoSaturation(double t_E, double E, double q, double V)
        {
            double C_avg;
            //Exposure duration less than release duration
            if (q > FloatingPointZero)
            {
                C_avg = E / (q * V) * (1 + (Math.Exp(-q * t_E) - 1) / (q * t_E));          // (A.9)
            }
            else
            {
                C_avg = E * t_E / (2 * V);
            }

            return C_avg;
        }

        public AirConcentration InstantaneousAirConcentration(Time time)
        {
            double t = time.InSeconds();
            double airConcentrationValue;

            ParseScenario();

            if (limitConcentrationToSaturatedAirConcentration)
            {
                C_sat = M * p_sat / (R * T);                                            // (B.1)

                if (C_R < C_sat)
                {
                    airConcentrationValue = AirConcentrationWithoutSaturation(t);
                }
                else
                {
                    if (q > FloatingPointZero)
                    {
                        t_sat = -(Math.Log(1 - q * V * C_sat / E)) / q;                     // (B.2)
                    }
                    else
                    {
                        t_sat = C_sat * V / E;                                              // Limit of (B.2) for q->0.
                    }

                    if (t < t_sat)
                    {
                        airConcentrationValue = AirConcentrationWithoutSaturation(t);
                    }
                    else if (q <= FloatingPointZero)
                    {
                        //No ventilation. So, once saturation is reached, concentration will never drop below it anymore.
                        airConcentrationValue = C_sat;
                    }
                    else
                    {
                        t_F = GetT_F();
                        if (t < t_F)
                        {
                            airConcentrationValue = C_sat;                                  // (B.6)
                        }
                        else
                        {
                            airConcentrationValue = C_sat * Math.Exp(-q * (t - t_F));
                        }
                    }
                }
            }
            else
            {
                airConcentrationValue = AirConcentrationWithoutSaturation(t);
            }

            return new AirConcentration()
            {
                Value = airConcentrationValue,
                Unit = DensityUnits.MilligramPerCubicMetre
            };
        }

        private double AirConcentrationWithoutSaturation(double time)
        {
            double airConcentrationValue;
            if (time < t_R)
            {
                if (q > FloatingPointZero)
                {
                    airConcentrationValue = E / (q * V) * (1 - Math.Exp(-q * time));        // (A.4)
                }
                else
                {
                    airConcentrationValue = E / V * time;
                }
            }
            else
            {
                airConcentrationValue = C_R * Math.Exp(-q * (time - t_R));              // (A.6)
            }
            return airConcentrationValue;
        }

        public AirConcentration MeanAirConcentration()
        {
            ParseScenario();
            return MeanAirConcentration(ApplicableExposureDuration.AsTime());
        }

        public override AirConcentration MeanAirConcentrationPeak()
        {
            ParseScenario();
            return PeakAirConcentration(scenario.InhalationExposure.DailyDuration.AsTimePerDay());
        }

        public IEnumerable<ValidationResult> Validate()
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (route.ReEntry)
            {
                RequireEmissionDurationReEntry(validationResults);
                RequireDailyDuration(validationResults);
                validationResults.AddRange(ValidateDurationAndFrequency(route.EmissionDurationReEntry, scenario.Frequency));
            }
            else
            {
                RequireExposureDuration(validationResults);
                RequireEmissionDuration(validationResults);
                validationResults.AddRange(ValidateDurationAndFrequency(route.ExposureDuration, scenario.Frequency));
            }

            RequireProductAmount(validationResults);
            RequireWeightFractionSubstance(validationResults);
            RequireRoomVolume(validationResults);
            RequireVentilationRate(validationResults);

            if (route.LimitConcentrationToSaturatedAirConcentration)
            {
                RequireVapourPressure(validationResults);
                RequireApplicationTemperature(validationResults);
                RequireMolecularWeight(validationResults, scenario.Assessment.Substance);
            }

            return validationResults;
        }

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        public override bool ModelIsDistributed
        {
            get
            {
                bool isDistributed = route.ProductAmount.IsDistributed
                   || route.WeightFractionSubstance.IsDistributed
                   || route.RoomVolume.IsDistributed
                   || route.VentilationRate.IsDistributed
                   || (route.LimitConcentrationToSaturatedAirConcentration && route.ApplicationTemperature.IsDistributed);

                if (route.ReEntry)
                {
                    isDistributed = isDistributed || route.EmissionDurationReEntry.IsDistributed || route.ProductAmount.IsDistributed;
                }
                else
                {
                    isDistributed = isDistributed || route.ExposureDuration.IsDistributed || route.EmissionDuration.IsDistributed;
                }

                return isDistributed;
            }
        }

        //Note: InhalationRate is not an intrinsic parameter for this model. It is only used in the conversion from Air Concentration to External Event Dose.
    }
}