using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// The base class for all units. It acts as an (extended) enumeration, which make inheritance possible.
    /// </summary>
    public abstract class UnitBase
    {
        /// <summary>
        /// Needed for EF.
        /// </summary>
        protected UnitBase()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitBase"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="order">The order.</param>
        /// <param name="conversionFactor">The conversion factor.</param>
        protected UnitBase(int code, string displayName, int order, double conversionFactor)
        {
            this.Code = code;
            this.DisplayName = displayName;
            this.Order = order;
            this.ConversionFactor = conversionFactor;
        }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code, acting as the enum name.
        /// </value>
        [Required]
        public int Code { get; protected set; }

        /// <summary>
        /// Gets or sets the display name, used for displaying to the user.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        [Required]
        public string DisplayName { get; protected set; }

        /// <summary>
        /// Gets or sets the order in selected lists, etc.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public int Order { get; protected set; }

        /// <summary>
        /// Gets or sets the conversion factor to multiple the value with, in order to express the quantity in the standard unit.
        /// </summary>
        /// <value>
        /// The conversion factor.
        /// </value>
        /// <remarks>Some quantities, like temperature, do not convert with a factor, but with an offset. Behaviour is overridden in these classes.
        /// </remarks>
        public double ConversionFactor { get; protected set; }
    }
}