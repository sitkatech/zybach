using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("ChemigationMainlineCheckValve")]
[Index("ChemigationMainlineCheckValveDisplayName", Name = "AK_ChemigationMainlineCheckValve_ChemigationMainlineCheckValveDisplayName", IsUnique = true)]
[Index("ChemigationMainlineCheckValveName", Name = "AK_ChemigationMainlineCheckValve_ChemigationMainlineCheckValveName", IsUnique = true)]
public partial class ChemigationMainlineCheckValve
{
    [Key]
    public int ChemigationMainlineCheckValveID { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string ChemigationMainlineCheckValveName { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string ChemigationMainlineCheckValveDisplayName { get; set; }

    [InverseProperty("ChemigationMainlineCheckValve")]
    public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; } = new List<ChemigationInspection>();
}
