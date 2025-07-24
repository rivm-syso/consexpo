using System.Xml.Serialization;

namespace RIVM.ConsExpo.DTO.Entities
{
    /// <summary>
    /// One bin of a non-parametric particle size distribution.
    /// </summary>
    public class NonParametricSizeBin
    {
        [XmlIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double UpperBound { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double RelativeMass { get; set; }

        [XmlIgnore]
        public int NonParametricSizeDistributionId { get; set; }

        [XmlIgnore]
        public virtual NonParametricSizeDistribution NonParametricSizeDistribution { get; set; }
    }
}