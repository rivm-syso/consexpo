using System;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Factsheets
{
    /// <summary>
    /// A class derived from scenario to store a scenario from the factsheet database, including the quality indication.
    /// By doing this, we can use the same logic for previewing and for importing.
    /// </summary>
    /// <seealso cref="RIVM.ConsExpo.DTO.Entities.ScenarioModel" />
    public class DermalQuality
    {
        // Scenario
        public string FREQUENCY_Q { get; set; }

        // Dermal
        public Nullable<decimal> EXPOSEDAREA_Q { get; set; }

        public Nullable<decimal> PRODUCTAMOUNT_Q { get; set; }
        public Nullable<decimal> DILUTION_Q { get; set; }
        public Nullable<decimal> LAYERTHICKNESS_Q { get; set; }
        public Nullable<decimal> LEACHABLEFRACTION_Q { get; set; }
        public Nullable<decimal> SKINCONTACTFACTOR_Q { get; set; }
        public Nullable<decimal> CONTACTRATE_Q { get; set; }
        public Nullable<decimal> DISLODGEABLEFORMULATION_Q { get; set; }
        public Nullable<decimal> RUBBEDSURFACE_Q { get; set; }
        public Nullable<decimal> TRANSFERCOEFFICIENT_Q { get; set; }
        public Nullable<decimal> EXPOSURETIME_Q { get; set; }
    }
}