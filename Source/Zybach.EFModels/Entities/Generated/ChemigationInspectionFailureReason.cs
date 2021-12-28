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
            ChemigationInspections = new HashSet<ChemigationInspections>();
        }

        [Key]
        public int ChemigationInspectionFailureReasonID { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationInspectionFailureReasonName { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationInspectionFailureReasonDisplayName { get; set; }

        [InverseProperty(nameof(Entities.ChemigationInspections.ChemigationInspectionFailureReason))]
        public virtual ICollection<ChemigationInspections> ChemigationInspections { get; set; }
    }
}
