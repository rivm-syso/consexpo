namespace RIVM.ConsExpo.DTO.Output
{
    public class BatchResult
    {
        public int Nr { get; set; }
        public string ScenarioName { get; set; }
        public string SubstanceName { get; set; }
        public string PopulationName { get; set; }
        public string RouteName { get; set; }
        public string ExposureModelName { get; set; }
        public string AbsorbtionModelName { get; set; }
        public Dose MeanEventConcentration { get; set; }
        public Dose DermalLoad { get; set; }
        public Dose ExternalEventDose { get; set; }
        public Dose ExternalDayDose { get; set; }
        public Dose InternalEventDose { get; set; }
        public Dose InternalDayDose { get; set; }
        public Dose InternalYearAverageDose { get; set; }
    }
}
