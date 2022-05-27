using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("SensorType")]
    [Index(nameof(SensorTypeDisplayName), Name = "AK_SensorType_SensorTypeDisplayName", IsUnique = true)]
    [Index(nameof(SensorTypeName), Name = "AK_SensorType_SensorTypeName", IsUnique = true)]
    public partial class SensorType
    {
        public SensorType()
        {
            Sensors = new HashSet<Sensor>();
        }

        [Key]
        public int SensorTypeID { get; set; }
        [Required]
        [StringLength(100)]
        public string SensorTypeName { get; set; }
        [Required]
        [StringLength(100)]
        public string SensorTypeDisplayName { get; set; }

        [InverseProperty(nameof(Sensor.SensorType))]
        public virtual ICollection<Sensor> Sensors { get; set; }
    }
}
