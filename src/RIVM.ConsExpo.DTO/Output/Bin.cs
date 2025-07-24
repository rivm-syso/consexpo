namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// A bin of a histogram.
    /// </summary>
    public class Bin
    {
        /// <summary>
        /// The lower bound (inclusive) of values in this bin.
        /// </summary>
        public double LowerBound { get; set; }

        /// <summary>
        /// The upper bound (exclusive, except for the last bin) of values in this bin.
        /// </summary>
        public double UpperBound { get; set; }

        /// <summary>
        /// Gets the average of lower and upper bounds of the bin.
        /// </summary>
        /// <value>
        /// The average.
        /// </value>
        /// <remarks>Useful for plotting the bin as a column in a chart.</remarks>
        public double Mean { get; set; }

        /// <summary>
        /// Gets or sets the number of outcomes in the bin.
        /// </summary>
        /// <value>
        /// The number of outcomes.
        /// </value>
        public int NumberOfOutcomes { get; set; }

        /// <summary>
        /// Gets or sets the cumulative fraction.
        /// </summary>
        /// <value>
        /// The cumulative fraction.
        /// </value>
        public double CumulativeFraction { get; set; }
    }
}