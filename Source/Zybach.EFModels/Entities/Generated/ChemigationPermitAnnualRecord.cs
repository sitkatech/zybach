using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationPermitAnnualRecord")]
    public partial class ChemigationPermitAnnualRecord
    {
        [Key]
        public int ChemigationPermitAnnualRecordID { get; set; }
        public int ChemigationPermitID { get; set; }
        public int RecordYear { get; set; }
        public int ChemigationPermitAnnualRecordStatusID { get; set; }
        [Required]
        [StringLength(100)]
        public string PivotName { get; set; }
        public int ChemigationInjectionUnitTypeID { get; set; }
        [Required]
        [StringLength(100)]
        public string ApplicantFirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string ApplicantLastName { get; set; }
        [Required]
        [StringLength(100)]
        public string ApplicantMailingAddress { get; set; }
        [Required]
        [StringLength(50)]
        public string ApplicantCity { get; set; }
        [Required]
        [StringLength(10)]
        public string ApplicantState { get; set; }
        public int ApplicantZipCode { get; set; }
        [StringLength(30)]
        public string ApplicantPhone { get; set; }
        [StringLength(30)]
        public string ApplicantMobilePhone { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateReceived { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DatePaid { get; set; }

        [ForeignKey(nameof(ChemigationInjectionUnitTypeID))]
        [InverseProperty("ChemigationPermitAnnualRecords")]
        public virtual ChemigationInjectionUnitType ChemigationInjectionUnitType { get; set; }
        [ForeignKey(nameof(ChemigationPermitID))]
        [InverseProperty("ChemigationPermitAnnualRecords")]
        public virtual ChemigationPermit ChemigationPermit { get; set; }
        [ForeignKey(nameof(ChemigationPermitAnnualRecordStatusID))]
        [InverseProperty("ChemigationPermitAnnualRecords")]
        public virtual ChemigationPermitAnnualRecordStatus ChemigationPermitAnnualRecordStatus { get; set; }
    }
}
