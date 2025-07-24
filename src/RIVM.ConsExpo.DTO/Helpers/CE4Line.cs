namespace RIVM.ConsExpo.DTO.Helpers
{
    /// <summary>
    /// Contains a line read from a CE4 file.
    /// </summary>
    public class CE4Line
    {
        /// <summary>
        /// Gets or sets the line.
        /// </summary>
        /// <value>
        /// The line.
        /// </value>
        public string Line { get; set; }

        /// <summary>
        /// Gets or sets the line number.
        /// </summary>
        /// <value>
        /// The line number.
        /// </value>
        public int LineNumber { get; set; }
    }
}