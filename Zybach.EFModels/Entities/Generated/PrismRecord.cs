using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("PrismRecord")]
[Index("Date", Name = "IX_PrismRecord_Date")]
public partial class PrismRecord
{
    [Key]
    public int PrismRecordID { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ElementType { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Date { get; set; }

    public int? X { get; set; }

    public int? Y { get; set; }

    public double? Value { get; set; }
}
