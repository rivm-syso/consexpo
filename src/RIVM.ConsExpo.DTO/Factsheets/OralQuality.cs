using System;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Factsheets
{
    /// <summary>
    /// A class derived from scenario to store a scenario from the factsheet database, including the quality indication.
    /// By doing this, we can use the same logic for previewing and for importing.
    /// </summary>
    /// <seealso cref="RIVM.ConsExpo.DTO.Entities.ScenarioModel" />
    public class OralQuality
    {
        public Nullable<decimal> PRODUCTAMOUNT_Q { get; set; }
        public Nullable<decimal> CONCENTRATIONPACKAGING_Q { get; set; }
        public Nullable<decimal> THICKNESSPACKAGING_Q { get; set; }
        public Nullable<decimal> CONTACTAREA_Q { get; set; }
        public Nullable<decimal> AMOUNTINGESTED_Q { get; set; }
        public Nullable<decimal> INGESTIONRATE_Q { get; set; }
        public Nullable<decimal> MIGRATIONRATE_Q { get; set; }
        public Nullable<decimal> EXPOSUREDURATION_Q { get; set; }
        public string PRODUCTCONCENTRATION_Q { get; set; }
        public string CONTACTAREAPACKAGING_Q { get; set; }
        public string STORAGETIME_Q { get; set; }
    }
}