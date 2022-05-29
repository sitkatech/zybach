using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationPermitStatus")]
    [Index("ChemigationPermitStatusDisplayName", Name = "AK_ChemigationPermitStatus_ChemigationPermitStatusDisplayName", IsUnique = true)]
    [Index("ChemigationPermitStatusName", Name = "AK_ChemigationPermitStatus_ChemigationPermitStatusName", IsUnique = true)]
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
        [Unicode(false)]
        public string ChemigationPermitStatusName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string ChemigationPermitStatusDisplayName { get; set; }

        [InverseProperty("ChemigationPermitStatus")]
        public virtual ICollection<ChemigationPermit> ChemigationPermits { get; set; }
    }
}
