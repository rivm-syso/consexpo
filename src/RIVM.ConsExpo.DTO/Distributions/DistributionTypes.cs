using System.ComponentModel.DataAnnotations;

namespace RIVM.ConsExpo.DTO.Distributions
{
    /// <summary>
    /// The types of distributions which can be used for model parameters in a Monte Carlo simulation.
    /// </summary>
    /// <remarks>These values are also switch in js, see 'data-discriminators' attributes.</remarks>
    public enum DistributionTypes
    {
        /// <summary>
        /// use a 'Degenerate distribution', i.e. Always use the same value.
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Degenerate_distribution"/>
        [Display(Name = "None")]
        PointValue = 0,

        /// <summary>
        /// Use a uniform distribution.
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Uniform_distribution_(continuous)"/>
        [Display(Name = "Uniform")]
        Uniform = 1,

        /// <summary>
        /// Use a normal distribution.
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Normal_distribution"/>
        [Display(Name = "Normal")]
        Normal = 2,

        /// <summary>
        /// use a log-normal distribution.
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Log-normal_distribution"/>
        [Display(Name = "Log-normal")]
        LogNormal = 3,

        /// <summary>
        ///
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Triangular_distribution"/>
        [Display(Name = "Triangular")]
        Triangular = 4,

        /// <summary>
        /// beta distribution
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Beta_distribution"/>
        [Display(Name = "Beta")]
        Beta = 5
    }
}