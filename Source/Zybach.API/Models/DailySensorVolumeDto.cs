using System;

namespace Zybach.API.Models
{
    public class DailySensorVolumeDto
    {
        public DailySensorVolumeDto()
        {
        }

        public DailySensorVolumeDto(double measurementValue, string sensorName, int pumpingRateGallonsPerMinute)
        {
            SensorName = sensorName;
            MeasurementValueGallons = Convert.ToInt32(Math.Round(measurementValue, 0));
            PumpingRateGallonsPerMinute = pumpingRateGallonsPerMinute;
        }

        public string SensorName { get; set; }
        public int MeasurementValueGallons { get; set; }
        public int PumpingRateGallonsPerMinute { get; set; }
    }
}