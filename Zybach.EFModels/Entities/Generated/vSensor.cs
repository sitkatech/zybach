using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Keyless]
public partial class vSensor
{
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

    [StringLength(100)]
    [Unicode(false)]
    public string WellRegistrationID { get; set; }

    public int? PageNumber { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string OwnerName { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string TownshipRangeSection { get; set; }

    public int? MeasurementTypeID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FirstReadingDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastReadingDate { get; set; }

    public double? LatestMeasurementValue { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastVoltageReadingDate { get; set; }

    public double? LastVoltageReading { get; set; }

    public int? LastMessageAgeInHours { get; set; }

    public int? MostRecentSupportTicketID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string MostRecentSupportTicketTitle { get; set; }

    public string SensorTypeName { get; set; }
}
