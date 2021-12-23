using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationInspectionFailureReason")]
    [Index(nameof(ChemigationInspectionFailureReasonDisplayName), Name = "AK_ChemigationInspectionFailureReason_ChemigationInspectionFailureReasonDisplayName", IsUnique = true)]
    [Index(nameof(ChemigationInspectionFailureReasonName), Name = "AK_ChemigationInspectionFailureReason_ChemigationInspectionFailureReasonName", IsUnique = true)]
    public partial class ChemigationInspectionFailureReason
    {
        public ChemigationInspectionFailureReason()
        {
            ChemigationInspections = new HashSet<ChemigationInspection>();
        }

        [Key]
        public int ChemigationInspectionFailureReasonID { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationInspectionFailureReasonName { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationInspectionFailureReasonDisplayName { get; set; }

        [InverseProperty(nameof(ChemigationInspection.ChemigationInspectionFailureReason))]
        public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; }

        public static IEnumerable<ChemigationInspectionFailureReasonDto> List(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationInspectionFailureReasons
                .AsNoTracking()
                .Select(x => x.AsDto()).ToList();
        }
    }
}
