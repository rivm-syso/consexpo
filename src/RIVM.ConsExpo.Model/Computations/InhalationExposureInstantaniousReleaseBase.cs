using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Submodels;
using RIVM.ConsExpo.Model.Submodels;
using System;
using System.Diagnostics;

namespace RIVM.ConsExpo.Model.Computations
{
    /// <summary>
    /// In this model, all of the substance is released at once, subsequently removed by ventilation.
    /// This model does a calculation that is valid for both vapour and spray.
    /// </summary>
    internal abstract class InhalationExposureInstantaniousReleaseBase : InhalationExposureBase
    {
        protected InhalationExposureInstantaniousReleaseBase(ScenarioModel scenario, InhalationExposureSubmodelTypes type, bool analytic)
            : base(scenario, type, analytic)
        {
        }

        private const double R = PhysicalConstants.GasConstant; // gas constant in Pa*m3/mol/K
        protected double T0;

        protected double wf; // Weight fraction of the substance in the applied product, possibly diluted.

        protected double V;
        protected double q;
        protected double A; // active substance
        protected Pressure vapourPressure;
        protected double molecularWeight;
        protected double applicationTemperature;
        protected bool limitConcentrationToSaturatedAirConcentration;

        public virtual AirConcentration InstantaneousAirConcentration(Time time)
        {
            ParseScenario();

            var t = time.InSeconds();

            // all of the substance released at once, subsequently removed by ventilation
            // C = A/V exp (-q*t)

            double airConcentration;
            if (!limitConcentrationToSaturatedAirConcentration)
            {
                airConcentration = A / V * Math.Exp(-q * t); //in mg/m3
            }
            else
            {
                // handle the saturated air case, the air concentration is assumed to be saturated,
                // the chemical is removed at a rate q*V*Csat until A < Asat, from then on the rest is
                // removed by 'normal ventilation'

                double Csat = ConcentrationAtSaturation;

                double Asat;
                double Tsat;

                Asat = Csat * V;
                if (A < Asat)
                {
                    // no saturation, all is the same as above
                    airConcentration = A / V * Math.Exp(-q * t); //in mg/m3
                }
                else if (q > FloatingPointZero)
                {
                    // -dA/dt = q*Asat; At = A - q*Asat*t; Tsat = (A - Asat)/q*Asat
                    Tsat = (A - Asat) / (q * Asat);
                    if (t < Tsat)
                    {
                        airConcentration = Csat;
                    }
                    else
                    {
                        airConcentration = (Asat / V) * Math.Exp(-q * (t - Tsat));
                    }
                }
                else
                {
                    airConcentration = Csat;
                }
            }

            return new AirConcentration()
            {
                Value = airConcentration,
                Unit = DensityUnits.MilligramPerCubicMetre
            };
        }

        /// <summary>
        /// Calculates the time-averaged air concentration for the specified scenario, up to the specified time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public override AirConcentration MeanAirConcentration(Time time)
        {
            ParseScenario();
            double t = time.InSeconds();

#if DEBUG
            Debug.Assert(t >= T0);
            //todo-1: error handling, what if V<= 0??  This can occur with Monte Carlo simulations.
#endif
            if (t == 0)
            {
                return InstantaneousAirConcentration(time);
            }

            double Cav;
            if (!limitConcentrationToSaturatedAirConcentration)
            {
                Cav = MeanAirConcentrationUnsaturated(t);
            }
            else
            {
                // handle the saturated air case, the air concentration is assumed to be saturated,
                // the chemical is removed at a rate q*V*Csat until A < Asat, from then on the rest is
                // removed by 'normal ventilation'
                double Csat = ConcentrationAtSaturation;

                double Asat;
                double Tsat;
                Asat = Csat * V;

                if (A < Asat)
                {
                    Cav = MeanAirConcentrationUnsaturated(t);
                }
                else // saturation
                {
#warning How about small q?
                    if (q > FloatingPointZero)
                    {
                        // -dA/dt = q*Asat; At = A - q*Asat*t; Tsat = (A - Asat)/q*Asat

                        Tsat = (A - Asat) / (q * Asat);
                        if (t < Tsat)
                        {
                            Cav = Csat;
                        }
                        else
                        {
                            if (T0 > Tsat)
                            {
                                Cav = 1 / (t - T0) * (1 / q) * (1 - Math.Exp(-q * (t - T0)));
                            }
                            else
                            {
                                // T0 < Tsat, Tend > Tsat
                                Cav = (1 / (t - T0)) * (Csat * (Tsat - T0) + (Asat / V) * (1 / q) * (1 - Math.Exp(-q * (t - Tsat))));
                            }
                        }
                    }
                    else
                    {
                        Cav = Csat;
                    }
                }
            }

            return new AirConcentration()
            {
                Value = Cav,
                Unit = DensityUnits.MilligramPerCubicMetre
            };
        }

        /// <summary>
        /// Calculates the unsaturated air concentration. This occurs when the limit is not imposed, or if the concentration of substance never reaches saturation.
        /// </summary>
        /// <param name="t">The time.</param>
        /// <returns></returns>
        private double MeanAirConcentrationUnsaturated(double t)
        {
            double Cav;
            if (q > FloatingPointZero)
            {
                Cav = 1 / (t - T0) * A / (q * V) * (Math.Exp(-q * T0) - Math.Exp(-q * t)); //in mg/m3
            }
            else
            {
                Cav = A / V;    // (limit q->0)
            }
            return Cav;
        }

        private double ConcentrationAtSaturation
        {
            get
            {
                double P = vapourPressure.InPascal();
                double M = molecularWeight;
                double T = applicationTemperature;
                double Csat = ConversionFactors.One2Milli * M * P / (R * T); // in mg/m3
                return Csat;
            }
        }

        public virtual AirConcentration MeanAirConcentration()
        {
            ParseScenario();

            return MeanAirConcentration(scenario.InhalationExposure.ExposureDuration.AsTime());
        }

        public override TimeInterval PeakInterval(Time time)
        {
            Time endTime;
            if (EndTimeOfExposure.InMinutes() < time.InMinutes())
            {
                endTime = EndTimeOfExposure;
            }
            else
            {
                endTime = Twa15TimeInterval;
            }

            return new TimeInterval(new Time { Value = 0, Unit = TimeUnits.Minute }, endTime);
        }

        /// <summary>
        /// Implementation must provide a parsing mechanism, which maps scenario fields to variables used in the calculation.
        /// </summary>
        protected abstract void ParseScenario();
    }
}