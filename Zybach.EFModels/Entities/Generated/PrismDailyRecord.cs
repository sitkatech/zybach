using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("PrismDailyRecord")]
[Index("Year", "Month", "Day", "PrismDataTypeID", Name = "AK_PrismDailyRecord_Year_Month_Day_PrismDataTypeID", IsUnique = true)]
public partial class PrismDailyRecord
{
    [Key]
    public int PrismDailyRecordID { get; set; }

    public int PrismMonthlySyncID { get; set; }

    public int PrismSyncStatusID { get; set; }

    public int PrismDataTypeID { get; set; }

    public int? BlobResourceID { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public int Day { get; set; }

    [Unicode(false)]
    public string ErrorMessage { get; set; }

    [ForeignKey("BlobResourceID")]
    [InverseProperty("PrismDailyRecords")]
    public virtual BlobResource BlobResource { get; set; }

    [ForeignKey("PrismMonthlySyncID")]
    [InverseProperty("PrismDailyRecords")]
    public virtual PrismMonthlySync PrismMonthlySync { get; set; }
}
