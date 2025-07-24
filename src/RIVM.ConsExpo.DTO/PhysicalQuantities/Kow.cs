#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

using DataAnnotationsExtensions;

namespace RIVM.ConsExpo.DTO.PhysicalQuantities
{
    public class Kow : PartitionCoefficient
    {
        public Kow()
            : base()
        { }

        public Kow(bool setDefaults)
            : base(setDefaults)
        { }

        [Min(-10)]
        public override double? Value
        {
            get => base.Value;
            set => base.Value = value;
        }

        protected override double MinForDefaultUnit => 1E-10;
    }
}