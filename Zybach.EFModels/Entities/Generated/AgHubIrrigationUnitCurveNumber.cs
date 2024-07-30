using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("AgHubIrrigationUnitCurveNumber")]
public partial class AgHubIrrigationUnitCurveNumber
{
    [Key]
    public int AgHubIrrigationUnitCurveNumberID { get; set; }

    public int AgHubIrrigationUnitID { get; set; }

    [StringLength(3)]
    [Unicode(false)]
    public string HydrologicSoilGroup { get; set; }

    public double MTillCurveNumber { get; set; }

    public double STillCurveNumber { get; set; }

    public double NTillCurveNumber { get; set; }

    public double CTillCurveNumber { get; set; }

    public double UndefinedTillCurveNumber { get; set; }

    [ForeignKey("AgHubIrrigationUnitID")]
    [InverseProperty("AgHubIrrigationUnitCurveNumbers")]
    public virtual AgHubIrrigationUnit AgHubIrrigationUnit { get; set; }
}
