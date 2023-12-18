using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("RobustReviewScenarioGETRunHistory")]
public partial class RobustReviewScenarioGETRunHistory
{
    [Key]
    public int RobustReviewScenarioGETRunHistoryID { get; set; }

    public int CreateByUserID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastUpdateDate { get; set; }

    public int? GETRunID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SuccessfulStartDate { get; set; }

    public bool IsTerminal { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string StatusMessage { get; set; }

    [StringLength(7)]
    [Unicode(false)]
    public string StatusHexColor { get; set; }

    [ForeignKey("CreateByUserID")]
    [InverseProperty("RobustReviewScenarioGETRunHistories")]
    public virtual User CreateByUser { get; set; }
}
