//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellSensorMeasurement]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WellSensorMeasurementExtensionMethods
    {
        public static WellSensorMeasurementDto AsDto(this WellSensorMeasurement wellSensorMeasurement)
        {
            var wellSensorMeasurementDto = new WellSensorMeasurementDto()
            {
                WellSensorMeasurementID = wellSensorMeasurement.WellSensorMeasurementID,
                WellRegistrationID = wellSensorMeasurement.WellRegistrationID,
                MeasurementType = wellSensorMeasurement.MeasurementType.AsDto(),
                ReadingDate = wellSensorMeasurement.ReadingDate,
                SensorName = wellSensorMeasurement.SensorName,
                MeasurementValue = wellSensorMeasurement.MeasurementValue
            };
            DoCustomMappings(wellSensorMeasurement, wellSensorMeasurementDto);
            return wellSensorMeasurementDto;
        }

        static partial void DoCustomMappings(WellSensorMeasurement wellSensorMeasurement, WellSensorMeasurementDto wellSensorMeasurementDto);

    }
}