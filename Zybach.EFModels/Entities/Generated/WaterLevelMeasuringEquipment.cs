using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("WaterLevelMeasuringEquipment")]
    [Index(nameof(WaterLevelMeasuringEquipmentDisplayName), Name = "AK_WaterLevelMeasuringEquipment_WaterLevelMeasuringEquipmentDisplayName", IsUnique = true)]
    [Index(nameof(WaterLevelMeasuringEquipmentName), Name = "AK_WaterLevelMeasuringEquipment_WaterLevelMeasuringEquipmentName", IsUnique = true)]
    public partial class WaterLevelMeasuringEquipment
    {
        [Key]
        public int WaterLevelMeasuringEquipmentID { get; set; }
        [Required]
        [StringLength(50)]
        public string WaterLevelMeasuringEquipmentName { get; set; }
        [Required]
        [StringLength(50)]
        public string WaterLevelMeasuringEquipmentDisplayName { get; set; }
    }
}
