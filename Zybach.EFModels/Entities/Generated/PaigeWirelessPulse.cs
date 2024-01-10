using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("PaigeWirelessPulse")]
public partial class PaigeWirelessPulse
{
    [Key]
    public int PaigeWirelessPulseID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime ReceivedDate { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string SensorName { get; set; }

    [Required]
    [Unicode(false)]
    public string EventMessage { get; set; }
}
