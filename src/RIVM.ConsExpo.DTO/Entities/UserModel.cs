using System;
using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Entities
{
    /// <summary>
    /// A user of ConsExpo.
    /// </summary>
    public class UserModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the ticket.
        /// </summary>
        /// <value>
        /// The ticket.
        /// </value>
        /// <seealso>InitUniqueNullableIndexes calls, used in DbInitializer for generation of a unique nullable key</seealso>
        public Nullable<Guid> Ticket { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        /// <seealso>InitUniqueNullableIndexes calls, used in DbInitializer for generation of a unique nullable key</seealso>
        [StringLength(254)] // Based on http://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
        public string EmailAddress { get; set; }

        [MaxLength(1)]
        public string DecimalSeparatorForNumericValuesInDownload { get; set; }
    }
}