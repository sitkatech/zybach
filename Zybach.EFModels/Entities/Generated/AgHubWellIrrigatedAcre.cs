using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("AgHubWellIrrigatedAcre")]
[Index("AgHubWellID", "IrrigationYear", Name = "AK_AgHubWellIrrigatedAcre_AgHubWellID_IrrigationYear", IsUnique = true)]
public partial class AgHubWellIrrigatedAcre
{
    [Key]
    public int AgHubWellIrrigatedAcreID { get; set; }

    public int AgHubWellID { get; set; }

    public int IrrigationYear { get; set; }

    public double Acres { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string CropType { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Tillage { get; set; }

    [ForeignKey("AgHubWellID")]
    [InverseProperty("AgHubWellIrrigatedAcres")]
    public virtual AgHubWell AgHubWell { get; set; }
}
