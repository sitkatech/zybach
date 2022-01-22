using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationPermitAnnualRecordFeeType")]
    [Index(nameof(ChemigationPermitAnnualRecordFeeTypeDisplayName), Name = "AK_ChemigationPermitAnnualRecordFeeType_ChemigationPermitAnnualRecordFeeTypeDisplayName", IsUnique = true)]
    [Index(nameof(ChemigationPermitAnnualRecordFeeTypeName), Name = "AK_ChemigationPermitAnnualRecordFeeType_ChemigationPermitAnnualRecordFeeTypeName", IsUnique = true)]
    public partial class ChemigationPermitAnnualRecordFeeType
    {
        public ChemigationPermitAnnualRecordFeeType()
        {
            ChemigationPermitAnnualRecords = new HashSet<ChemigationPermitAnnualRecord>();
        }

        [Key]
        public int ChemigationPermitAnnualRecordFeeTypeID { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationPermitAnnualRecordFeeTypeName { get; set; }
        [Required]
        [StringLength(50)]
        public string ChemigationPermitAnnualRecordFeeTypeDisplayName { get; set; }
        [Column(TypeName = "money")]
        public decimal FeeAmount { get; set; }

        [InverseProperty(nameof(ChemigationPermitAnnualRecord.ChemigationPermitAnnualRecordFeeType))]
        public virtual ICollection<ChemigationPermitAnnualRecord> ChemigationPermitAnnualRecords { get; set; }
    }
}
