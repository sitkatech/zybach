using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("WellSensorMeasurementStaging")]
    public partial class WellSensorMeasurementStaging
    {
        [Key]
        public int WellSensorMeasurementStagingID { get; set; }
        [Required]
        [StringLength(100)]
        public string WellRegistrationID { get; set; }
        public int MeasurementTypeID { get; set; }
        public int ReadingYear { get; set; }
        public int ReadingMonth { get; set; }
        public int ReadingDay { get; set; }
        [StringLength(100)]
        public string SensorName { get; set; }
        public double MeasurementValue { get; set; }

        [ForeignKey(nameof(MeasurementTypeID))]
        [InverseProperty("WellSensorMeasurementStagings")]
        public virtual MeasurementType MeasurementType { get; set; }
    }
}
