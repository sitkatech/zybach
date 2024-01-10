using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("ChemigationPermitAnnualRecord")]
[Index("ChemigationPermitID", "RecordYear", Name = "AK_ChemigationPermitAnnualRecord_ChemigationPermitID_RecordYear", IsUnique = true)]
public partial class ChemigationPermitAnnualRecord
{
    [Key]
    public int ChemigationPermitAnnualRecordID { get; set; }

    public int ChemigationPermitID { get; set; }

    public int RecordYear { get; set; }

    public int ChemigationPermitAnnualRecordStatusID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string PivotName { get; set; }

    public int ChemigationInjectionUnitTypeID { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string ApplicantFirstName { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string ApplicantLastName { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string ApplicantMailingAddress { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string ApplicantCity { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string ApplicantState { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string ApplicantZipCode { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string ApplicantPhone { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string ApplicantMobilePhone { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateReceived { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DatePaid { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string ApplicantEmail { get; set; }

    [Column(TypeName = "decimal(4, 2)")]
    public decimal? NDEEAmount { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string TownshipRangeSection { get; set; }

    [StringLength(200)]
    [Unicode(false)]
    public string ApplicantCompany { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string AnnualNotes { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DateApproved { get; set; }

    public int? ChemigationPermitAnnualRecordFeeTypeID { get; set; }

    [InverseProperty("ChemigationPermitAnnualRecord")]
    public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; } = new List<ChemigationInspection>();

    [ForeignKey("ChemigationPermitID")]
    [InverseProperty("ChemigationPermitAnnualRecords")]
    public virtual ChemigationPermit ChemigationPermit { get; set; }

    [InverseProperty("ChemigationPermitAnnualRecord")]
    public virtual ICollection<ChemigationPermitAnnualRecordApplicator> ChemigationPermitAnnualRecordApplicators { get; set; } = new List<ChemigationPermitAnnualRecordApplicator>();

    [InverseProperty("ChemigationPermitAnnualRecord")]
    public virtual ICollection<ChemigationPermitAnnualRecordChemicalFormulation> ChemigationPermitAnnualRecordChemicalFormulations { get; set; } = new List<ChemigationPermitAnnualRecordChemicalFormulation>();
}
