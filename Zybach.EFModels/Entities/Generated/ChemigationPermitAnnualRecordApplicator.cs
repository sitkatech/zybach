using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationPermitAnnualRecordApplicator")]
    public partial class ChemigationPermitAnnualRecordApplicator
    {
        [Key]
        public int ChemigationPermitAnnualRecordApplicatorID { get; set; }
        public int ChemigationPermitAnnualRecordID { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string ApplicatorName { get; set; }
        public int? CertificationNumber { get; set; }
        public int? ExpirationYear { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string HomePhone { get; set; }
        [StringLength(30)]
        [Unicode(false)]
        public string MobilePhone { get; set; }

        [ForeignKey("ChemigationPermitAnnualRecordID")]
        [InverseProperty("ChemigationPermitAnnualRecordApplicators")]
        public virtual ChemigationPermitAnnualRecord ChemigationPermitAnnualRecord { get; set; }
    }
}
