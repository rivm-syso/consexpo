using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Output
{
    public enum RouteTypes
    {
        [Display(Name = "Inhalation")]
        Inhalation = 1,

        [Display(Name = "Dermal")]
        Dermal = 2,

        [Display(Name = "Oral")]
        Oral = 3
    }
}