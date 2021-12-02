using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationCounty")]
    [Index(nameof(ChemigationCountyDisplayName), Name = "AK_ChemigationCounty_ChemigationCountyDisplayName", IsUnique = true)]
    [Index(nameof(ChemigationCountyName), Name = "AK_ChemigationCounty_ChemigationCountyName", IsUnique = true)]
    public partial class ChemigationCounty
    {
        public ChemigationCounty()
        {
            ChemigationPermits = new HashSet<ChemigationPermit>();
        }

        [Key]
        public int ChemigationCountyID { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationCountyName { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationCountyDisplayName { get; set; }

        [InverseProperty(nameof(ChemigationPermit.ChemigationCounty))]
        public virtual ICollection<ChemigationPermit> ChemigationPermits { get; set; }
    }
}
