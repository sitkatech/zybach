using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("MeasurementType")]
    [Index(nameof(MeasurementTypeDisplayName), Name = "AK_MeasurementType_MeasurementTypeDisplayName", IsUnique = true)]
    [Index(nameof(MeasurementTypeName), Name = "AK_MeasurementType_MeasurementTypeName", IsUnique = true)]
    public partial class MeasurementType
    {
        public MeasurementType()
        {
            WellSensorMeasurementStagings = new HashSet<WellSensorMeasurementStaging>();
            WellSensorMeasurements = new HashSet<WellSensorMeasurement>();
        }

        [Key]
        public int MeasurementTypeID { get; set; }
        [Required]
        [StringLength(100)]
        public string MeasurementTypeName { get; set; }
        [Required]
        [StringLength(100)]
        public string MeasurementTypeDisplayName { get; set; }

        [InverseProperty(nameof(WellSensorMeasurementStaging.MeasurementType))]
        public virtual ICollection<WellSensorMeasurementStaging> WellSensorMeasurementStagings { get; set; }
        [InverseProperty(nameof(WellSensorMeasurement.MeasurementType))]
        public virtual ICollection<WellSensorMeasurement> WellSensorMeasurements { get; set; }
    }
}
