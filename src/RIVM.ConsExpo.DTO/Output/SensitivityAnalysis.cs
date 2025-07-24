using System.Collections.Generic;

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// An object containing the results of a sensitivity analysis.
    /// </summary>
    public class SensitivityAnalysis
    {
        /// <summary>
        /// Gets or sets the unit of the analysed endpoint.
        /// </summary>
        /// <value>
        /// The endpoint unit.
        /// </value>
        public string EndpointUnit { get; set; }

        /// <summary>
        /// Gets or sets the name of the analysed endpoint.
        /// </summary>
        /// <value>
        /// The name of the endpoint.
        /// </value>
        public string EndpointName { get; set; }

        /// <summary>
        /// Gets or sets the name of the analysed model parameter.
        /// </summary>
        /// <value>
        /// The name of the model parameter.
        /// </value>
        public string ModelParameterName { get; set; }

        /// <summary>
        /// Gets or sets the model unit of the analysed parameter.
        /// </summary>
        /// <value>
        /// The model parameter unit.
        /// </value>
        public string ModelParameterUnit { get; set; }

        /// <summary>
        /// Gets or sets the messages for issues with the configuration of the analysis.
        /// </summary>
        /// <value>
        /// The messages.
        /// </value>
        public List<string> Messages { get; set; }

        /// <summary>
        /// The data points of the scan over the parameter varied in the sensitivity analysis.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public List<SensitivityAnalysisPoint> Points { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SensitivityAnalysis" /> class, with an empty list of data points.
        /// </summary>
        public SensitivityAnalysis()
        {
            Points = new List<SensitivityAnalysisPoint>();
        }
    }
}