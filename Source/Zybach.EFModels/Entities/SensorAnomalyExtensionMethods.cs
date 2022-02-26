using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class SensorAnomalyExtensionMethods
    {
        static partial void DoCustomSimpleDtoMappings(SensorAnomaly sensorAnomaly, SensorAnomalySimpleDto sensorAnomalySimpleDto)
        {
            sensorAnomalySimpleDto.SensorName = sensorAnomaly.Sensor.SensorName;
            sensorAnomalySimpleDto.WellRegistrationID = sensorAnomaly.Sensor.Well.WellRegistrationID;
            sensorAnomalySimpleDto.NumberOfAnomalousDays = (int)(sensorAnomaly.EndDate - sensorAnomaly.StartDate).TotalDays;
        }
    }
}