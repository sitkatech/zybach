using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("OpenETSync")]
[Index("Year", "Month", "OpenETDataTypeID", Name = "AK_OpenETSync_Year_Month_OpenETDataTypeID", IsUnique = true)]
public partial class OpenETSync
{
    [Key]
    public int OpenETSyncID { get; set; }

    public int OpenETDataTypeID { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FinalizeDate { get; set; }

    [InverseProperty("OpenETSync")]
    public virtual ICollection<OpenETSyncHistory> OpenETSyncHistories { get; set; } = new List<OpenETSyncHistory>();
}
