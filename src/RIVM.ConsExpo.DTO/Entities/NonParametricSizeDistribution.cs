using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace RIVM.ConsExpo.DTO.Entities
{
    /// <summary>
    /// A size distribution that it specified by a set of bins, typically by an upload.
    /// </summary>
    public class NonParametricSizeDistribution
    {
        public NonParametricSizeDistribution()
        {
            Bins = new List<NonParametricSizeBin>();
        }

        [XmlIgnore] public int Id { get; set; }

        /// <summary>
        /// The user to whom the distributions belongs.
        /// </summary>
        [XmlIgnore]
        public int UserId { get; set; }

        [XmlIgnore]
        public virtual UserModel User { get; set; }

        public const int MaxNameLength = 200;

        /// <summary>
        /// The name of the distribution, as supplied by the user.
        /// </summary>
        [StringLength(MaxNameLength)]
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }


        public virtual List<NonParametricSizeBin> Bins { get; set; }

        [XmlIgnore]
        public virtual List<InhalationExposureModel> InhalationExposureModels { get; set; }
    }
}