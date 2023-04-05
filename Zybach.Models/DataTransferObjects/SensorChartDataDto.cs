using System;
using System.Collections.Generic;
using System.Linq;

namespace Zybach.Models.DataTransferObjects;

public class SensorChartDataDto
{
    public List<SensorMeasurementDto> SensorMeasurements { get; set; }
    public DateTime? FirstReadingDate { get; set; }
    public DateTime? LastReadingDate { get; set; }
    public string ChartSpec { get; set; }

    public SensorChartDataDto(List<SensorMeasurementDto> sensorMeasurements, string chartSpec)
    {
        SensorMeasurements = sensorMeasurements;
        FirstReadingDate = sensorMeasurements.Any() ? sensorMeasurements.Min(x => x.MeasurementDate) : null;
        LastReadingDate = sensorMeasurements.Any() ? sensorMeasurements.Max(x => x.MeasurementDate) : null;
        ChartSpec = chartSpec;
    }
}