using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("County")]
    [Index("CountyDisplayName", Name = "AK_County_CountyDisplayName", IsUnique = true)]
    [Index("CountyName", Name = "AK_County_CountyName", IsUnique = true)]
    public partial class County
    {
        public County()
        {
            ChemigationPermits = new HashSet<ChemigationPermit>();
            Wells = new HashSet<Well>();
        }

        [Key]
        public int CountyID { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string CountyName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string CountyDisplayName { get; set; }

        [InverseProperty("County")]
        public virtual ICollection<ChemigationPermit> ChemigationPermits { get; set; }
        [InverseProperty("County")]
        public virtual ICollection<Well> Wells { get; set; }
    }
}
