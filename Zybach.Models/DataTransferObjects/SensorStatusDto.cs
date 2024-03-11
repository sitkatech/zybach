using System;

namespace Zybach.Models.DataTransferObjects;

public class SensorStatusDto
{
    public int SensorID { get; set; }
    public string SensorName { get; set; }
    public int SensorTypeID { get; set; }
    public bool IsActive { get; set; }
    public int? ContinuityMeterStatusID { get; set; }
    public DateTime? SnoozeStartDate { get; set; }
    public string SensorTypeName { get; set; }
    public int? LastMessageAgeInHours { get; set; }
    public double? LastVoltageReading { get; set; }
    public DateTime? LastVoltageReadingDate { get; set; }
    public DateTime? LastReadingDate { get; set; }
    public int? MostRecentSupportTicketID { get; set; }
    public string MostRecentSupportTicketTitle { get; set; }
}