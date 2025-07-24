namespace RIVM.ConsExpo.DTO.Distributions
{
    /// <summary>
    /// A bin in a discrete probability distribution.
    /// </summary>
    public class SizeBin
    {
        /// <summary>
        /// The value for which a probability is specified.
        /// </summary>
        public double Variable { get; set; }

        /// <summary>
        /// Gets or sets delta.
        /// </summary>
        /// <value>
        /// The width of the bin.
        /// </value>
        public double Delta { get; set; }

        /// <summary>
        /// The (relative) number of elements in the bin.
        /// </summary>
        public double ProbabilityMass { get; set; }
    }
}