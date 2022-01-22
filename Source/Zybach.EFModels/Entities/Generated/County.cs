using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("County")]
    [Index(nameof(CountyDisplayName), Name = "AK_County_CountyDisplayName", IsUnique = true)]
    [Index(nameof(CountyName), Name = "AK_County_CountyName", IsUnique = true)]
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
        public string CountyName { get; set; }
        [Required]
        [StringLength(50)]
        public string CountyDisplayName { get; set; }

        [InverseProperty(nameof(ChemigationPermit.County))]
        public virtual ICollection<ChemigationPermit> ChemigationPermits { get; set; }
        [InverseProperty(nameof(Well.County))]
        public virtual ICollection<Well> Wells { get; set; }
    }
}
