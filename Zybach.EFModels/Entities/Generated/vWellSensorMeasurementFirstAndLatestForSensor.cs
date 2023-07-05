using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Keyless]
    public partial class vWellSensorMeasurementFirstAndLatestForSensor
    {
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string SensorName { get; set; }
        public int MeasurementTypeID { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string WellRegistrationID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FirstReadingDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastReadingDate { get; set; }
        public double LatestMeasurementValue { get; set; }
    }
}
