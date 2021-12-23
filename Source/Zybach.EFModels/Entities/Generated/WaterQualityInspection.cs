using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("WaterQualityInspection")]
    public partial class WaterQualityInspection
    {
        [Key]
        public int WaterQualityInspectionID { get; set; }
        public int WellID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime InspectionDate { get; set; }
        public int InspectorUserID { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? Temperature { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? PH { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? Conductivity { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? FieldAlkilinity { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? FieldNitrates { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? LabNitrates { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? Salinity { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? MV { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? Sodium { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? Calcium { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? Magnesium { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? Potassium { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? HydrogenCarbonate { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? CalciumCarbonate { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? Sulfate { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? Chloride { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? SiliconDioxide { get; set; }
        public int? CropTypeID { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? PreWaterLevel { get; set; }
        [Column(TypeName = "decimal(12, 4)")]
        public decimal? PostWaterLevel { get; set; }

        [ForeignKey(nameof(CropTypeID))]
        [InverseProperty("WaterQualityInspections")]
        public virtual CropType CropType { get; set; }
        [ForeignKey(nameof(InspectorUserID))]
        [InverseProperty(nameof(User.WaterQualityInspections))]
        public virtual User InspectorUser { get; set; }
        [ForeignKey(nameof(WellID))]
        [InverseProperty("WaterQualityInspections")]
        public virtual Well Well { get; set; }
    }
}
