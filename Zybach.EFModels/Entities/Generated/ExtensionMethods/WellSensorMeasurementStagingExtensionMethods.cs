//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellSensorMeasurementStaging]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WellSensorMeasurementStagingExtensionMethods
    {
        public static WellSensorMeasurementStagingDto AsDto(this WellSensorMeasurementStaging wellSensorMeasurementStaging)
        {
            var wellSensorMeasurementStagingDto = new WellSensorMeasurementStagingDto()
            {
                WellSensorMeasurementStagingID = wellSensorMeasurementStaging.WellSensorMeasurementStagingID,
                WellRegistrationID = wellSensorMeasurementStaging.WellRegistrationID,
                MeasurementType = wellSensorMeasurementStaging.MeasurementType.AsDto(),
                ReadingYear = wellSensorMeasurementStaging.ReadingYear,
                ReadingMonth = wellSensorMeasurementStaging.ReadingMonth,
                ReadingDay = wellSensorMeasurementStaging.ReadingDay,
                SensorName = wellSensorMeasurementStaging.SensorName,
                MeasurementValue = wellSensorMeasurementStaging.MeasurementValue,
                IsElectricSource = wellSensorMeasurementStaging.IsElectricSource
            };
            DoCustomMappings(wellSensorMeasurementStaging, wellSensorMeasurementStagingDto);
            return wellSensorMeasurementStagingDto;
        }

        static partial void DoCustomMappings(WellSensorMeasurementStaging wellSensorMeasurementStaging, WellSensorMeasurementStagingDto wellSensorMeasurementStagingDto);

        public static WellSensorMeasurementStagingSimpleDto AsSimpleDto(this WellSensorMeasurementStaging wellSensorMeasurementStaging)
        {
            var wellSensorMeasurementStagingSimpleDto = new WellSensorMeasurementStagingSimpleDto()
            {
                WellSensorMeasurementStagingID = wellSensorMeasurementStaging.WellSensorMeasurementStagingID,
                WellRegistrationID = wellSensorMeasurementStaging.WellRegistrationID,
                MeasurementTypeID = wellSensorMeasurementStaging.MeasurementTypeID,
                ReadingYear = wellSensorMeasurementStaging.ReadingYear,
                ReadingMonth = wellSensorMeasurementStaging.ReadingMonth,
                ReadingDay = wellSensorMeasurementStaging.ReadingDay,
                SensorName = wellSensorMeasurementStaging.SensorName,
                MeasurementValue = wellSensorMeasurementStaging.MeasurementValue,
                IsElectricSource = wellSensorMeasurementStaging.IsElectricSource
            };
            DoCustomSimpleDtoMappings(wellSensorMeasurementStaging, wellSensorMeasurementStagingSimpleDto);
            return wellSensorMeasurementStagingSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(WellSensorMeasurementStaging wellSensorMeasurementStaging, WellSensorMeasurementStagingSimpleDto wellSensorMeasurementStagingSimpleDto);
    }
}