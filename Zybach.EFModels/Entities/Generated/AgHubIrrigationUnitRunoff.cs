using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("AgHubIrrigationUnitRunoff")]
[Index("AgHubIrrigationUnitID", "Year", "Month", "Day", Name = "AK_AgHubIrrigationUnitRunoff_Year_Month_Day", IsUnique = true)]
public partial class AgHubIrrigationUnitRunoff
{
    [Key]
    public int AgHubIrrigationUnitRunoffID { get; set; }

    public int AgHubIrrigationUnitID { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public int Day { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string CropType { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Tillage { get; set; }

    public double CurveNumber { get; set; }

    public double Precipitation { get; set; }

    public double Area { get; set; }

    public double RunoffDepth { get; set; }

    public double RunoffVolume { get; set; }

    [ForeignKey("AgHubIrrigationUnitID")]
    [InverseProperty("AgHubIrrigationUnitRunoffs")]
    public virtual AgHubIrrigationUnit AgHubIrrigationUnit { get; set; }
}
