//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[Sensor]
namespace Zybach.EFModels.Entities
{
    public partial class Sensor
    {
        public SensorType SensorType => SensorTypeID.HasValue ? SensorType.AllLookupDictionary[SensorTypeID.Value] : null;
    }
}