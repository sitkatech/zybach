using System;

namespace Zybach.Models.DataTransferObjects
{
    public partial class WellSensorMeasurementDto
    {
        public DateTime ReadingDate => new DateTime(ReadingYear, ReadingMonth, ReadingDay);
    }
}