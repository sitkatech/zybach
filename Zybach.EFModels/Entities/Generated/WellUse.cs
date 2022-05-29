using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("WellUse")]
    [Index("WellUseDisplayName", Name = "AK_WellUse_WellUseDisplayName", IsUnique = true)]
    [Index("WellUseName", Name = "AK_WellUse_WellUseName", IsUnique = true)]
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
        [Unicode(false)]
        public string WellUseName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string WellUseDisplayName { get; set; }

        [InverseProperty("WellUse")]
        public virtual ICollection<Well> Wells { get; set; }
    }
}
