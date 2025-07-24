using System;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Factsheets
{
    /// <summary>
    /// A class derived from scenario to store a scenario from the factsheet database, including the quality indication.
    /// By doing this, we can use the same logic for previewing a for importing.
    /// </summary>
    /// <seealso cref="RIVM.ConsExpo.DTO.Entities.ScenarioModel" />
    public class GeneralQuality
    {
        // Assessment

        public Nullable<decimal> BODYWEIGHT_Q { get; set; }

        // Scenario
        public Nullable<decimal> FREQUENCY_Q { get; set; }
    }
}