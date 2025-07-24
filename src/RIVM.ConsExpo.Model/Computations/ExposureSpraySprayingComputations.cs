using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using System;
using System.Linq;

namespace RIVM.ConsExpo.Model.Computations
{
    /// <summary>
    /// This class can perform the calculations needed in the inhalation and in the oral spraying models. Calculations are almost the same, only for inhalation the particles with diameter below the cut-off diameter are used and for oral the particle above the cur-off diameter.
    /// In this model, all of the substance is released as spray at once, subsequently removed by ventilation.
    /// </summary>
    /// <remarks>In contrast to 'InhalatoryExposureInstantaniousReleaseBase', this class is not the base class for the oral and inhalation spray submodels.were derived from the same base submodel, which need these computations, as the base model could be neither an inhalation submodel nor an oral submodel. Therefore, methods like Require&lt;parameter&gt; could not be invoked directly. Therefore, this class must be invoked separately.</remarks>
    internal class ExposureSpraySprayingComputations
    {
        private const int NumberOfBinsAerosolDistribution = 20;    // number of size bins used when an aerosol size distribution is constructed
        private const double MinAerosolDiameter = 1e-9;            // minimum aerosol diameter allowed, below this size, aerosol is assumed to be completely dissolved

        private ScenarioModel scenario;
        private readonly bool belowCutOff;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExposureSpraySprayingComputations" /> class.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <param name="belowCutOff">True if the size distribution must be used from 0 to cut-off. Otherwise, go from cut-off to max.</param>
        public ExposureSpraySprayingComputations(ScenarioModel scenario, bool belowCutOff)
        {
            this.scenario = scenario;
            this.belowCutOff = belowCutOff;
        }

        /// <summary>
        /// Optimization. The size distribution is time-independent. It can be prepared once.
        /// </summary>
        /// <param name="maxTime">The maximum time.</param>
        public void PrepareSolution(Time maxTime)
        {
            InitializeAerosolDiameterDistribution();
        }

        public AirConcentration InstantaneousAirConcentration(Time time)
        {
            var t = time.InHours();

            double wf = scenario.InhalationExposure.WeightFractionSubstance.AsFraction();     // mass weight fraction of substance material (dimensionless)
            double fAir = scenario.InhalationExposure.AirborneFraction.AsFraction();             // fraction of the spray that becomes airborne (dimensionless)
            double r = scenario.InhalationExposure.MassGenerationRate.InGramPerHour() * fAir * wf;  // generation rate of airborne substance mass (g/hr)

            double instantaneousAirConcentrationSubstance = 0.0;

            foreach (SizeBin iterateBin in DistributionAerosolDiameter.Bins)
            {
                var instantaneousAirConcentrationForAerosolSize = InstantaneousAirConcentrationForAerosolSize(iterateBin.Variable, iterateBin.ProbabilityMass, t);
                instantaneousAirConcentrationSubstance += instantaneousAirConcentrationForAerosolSize;

                //Enable only when needed. Consumes a lot of resource in Monte Carlo simulations.
                //Debug.WriteLine("{0};{1};{2}", iterateBin.Variable, instantaneousAirConcentrationForAerosolSize, instantaneousAirConcentrationSubstance);
            }

            return AirConcentration.NewFromGramPerCubicMetre(instantaneousAirConcentrationSubstance);
        }

        private double InstantaneousAirConcentrationForAerosolSize(double diameter, double substanceGenerationRate, double time)
        {
            double PrecisionLimit = 1 - Math.Pow(10, -7); //Estimated by testing with various scenarios.

            // calculates the inhaled dose of aerosols that become airborne after use of a consumer spray product
            // the model assumes homogeneous mixing of indoor air (and aerosol), and removal by ventilation and gravitational settling (falling to the floor)
            // gravitational settling is dependent on the mass density of the aerosol and its (hydrodynamic) diameter.
            // mass sprayed follows from the mass generation rate of the spray and the spray duration. Of the total amount sprayed, only a fraction becomes airborne. This is to account for
            // sprays that are applied to a surface
            // when the spray is used on the person, the volume during spraying is assumed to be equal to the cloud volume, after spraying, the volume is set to the room volume.
            // ventilation is assumed independent of volume.
            // Note that this deviates from the ConsExpo model, where the volume is assumed to increase linearly during spraying

            const double g = PhysicalConstants.GravityConstant;                                     // gravity constant in m/s2
            const double u = PhysicalConstants.DynamicViscosityOfAir;                               // dynamic viscosity of air in g/m/s

            double d = diameter;                                                                    // aerosol diameter in m

            double rho = scenario.InhalationExposure.DensityNonVolatile.InGramPerCubicMetre();  // density aerosol g/m3

            double ts = scenario.InhalationExposure.SprayDuration.InHours();                    // spray duration in h

            double r = substanceGenerationRate;                                                        // release rate of the substance material sprayed in g/s

            double q = scenario.InhalationExposure.VentilationRate.InTimesPerHour();        // ventilation rate in 1/h
            double h = scenario.InhalationExposure.RoomHeight.InMetre();                        // room height in meters
            double v = scenario.InhalationExposure.RoomVolume.InCubicMetres();             // room volume in m3

            double tex = time;                                                                      // exposure duration in h

            double stokesFact = g * rho / u / 18 * 3600;                                            // [1/m/h] multiplying this with the aerosol diameter squared in m2 gives the Stokes settling velocity in m/h
            double vs = stokesFact * d * d;                                                         // Stokes' settling velocity in m/h
            double kr = q + vs / h;                                                                 // elimination rate (?)

            double instantaneousAirConcentrationForAerosolSize;

            if (tex <= 0)
            {
                instantaneousAirConcentrationForAerosolSize = 0;
            }
            else
            {
                double vprod; //The volume containing the sprayed product
                if (scenario.InhalationExposure.SprayingTowardsPerson && (tex <= ts))
                {
                    double vcl = scenario.InhalationExposure.CloudVolume.InCubicMetres();
                    // Calculate (personal) volume of the spray cloud during spraying.
                    // The cloud is assumed to grow with a rate of vcl per second.
                    // After spraying, the volume is set to the room volume. (Even if it is (much) less when the spraying stops!?)
                    // In calculating the personal volume, care should be taken of the fact that the cloud may grow larger than the room volume, which is unphysical.

                    vprod = Math.Min(v, vcl / ConversionFactors.HoursPerSecond * tex);
                }
                else
                {
                    vprod = v;
                }

                if (Math.Exp(-kr * ts) > PrecisionLimit) // case of no elimination
                {
                    // two cases: texp > ts
                    if (tex > ts)
                    {
                        instantaneousAirConcentrationForAerosolSize = r * ((Math.Exp(kr * ts) - 1) / (kr * vprod) + (1 - Math.Exp(kr * ts) * ts / vprod)); //First order Taylor expansion of (1)
                    }
                    else
                    {
                        instantaneousAirConcentrationForAerosolSize = r * tex / vprod; //First order Taylor expansion of (2)
                    }
                }
                else // kr > 0
                {
                    // two cases: texp > ts
                    if (tex > ts)
                    {
                        instantaneousAirConcentrationForAerosolSize = r * Math.Exp(-kr * tex) * (Math.Exp(kr * ts) - 1) / (kr * vprod); // (1)
                    }
                    else
                    {
                        instantaneousAirConcentrationForAerosolSize = r * (1 - Math.Exp(-kr * tex)) / (kr * vprod); // (2)
                    }
                }
            }

            return instantaneousAirConcentrationForAerosolSize;
        }

        public AirConcentration MeanAirConcentration()
        {
            InitializeAerosolDiameterDistribution();
            return MeanAirConcentration(scenario.InhalationExposure.ExposureDuration.AsTime());
        }

        /// <summary>
        /// Calculates the mean air concentration.
        /// </summary>
        /// <param name="time">The end time for calculation of the mean</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public AirConcentration MeanAirConcentration(Time time)
        {
            var t = time.InHours();

            double meanAirConcentrationSubstance = 0.0;

            switch (scenario.InhalationExposure.AerosolDiameterDistributionType)
            {
                case SizeDistributionTypes.Normal:
                case SizeDistributionTypes.LogNormal:
                case SizeDistributionTypes.NonParametric:

                    foreach (SizeBin iterateBin in DistributionAerosolDiameter.Bins)
                    {
                        var meanAirConcentrationForAerosolSize = MeanAirConcentrationForAerosolSize(iterateBin.Variable, iterateBin.ProbabilityMass, t);
                        meanAirConcentrationSubstance += meanAirConcentrationForAerosolSize;

                        //Enable only when needed. Consumes a lot of resource in Monte Carlo simulations.
                        //Debug.WriteLine("{0};{1};{2}", iterateBin.Variable, meanAirConcentrationForAerosolSize, meanAirConcentrationSubstance);
                    }
                    break;

                default:
                    throw new NotSupportedException(string.Format("Unsupported distribution type '{0}'", scenario.InhalationExposure.AerosolDiameterDistributionType.ToString()));
            }

            return AirConcentration.NewFromGramPerCubicMetre(meanAirConcentrationSubstance);
        }

        private double MeanAirConcentrationForAerosolSize(double diameter, double substanceGenerationRate, double time)
        {
            double PrecisionLimit = 1 - Math.Pow(10, -7); //Estimated by testing with various scenarios.

            // Calculates the inhaled dose of aerosols that become airborne after use of a consumer spray product
            // the model assumes homogeneous mixing of indoor air (and aerosol), and removal by ventilation and gravitational settling (falling to the floor)
            // gravitational settling is dependent on the mass density of the aerosol and its (hydrodynamic) diameter.
            // mass sprayed follows from the mass generation rate of the spray and the spray duration. Of the total amount sprayed, only a fraction becomes airborne. This is to account for
            // sprays that are applied to a surface
            // when the spray is used on the person, the volume during spraying is assumed to be equal to the cloud volume, after spraying, the volume is set to the room volume.
            // ventilation is assumed independent of volume.
            // Note that this deviates from the ConsExpo model, where the volume is assumed to increase linearly during spraying

            const double g = PhysicalConstants.GravityConstant;                                     // gravity constant in m/s2
            const double u = PhysicalConstants.DynamicViscosityOfAir;                               // dynamic viscosity of air in g/m/s

            double d = diameter;                                                                    // aerosol diameter in m

            double rho = scenario.InhalationExposure.DensityNonVolatile.InGramPerCubicMetre();  // density aerosol g/m3

            double ts = scenario.InhalationExposure.SprayDuration.InHours();                    // spray duration in h

            double r = substanceGenerationRate;                                                     // release rate of the substance material sprayed in g/s

            double q = scenario.InhalationExposure.VentilationRate.InTimesPerHour();                // ventilation rate in 1/h
            double h = scenario.InhalationExposure.RoomHeight.InMetre();                            // room height in meters
            double v = scenario.InhalationExposure.RoomVolume.InCubicMetres();                      // room volume in m3

            double tex = time;                                                                      // exposure duration in h

            double stokesFact = g * rho / u / 18 * 3600;                                            // [1/m/h] multiplying this with the aerosol diameter squared in m2 gives the Stokes settling velocity in m/h
            double vs = stokesFact * d * d;                                                         // Stokes' settling velocity in m/h
            double kr = q + vs / h;                                                                 // elimination rate (?)

            double timeToCloudFillsRoom;
            double persVolume;
            double meanAirConcentrationForAerosolSize;

            if (scenario.InhalationExposure.SprayingTowardsPerson)
            {
                double vcl = scenario.InhalationExposure.CloudVolume.InCubicMetres();
                // calculate average (personal) volume of the spray cloud during spraying
                // the cloud is assumed to grow with a rate of vcl per second
                // below we take the average volume of this cloud and use this as the reduced 'personal' volume during
                // spraying. After spraying, the volume is set to the room volume
                // in calculating the personal volume, care should be taken of the fact that the cloud may grow larger than
                // the room volume, which is unphysical
                timeToCloudFillsRoom = v / vcl / ConversionFactors.SecondsPerHour; // time in hours it takes with cloud volume growth rate of vcl/s to fill the v of the room

                if (timeToCloudFillsRoom > ts)
                {
                    // cloud does not fill room; vpers = 1/ts * (vcl * ts^2)/2
                    // note that, implicitly, vcl is a rate of volume growth per second,
                    // this should be converted to per hour
                    persVolume = 0.5 * vcl * ts / ConversionFactors.HoursPerSecond;
                }
                else
                {
                    // cloud fills room, take time weighted average of t < timeToCloudFillsRoom
                    // and timeToCloudFillsRoom < t < ts; noting that timeToCloudFillsRoom * vcl = v
                    persVolume = 1 / ts * (0.5 * v * timeToCloudFillsRoom + v * (ts - timeToCloudFillsRoom));
                }

                if (Math.Exp(-kr * ts) > PrecisionLimit) // case of no elimination
                {
                    // two cases: texp > ts
                    if (tex > ts)
                    {
                        meanAirConcentrationForAerosolSize = r * ts * (2 * persVolume * (tex - ts) + ts * v) / 2 * persVolume * tex * v;
                    }
                    else
                    {
                        meanAirConcentrationForAerosolSize = r * tex / (2 * persVolume);
                    }
                }
                else // kr > 0
                {
                    // two cases: texp > ts
                    if (tex > ts)
                    {
                        meanAirConcentrationForAerosolSize = r / tex / persVolume / kr * (ts - (1 - Math.Exp(-kr * ts)) / kr) + r / tex / v / kr * (1 - Math.Exp(-kr * ts)) * (1 - Math.Exp(-kr * (tex - ts))) / kr;
                    }
                    else
                    {
                        meanAirConcentrationForAerosolSize = r * (kr * tex - 1 + Math.Exp(-kr * tex)) / (kr * kr * persVolume * tex);
                    }
                }
            }
            else // no spraying to person
                if (Math.Exp(-kr * ts) > PrecisionLimit) // case of no elimination
            {
                // two cases: texp > ts
                if (tex > ts)
                {
                    meanAirConcentrationForAerosolSize = r * (2 * tex - ts) * ts / (2 * tex * v);
                }
                else
                {
                    meanAirConcentrationForAerosolSize = r * tex / (2 * v);
                }
            }
            else
            {
                // two cases: texp > ts
                if (tex > ts)
                {
#warning To Do: test SpraySpraying_LowEliminiationTest fails because (ts - (1 - Math.Exp(-kr * ts)) / kr) is less than 0 due to rounding errors.
                    meanAirConcentrationForAerosolSize = r / v / kr / tex * (ts - (1 - Math.Exp(-kr * ts)) / kr) + r / v / kr * (1 - Math.Exp(-kr * ts)) * (1 - Math.Exp(-kr * (tex - ts))) / (kr * tex); // qinh x tex x the average aerosol air concentration in g
                }
                else
                {
                    meanAirConcentrationForAerosolSize = r * (tex - (1 - Math.Exp(-kr * tex)) / kr) / (v * kr * tex);
                }
            }

            return meanAirConcentrationForAerosolSize;
        }

        protected SizeDistribution DistributionAerosolDiameter;

        protected void InitializeAerosolDiameterDistribution()
        {
            double wf = scenario.InhalationExposure.WeightFractionSubstance.AsFraction();           // mass weight fraction of substance material (dimensionless)
            double fAir = scenario.InhalationExposure.AirborneFraction.AsFraction();                // fraction of the spray that becomes airborne (dimensionless)
            double r = scenario.InhalationExposure.MassGenerationRate.InGramPerHour() * fAir * wf;  // generation rate of airborne substance mass (g/hr)

            DistributionAerosolDiameter = new SizeDistribution();

            switch (scenario.InhalationExposure.AerosolDiameterDistributionType)
            {
                case SizeDistributionTypes.Normal:
                case SizeDistributionTypes.LogNormal:

                    double minDiameterToUse;
                    double maxDiameterToUse;

                    if (belowCutOff)
                    {
                        minDiameterToUse = Math.Max(0, EffectiveMinDiameter.InMetre());
                        maxDiameterToUse = Math.Min(scenario.InhalationExposure.InhalationCutOffDiameter.InMetre(), EffectiveMaxDiameter.InMetre());
                    }
                    else
                    {
                        minDiameterToUse = Math.Max(scenario.InhalationExposure.InhalationCutOffDiameter.InMetre(), EffectiveMinDiameter.InMetre());
                        maxDiameterToUse = Math.Min(EffectiveMaxDiameter.InMetre(), scenario.InhalationExposure.MaximumDiameter.InMetre());
                    }

                    if (minDiameterToUse < maxDiameterToUse)
                    {
                        //A distribution is one meaningful if the min is larger than the max.
                        // If max <= min, the distribution will have no bins and the total exposure will just become zero.

                        if (scenario.InhalationExposure.AerosolDiameterDistributionType == SizeDistributionTypes.Normal)
                        {
                            double mean = scenario.InhalationExposure.MeanDiameter.InMetre();
                            double sd = scenario.InhalationExposure.StandardDeviation.InMetre();

                            DistributionAerosolDiameter.InitNormal(mean, sd, minDiameterToUse, maxDiameterToUse,
                                NumberOfBinsAerosolDistribution, r);
                        }
                        else
                        {
                            double median = scenario.InhalationExposure.MedianDiameter.InMetre();
                            double aCoV = scenario.InhalationExposure.ArithmicCoefficientOfVariation.Value;

                            DistributionAerosolDiameter.InitLogNormal(median, aCoV, minDiameterToUse, maxDiameterToUse, NumberOfBinsAerosolDistribution, r);
                        }
                    }

                    break;

                case SizeDistributionTypes.NonParametric:
                    DistributionAerosolDiameter.InitNonParametric(scenario.InhalationExposure.NonParametricSizeDistribution, belowCutOff, scenario.InhalationExposure.InhalationCutOffDiameter.InMetre(), r);
                    break;

                default:
                    throw new NotSupportedException(string.Format("Unsupported distribution type '{0}'", scenario.InhalationExposure.AerosolDiameterDistributionType.ToString()));
            }
        }

        /// <summary>
        /// Tests if one or more of the parameters used in the model is specified as a distribution.
        /// </summary>
        public virtual bool ModelIsDistributed
        {
            get
            {
                //These parameters cannot be distributed:
                //ArithmicCoefficientOfVariation
                //MaximumDiameter
                //MeanDiameter
                //MedianDiameter
                //StandardDeviation

                var route = scenario.InhalationExposure;

                return route.AirborneFraction.IsDistributed
                    || (route.SprayingTowardsPerson && route.CloudVolume.IsDistributed)
                    || route.DensityNonVolatile.IsDistributed
                    || route.ExposureDuration.IsDistributed
                    || route.InhalationCutOffDiameter.IsDistributed
                    || route.MassGenerationRate.IsDistributed
                    || route.RoomHeight.IsDistributed
                    || route.RoomVolume.IsDistributed
                    || route.SprayDuration.IsDistributed
                    || route.VentilationRate.IsDistributed
                    || route.WeightFractionSubstance.IsDistributed;

                //Note: InhalationRate is not an intrinsic parameter for this model. It is only used in the conversion from Air Concentration to External Event Dose.
            }
        }

        // The number of CoV's around the median, that determine the relevant part of the distribution.
        private const int NumberOfCovAroundMedian = 3;

        // The number of Standard deviations around the mean, that determine the relevant part of the distribution.
        private const int NumberOfSdsAroundMean = 3;

        /// <summary>
        /// Gets the effective maximum diameter.
        /// </summary>
        /// <value>
        /// The effective maximum diameter.
        /// </value>
        /// <remarks>
        /// Log normal:
        ///     dmineff = exp( ln(µ) - 3 x σ) = exp (ln(µ) - 3 x  √(cv^2+1) )
        /// Normal:
        ///     dmineff = dmean - 3 x sd
        ///     </remarks>
        protected FixedDiameter EffectiveMinDiameter
        {
            get
            {
                double? dmineff;
                var route = scenario.InhalationExposure;
                LengthUnits unit;

                switch (route.AerosolDiameterDistributionType)
                {
                    case SizeDistributionTypes.LogNormal:

                        double? mu = route.MedianDiameter.Value;
                        double? cov = route.ArithmicCoefficientOfVariation;

                        if (mu.HasValue && cov.HasValue)
                        {
                            dmineff = Math.Exp(Math.Log(mu.Value) - NumberOfCovAroundMedian * Math.Sqrt(Math.Pow(cov.Value, 2) + 1));
                        }
                        else
                        {
                            dmineff = null;
                        }

                        unit = route.MedianDiameter.Unit;

                        break;

                    case SizeDistributionTypes.Normal:

                        if (route.MeanDiameter.HasValue && route.StandardDeviation.HasValue)
                        {
                            dmineff = route.MeanDiameter.Value - NumberOfSdsAroundMean * route.StandardDeviation.ConvertedValue(route.MeanDiameter.Unit);
                        }
                        else
                        {
                            dmineff = null;
                        }

                        unit = route.MeanDiameter.Unit;

                        break;

                    case SizeDistributionTypes.NonParametric:
                        dmineff = scenario.InhalationExposure.NonParametricSizeDistribution.Bins.Where(sb => sb.RelativeMass > 0).Min(sb => sb.UpperBound);
                        unit = LengthUnits.Micrometre;
                        break;

                    default:
                        throw new NotSupportedException(string.Format("Unsupported distribution type '{0}'", route.AerosolDiameterDistributionType.ToString()));
                }

                return new FixedDiameter
                {
                    Value = dmineff,
                    Unit = unit
                };
            }
        }

        /// <summary>
        /// Gets the effective maximum diameter.
        /// </summary>
        /// <value>
        /// The effective maximum diameter.
        /// </value>
        /// <remarks>
        /// Log normal:
        ///     dmaxeff = exp( ln(µ) + 2 x σ) = exp (ln(µ) + 2 x  √(cv^2+1) )
        /// Normal:
        ///     dmaxeff = dmean + 2 x sd
        ///     </remarks>
        protected FixedDiameter EffectiveMaxDiameter
        {
            get
            {
                double? dmaxeff;
                var route = scenario.InhalationExposure;
                LengthUnits unit;

                switch (route.AerosolDiameterDistributionType)
                {
                    case SizeDistributionTypes.LogNormal:

                        double? mu = route.MedianDiameter.Value;
                        double? cov = route.ArithmicCoefficientOfVariation;

                        if (mu.HasValue && cov.HasValue)
                        {
                            dmaxeff = Math.Exp(Math.Log(mu.Value) + NumberOfCovAroundMedian * Math.Sqrt(Math.Pow(cov.Value, 2) + 1));
                        }
                        else
                        {
                            dmaxeff = null;
                        }

                        unit = route.MedianDiameter.Unit;

                        break;

                    case SizeDistributionTypes.Normal:

                        if (route.MeanDiameter.HasValue && route.StandardDeviation.HasValue)
                        {
                            dmaxeff = route.MeanDiameter.Value + NumberOfSdsAroundMean * route.StandardDeviation.ConvertedValue(route.MeanDiameter.Unit);
                        }
                        else
                        {
                            dmaxeff = null;
                        }

                        unit = route.MeanDiameter.Unit;

                        break;

                    case SizeDistributionTypes.NonParametric:
                        dmaxeff = scenario.InhalationExposure.NonParametricSizeDistribution.Bins.Where(sb => sb.RelativeMass > 0).Max(sb => sb.UpperBound);
                        unit = LengthUnits.Micrometre;
                        break;

                    default:
                        throw new NotSupportedException(string.Format("Unsupported distribution type '{0}'", route.AerosolDiameterDistributionType.ToString()));
                }

                return new FixedDiameter
                {
                    Value = dmaxeff,
                    Unit = unit
                };
            }
        }
    }
}