﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("Sensor")]
    [Index(nameof(SensorName), Name = "AK_Sensor_SensorName", IsUnique = true)]
    public partial class Sensor
    {
        [Key]
        public int SensorID { get; set; }
        [Required]
        [StringLength(100)]
        public string SensorName { get; set; }
        public int? SensorTypeID { get; set; }
        public int? WellID { get; set; }
        public bool InGeoOptix { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastUpdateDate { get; set; }

        [ForeignKey(nameof(SensorTypeID))]
        [InverseProperty("Sensors")]
        public virtual SensorType SensorType { get; set; }
        [ForeignKey(nameof(WellID))]
        [InverseProperty("Sensors")]
        public virtual Well Well { get; set; }
    }
}