using System.Collections.Generic;
using System.Linq;

namespace Zybach.EFModels.Entities;

public partial class Sensor
{
    public List<string> GetChartDomains()
    {
        var values = new List<string> { $"\"{GetChartDataSourceName()}\"" };
        if (SensorAnomalies != null && SensorAnomalies.Any())
        {
            values.Add($"\"{GetChartAnomaliesDataSourceName()}\"");
        }

        return values;
    }

    public List<string> GetChartColors()
    {
        var values = new List<string> { $"\"{SensorType.ChartColor}\"" };
        if (SensorAnomalies != null && SensorAnomalies.Any())
        {
            values.Add($"\"{SensorType.AnomalousChartColor}\"");
        }

        return values;
    }

    public List<string> GetChartTooltipFields()
    {
        var values = new List<string> { $"{{\"field\": \"{GetChartDataSourceName()}\", \"type\": \"ordinal\" }}" };
        if (SensorAnomalies != null && SensorAnomalies.Any())
        {
            values.Add($"{{\"field\": \"{GetChartAnomaliesDataSourceName()}\", \"type\": \"ordinal\" }}");
        }

        return values;
    }

    public string GetChartDataSourceName()
    {
        return $"{SensorName} - {SensorType.SensorTypeDisplayName}";
    }

    public string GetChartAnomaliesDataSourceName()
    {
        return $"{SensorName} - {SensorType.SensorTypeDisplayName} Anomalies";
    }
}