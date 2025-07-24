using System.Collections.Generic;

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// This class exists just as an abbreviation for the generic list.
    /// </summary>
    public class DermalTimeSeries : List<TimeSeriesPoint<DermalExposureOutcome, DermalAbsorptionOutcome>>
    {
    }
}