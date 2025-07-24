using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    /// <summary>
    /// A special class for the standard deviation of a diameter, because it has its own minimum value.
    /// </summary>
    /// <see href="http://stackoverflow.com/questions/22680342/value-cannot-be-null-parameter-name-entityset/33614273#33614273">For an explanation why this class cannot derive from Diameter.</see>
    public class DiameterStandardDeviation : FixedDiameter
    {
        [Display(Name = "Standard deviation")]
        [Min(1E-2, ErrorMessage = "The value of {0} must be greater than or equal to {1}")] //Prevent a 'spike' distribution, because sampling it might miss the spike.
        public override double? Value
        {
            get => base.Value;
            set => base.Value = value;
        }
    }
}