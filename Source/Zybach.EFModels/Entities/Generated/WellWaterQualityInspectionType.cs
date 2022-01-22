using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("WellWaterQualityInspectionType")]
    public partial class WellWaterQualityInspectionType
    {
        [Key]
        public int WellWaterQualityInspectionTypeID { get; set; }
        public int WellID { get; set; }
        public int WaterQualityInspectionTypeID { get; set; }

        [ForeignKey(nameof(WaterQualityInspectionTypeID))]
        [InverseProperty("WellWaterQualityInspectionTypes")]
        public virtual WaterQualityInspectionType WaterQualityInspectionType { get; set; }
        [ForeignKey(nameof(WellID))]
        [InverseProperty("WellWaterQualityInspectionTypes")]
        public virtual Well Well { get; set; }
    }
}
