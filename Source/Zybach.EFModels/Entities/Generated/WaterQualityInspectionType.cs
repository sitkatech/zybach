using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("WaterQualityInspectionType")]
    [Index(nameof(WaterQualityInspectionTypeDisplayName), Name = "AK_WaterQualityInspectionType_WaterQualityInspectionTypeDisplayName", IsUnique = true)]
    [Index(nameof(WaterQualityInspectionTypeName), Name = "AK_WaterQualityInspectionType_WaterQualityInspectionTypeName", IsUnique = true)]
    public partial class WaterQualityInspectionType
    {
        public WaterQualityInspectionType()
        {
            WaterQualityInspections = new HashSet<WaterQualityInspection>();
            WellWaterQualityInspectionTypes = new HashSet<WellWaterQualityInspectionType>();
        }

        [Key]
        public int WaterQualityInspectionTypeID { get; set; }
        [Required]
        [StringLength(50)]
        public string WaterQualityInspectionTypeName { get; set; }
        [Required]
        [StringLength(50)]
        public string WaterQualityInspectionTypeDisplayName { get; set; }

        [InverseProperty(nameof(WaterQualityInspection.WaterQualityInspectionType))]
        public virtual ICollection<WaterQualityInspection> WaterQualityInspections { get; set; }
        [InverseProperty(nameof(WellWaterQualityInspectionType.WaterQualityInspectionType))]
        public virtual ICollection<WellWaterQualityInspectionType> WellWaterQualityInspectionTypes { get; set; }
    }
}
