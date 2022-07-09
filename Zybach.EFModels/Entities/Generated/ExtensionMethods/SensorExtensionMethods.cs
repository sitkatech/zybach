//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[Sensor]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class SensorExtensionMethods
    {
        public static SensorDto AsDto(this Sensor sensor)
        {
            var sensorDto = new SensorDto()
            {
                SensorID = sensor.SensorID,
                SensorName = sensor.SensorName,
                SensorType = sensor.SensorType.AsDto(),
                Well = sensor.Well?.AsDto(),
                InGeoOptix = sensor.InGeoOptix,
                CreateDate = sensor.CreateDate,
                LastUpdateDate = sensor.LastUpdateDate,
                IsActive = sensor.IsActive,
                RetirementDate = sensor.RetirementDate
            };
            DoCustomMappings(sensor, sensorDto);
            return sensorDto;
        }

        static partial void DoCustomMappings(Sensor sensor, SensorDto sensorDto);

        public static SensorSimpleDto AsSimpleDto(this Sensor sensor)
        {
            var sensorSimpleDto = new SensorSimpleDto()
            {
                SensorID = sensor.SensorID,
                SensorName = sensor.SensorName,
                SensorTypeID = sensor.SensorTypeID,
                WellID = sensor.WellID,
                InGeoOptix = sensor.InGeoOptix,
                CreateDate = sensor.CreateDate,
                LastUpdateDate = sensor.LastUpdateDate,
                IsActive = sensor.IsActive,
                RetirementDate = sensor.RetirementDate
            };
            DoCustomSimpleDtoMappings(sensor, sensorSimpleDto);
            return sensorSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(Sensor sensor, SensorSimpleDto sensorSimpleDto);
    }
}