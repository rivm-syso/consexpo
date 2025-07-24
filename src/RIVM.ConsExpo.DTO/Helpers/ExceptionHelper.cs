using RIVM.ConsExpo.DTO.PhysicalQuantities;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Helpers
{
    internal class ExceptionHelper
    {
        public static string NoStandardizedValueMessage(string className, IDistributablePhysicalQuantity<UnitBase> distributablePhysicalQuantity)
        {
            if (distributablePhysicalQuantity.IsDistributed)
            {
                return string.Format("A standarized value is requested for a '{0}', but the value is not specified and no value can be derived from the distribution of type '{1}'.", className, distributablePhysicalQuantity.DistributionType.ToString());
            }
            else
            {
                return string.Format("A standarized value is requested for a '{0}', but the value is not specified and no distribution is specified to derive a median value from.", className);
            }
        }

        public static string NoStandardizedValueMessage(string className, IPhysicalQuantity<UnitBase> physicalQuantity)
        {
            if (physicalQuantity.IsDistributed)
            {
                return string.Format("A standarized value is requested for a '{0}', but the value is not specified and no value can be derived from the distribution.", className);
            }
            else
            {
                return string.Format("A standarized value is requested for a '{0}', but the value is not specified.", className);
            }
        }
    }
}