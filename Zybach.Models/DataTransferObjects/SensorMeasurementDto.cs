using System;

namespace Zybach.Models.DataTransferObjects;

public class SensorMeasurementDto
{
    public string DataSourceName { get; set; }
    public DateTime MeasurementDate { get; set; }
    public string MeasurementValueString { get; set; }
    public double? MeasurementValue { get; set; }
    public bool IsAnomalous { get; set; }

    public SensorMeasurementDto()
    {
    }

    public SensorMeasurementDto(string dataSourceName, DateTime measurementDate,
        double? measurementValue, string measurementValueString, bool isAnomalous)
    {
        DataSourceName = dataSourceName;
        MeasurementValue = measurementValue;
        MeasurementDate = measurementDate;
        MeasurementValueString = measurementValueString;
        IsAnomalous = isAnomalous;
    }
}