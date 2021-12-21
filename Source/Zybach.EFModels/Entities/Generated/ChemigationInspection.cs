using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("ChemigationInspection")]
    public partial class ChemigationInspection
    {
        [Key]
        public int ChemigationInspectionID { get; set; }
        public int ChemigationPermitAnnualRecordID { get; set; }
        public int ChemigationInspectionStatusID { get; set; }
        public int? ChemigationInspectionTypeID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? InspectionDate { get; set; }
        public int? InspectorUserID { get; set; }
        public int? ChemigationMainlineCheckValveID { get; set; }
        public bool? HasVacuumReliefValve { get; set; }
        public bool? HasInspectionPort { get; set; }
        public int? ChemigationLowPressureValveID { get; set; }
        public int? ChemigationInjectionValveID { get; set; }
        public int? TillageID { get; set; }
        public int? CropTypeID { get; set; }
        [StringLength(500)]
        public string InspectionNotes { get; set; }

        [ForeignKey(nameof(ChemigationInjectionValveID))]
        [InverseProperty("ChemigationInspections")]
        public virtual ChemigationInjectionValve ChemigationInjectionValve { get; set; }
        [ForeignKey(nameof(ChemigationInspectionStatusID))]
        [InverseProperty("ChemigationInspections")]
        public virtual ChemigationInspectionStatus ChemigationInspectionStatus { get; set; }
        [ForeignKey(nameof(ChemigationInspectionTypeID))]
        [InverseProperty("ChemigationInspections")]
        public virtual ChemigationInspectionType ChemigationInspectionType { get; set; }
        [ForeignKey(nameof(ChemigationLowPressureValveID))]
        [InverseProperty("ChemigationInspections")]
        public virtual ChemigationLowPressureValve ChemigationLowPressureValve { get; set; }
        [ForeignKey(nameof(ChemigationMainlineCheckValveID))]
        [InverseProperty("ChemigationInspections")]
        public virtual ChemigationMainlineCheckValve ChemigationMainlineCheckValve { get; set; }
        [ForeignKey(nameof(ChemigationPermitAnnualRecordID))]
        [InverseProperty("ChemigationInspections")]
        public virtual ChemigationPermitAnnualRecord ChemigationPermitAnnualRecord { get; set; }
        [ForeignKey(nameof(CropTypeID))]
        [InverseProperty("ChemigationInspections")]
        public virtual CropType CropType { get; set; }
        [ForeignKey(nameof(InspectorUserID))]
        [InverseProperty(nameof(User.ChemigationInspections))]
        public virtual User InspectorUser { get; set; }
        [ForeignKey(nameof(TillageID))]
        [InverseProperty("ChemigationInspections")]
        public virtual Tillage Tillage { get; set; }
    }
}
