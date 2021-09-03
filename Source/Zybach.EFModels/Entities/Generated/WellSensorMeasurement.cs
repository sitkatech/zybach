using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("WellSensorMeasurement")]
    [Index(nameof(WellRegistrationID), nameof(MeasurementTypeID), nameof(SensorName), nameof(ReadingYear), nameof(ReadingMonth), nameof(ReadingDay), Name = "AK_WellSensorMeasurement_WellRegistrationID_MeasurementTypeID_SensorName_ReadingDate", IsUnique = true)]
    public partial class WellSensorMeasurement
    {
        [Key]
        public int WellSensorMeasurementID { get; set; }
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
        [InverseProperty("WellSensorMeasurements")]
        public virtual MeasurementType MeasurementType { get; set; }
    }
}
