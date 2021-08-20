using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("WellSensorMeasurement")]
    public partial class WellSensorMeasurement
    {
        [Key]
        public int WellSensorMeasurementID { get; set; }
        [Required]
        [StringLength(100)]
        public string WellRegistrationID { get; set; }
        public int MeasurementTypeID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ReadingDate { get; set; }
        [StringLength(100)]
        public string SensorName { get; set; }
        public double MeasurementValue { get; set; }

        [ForeignKey(nameof(MeasurementTypeID))]
        [InverseProperty("WellSensorMeasurements")]
        public virtual MeasurementType MeasurementType { get; set; }
    }
}
