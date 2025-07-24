using RIVM.ConsExpo.DTO.Models;
using RIVM.ConsExpo.DTO.Output;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RIVM.ConsExpo.Model.Interfaces.Submodels
{
    /// <summary>
    /// The common interface for all submodels that implement calculations for a route.
    /// </summary>
    public interface ISubmodel
    {
        /// <summary>
        /// Gets a value indicating whether the model is time dependent. Only if it is, it can be used in time series calculation.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is time dependent; otherwise, <c>false</c>.
        /// </value>
        bool IsTimeDependent { get; }

        /// <summary>
        /// The End points available for sensitivity analysis.
        /// </summary>
        /// <returns></returns>
        List<DoseMeasureType> EndPointsForSensitivityAnalysis();

        /// <summary>
        /// The physical quantities that are available for sensitivity analysis when this submodel is in use.
        /// </summary>
        List<ModelParameters> ModelParameters();

        /// <summary>
        /// Validates the specified scenario on completeness and consistency of the input parameters.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ValidationResult> Validate();
    }
}