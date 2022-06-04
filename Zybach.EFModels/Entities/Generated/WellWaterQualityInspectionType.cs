using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("WellWaterQualityInspectionType")]
    public partial class WellWaterQualityInspectionType
    {
        [Key]
        public int WellWaterQualityInspectionTypeID { get; set; }
        public int WellID { get; set; }
        public int WaterQualityInspectionTypeID { get; set; }

        [ForeignKey("WellID")]
        [InverseProperty("WellWaterQualityInspectionTypes")]
        public virtual Well Well { get; set; }
    }
}
