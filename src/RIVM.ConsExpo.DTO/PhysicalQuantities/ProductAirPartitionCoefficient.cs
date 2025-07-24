#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

using DataAnnotationsExtensions;

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class ProductAirPartitionCoefficient : PartitionCoefficient
    {
        public ProductAirPartitionCoefficient()
            : base()
        { }

        public ProductAirPartitionCoefficient(bool setDefaults)

            : base(setDefaults)
        { }

        [Min(-4)]
        public override double? Value
        {
            get => base.Value;
            set => base.Value = value;
        }

        protected override double MinForDefaultUnit => 0.0001;

        protected override double? MaxForDefaultUnit => 1E15;
    }
}