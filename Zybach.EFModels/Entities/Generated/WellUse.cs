using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("WellUse")]
    [Index(nameof(WellUseDisplayName), Name = "AK_WellUse_WellUseDisplayName", IsUnique = true)]
    [Index(nameof(WellUseName), Name = "AK_WellUse_WellUseName", IsUnique = true)]
    public partial class WellUse
    {
        public WellUse()
        {
            Wells = new HashSet<Well>();
        }

        [Key]
        public int WellUseID { get; set; }
        [Required]
        [StringLength(50)]
        public string WellUseName { get; set; }
        [Required]
        [StringLength(50)]
        public string WellUseDisplayName { get; set; }

        [InverseProperty(nameof(Well.WellUse))]
        public virtual ICollection<Well> Wells { get; set; }
    }
}
