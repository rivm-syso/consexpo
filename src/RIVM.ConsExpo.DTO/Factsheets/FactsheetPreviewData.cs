using System.Collections.Generic;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Factsheets
{
    public class FactsheetPreviewData
    {
        public List<FactsheetPreviewValue> GeneralPreviewValues { get; private set; }
        public List<FactsheetPreviewValue> DermalPreviewValues { get; private set; }
        public List<FactsheetPreviewValue> InhalationPreviewValues { get; private set; }
        public List<FactsheetPreviewValue> OralPreviewValues { get; private set; }

        public bool DermalRouteInUse { get; set; }
        public bool InhalationRouteInUse { get; set; }
        public bool OralRouteInUse { get; set; }

        public FactsheetPreviewData()
        {
            GeneralPreviewValues = new List<FactsheetPreviewValue>();
            DermalPreviewValues = new List<FactsheetPreviewValue>();
            InhalationPreviewValues = new List<FactsheetPreviewValue>();
            OralPreviewValues = new List<FactsheetPreviewValue>();
        }
    }
}