using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("GeoOptixSensorStaging")]
    public partial class GeoOptixSensorStaging
    {
        [Key]
        public int GeoOptixSensorStagingID { get; set; }
        [Required]
        [StringLength(100)]
        public string WellRegistrationID { get; set; }
        [Required]
        [StringLength(100)]
        public string SensorName { get; set; }
        [Required]
        [StringLength(100)]
        public string SensorType { get; set; }
    }
}
