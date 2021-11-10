using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationPermitAnnualRecordStatus")]
    [Index(nameof(ChemigationPermitAnnualRecordStatusDisplayName), Name = "AK_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusDisplayName", IsUnique = true)]
    [Index(nameof(ChemigationPermitAnnualRecordStatusName), Name = "AK_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusName", IsUnique = true)]
    public partial class ChemigationPermitAnnualRecordStatus
    {
        public ChemigationPermitAnnualRecordStatus()
        {
            ChemigationPermitAnnualRecords = new HashSet<ChemigationPermitAnnualRecord>();
        }

        [Key]
        public int ChemigationPermitAnnualRecordStatusID { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationPermitAnnualRecordStatusName { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationPermitAnnualRecordStatusDisplayName { get; set; }

        [InverseProperty(nameof(ChemigationPermitAnnualRecord.ChemigationPermitAnnualRecordStatus))]
        public virtual ICollection<ChemigationPermitAnnualRecord> ChemigationPermitAnnualRecords { get; set; }
    }
}
