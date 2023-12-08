//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellSensorMeasurementStaging]
namespace Zybach.EFModels.Entities
{
    public partial class WellSensorMeasurementStaging
    {
        public int PrimaryKey => WellSensorMeasurementStagingID;
        public MeasurementType MeasurementType => MeasurementType.AllLookupDictionary[MeasurementTypeID];

        public static class FieldLengths
        {
            public const int WellRegistrationID = 100;
            public const int SensorName = 100;
        }
    }
}