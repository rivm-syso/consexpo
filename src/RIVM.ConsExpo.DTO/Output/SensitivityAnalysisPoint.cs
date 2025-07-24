using System;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// A class that can contain the data for one point in a sensitivity analysis.
    /// </summary>
    public class SensitivityAnalysisPoint
    {
        /// <summary>
        /// Gets or sets the value of the analysed physical quantity for the current step.
        /// </summary>
        /// <value>
        /// The analysis value.
        /// </value>
        public double AnalysisValue { get; set; }

        public bool EndPointAvailable { get; set; }

        public Exception ErrorForPoint { get; set; }

        /// <summary>
        /// Gets or sets the end point value, if it could be calculated.
        /// </summary>
        /// <value>
        /// The end point value.
        /// </value>
        public Dose EndPointValue { get; set; }
    }
}