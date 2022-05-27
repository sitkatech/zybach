using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class WellSensorMeasurementExtensionMethods
    {
        static partial void DoCustomMappings(WellSensorMeasurement wellSensorMeasurement, WellSensorMeasurementDto wellSensorMeasurementDto)
        {
            wellSensorMeasurementDto.MeasurementDate = wellSensorMeasurement.MeasurementDate;
            wellSensorMeasurementDto.MeasurementDateInPacificTime = wellSensorMeasurement.MeasurementDateInPacificTime;
            wellSensorMeasurementDto.MeasurementValueString = wellSensorMeasurement.MeasurementValueString;
        }
    }
}