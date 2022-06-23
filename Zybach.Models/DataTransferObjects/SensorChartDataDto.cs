using System;
using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects;

public class SensorChartDataDto
{
    public List<SensorMeasurementDto> SensorMeasurements { get; set; }
    public DateTime? FirstReadingDate { get; set; }
    public DateTime? LastReadingDate { get; set; }
    public string ChartSpec { get; set; }
}