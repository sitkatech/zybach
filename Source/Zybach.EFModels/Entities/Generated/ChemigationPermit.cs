using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationPermit")]
    [Index(nameof(ChemigationPermitNumber), Name = "AK_ChemigationPermit_ChemigationPermitNumber", IsUnique = true)]
    public partial class ChemigationPermit
    {
        public ChemigationPermit()
        {
            ChemigationPermitAnnualRecords = new HashSet<ChemigationPermitAnnualRecord>();
        }

        [Key]
        public int ChemigationPermitID { get; set; }
        public int ChemigationPermitNumber { get; set; }
        public int ChemigationPermitStatusID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateReceived { get; set; }
        [Required]
        [StringLength(100)]
        public string TownshipRangeSection { get; set; }

        [ForeignKey(nameof(ChemigationPermitStatusID))]
        [InverseProperty("ChemigationPermits")]
        public virtual ChemigationPermitStatus ChemigationPermitStatus { get; set; }
        [InverseProperty(nameof(ChemigationPermitAnnualRecord.ChemigationPermit))]
        public virtual ICollection<ChemigationPermitAnnualRecord> ChemigationPermitAnnualRecords { get; set; }
    }
}
