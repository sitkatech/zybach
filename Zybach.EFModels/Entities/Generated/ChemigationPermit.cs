using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("ChemigationPermit")]
[Index("ChemigationPermitNumber", Name = "AK_ChemigationPermit_ChemigationPermitNumber", IsUnique = true)]
public partial class ChemigationPermit
{
    [Key]
    public int ChemigationPermitID { get; set; }

    public int ChemigationPermitNumber { get; set; }

    public int ChemigationPermitStatusID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateCreated { get; set; }

    public int CountyID { get; set; }

    public int? WellID { get; set; }

    [InverseProperty("ChemigationPermit")]
    public virtual ICollection<ChemigationPermitAnnualRecord> ChemigationPermitAnnualRecords { get; set; } = new List<ChemigationPermitAnnualRecord>();

    [ForeignKey("WellID")]
    [InverseProperty("ChemigationPermits")]
    public virtual Well Well { get; set; }
}
