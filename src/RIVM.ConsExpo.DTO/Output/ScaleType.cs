using System.ComponentModel.DataAnnotations;

namespace RIVM.ConsExpo.DTO.Output
{
    public enum ScaleType
    {
        [Display(Name = "Linear")]
        Linear = 0,

        [Display(Name = "Logarithmic")]
        Logarithmic = 1,
    }
}
