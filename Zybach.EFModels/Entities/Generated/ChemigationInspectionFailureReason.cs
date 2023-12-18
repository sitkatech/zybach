using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("ChemigationInspectionFailureReason")]
[Index("ChemigationInspectionFailureReasonDisplayName", Name = "AK_ChemigationInspectionFailureReason_ChemigationInspectionFailureReasonDisplayName", IsUnique = true)]
[Index("ChemigationInspectionFailureReasonName", Name = "AK_ChemigationInspectionFailureReason_ChemigationInspectionFailureReasonName", IsUnique = true)]
public partial class ChemigationInspectionFailureReason
{
    [Key]
    public int ChemigationInspectionFailureReasonID { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string ChemigationInspectionFailureReasonName { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string ChemigationInspectionFailureReasonDisplayName { get; set; }

    [InverseProperty("ChemigationInspectionFailureReason")]
    public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; } = new List<ChemigationInspection>();
}
