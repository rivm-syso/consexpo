using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Entities
{
    /// <summary>
    /// A batch assessment definition for one user.
    /// </summary>
    public class BatchAssessmentModel
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }

        public virtual UserModel User { get; set; }

        public const int MaxNameLength = 200;

        [Required(AllowEmptyStrings = false)]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        public int DefaultPopulationDatabaseId { get; set; }

        public DefaultPopulationDatabase DefaultPopulationDatabase { get; set; }

        public virtual List<BatchLineModel> Lines { get; set; }
    }
}