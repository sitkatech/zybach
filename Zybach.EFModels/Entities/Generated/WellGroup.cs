using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("WellGroup")]
[Index("WellGroupName", Name = "AK_WellGroup_WellGroupName", IsUnique = true)]
public partial class WellGroup
{
    [Key]
    public int WellGroupID { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string WellGroupName { get; set; }

    [InverseProperty("WellGroup")]
    public virtual ICollection<WellGroupWell> WellGroupWells { get; set; } = new List<WellGroupWell>();
}
