using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class SensorExtensionMethods
    {
        static partial void DoCustomSimpleDtoMappings(Sensor sensor,
            SensorSimpleDto sensorSimpleDto)
        {
            sensorSimpleDto.SensorTypeName = sensor.SensorType?.SensorTypeDisplayName;
        }
    }
}
