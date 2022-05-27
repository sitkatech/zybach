using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("WaterLevelInspection")]
    public partial class WaterLevelInspection
    {
        [Key]
        public int WaterLevelInspectionID { get; set; }
        public int WellID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime InspectionDate { get; set; }
        public int InspectorUserID { get; set; }
        [StringLength(100)]
        public string WaterLevelInspectionStatus { get; set; }
        [StringLength(100)]
        public string MeasuringEquipment { get; set; }
        [StringLength(100)]
        public string Crop { get; set; }
        public bool HasOil { get; set; }
        public bool HasBrokenTape { get; set; }
        [StringLength(100)]
        public string Accuracy { get; set; }
        [StringLength(100)]
        public string Method { get; set; }
        [StringLength(100)]
        public string Party { get; set; }
        [StringLength(100)]
        public string SourceAgency { get; set; }
        [StringLength(100)]
        public string SourceCode { get; set; }
        [StringLength(100)]
        public string TimeDatumCode { get; set; }
        [StringLength(100)]
        public string TimeDatumReliability { get; set; }
        [StringLength(100)]
        public string LevelTypeCode { get; set; }
        [StringLength(100)]
        public string AgencyCode { get; set; }
        [StringLength(100)]
        public string Access { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? Hold { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? Cut { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? MP { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? Measurement { get; set; }
        public bool? IsPrimary { get; set; }
        [Column(TypeName = "decimal(12, 2)")]
        public decimal? WaterLevel { get; set; }
        public int? CropTypeID { get; set; }
        [StringLength(500)]
        public string InspectionNotes { get; set; }
        [StringLength(100)]
        public string InspectionNickname { get; set; }

        [ForeignKey(nameof(CropTypeID))]
        [InverseProperty("WaterLevelInspections")]
        public virtual CropType CropType { get; set; }
        [ForeignKey(nameof(InspectorUserID))]
        [InverseProperty(nameof(User.WaterLevelInspections))]
        public virtual User InspectorUser { get; set; }
        [ForeignKey(nameof(WellID))]
        [InverseProperty("WaterLevelInspections")]
        public virtual Well Well { get; set; }
    }
}
