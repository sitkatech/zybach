using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class SensorAnomalyExtensionMethods
    {
        static partial void DoCustomSimpleDtoMappings(SensorAnomaly sensorAnomaly, SensorAnomalySimpleDto sensorAnomalySimpleDto)
        {
            sensorAnomalySimpleDto.Sensor = sensorAnomaly.Sensor.AsSimpleDto();
            sensorAnomalySimpleDto.NumberOfAnomalousDays = (int)(sensorAnomaly.EndDate - sensorAnomaly.StartDate).TotalDays + 1;
        }
    }
}