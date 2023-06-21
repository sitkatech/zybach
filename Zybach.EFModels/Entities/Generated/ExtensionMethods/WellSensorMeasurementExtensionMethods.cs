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
                ReadingYear = wellSensorMeasurement.ReadingYear,
                ReadingMonth = wellSensorMeasurement.ReadingMonth,
                ReadingDay = wellSensorMeasurement.ReadingDay,
                SensorName = wellSensorMeasurement.SensorName,
                MeasurementValue = wellSensorMeasurement.MeasurementValue,
                IsAnomalous = wellSensorMeasurement.IsAnomalous,
                IsElectricSource = wellSensorMeasurement.IsElectricSource
            };
            DoCustomMappings(wellSensorMeasurement, wellSensorMeasurementDto);
            return wellSensorMeasurementDto;
        }

        static partial void DoCustomMappings(WellSensorMeasurement wellSensorMeasurement, WellSensorMeasurementDto wellSensorMeasurementDto);

        public static WellSensorMeasurementSimpleDto AsSimpleDto(this WellSensorMeasurement wellSensorMeasurement)
        {
            var wellSensorMeasurementSimpleDto = new WellSensorMeasurementSimpleDto()
            {
                WellSensorMeasurementID = wellSensorMeasurement.WellSensorMeasurementID,
                WellRegistrationID = wellSensorMeasurement.WellRegistrationID,
                MeasurementTypeID = wellSensorMeasurement.MeasurementTypeID,
                ReadingYear = wellSensorMeasurement.ReadingYear,
                ReadingMonth = wellSensorMeasurement.ReadingMonth,
                ReadingDay = wellSensorMeasurement.ReadingDay,
                SensorName = wellSensorMeasurement.SensorName,
                MeasurementValue = wellSensorMeasurement.MeasurementValue,
                IsAnomalous = wellSensorMeasurement.IsAnomalous,
                IsElectricSource = wellSensorMeasurement.IsElectricSource
            };
            DoCustomSimpleDtoMappings(wellSensorMeasurement, wellSensorMeasurementSimpleDto);
            return wellSensorMeasurementSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(WellSensorMeasurement wellSensorMeasurement, WellSensorMeasurementSimpleDto wellSensorMeasurementSimpleDto);
    }
}