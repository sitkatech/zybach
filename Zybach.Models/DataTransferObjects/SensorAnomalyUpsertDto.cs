using System;
using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects;

public class SensorAnomalyUpsertDto
{
    public int SensorAnomalyID { get; set; }
    [Required]
    public int SensorID { get; set; }
    [Required]
    public DateTime? StartDate { get; set; }
    [Required]
    public DateTime? EndDate { get; set; }
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
    public string Notes { get; set; }
}