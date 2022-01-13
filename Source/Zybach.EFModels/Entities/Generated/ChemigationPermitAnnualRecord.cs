using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationPermitAnnualRecord")]
    [Index(nameof(ChemigationPermitID), nameof(RecordYear), Name = "AK_ChemigationPermitAnnualRecord_ChemigationPermitID_RecordYear", IsUnique = true)]
    public partial class ChemigationPermitAnnualRecord
    {
        public ChemigationPermitAnnualRecord()
        {
            ChemigationInspections = new HashSet<ChemigationInspection>();
            ChemigationPermitAnnualRecordApplicators = new HashSet<ChemigationPermitAnnualRecordApplicator>();
            ChemigationPermitAnnualRecordChemicalFormulations = new HashSet<ChemigationPermitAnnualRecordChemicalFormulation>();
        }

        [Key]
        public int ChemigationPermitAnnualRecordID { get; set; }
        public int ChemigationPermitID { get; set; }
        public int RecordYear { get; set; }
        public int ChemigationPermitAnnualRecordStatusID { get; set; }
        [StringLength(100)]
        public string PivotName { get; set; }
        public int ChemigationInjectionUnitTypeID { get; set; }
        [StringLength(200)]
        public string ApplicantFirstName { get; set; }
        [StringLength(200)]
        public string ApplicantLastName { get; set; }
        [StringLength(100)]
        public string ApplicantMailingAddress { get; set; }
        [StringLength(50)]
        public string ApplicantCity { get; set; }
        [StringLength(20)]
        public string ApplicantState { get; set; }
        [StringLength(10)]
        public string ApplicantZipCode { get; set; }
        [StringLength(30)]
        public string ApplicantPhone { get; set; }
        [StringLength(30)]
        public string ApplicantMobilePhone { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateReceived { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DatePaid { get; set; }
        [StringLength(255)]
        public string ApplicantEmail { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        public decimal? NDEEAmount { get; set; }
        [Required]
        [StringLength(100)]
        public string TownshipRangeSection { get; set; }
        [StringLength(200)]
        public string ApplicantCompany { get; set; }
        [StringLength(500)]
        public string AnnualNotes { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateApproved { get; set; }
        public int? ChemigationPermitAnnualRecordFeeTypeID { get; set; }

        [ForeignKey(nameof(ChemigationInjectionUnitTypeID))]
        [InverseProperty("ChemigationPermitAnnualRecords")]
        public virtual ChemigationInjectionUnitType ChemigationInjectionUnitType { get; set; }
        [ForeignKey(nameof(ChemigationPermitID))]
        [InverseProperty("ChemigationPermitAnnualRecords")]
        public virtual ChemigationPermit ChemigationPermit { get; set; }
        [ForeignKey(nameof(ChemigationPermitAnnualRecordFeeTypeID))]
        [InverseProperty("ChemigationPermitAnnualRecords")]
        public virtual ChemigationPermitAnnualRecordFeeType ChemigationPermitAnnualRecordFeeType { get; set; }
        [ForeignKey(nameof(ChemigationPermitAnnualRecordStatusID))]
        [InverseProperty("ChemigationPermitAnnualRecords")]
        public virtual ChemigationPermitAnnualRecordStatus ChemigationPermitAnnualRecordStatus { get; set; }
        [InverseProperty(nameof(ChemigationInspection.ChemigationPermitAnnualRecord))]
        public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; }
        [InverseProperty(nameof(ChemigationPermitAnnualRecordApplicator.ChemigationPermitAnnualRecord))]
        public virtual ICollection<ChemigationPermitAnnualRecordApplicator> ChemigationPermitAnnualRecordApplicators { get; set; }
        [InverseProperty(nameof(ChemigationPermitAnnualRecordChemicalFormulation.ChemigationPermitAnnualRecord))]
        public virtual ICollection<ChemigationPermitAnnualRecordChemicalFormulation> ChemigationPermitAnnualRecordChemicalFormulations { get; set; }
    }
}
