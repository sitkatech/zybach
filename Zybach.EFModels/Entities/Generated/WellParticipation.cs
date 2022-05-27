using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("WellParticipation")]
    [Index(nameof(WellParticipationDisplayName), Name = "AK_WellParticipation_WellParticipationDisplayName", IsUnique = true)]
    [Index(nameof(WellParticipationName), Name = "AK_WellParticipation_WellParticipationName", IsUnique = true)]
    public partial class WellParticipation
    {
        public WellParticipation()
        {
            Wells = new HashSet<Well>();
        }

        [Key]
        public int WellParticipationID { get; set; }
        [Required]
        [StringLength(50)]
        public string WellParticipationName { get; set; }
        [Required]
        [StringLength(50)]
        public string WellParticipationDisplayName { get; set; }

        [InverseProperty(nameof(Well.WellParticipation))]
        public virtual ICollection<Well> Wells { get; set; }
    }
}
