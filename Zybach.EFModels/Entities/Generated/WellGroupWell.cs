using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("WellGroupWell")]
public partial class WellGroupWell
{
    [Key]
    public int WellGroupWellID { get; set; }

    public int WellGroupID { get; set; }

    public int WellID { get; set; }

    public bool IsPrimary { get; set; }

    [ForeignKey("WellID")]
    [InverseProperty("WellGroupWells")]
    public virtual Well Well { get; set; }

    [ForeignKey("WellGroupID")]
    [InverseProperty("WellGroupWells")]
    public virtual WellGroup WellGroup { get; set; }
}
