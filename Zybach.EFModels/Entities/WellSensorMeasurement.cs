using System;

namespace Zybach.EFModels.Entities
{
    public partial class WellSensorMeasurement
    {
        public DateTime MeasurementDateInPacificTime =>
            new DateTimeOffset(ReadingYear, ReadingMonth, ReadingDay, 0, 0, 0, new TimeSpan(-7, 0, 0)).UtcDateTime;

        public DateTime MeasurementDate => new(ReadingYear, ReadingMonth, ReadingDay);

        public string MeasurementValueString => $"{MeasurementValue:N1} {(MeasurementType.MeasurementTypeID == (int) MeasurementTypeEnum.WellPressure ? "feet" : "gallons")}";
    }
}