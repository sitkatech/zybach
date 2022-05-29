using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("WaterLevelMeasuringEquipment")]
    [Index("WaterLevelMeasuringEquipmentDisplayName", Name = "AK_WaterLevelMeasuringEquipment_WaterLevelMeasuringEquipmentDisplayName", IsUnique = true)]
    [Index("WaterLevelMeasuringEquipmentName", Name = "AK_WaterLevelMeasuringEquipment_WaterLevelMeasuringEquipmentName", IsUnique = true)]
    public partial class WaterLevelMeasuringEquipment
    {
        [Key]
        public int WaterLevelMeasuringEquipmentID { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string WaterLevelMeasuringEquipmentName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string WaterLevelMeasuringEquipmentDisplayName { get; set; }
    }
}
