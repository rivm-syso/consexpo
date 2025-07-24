using RIVM.ConsExpo.DTO.PhysicalUnits;
using System.Collections.Generic;

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// An interface for physical quantities, for displaying purposes.
    /// </summary>
    public interface IPhysicalQuantityBase
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        double? Value { get; set; }

        /// <summary>
        /// Gets or sets the unit code.
        /// </summary>
        /// <value>
        /// The unit code.
        /// </value>
        int UnitCode { get; set; }

        /// <summary>
        /// Gets the dynamic minimum allowed value for the instance, based on the current unit.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        double Min { get; }

        /// <summary>
        /// Gets the optional dynamic maximum allowed value for the instance, based on the current unit.
        /// </summary>
        /// <value>
        /// The minimum.
        /// </value>
        double? Max { get; }

        /// <summary>
        /// Gets the a user-friend string representation of the unit.
        /// </summary>
        /// <value>
        /// The unit display.
        /// </value>
        string UnitDisplay { get; }

        /// <summary>
        /// Gets a value indicating whether the physical quantity supports specification of a distribution.
        /// </summary>
        /// <value>
        /// <c>true</c> if [supports distributions]; otherwise, <c>false</c>.
        /// </value>
        bool SupportsDistributions { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is distributed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is distributed; otherwise, <c>false</c>.
        /// </value>
        bool IsDistributed { get; }

        /// <summary>
        /// Gets the available units upcasted to base units.
        /// </summary>
        /// <value>
        /// The available base units.
        /// </value>
        IEnumerable<UnitBase> AvailableBaseUnits { get; }

        
    }
}