using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationPermitStatus")]
    [Index(nameof(ChemigationPermitStatusDisplayName), Name = "AK_ChemigationPermitStatus_ChemigationPermitStatusDisplayName", IsUnique = true)]
    [Index(nameof(ChemigationPermitStatusName), Name = "AK_ChemigationPermitStatus_ChemigationPermitStatusName", IsUnique = true)]
    public partial class ChemigationPermitStatus
    {
        public ChemigationPermitStatus()
        {
            ChemigationPermits = new HashSet<ChemigationPermit>();
        }

        [Key]
        public int ChemigationPermitStatusID { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationPermitStatusName { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationPermitStatusDisplayName { get; set; }

        [InverseProperty(nameof(ChemigationPermit.ChemigationPermitStatus))]
        public virtual ICollection<ChemigationPermit> ChemigationPermits { get; set; }
    }
}
