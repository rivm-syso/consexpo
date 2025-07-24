using RIVM.ConsExpo.DTO.Distributions;
using RIVM.ConsExpo.DTO.Entities;
using RIVM.ConsExpo.DTO.Exceptions;
using RIVM.ConsExpo.DTO.Helpers;
using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;
using RIVM.ConsExpo.DTO.Settings;
using RIVM.ConsExpo.Model.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

namespace RIVM.ConsExpo.Model.Models
{
    /// <summary>
    /// This class organizes all simulations supported by ConsExpo.
    /// </summary>
    public class Simulation : ISimulation
    {
        /// <summary>
        /// The inhalation simulation
        /// </summary>
        protected IInhalationSimulation inhalationSimulation;

        /// <summary>
        /// The dermal simulation
        /// </summary>
        protected IDermalSimulation dermalSimulation;

        /// <summary>
        /// The oral simulation
        /// </summary>
        protected IOralSimulation oralSimulation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Simulation"/> class.
        /// </summary>
        /// <param name="inhalationSimulation">The inhalation simulation.</param>
        /// <param name="dermalSimulation">The dermal simulation.</param>
        /// <param name="oralSimulation">The oral simulation.</param>
        public Simulation(IInhalationSimulation inhalationSimulation, IDermalSimulation dermalSimulation, IOralSimulation oralSimulation)
        {
            this.inhalationSimulation = inhalationSimulation;
            this.dermalSimulation = dermalSimulation;
            this.oralSimulation = oralSimulation;
        }

        /// <summary>
        /// Validates the specified scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        public List<ValidationResult> Validate(ScenarioModel scenario)
        {
            var validationResults = new List<ValidationResult>();

            if (!(scenario.DermalExposureRouteInUse || scenario.DermalAbsorptionRouteInUse
                || scenario.InhalationExposureRouteInUse || scenario.InhalationAbsorptionRouteInUse
                || scenario.OralExposureRouteInUse || scenario.OralAbsorptionRouteInUse))
            {
                validationResults.Add(new ValidationResult("At least one route must be specified."));
            }

            if (scenario.InhalationExposureRouteInUse || scenario.InhalationAbsorptionRouteInUse)
            {
                validationResults.AddRange(inhalationSimulation.Validate(scenario));
            }

            if (scenario.DermalExposureRouteInUse || scenario.DermalAbsorptionRouteInUse)
            {
                validationResults.AddRange(dermalSimulation.Validate(scenario));
            }

            if (scenario.OralExposureRouteInUse || scenario.OralAbsorptionRouteInUse)
            {
                validationResults.AddRange(oralSimulation.Validate(scenario));
            }

            return validationResults;
        }

        /// <summary>
        /// Validates a model for the sampled values.
        /// </summary>
        /// <param name="scenario"></param>
        /// <returns></returns>
        /// <remarks>Assumes some parameters are distributed and that samples have been taken.</remarks>
        public bool ValidateSampled(ScenarioModel scenario)
        {
            var f = scenario.Frequency;

            var id = scenario.InhalationExposureRouteInUse ? inhalationSimulation.ApplicableExposureDuration(scenario) : null;
            var dd = scenario.DermalExposureRouteInUse ? dermalSimulation.ApplicableExposureDuration(scenario) : null;
            var od = scenario.OralExposureRouteInUse ? oralSimulation.ApplicableExposureDuration(scenario) : null;
            if (((id?.HasValue ?? false) && f.HasValue && id.InDays() > 1 / f.InTimesPerDay())
                || ((dd?.HasValue ?? false) && f.HasValue && dd.InDays() > 1 / f.InTimesPerDay())
                || ((od?.HasValue ?? false) && f.HasValue && od.InDays() > 1 / f.InTimesPerDay()))
            {
                return false;
            }

            return true;
        }

        private bool SomeRouteSpecified(ScenarioModel scenario)
        {
            return scenario.DermalExposureRouteInUse || scenario.DermalAbsorptionRouteInUse
                || scenario.InhalationExposureRouteInUse || scenario.InhalationAbsorptionRouteInUse
                || scenario.OralExposureRouteInUse || scenario.OralAbsorptionRouteInUse;
        }

        private bool SomeRouteValid(ScenarioModel scenario)
        {
            return inhalationSimulation.IsValid(scenario) || dermalSimulation.IsValid(scenario) || oralSimulation.IsValid(scenario);
        }

        /// <summary>
        /// Indicates whether or not end points can be calculated. This depends on the scenario specification.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        public bool EndPointsEnabled(ScenarioModel scenario)
        {
            if (!SomeRouteSpecified(scenario))
            {
                return false;
            }
            else
            {
                return SomeRouteValid(scenario);
            }
        }

        /// <summary>
        /// Calculate endpoints for the specified scenario, but only if it is not distributed.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        public SimulationResults CalculateResults(ScenarioModel scenario)
        {
            return CalculateResults(scenario, false, 0, 0, ScaleType.Linear);
        }

        /// <summary>
        /// Calculate endpoints for the specified scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <param name="calculateDistributions">if set to <c>true</c> calculate distributed end points. Otherwise, only calculate deterministic endpoints.</param>
        /// <param name="numberOfIterations">The number of iterations.</param>
        /// <param name="numberOfBins">The number of bins.</param>
        /// <param name="outputScale">The output scale.</param>
        /// <returns></returns>
        public SimulationResults CalculateResults(ScenarioModel scenario, bool calculateDistributions, int numberOfIterations, int numberOfBins, ScaleType outputScale)
        {
            RouteOutcomes<DermalExposureOutcome, DermalAbsorptionOutcome> dermalPointValue = null;
            RouteOutcomes<InhalationExposureOutcome, InhalationAbsorptionOutcome> inhalationPointValue = null;
            RouteOutcomes<OralExposureOutcome, OralAbsorptionOutcome> oralPointValue = null;

            //Stores the outcomes of the calculations.
            Outcomes outcomes =
                new Outcomes
                {
                    DistributedEndPoints = GetDistributedEndPoints(scenario)
                };

            // Prepare all routes to either store the results of a Monte Carlo simulation or the results of a single deterministic calculation.
            // Optimize performance by specifying the capacity.
            if (calculateDistributions && outcomes.DistributedEndPoints.Dermal.AnyEndPointDistributed)
            {
                outcomes.Dermal = new EndPoint<DermalExposureOutcome, DermalAbsorptionOutcome>(numberOfIterations);
            }

            if (!outcomes.DistributedEndPoints.Dermal.AllEndPointsDistributed)
            {
                dermalPointValue = dermalSimulation.CalculatePointValues(scenario);
                outcomes.Dermal.PointValue = dermalPointValue;
            }

            if (calculateDistributions && outcomes.DistributedEndPoints.Inhalation.AnyEndPointDistributed)
            {
                outcomes.Inhalation = new EndPoint<InhalationExposureOutcome, InhalationAbsorptionOutcome>(numberOfIterations);
            }

            if (!outcomes.DistributedEndPoints.Inhalation.AllEndPointsDistributed)
            {
                inhalationPointValue = inhalationSimulation.CalculatePointValues(scenario);
                outcomes.Inhalation.PointValue = inhalationPointValue;
            }

            if (calculateDistributions && outcomes.DistributedEndPoints.Oral.AnyEndPointDistributed)
            {
                outcomes.Oral = new EndPoint<OralExposureOutcome, OralAbsorptionOutcome>(numberOfIterations);
            }

            if (!outcomes.DistributedEndPoints.Oral.AllEndPointsDistributed)
            {
                oralPointValue = oralSimulation.CalculatePointValues(scenario);
                outcomes.Oral.PointValue = oralPointValue;
            }

            if (calculateDistributions && outcomes.DistributedEndPoints.Integrated.AnyEndPointDistributed)
            {
                outcomes.Integrated = new EndPoint<IntegratedExposureOutcome, IntegratedAbsorptionOutcome>(numberOfIterations);
            }

            if (!outcomes.DistributedEndPoints.Integrated.AllEndPointsDistributed)
            {
                outcomes.Integrated = new EndPoint<IntegratedExposureOutcome, IntegratedAbsorptionOutcome>
                {
                    PointValue = GetIntegratedOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency,
                        dermalPointValue, inhalationPointValue, oralPointValue)
                };
            }

            if (calculateDistributions && outcomes.DistributedEndPoints.AnyEndPointDistributed)
            {
                //Lower the priority, so other users requesting normal pages will receive more resources.
                //A note of caution:
                //http://stackoverflow.com/questions/5589376/why-not-change-the-priority-of-a-threadpool-or-task-thread
                ThreadPriority originalPriority = Thread.CurrentThread.Priority;
                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
                int maxSampleAttempts = 10 * numberOfIterations;
                int sampleAttempts = 0;
                try
                {
                    for (int simulationIteration = 0; simulationIteration < numberOfIterations; simulationIteration++)
                    {
#warning Future extension: It were better if the sampled values for a Monte Carlo simulation were validated.
                        // Specifically, if consistency between frequency and exposure duration were tested, as is done in RIVM.ConsExpo.Model.Submodels.ExposureBase.Validate().
                        // This is quite slow, however. A test scenario with Dermal Instant Application, with 10,000 samples, required 16,600 samples and the simulation took 201 seconds, instead of 42 seconds when validation did not take place.

                        do
                        {
                            if (++sampleAttempts > maxSampleAttempts)
                            {
                                throw new TooManyIterationsException($"Aborting the simulation after {simulationIteration} of {numberOfIterations} iterations. Too many samples were rejected, because they would lead to non-physical conditions. Please, inspect the specifications of {nameof(scenario.Frequency).ToLower()} and the event duration.");
                            }
                            scenario.SampleAll();
                        } while (!ValidateSampled(scenario));

                        if (outcomes.DistributedEndPoints.Dermal.AnyEndPointDistributed)
                        {
                            dermalPointValue = dermalSimulation.CalculatePointValues(scenario);
                            outcomes.Dermal.Points.Add(simulationIteration, dermalPointValue);
                        }

                        if (outcomes.DistributedEndPoints.Inhalation.AnyEndPointDistributed)
                        {
                            inhalationPointValue = inhalationSimulation.CalculatePointValues(scenario);
                            outcomes.Inhalation.Points.Add(simulationIteration, inhalationPointValue);
                        }

                        if (outcomes.DistributedEndPoints.Oral.AnyEndPointDistributed)
                        {
                            oralPointValue = oralSimulation.CalculatePointValues(scenario);
                            outcomes.Oral.Points.Add(simulationIteration, oralPointValue);
                        }

                        if (outcomes.DistributedEndPoints.Integrated.AnyEndPointDistributed)
                        {
                            outcomes.Integrated.Points.Add(simulationIteration, GetIntegratedOutcome(scenario.Assessment.Population.BodyWeight, scenario.Frequency, dermalPointValue, inhalationPointValue, oralPointValue));
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    Thread.CurrentThread.Priority = originalPriority;
                }
            }

            return DeriveResults(scenario, calculateDistributions, numberOfIterations, outcomes, numberOfBins, outputScale);
        }

        /// <summary>
        /// Derives the results from the outcomes.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <param name="calculateDistributions">if set to <c>true</c> [calculate distributions].</param>
        /// <param name="numberOfIterations">The number of iterations.</param>
        /// <param name="outcomes">The outcomes.</param>
        /// <param name="numberOfBins">The number of bins.</param>
        /// <param name="outputScale">The output scale.</param>
        /// <returns></returns>
        private SimulationResults DeriveResults(ScenarioModel scenario, bool calculateDistributions, int numberOfIterations, Outcomes outcomes, int numberOfBins, ScaleType outputScale)
        {
            SimulationResults results = new SimulationResults(scenario.DermalExposureRouteInUse, scenario.InhalationExposureRouteInUse, scenario.OralExposureRouteInUse);    //Representation of the outcomes, fit for rendering of the results.
            results.NumberOfIterations = numberOfIterations;
            results.OutputScale = outputScale;

            DeriveInhalationResults(scenario, calculateDistributions, outcomes, numberOfBins, outputScale, results);

            DeriveDermalResults(scenario, calculateDistributions, outcomes, numberOfBins, outputScale, results);

            DeriveOralResults(scenario, calculateDistributions, outcomes, numberOfBins, outputScale, results);

            DeriveIntegratedResults(scenario, calculateDistributions, outcomes, numberOfBins, outputScale, results);

            return results;
        }

        private void DeriveInhalationResults(ScenarioModel scenario, bool calculateDistributions, Outcomes outcomes, int numberOfBins, ScaleType outputScale, SimulationResults results)
        {
            if (scenario.InhalationExposureRouteInUse)
            {
                if (outcomes.DistributedEndPoints.Inhalation.Exposure.MeanEventConcentrationIsDistributed)
                {
                    if (calculateDistributions)
                    {
                        results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.MeanEventConcentration, outcomes.Inhalation.Points.Select(p => p.Value.Exposure.AsMeanEventConcentration.Value).ToList(), numberOfBins, outputScale));
                    }
                    else
                    {
                        results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.MeanEventConcentration));
                    }
                }
                else
                {
                    results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.MeanEventConcentration, outcomes.Inhalation.PointValue.Exposure.AsMeanEventConcentration));
                }

                if (inhalationSimulation.SupportsPeakAirConcentration(scenario))
                {
                    if (outcomes.DistributedEndPoints.Inhalation.Exposure.PeakConcentrationIsDistributed)
                    {
                        if (calculateDistributions)
                        {
                            results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.PeakAirConcentration, outcomes.Inhalation.Points.Select(p => p.Value.Exposure.AsPeakAirConcentration.Value).ToList(), numberOfBins, outputScale));
                        }
                        else
                        {
                            results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.PeakAirConcentration));
                        }
                    }
                    else
                    {
                        results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.PeakAirConcentration, outcomes.Inhalation.PointValue.Exposure.AsPeakAirConcentration));
                    }
                }

                if (inhalationSimulation.SupportsMeanDayConcentration(scenario))
                {
                    if (outcomes.DistributedEndPoints.Inhalation.Exposure.MeanDayConcentrationIsDistributed)
                    {
                        if (calculateDistributions)
                        {
                            results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.MeanDayConcentration, outcomes.Inhalation.Points.Select(p => p.Value.Exposure.AsMeanDayConcentration.Value).ToList(), numberOfBins, outputScale));
                        }
                        else
                        {
                            results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.MeanDayConcentration));
                        }
                    }
                    else
                    {
                        results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.MeanDayConcentration, outcomes.Inhalation.PointValue.Exposure.AsMeanDayConcentration));
                    }
                }

                if (outcomes.DistributedEndPoints.Inhalation.Exposure.MeanYearConcentrationIsDistributed)
                {
                    if (calculateDistributions)
                    {
                        results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.MeanYearConcentration, outcomes.Inhalation.Points.Select(p => p.Value.Exposure.AsMeanYearConcentration.Value).ToList(), numberOfBins, outputScale));
                    }
                    else
                    {
                        results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.MeanYearConcentration));
                    }
                }
                else
                {
                    results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.MeanYearConcentration, outcomes.Inhalation.PointValue.Exposure.AsMeanYearConcentration));
                }

                if (outcomes.DistributedEndPoints.Inhalation.Exposure.ExternalEventDoseIsDistributed)
                {
                    if (calculateDistributions)
                    {
                        results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalEventDose, outcomes.Inhalation.Points.Select(p => p.Value.Exposure.AsExternalEventDose.Value).ToList(), numberOfBins, outputScale));
                    }
                    else
                    {
                        results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalEventDose));
                    }
                }
                else
                {
                    results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalEventDose, outcomes.Inhalation.PointValue.Exposure.AsExternalEventDose));
                }

                if (inhalationSimulation.SupportsExternalDayDose(scenario))
                {
                    if (outcomes.DistributedEndPoints.Inhalation.Exposure.ExternalDayDoseIsDistributed)
                    {
                        if (calculateDistributions)
                        {
                            results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalDayDose, outcomes.Inhalation.Points.Select(p => p.Value.Exposure.AsExternalDayDose.Value).ToList(), numberOfBins, outputScale));
                        }
                        else
                        {
                            results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalDayDose));
                        }
                    }
                    else
                    {
                        results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalDayDose, outcomes.Inhalation.PointValue.Exposure.AsExternalDayDose));
                    }
                }

                if (outcomes.DistributedEndPoints.Inhalation.Exposure.ExposureFractionIsDistributed)
                {
                    if (calculateDistributions)
                    {
                        results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExposureFraction, outcomes.Inhalation.Points.Select(p => p.Value.Exposure.AsExposureFraction.Value).ToList(), numberOfBins, outputScale));
                    }
                    else
                    {
                        results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExposureFraction));
                    }
                }
                else
                {
                    results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExposureFraction, outcomes.Inhalation.PointValue.Exposure.AsExposureFraction));
                }

                if (scenario.InhalationAbsorptionRouteInUse)
                {
                    if (outcomes.DistributedEndPoints.Inhalation.Absorption.InternalEventDoseIsDistributed)
                    {
                        if (calculateDistributions)
                        {
                            results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalEventDose, outcomes.Inhalation.Points.Select(p => p.Value.Absorption.AsInternalEventDose.Value).ToList(), numberOfBins, outputScale));
                        }
                        else
                        {
                            results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalEventDose));
                        }
                    }
                    else
                    {
                        results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalEventDose, outcomes.Inhalation.PointValue.Absorption.AsInternalEventDose));
                    }

                    if (inhalationSimulation.SupportsInternalDayDose(scenario))
                    {
                        if (outcomes.DistributedEndPoints.Inhalation.Absorption.InternalDayDoseIsDistributed)
                        {
                            if (calculateDistributions)
                            {
                                results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalDayDose, outcomes.Inhalation.Points.Select(p => p.Value.Absorption.AsInternalDayDose.Value).ToList(), numberOfBins, outputScale));
                            }
                            else
                            {
                                results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalDayDose));
                            }
                        }
                        else
                        {
                            results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalDayDose, outcomes.Inhalation.PointValue.Absorption.AsInternalDayDose));
                        }
                    }

                    if (outcomes.DistributedEndPoints.Inhalation.Absorption.InternalYearAverageDoseIsDistributed)
                    {
                        if (calculateDistributions)
                        {
                            results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalYearAverageDose, outcomes.Inhalation.Points.Select(p => p.Value.Absorption.AsInternalYearAverageDose.Value).ToList(), numberOfBins, outputScale));
                        }
                        else
                        {
                            results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalYearAverageDose));
                        }
                    }
                    else
                    {
                        results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalYearAverageDose, outcomes.Inhalation.PointValue.Absorption.AsInternalYearAverageDose));
                    }
                }
            }
        }

        private void DeriveDermalResults(ScenarioModel scenario, bool calculateDistributions, Outcomes outcomes, int numberOfBins, ScaleType outputScale, SimulationResults results)
        {
            if (scenario.DermalExposureRouteInUse)
            {
                if (outcomes.DistributedEndPoints.Dermal.Exposure.DermalLoadIsDistributed)
                {
                    if (calculateDistributions)
                    {
                        results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.DermalLoad, outcomes.Dermal.Points.Select(p => p.Value.Exposure.AsDermalLoad.Value).ToList(), numberOfBins, outputScale));
                    }
                    else
                    {
                        results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.DermalLoad));
                    }
                }
                else
                {
                    results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.DermalLoad, outcomes.Dermal.PointValue.Exposure.AsDermalLoad));
                }

                if (outcomes.DistributedEndPoints.Dermal.Exposure.ExternalEventDoseIsDistributed)
                {
                    if (calculateDistributions)
                    {
                        results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalEventDose, outcomes.Dermal.Points.Select(p => p.Value.Exposure.AsExternalEventDose.Value).ToList(), numberOfBins, outputScale));
                    }
                    else
                    {
                        results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalEventDose));
                    }
                }
                else
                {
                    results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalEventDose, outcomes.Dermal.PointValue.Exposure.AsExternalEventDose));
                }

                if (outcomes.DistributedEndPoints.Dermal.Exposure.ExternalDayDoseIsDistributed)
                {
                    if (calculateDistributions)
                    {
                        results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalDayDose, outcomes.Dermal.Points.Select(p => p.Value.Exposure.AsExternalDayDose.Value).ToList(), numberOfBins, outputScale));
                    }
                    else
                    {
                        results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalDayDose));
                    }
                }
                else
                {
                    results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalDayDose, outcomes.Dermal.PointValue.Exposure.AsExternalDayDose));
                }

                if (outcomes.DistributedEndPoints.Dermal.Exposure.ExposureFractionIsDistributed)
                {
                    if (calculateDistributions)
                    {
                        results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExposureFraction, outcomes.Dermal.Points.Select(p => p.Value.Exposure.AsExposureFraction.Value).ToList(), numberOfBins, outputScale));
                    }
                    else
                    {
                        results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExposureFraction));
                    }
                }
                else
                {
                    results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExposureFraction, outcomes.Dermal.PointValue.Exposure.AsExposureFraction));
                }

                if (scenario.DermalAbsorptionRouteInUse)
                {
                    if (outcomes.DistributedEndPoints.Dermal.Absorption.InternalEventDoseIsDistributed)
                    {
                        if (calculateDistributions)
                        {
                            results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalEventDose, outcomes.Dermal.Points.Select(p => p.Value.Absorption.AsInternalEventDose.Value).ToList(), numberOfBins, outputScale));
                        }
                        else
                        {
                            results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalEventDose));
                        }
                    }
                    else
                    {
                        results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalEventDose, outcomes.Dermal.PointValue.Absorption.AsInternalEventDose));
                    }

                    if (outcomes.DistributedEndPoints.Dermal.Absorption.InternalDayDoseIsDistributed)
                    {
                        if (calculateDistributions)
                        {
                            results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalDayDose, outcomes.Dermal.Points.Select(p => p.Value.Absorption.AsInternalDayDose.Value).ToList(), numberOfBins, outputScale));
                        }
                        else
                        {
                            results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalDayDose));
                        }
                    }
                    else
                    {
                        results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalDayDose, outcomes.Dermal.PointValue.Absorption.AsInternalDayDose));
                    }

                    if (outcomes.DistributedEndPoints.Dermal.Absorption.InternalYearAverageDoseIsDistributed)
                    {
                        if (calculateDistributions)
                        {
                            results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalYearAverageDose, outcomes.Dermal.Points.Select(p => p.Value.Absorption.AsInternalYearAverageDose.Value).ToList(), numberOfBins, outputScale));
                        }
                        else
                        {
                            results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalYearAverageDose));
                        }
                    }
                    else
                    {
                        results.Dermal.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalYearAverageDose, outcomes.Dermal.PointValue.Absorption.AsInternalYearAverageDose));
                    }
                }
            }
        }

        private void DeriveOralResults(ScenarioModel scenario, bool calculateDistributions, Outcomes outcomes, int numberOfBins, ScaleType outputScale, SimulationResults results)
        {
            if (scenario.OralExposureRouteInUse)
            {
                if (outcomes.DistributedEndPoints.Oral.Exposure.ExternalEventDoseIsDistributed)
                {
                    if (calculateDistributions)
                    { results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalEventDose, outcomes.Oral.Points.Select(p => p.Value.Exposure.AsExternalEventDose.Value).ToList(), numberOfBins, outputScale)); }
                    else
                    {
                        results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalEventDose));
                    }
                }
                else
                {
                    results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalEventDose, outcomes.Oral.PointValue.Exposure.AsExternalEventDose));
                }

                if (outcomes.DistributedEndPoints.Oral.Exposure.ExternalDayDoseIsDistributed)
                {
                    if (calculateDistributions)
                    { results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalDayDose, outcomes.Oral.Points.Select(p => p.Value.Exposure.AsExternalDayDose.Value).ToList(), numberOfBins, outputScale)); }
                    else
                    {
                        results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalDayDose));
                    }
                }
                else
                {
                    results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExternalDayDose, outcomes.Oral.PointValue.Exposure.AsExternalDayDose));
                }

                if (outcomes.DistributedEndPoints.Oral.Exposure.ExposureFractionIsDistributed)
                {
                    if (calculateDistributions)
                    {
                        results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExposureFraction, outcomes.Oral.Points.Select(p => p.Value.Exposure.AsExposureFraction.Value).ToList(), numberOfBins, outputScale));
                    }
                    else
                    {
                        results.Inhalation.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExposureFraction));
                    }
                }
                else
                {
                    results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.ExposureFraction, outcomes.Oral.PointValue.Exposure.AsExposureFraction));
                }

                if (scenario.OralAbsorptionRouteInUse)
                {
                    if (outcomes.DistributedEndPoints.Oral.Absorption.InternalEventDoseIsDistributed)
                    {
                        if (calculateDistributions)
                        {
                            results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalEventDose, outcomes.Oral.Points.Select(p => p.Value.Absorption.AsInternalEventDose.Value).ToList(), numberOfBins, outputScale));
                        }
                        else
                        {
                            results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalEventDose));
                        }
                    }
                    else
                    {
                        results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalEventDose, outcomes.Oral.PointValue.Absorption.AsInternalEventDose));
                    }

                    if (outcomes.DistributedEndPoints.Oral.Absorption.InternalDayDoseIsDistributed)
                    {
                        if (calculateDistributions)
                        {
                            results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalDayDose, outcomes.Oral.Points.Select(p => p.Value.Absorption.AsInternalDayDose.Value).ToList(), numberOfBins, outputScale));
                        }
                        else
                        {
                            results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalDayDose));
                        }
                    }
                    else
                    {
                        results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalDayDose, outcomes.Oral.PointValue.Absorption.AsInternalDayDose));
                    }

                    if (outcomes.DistributedEndPoints.Oral.Absorption.InternalYearAverageDoseIsDistributed)
                    {
                        if (calculateDistributions)
                        {
                            results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalYearAverageDose, outcomes.Oral.Points.Select(p => p.Value.Absorption.AsInternalYearAverageDose.Value).ToList(), numberOfBins, outputScale));
                        }
                        else
                        {
                            results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalYearAverageDose));
                        }
                    }
                    else
                    {
                        results.Oral.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalYearAverageDose, outcomes.Oral.PointValue.Absorption.AsInternalYearAverageDose));
                    }
                }
            }
        }

        private void DeriveIntegratedResults(ScenarioModel scenario, bool calculateDistributions, Outcomes outcomes, int numberOfBins, ScaleType outputScale, SimulationResults results)
        {
            results.Integrated.IsEnabled = false;

            if (scenario.DermalAbsorptionRouteInUse || scenario.InhalationAbsorptionRouteInUse || scenario.OralAbsorptionRouteInUse)
            {
                if (outcomes.DistributedEndPoints.Integrated.Absorption.InternalEventDoseIsDistributed)
                {
                    if (calculateDistributions)
                    {
                        results.Integrated.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalEventDose, outcomes.Integrated.Points.Select(p => p.Value.Absorption.AsInternalEventDose.Value).ToList(), numberOfBins, outputScale));
                    }
                    else
                    {
                        results.Integrated.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalEventDose));
                    }
                }
                else
                {
                    results.Integrated.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalEventDose, outcomes.Integrated.PointValue.Absorption.AsInternalEventDose));
                }

                if (!scenario.InhalationAbsorptionRouteInUse || inhalationSimulation.SupportsInternalDayDose(scenario))
                {
                    if (outcomes.DistributedEndPoints.Integrated.Absorption.InternalDayDoseIsDistributed)
                    {
                        if (calculateDistributions)
                        {
                            results.Integrated.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalDayDose, outcomes.Integrated.Points.Select(p => p.Value.Absorption.AsInternalDayDose.Value).ToList(), numberOfBins, outputScale));
                        }
                        else
                        {
                            results.Integrated.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalDayDose));
                        }
                    }
                    else
                    {
                        results.Integrated.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalDayDose, outcomes.Integrated.PointValue.Absorption.AsInternalDayDose));
                    }
                }

                if (outcomes.DistributedEndPoints.Integrated.Absorption.InternalYearAverageDoseIsDistributed)
                {
                    if (calculateDistributions)
                    {
                        results.Integrated.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalYearAverageDose, outcomes.Integrated.Points.Select(p => p.Value.Absorption.AsInternalYearAverageDose.Value).ToList(), numberOfBins, outputScale));
                    }
                    else
                    {
                        results.Integrated.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalYearAverageDose));
                    }
                }
                else
                {
                    results.Integrated.EndPointResults.Add(new EndPointResults(DoseMeasureType.InternalYearAverageDose, outcomes.Integrated.PointValue.Absorption.AsInternalYearAverageDose));
                }

                if (results.Integrated.EndPointResults.Count > 0)
                {
                    results.Integrated.IsEnabled = true;
                }
            }
        }

        private static RouteOutcomes<IntegratedExposureOutcome, IntegratedAbsorptionOutcome> GetIntegratedOutcome(BodyWeight bodyWeight, Frequency frequency, RouteOutcomes<DermalExposureOutcome, DermalAbsorptionOutcome> dermalPointValue, RouteOutcomes<InhalationExposureOutcome, InhalationAbsorptionOutcome> inhalationPointValue, RouteOutcomes<OralExposureOutcome, OralAbsorptionOutcome> oralPointValue)
        {
            var integratedAbsorption = new IntegratedAbsorptionOutcome(bodyWeight, frequency)
            {
                DermalAbsorptionOutcome = dermalPointValue?.Absorption,
                InhalationAbsorptionOutcome = inhalationPointValue?.Absorption,
                OralAbsorptionOutcome = oralPointValue?.Absorption
            };

            var integratedPointValue = new RouteOutcomes<IntegratedExposureOutcome, IntegratedAbsorptionOutcome>()
            {
                Absorption = integratedAbsorption
            };
            return integratedPointValue;
        }

        /// <summary>
        /// Determines whether the specified scenario is time dependent.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        public bool IsTimeDependent(ScenarioModel scenario)
        {
            return (dermalSimulation.IsTimeDependent(scenario))
                || (inhalationSimulation.IsTimeDependent(scenario))
                || (oralSimulation.IsTimeDependent(scenario));
        }

        /// <summary>
        /// Calculates a time series for the specified scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        public TimeSeries CalculateTimeSeries(ScenarioModel scenario)
        {
            var timeSeries = new TimeSeries();

            timeSeries.Inhalation = inhalationSimulation.CalculateTimeSeries(scenario);
            timeSeries.Dermal = dermalSimulation.CalculateTimeSeries(scenario);
            timeSeries.Oral = oralSimulation.CalculateTimeSeries(scenario);

            return timeSeries;
        }

        /// <summary>
        /// Gets the distributed end points.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        public DistributedEndPoints GetDistributedEndPoints(ScenarioModel scenario)
        {
            DistributedEndPoints endPoints = new DistributedEndPoints();

            if (scenario.InhalationExposureRouteInUse)
            {
                endPoints.Inhalation = inhalationSimulation.GetDistributedEndPoints(scenario);
            }

            if (scenario.DermalExposureRouteInUse)
            {
                endPoints.Dermal = dermalSimulation.GetDistributedEndPoints(scenario);
            }

            if (scenario.OralExposureRouteInUse)
            {
                endPoints.Oral = oralSimulation.GetDistributedEndPoints(scenario);
            }

            endPoints.ApplyIntegratedEndPoints();

            return endPoints;
        }

        /// <see cref="AnyEndPointDistributed"/>
        public bool AnyEndPointDistributed(ScenarioModel scenario)
        {
            var distributedEndPoints = GetDistributedEndPoints(scenario);
            return distributedEndPoints.AnyEndPointDistributed;
        }

        /// <summary>
        /// Indicates whether or not the specified scenario can be used for a sensitivity analysis.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        public bool SensitivityAnalysisEnabled(ScenarioModel scenario)
        {
            if (!SomeRouteSpecified(scenario))
            {
                return false;
            }

            return SomeRouteValid(scenario);
        }

        /// <summary>
        /// Returns the routes available for a sensitivity analysis, based on the scenario.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        public List<RouteTypes> RoutesForSensitivityAnalysis(ScenarioModel scenario)
        {
#warning ToDo: could take into account which routes have been fully specified.
            var routes = new List<RouteTypes>();

            if (scenario.InhalationExposureRouteInUse)
            { routes.Add(RouteTypes.Inhalation); }

            if (scenario.DermalExposureRouteInUse)
            { routes.Add(RouteTypes.Dermal); }

            if (scenario.OralExposureRouteInUse)
            { routes.Add(RouteTypes.Oral); }
            return routes;
        }

        /// <summary>
        /// Returns the end points available for a sensitivity analysis, based on the scenario and the selected route.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <param name="routeToAnalyse">The route to analyse.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public List<DoseMeasureType> EndPointsForSensitivityAnalysis(ScenarioModel scenario, RouteTypes routeToAnalyse)
        {
            var endPointsForSensitivityAnalysis = new List<DoseMeasureType>();
            switch (routeToAnalyse)
            {
                case RouteTypes.Dermal:
                    endPointsForSensitivityAnalysis.AddRange(dermalSimulation.EndPointsForSensitivityAnalysis(scenario));
                    break;

                case RouteTypes.Inhalation:
                    endPointsForSensitivityAnalysis.AddRange(inhalationSimulation.EndPointsForSensitivityAnalysis(scenario));
                    break;

                case RouteTypes.Oral:
                    endPointsForSensitivityAnalysis.AddRange(oralSimulation.EndPointsForSensitivityAnalysis(scenario));
                    break;

                default:
                    throw new NotSupportedException(string.Format("Unsupported route '{0}'", routeToAnalyse.ToString()));
            }

            return endPointsForSensitivityAnalysis;
        }

        /// <summary>
        /// Returns the model parameters that can be used for a sensitivity analysis for the specified scenario and the selected route and endpoint.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <param name="routeToAnalyse">The route to analyse.</param>
        /// <param name="endpointToAnalyse">The endpoint to analyse.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public IEnumerable<ModelParameters> ModelParametersForSensitivityAnalysis(ScenarioModel scenario, RouteTypes routeToAnalyse, DoseMeasureType endpointToAnalyse)
        {
            var modelParametersForSensitivityAnalysis = new List<ModelParameters>();

            switch (routeToAnalyse)
            {
                case RouteTypes.Dermal:
                    modelParametersForSensitivityAnalysis.AddRange(dermalSimulation.ModelParametersForSensitivityAnalysis(scenario, endpointToAnalyse));
                    break;

                case RouteTypes.Inhalation:
                    modelParametersForSensitivityAnalysis.AddRange(inhalationSimulation.ModelParametersForSensitivityAnalysis(scenario, endpointToAnalyse));
                    modelParametersForSensitivityAnalysis.Add(ModelParameters.InhalationExposureInhalationRate);
                    break;

                case RouteTypes.Oral:
                    modelParametersForSensitivityAnalysis.AddRange(oralSimulation.ModelParametersForSensitivityAnalysis(scenario, endpointToAnalyse));
                    break;

                default:
                    throw new ApplicationException(string.Format("Unsupported route to analyse '{0}'", routeToAnalyse));
            }

            if (endpointToAnalyse == DoseMeasureType.ExternalEventDose || endpointToAnalyse == DoseMeasureType.ExternalDayDose
                || endpointToAnalyse == DoseMeasureType.InternalEventDose || endpointToAnalyse == DoseMeasureType.InternalDayDose || endpointToAnalyse == DoseMeasureType.InternalYearAverageDose)
            {
                modelParametersForSensitivityAnalysis.Add(ModelParameters.AssessmentBodyWeight);
            }

            if (endpointToAnalyse == DoseMeasureType.ExternalDayDose
                || endpointToAnalyse == DoseMeasureType.MeanDayConcentration || endpointToAnalyse == DoseMeasureType.MeanYearConcentration
                || endpointToAnalyse == DoseMeasureType.InternalDayDose || endpointToAnalyse == DoseMeasureType.InternalYearAverageDose)
            {
                modelParametersForSensitivityAnalysis.Add(ModelParameters.ScenarioFrequency);
            }

            return modelParametersForSensitivityAnalysis;
        }

        /// <summary>
        /// Returns the model parameters that are to be included in an export to Chesar.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public IEnumerable<ModelParameters> ModelParametersForChesarExport(ScenarioModel scenario)
        {
            var modelParametersForChesarExport = new List<ModelParameters>();

            if (scenario.DermalExposureRouteInUse)
            {
                modelParametersForChesarExport.AddRange(dermalSimulation.ModelParametersForChesarExport(scenario));
            }

            if (scenario.InhalationExposureRouteInUse)
            {
                modelParametersForChesarExport.AddRange(inhalationSimulation.ModelParametersForChesarExport(scenario));
            }

            if (scenario.OralExposureRouteInUse)
            {
                modelParametersForChesarExport.AddRange(oralSimulation.ModelParametersForChesarExport(scenario));
            }

            return modelParametersForChesarExport;
        }

        /// <summary>
        /// Returns the available units for the specified model parameter.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <param name="modelParameterToAnalyse">The model parameter to analyse.</param>
        /// <returns></returns>
        public List<UnitBase> UnitsForSensitivityAnalysis(ScenarioModel scenario, ModelParameters modelParameterToAnalyse)
        {
            IPhysicalQuantityBase modelParameter = ModelParameterHelpers.GetModelParameterInstance(scenario, modelParameterToAnalyse);

            List<UnitBase> units = modelParameter.AvailableBaseUnits.ToList();

            return units;
        }

        /// <summary>
        /// Calculates the sensitivity analysis.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        /// <param name="runSettings">The run settings.</param>
        /// <returns></returns>
        public SensitivityAnalysis CalculateSensitivityAnalysis(ScenarioModel scenario, SensitivityAnalysisSettings runSettings)
        {
            const int TotalAnalysisSteps = 101;

            var lowerBound = runSettings.LowerBound;
            var upperBound = runSettings.UpperBound;
            var sensitivityAnalysis = new SensitivityAnalysis();

            var modelParameterToAnalyse = ModelParameterHelpers.GetModelParameterInstance(scenario, runSettings.ModelParameterToAnalyse);

            if (modelParameterToAnalyse.SupportsDistributions && modelParameterToAnalyse.IsDistributed)
            {
                var x = (IDistributablePhysicalQuantityBase)modelParameterToAnalyse;
                x.Distribution.DistributionType = DistributionTypes.PointValue;
            }

            for (int analysisStep = 0; analysisStep < TotalAnalysisSteps; analysisStep++)
            {
                var stepValue = lowerBound + (upperBound - lowerBound) * analysisStep / (TotalAnalysisSteps - 1);
                ApplyStepValueToScenario(scenario, runSettings.ModelParameterToAnalyse, stepValue, runSettings.UnitCode);

                Dose stepOutputValue;

                try
                {
                    switch (runSettings.RouteToAnalyse)
                    {
                        case RouteTypes.Inhalation:
                            stepOutputValue = inhalationSimulation.CalculateSensitivityAnalysis(scenario, runSettings);
                            break;

                        case RouteTypes.Dermal:
                            stepOutputValue = dermalSimulation.CalculateSensitivityAnalysis(scenario, runSettings);
                            break;

                        case RouteTypes.Oral:
                            stepOutputValue = oralSimulation.CalculateSensitivityAnalysis(scenario, runSettings);
                            break;

                        default:
                            throw new ApplicationException(string.Format("Unsupported route to analyse '{0}'", runSettings.RouteToAnalyse));
                    }

                    sensitivityAnalysis.Points.Add(new SensitivityAnalysisPoint()
                    {
                        EndPointAvailable = true,
                        AnalysisValue = stepValue,
                        EndPointValue = stepOutputValue
                    });

                    //Since some point value may raise an error, preserve the unit for one that did not return an error.
                    if (sensitivityAnalysis.EndpointUnit == null)
                    { sensitivityAnalysis.EndpointUnit = EnumHelper2<DoseUnits>.GetDisplayValue(stepOutputValue.Unit); }
                }
                catch (ODEIntegrationException exc)
                {
                    sensitivityAnalysis.Points.Add(new SensitivityAnalysisPoint()
                    {
                        EndPointAvailable = false,
                        ErrorForPoint = exc,
                        AnalysisValue = stepValue,
                        EndPointValue = null
                    });
                }
            }

            // Parameter value and unit
            sensitivityAnalysis.ModelParameterName = EnumHelper2<ModelParameters>.GetDisplayValue(runSettings.ModelParameterToAnalyse);
            modelParameterToAnalyse.UnitCode = runSettings.UnitCode;
            sensitivityAnalysis.ModelParameterUnit = modelParameterToAnalyse.UnitDisplay;

            // End Point value and unit
            sensitivityAnalysis.EndpointName = EnumHelper2<DoseMeasureType>.GetDisplayValue(runSettings.EndPointToAnalyse);

            return sensitivityAnalysis;
        }

        /// <summary>
        /// Modify the scenario by setting the analysed parameter to a new step value.
        /// </summary>
        private void ApplyStepValueToScenario(ScenarioModel scenario, ModelParameters modelParameterToAnalyse, double stepValue, int unitCode)
        {
            IPhysicalQuantityBase modelParameter = ModelParameterHelpers.GetModelParameterInstance(scenario, modelParameterToAnalyse);

            modelParameter.UnitCode = unitCode;
            modelParameter.Value = stepValue;
        }

        /// <summary>
        /// Method for filtering for end points needed for standard results.
        /// </summary>
        /// <remarks>public static, so it can be called when the results have been stored.</remarks>
        public static void FilterForStandardResults(SimulationResults results)
        {
            results?.Inhalation?.EndPointResults.RemoveAll(epr => epr.DoseMeasureType == DoseMeasureType.ExposureFraction);
            results?.Dermal?.EndPointResults.RemoveAll(epr => epr.DoseMeasureType == DoseMeasureType.ExposureFraction);
            results?.Oral?.EndPointResults.RemoveAll(epr => epr.DoseMeasureType == DoseMeasureType.ExposureFraction);
        }

        /// <summary>
        /// Method for filtering for end points needed for exposure fractions.
        /// </summary>
        /// <remarks>public static, so it can be called when the results have been stored.</remarks>
        public static void FilterForExposureFractions(SimulationResults results)
        {
            results?.Inhalation?.EndPointResults.RemoveAll(epr => epr.DoseMeasureType != DoseMeasureType.ExposureFraction);
            results?.Dermal?.EndPointResults.RemoveAll(epr => epr.DoseMeasureType != DoseMeasureType.ExposureFraction);
            results?.Oral?.EndPointResults.RemoveAll(epr => epr.DoseMeasureType != DoseMeasureType.ExposureFraction);
            results.Integrated.EndPointResults.Clear();
            results.Integrated.IsEnabled = false;
        }
    }
}