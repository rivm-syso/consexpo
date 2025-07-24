using System.ComponentModel.DataAnnotations;

namespace RIVM.ConsExpo.DTO.Distributions
{
    /// <summary>
    /// The types of distributions which can be used for particle diameters.
    /// </summary>
    public enum SizeDistributionTypes
    {
        /// <summary>
        /// The normal distribution.
        /// </summary>
        [Display(Name = "Normal")]
        Normal = 2,

        /// <summary>
        /// The log-normal distribution
        /// </summary>
        [Display(Name = "Log-normal")]
        LogNormal = 3,

        /// <summary>
        /// The log-normal distribution
        /// </summary>
        [Display(Name = "Non-parametric")]
        NonParametric = 4
    }
}