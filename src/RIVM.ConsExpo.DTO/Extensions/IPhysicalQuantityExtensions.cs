using System;
using RIVM.ConsExpo.DTO.PhysicalQuantities;

namespace RIVM.ConsExpo.DTO.Extensions
{
    public static class IPhysicalQuantityExtensions
    {
        /// <summary>
        /// Assign the value and unit from the source to the target.
        /// </summary>
        public static void AssignValueAndUnit(this IPhysicalQuantityBase target, IPhysicalQuantityBase source)
        {
            if (target.IsDistributed)
                throw new ArgumentException("Cannot use this method to copy value and unit of a distributed physical quantity.");

            target.Value = source.Value;
            target.UnitCode = source.UnitCode;
        }
    }
}