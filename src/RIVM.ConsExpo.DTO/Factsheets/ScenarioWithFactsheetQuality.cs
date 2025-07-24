using RIVM.ConsExpo.DTO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RIVM.ConsExpo.DTO.Factsheets
{
    /// <summary>
    /// A class on top of a scenario to store a scenario from the factsheet database, including the quality indication.
    /// By doing this, we can use the same logic for previewing as for importing.
    /// </summary>
    public class ScenarioWithFactsheetQuality
    {
        public ScenarioWithFactsheetQuality(bool setDefaults)
        {
            Init(setDefaults);
        }

        private void Init(bool setDefaults)
        {
            Scenario = new ScenarioModel(setDefaults);
            GeneralQuality = new GeneralQuality();
            DermalQuality = new DermalQuality();
            InhalationQuality = new InhalationQuality();
            OralQuality = new OralQuality();
        }

        public ScenarioModel Scenario { get; protected set; }

        public GeneralQuality GeneralQuality { get; protected set; }

        public DermalQuality DermalQuality { get; protected set; }

        public InhalationQuality InhalationQuality { get; protected set; }

        public OralQuality OralQuality { get; protected set; }
    }
}