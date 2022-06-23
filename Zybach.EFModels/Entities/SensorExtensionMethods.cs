using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class SensorExtensionMethods
    {
        static partial void DoCustomSimpleDtoMappings(Sensor sensor,
            SensorSimpleDto sensorSimpleDto)
        {
            sensorSimpleDto.SensorTypeName = sensor.SensorType.SensorTypeDisplayName;
            sensorSimpleDto.WellRegistrationID = sensor.Well?.WellRegistrationID;
            sensorSimpleDto.WellPageNumber = sensor.Well?.PageNumber;
            sensorSimpleDto.WellOwnerName = sensor.Well?.OwnerName;
            sensorSimpleDto.WellTownshipRangeSection = sensor.Well?.TownshipRangeSection;
        }
    }
}
