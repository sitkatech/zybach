using System;

namespace Zybach.Swagger.Models
{
    public class DailySensorVolumeDto
    {
        public DailySensorVolumeDto()
        {
        }

        public DailySensorVolumeDto(double measurementValue, string sensorName, int pumpingRateGallonsPerMinute, bool isAnomalous)
        {
            SensorName = sensorName;
            MeasurementValueGallons = Convert.ToInt32(Math.Round(measurementValue, 0));
            PumpingRateGallonsPerMinute = pumpingRateGallonsPerMinute;
            IsAnomalous = isAnomalous;
        }

        public string SensorName { get; set; }
        public int MeasurementValueGallons { get; set; }
        public int PumpingRateGallonsPerMinute { get; set; }
        public bool IsAnomalous { get; set; }
    }
}