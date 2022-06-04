using System;

namespace Zybach.Models.DataTransferObjects
{
    public class WellSensorReadingDateDto
    {
        public string WellRegistrationID { get; set; }
        public string SensorName { get; set; }
        public DateTime FirstReadingDate { get; set; }

        public WellSensorReadingDateDto()
        {
        }

        public WellSensorReadingDateDto(string wellRegistrationID, string sensorName, DateTime firstReadingDate)
        {
            WellRegistrationID = wellRegistrationID;
            SensorName = sensorName;
            FirstReadingDate = new DateTime(firstReadingDate.Year, firstReadingDate.Month, firstReadingDate.Day); // we need to strip the time component from this
        }
    }
}