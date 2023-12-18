using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("Tillage")]
[Index("TillageDisplayName", Name = "AK_Tillage_TillageDisplayName", IsUnique = true)]
[Index("TillageName", Name = "AK_Tillage_TillageName", IsUnique = true)]
public partial class Tillage
{
    [Key]
    public int TillageID { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string TillageName { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string TillageDisplayName { get; set; }

    [InverseProperty("Tillage")]
    public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; } = new List<ChemigationInspection>();
}
