using System;

namespace Zybach.Models.DataTransferObjects
{
    public partial class WellSensorMeasurementDto
    {
        public DateTime MeasurementDate => new DateTimeOffset(ReadingYear, ReadingMonth, ReadingDay, 0, 0, 0, new TimeSpan(-7, 0, 0)).UtcDateTime;
    }
}