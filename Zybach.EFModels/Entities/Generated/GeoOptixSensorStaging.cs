using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("GeoOptixSensorStaging")]
    public partial class GeoOptixSensorStaging
    {
        [Key]
        public int GeoOptixSensorStagingID { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string WellRegistrationID { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string SensorName { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string SensorType { get; set; }
    }
}
