using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Keyless]
public partial class vSensorLatestBatteryVoltage
{
    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string SensorName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastVoltageReadingDate { get; set; }

    public double LastVoltageReading { get; set; }
}
