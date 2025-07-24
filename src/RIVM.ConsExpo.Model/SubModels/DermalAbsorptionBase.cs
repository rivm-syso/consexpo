using RIVM.ConsExpo.DTO.Output;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using System.Collections.Generic;

namespace RIVM.ConsExpo.Model.SubModels
{
    internal abstract class DermalAbsorptionBase
    {
        public virtual void PrepareTimeSeries(Time maxTime)
        {
            //By default, assume no preparation is needed.
        }

        public virtual List<DoseMeasureType> EndPointsForSensitivityAnalysis()
        {
#warning To Do: check relevant parameters values to see if the end point is really available.
            var endPoints = new List<DoseMeasureType>
            {
                DoseMeasureType.InternalEventDose
            };

            return endPoints;
        }
    }
}