using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;

namespace RIVM.ConsExpo.DTO.Settings
{
    /// <summary>
    /// Settings that are used for a sensitivity analysis, which are needed in addition to the data in the scenario model.
    /// </summary>
    public class SensitivityAnalysisSettings
    {
        /// <summary>
        /// Gets or sets the route to analyse.
        /// </summary>
        /// <value>
        /// The route to analyse.
        /// </value>
        public RouteTypes RouteToAnalyse { get; set; }

        /// <summary>
        /// Gets or sets the endpoint to analyse.
        /// </summary>
        /// <value>
        /// The endpoint to analyse.
        /// </value>
        public DoseMeasureType EndPointToAnalyse { get; set; }

        /// <summary>
        /// Gets or sets the physical quantity to analyse.
        /// </summary>
        /// <value>
        /// The physical quantity to analyse.
        /// </value>
        public ModelParameters ModelParameterToAnalyse { get; set; }

        /// <summary>
        /// Gets or sets the lowest value for the physical quantity to analyse.
        /// </summary>
        /// <value>
        /// The lower bound.
        /// </value>
        public double LowerBound { get; set; }

        /// <summary>
        /// Gets or sets the highest value for the physical quantity to analyse.
        /// </summary>
        /// <value>
        /// The upper bound.
        /// </value>
        public double UpperBound { get; set; }

        /// <summary>
        /// Gets or sets the unit for the lower and upper bounds.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        public int UnitCode { get; set; }
    }
}