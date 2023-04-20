using System;

namespace Zybach.Models.DataTransferObjects;

public class SensorPulseDto
{
    public string SensorName { get; set; }
    public string EventMessage { get; set; }
    public DateTime ReceivedDate { get; set; }
}