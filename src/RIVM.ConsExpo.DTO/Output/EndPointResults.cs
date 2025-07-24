using System;
using System.Collections.Generic;
using System.Linq;
using RIVM.ConsExpo.DTO.Extensions;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// Stores the results for one end point, ready for rendering in a view.
    /// </summary>
    public class EndPointResults
    {
        [Obsolete("Added to solve \"RIVM.ConsExpo.DTO.Output.EndPointResults cannot be serialized because it does not have a parameterless constructor.\"")]
        public EndPointResults()
        { }

        public EndPointResults(DoseMeasureType doseMeasureType, Dose dose)
        {
            this.DoseMeasureType = doseMeasureType;
            IsDistributed = false;
            HasResults = true;
            PointValue = dose;
            Statistics = null;
            Histogram = null;
        }

        public EndPointResults(DoseMeasureType doseMeasureType, List<double?> doseValues, int numberOfBins, ScaleType outputScale)
        {
            DoseMeasureType = doseMeasureType;
            DoseUnits doseUnit = doseMeasureType.GetDoseUnit();
            IsDistributed = true;
            HasResults = true;
            PointValue = null;

            List<double> assignedValues = doseValues
                .Where(dv => dv.HasValue)
                .Select(dv => dv.Value)
                .ToList();

            Statistics = new Statistics(assignedValues, doseUnit);
            Histogram = new Histogram(assignedValues, numberOfBins, outputScale, doseUnit);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndPointResults"/> class that is an empty result.
        /// </summary>
        /// <param name="doseMeasureType">Type of the dose measure.</param>
        public EndPointResults(DoseMeasureType doseMeasureType)
        {
            DoseMeasureType = doseMeasureType;
            IsDistributed = true;
            HasResults = false;
        }

        /// <remarks>The setter is only to be used for deserialization of stored results</remarks>
        public DoseMeasureType DoseMeasureType { get; set; }

        /// <remarks>The setter is only to be used for deserialization of stored results</remarks>
        public bool IsDistributed { get; set; }

        /// <remarks>The setter is only to be used for deserialization of stored results</remarks>
        public bool HasResults { get; set; }

        /// <remarks>The setter is only to be used for deserialization of stored results</remarks>
        public Dose PointValue { get; set; }

        /// <remarks>The setter is only to be used for deserialization of stored results</remarks>
        public Statistics Statistics { get; set; }

        /// <remarks>The setter is only to be used for deserialization of stored results</remarks>
        public Histogram Histogram { get; set; }
    }
}