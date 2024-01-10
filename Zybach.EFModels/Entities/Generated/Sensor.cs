using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("Sensor")]
[Index("SensorName", Name = "AK_Sensor_SensorName", IsUnique = true)]
public partial class Sensor
{
    [Key]
    public int SensorID { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string SensorName { get; set; }

    public int SensorTypeID { get; set; }

    public int? WellID { get; set; }

    public bool InGeoOptix { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime LastUpdateDate { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? RetirementDate { get; set; }

    public int? ContinuityMeterStatusID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ContinuityMeterStatusLastUpdated { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? SnoozeStartDate { get; set; }

    [InverseProperty("Sensor")]
    public virtual ICollection<SensorAnomaly> SensorAnomalies { get; set; } = new List<SensorAnomaly>();

    [InverseProperty("Sensor")]
    public virtual ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();

    [ForeignKey("WellID")]
    [InverseProperty("Sensors")]
    public virtual Well Well { get; set; }
}
