using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("WellSensorMeasurement")]
    [Index("WellRegistrationID", "MeasurementTypeID", "SensorName", "ReadingYear", "ReadingMonth", "ReadingDay", Name = "AK_WellSensorMeasurement_WellRegistrationID_MeasurementTypeID_SensorName_ReadingDate", IsUnique = true)]
    public partial class WellSensorMeasurement
    {
        [Key]
        public int WellSensorMeasurementID { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string WellRegistrationID { get; set; }
        public int MeasurementTypeID { get; set; }
        public int ReadingYear { get; set; }
        public int ReadingMonth { get; set; }
        public int ReadingDay { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string SensorName { get; set; }
        public double MeasurementValue { get; set; }
        public bool? IsAnomalous { get; set; }
    }
}
