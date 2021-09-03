using System;

namespace Zybach.API.Models
{
    public class IntervalVolumeDto
    {
        public IntervalVolumeDto()
        {
        }

        public IntervalVolumeDto(string wellRegistrationID, DateTime measurementDate, double measurementValue, string measurementType, string sensorName, int pumpingRateGallonsPerMinute)
        {
            WellRegistrationID = wellRegistrationID;
            MeasurementDate = measurementDate.ToString("yyyy-MM-dd");
            MeasurementType = measurementType;
            SensorName = sensorName;
            MeasurementValueGallons = Convert.ToInt32(Math.Round(measurementValue, 0));
            PumpingRateGallonsPerMinute = pumpingRateGallonsPerMinute;
        }

        public string WellRegistrationID { get; set; }
        public string MeasurementDate { get; set; }
        public string MeasurementType { get; set; }
        public string SensorName { get; set; }
        public int MeasurementValueGallons { get; set; }
        public int PumpingRateGallonsPerMinute { get; set; }
    }
}