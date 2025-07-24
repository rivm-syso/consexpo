using System;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Factsheets
{
    /// <summary>
    /// A class derived from scenario to store a scenario from the factsheet database, including the quality indication.
    /// By doing this, we can use the same logic for previewing and for importing.
    /// </summary>
    /// <seealso cref="RIVM.ConsExpo.DTO.Entities.ScenarioModel" />
    public class InhalationQuality
    {
        public Nullable<decimal> ROOMVOLUME_Q { get; set; }
        public Nullable<decimal> EMISSIONRATE_Q { get; set; }
        public Nullable<decimal> VENTILATIONRATE_Q { get; set; }
        public Nullable<decimal> RELEASEAREA_Q { get; set; }
        public Nullable<decimal> PRODUCTAMOUNT_Q { get; set; }
        public Nullable<decimal> EMISSIONDURATION_Q { get; set; }
        public Nullable<decimal> APPLICATIONDURATION_Q { get; set; }
        public Nullable<decimal> MOLWEIGHTMATRIX_Q { get; set; }
        public Nullable<decimal> DILUTION_Q { get; set; }
        public Nullable<decimal> INHALATIONRATE_Q { get; set; }
        public Nullable<decimal> EXPOSUREDURATION_Q { get; set; }
        public Nullable<decimal> SPRAYDURATION_Q { get; set; }
        public Nullable<decimal> CLOUDVOLUME_Q { get; set; }
        public Nullable<decimal> ROOMHEIGHT_Q { get; set; }
        public Nullable<decimal> MASSRELEASERATE_Q { get; set; }
        public Nullable<decimal> AIRBORNFRACTION_Q { get; set; }
        public Nullable<decimal> WEIGHTFRACTIONPROPELLANT_Q { get; set; }
        public Nullable<decimal> WEIGHTFRACTIONNONVOLATILE_Q { get; set; }
        public Nullable<decimal> WEIGHTFRACTIONSOLVENT_Q { get; set; }
        public Nullable<decimal> DENSITYSOLVENT_Q { get; set; }
        public Nullable<decimal> DENSITYNONVOLATILE_Q { get; set; }
        public Nullable<decimal> PARTICLEDISTRIBUTIONMEDIAN_Q { get; set; }
        public Nullable<decimal> PARTICLEDISTRIBUTIONCV_Q { get; set; }
        public Nullable<decimal> MASSTRANSFERCOEFFICIENT_Q { get; set; }
    }
}