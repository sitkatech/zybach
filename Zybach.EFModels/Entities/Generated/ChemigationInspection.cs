using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

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
        public int? ChemigationInterlockTypeID { get; set; }
        public int? TillageID { get; set; }
        public int? CropTypeID { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string InspectionNotes { get; set; }
        public int? ChemigationInspectionFailureReasonID { get; set; }

        [ForeignKey("ChemigationInjectionValveID")]
        [InverseProperty("ChemigationInspections")]
        public virtual ChemigationInjectionValve ChemigationInjectionValve { get; set; }
        [ForeignKey("ChemigationInspectionFailureReasonID")]
        [InverseProperty("ChemigationInspections")]
        public virtual ChemigationInspectionFailureReason ChemigationInspectionFailureReason { get; set; }
        [ForeignKey("ChemigationMainlineCheckValveID")]
        [InverseProperty("ChemigationInspections")]
        public virtual ChemigationMainlineCheckValve ChemigationMainlineCheckValve { get; set; }
        [ForeignKey("ChemigationPermitAnnualRecordID")]
        [InverseProperty("ChemigationInspections")]
        public virtual ChemigationPermitAnnualRecord ChemigationPermitAnnualRecord { get; set; }
        [ForeignKey("CropTypeID")]
        [InverseProperty("ChemigationInspections")]
        public virtual CropType CropType { get; set; }
        [ForeignKey("InspectorUserID")]
        [InverseProperty("ChemigationInspections")]
        public virtual User InspectorUser { get; set; }
        [ForeignKey("TillageID")]
        [InverseProperty("ChemigationInspections")]
        public virtual Tillage Tillage { get; set; }
    }
}
