using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("WellParticipation")]
[Index("WellParticipationDisplayName", Name = "AK_WellParticipation_WellParticipationDisplayName", IsUnique = true)]
[Index("WellParticipationName", Name = "AK_WellParticipation_WellParticipationName", IsUnique = true)]
public partial class WellParticipation
{
    [Key]
    public int WellParticipationID { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string WellParticipationName { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string WellParticipationDisplayName { get; set; }

    [InverseProperty("WellParticipation")]
    public virtual ICollection<Well> Wells { get; set; } = new List<Well>();
}
