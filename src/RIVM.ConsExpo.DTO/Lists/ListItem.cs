namespace RIVM.ConsExpo.DTO.Lists
{
    /// <summary>
    /// And abstract version of list items that does not depend on System.Web.
    /// </summary>
    public class ListItem
    {
        private bool disabled = false;

        private bool selected = false;

        /// <summary>
        /// The item is disabled.
        /// </summary>
        public bool Disabled { get { return disabled; } set { disabled = value; } }

        /// <summary>
        /// The selectedThe item is selected
        /// </summary>
        public bool Selected { get { return selected; } set { selected = value; } }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>
        /// The text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }
    }
}