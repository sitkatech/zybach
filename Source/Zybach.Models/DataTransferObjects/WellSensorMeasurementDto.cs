using System;

namespace Zybach.Models.DataTransferObjects
{
    public partial class WellSensorMeasurementDto
    {
        public DateTime ReadingDate => new DateTimeOffset(ReadingYear, ReadingMonth, ReadingDay, 0, 0, 0, new TimeSpan(-7, 0, 0)).UtcDateTime;
    }
}