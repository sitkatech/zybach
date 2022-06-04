//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SensorAnomaly]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class SensorAnomalyExtensionMethods
    {
        public static SensorAnomalyDto AsDto(this SensorAnomaly sensorAnomaly)
        {
            var sensorAnomalyDto = new SensorAnomalyDto()
            {
                SensorAnomalyID = sensorAnomaly.SensorAnomalyID,
                Sensor = sensorAnomaly.Sensor.AsDto(),
                StartDate = sensorAnomaly.StartDate,
                EndDate = sensorAnomaly.EndDate,
                Notes = sensorAnomaly.Notes
            };
            DoCustomMappings(sensorAnomaly, sensorAnomalyDto);
            return sensorAnomalyDto;
        }

        static partial void DoCustomMappings(SensorAnomaly sensorAnomaly, SensorAnomalyDto sensorAnomalyDto);

        public static SensorAnomalySimpleDto AsSimpleDto(this SensorAnomaly sensorAnomaly)
        {
            var sensorAnomalySimpleDto = new SensorAnomalySimpleDto()
            {
                SensorAnomalyID = sensorAnomaly.SensorAnomalyID,
                SensorID = sensorAnomaly.SensorID,
                StartDate = sensorAnomaly.StartDate,
                EndDate = sensorAnomaly.EndDate,
                Notes = sensorAnomaly.Notes
            };
            DoCustomSimpleDtoMappings(sensorAnomaly, sensorAnomalySimpleDto);
            return sensorAnomalySimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(SensorAnomaly sensorAnomaly, SensorAnomalySimpleDto sensorAnomalySimpleDto);
    }
}