using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("PrismMonthlySync")]
[Index("Year", "Month", "PrismDataTypeID", Name = "AK_PrismMonthlySync_Year_Month_PrismDataTypeID", IsUnique = true)]
public partial class PrismMonthlySync
{
    [Key]
    public int PrismMonthlySyncID { get; set; }

    public int PrismSyncStatusID { get; set; }

    public int RunoffCalculationStatusID { get; set; }

    public int PrismDataTypeID { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastSynchronizedDate { get; set; }

    public int? LastSynchronizedByUserID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastRunoffCalculationDate { get; set; }

    public int? LastRunoffCalculatedByUserID { get; set; }

    [Unicode(false)]
    public string LastRunoffCalculationError { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FinalizeDate { get; set; }

    public int? FinalizeByUserID { get; set; }

    [ForeignKey("FinalizeByUserID")]
    [InverseProperty("PrismMonthlySyncFinalizeByUsers")]
    public virtual User FinalizeByUser { get; set; }

    [ForeignKey("LastRunoffCalculatedByUserID")]
    [InverseProperty("PrismMonthlySyncLastRunoffCalculatedByUsers")]
    public virtual User LastRunoffCalculatedByUser { get; set; }

    [ForeignKey("LastSynchronizedByUserID")]
    [InverseProperty("PrismMonthlySyncLastSynchronizedByUsers")]
    public virtual User LastSynchronizedByUser { get; set; }

    [InverseProperty("PrismMonthlySync")]
    public virtual ICollection<PrismDailyRecord> PrismDailyRecords { get; set; } = new List<PrismDailyRecord>();
}
