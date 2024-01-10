using System;
using System.Collections.Generic;
using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.Swagger.Models
{
    public class IntervalVolumeDto
    {
        public IntervalVolumeDto()
        {
        }

        public IntervalVolumeDto(string wellRegistrationID, DateTime measurementDate, List<WellSensorMeasurementDto> wellSensorMeasurementDtos, string measurementType)
        {
            WellRegistrationID = wellRegistrationID;
            MeasurementDate = measurementDate.ToString("yyyy-MM-dd");
            MeasurementType = measurementType;
            var totalMeasurementValue = wellSensorMeasurementDtos.Sum(x => x.MeasurementValue);
            TotalMeasurementValueGallons = Convert.ToInt32(Math.Round(totalMeasurementValue, 0));
        }

        public string WellRegistrationID { get; set; }
        public string MeasurementDate { get; set; }
        public string MeasurementType { get; set; }
        public int TotalMeasurementValueGallons { get; set; }
        public List<DailySensorVolumeDto> SensorVolumes { get; set; }
    }
}