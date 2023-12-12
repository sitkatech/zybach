//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellSensorMeasurement]
namespace Zybach.EFModels.Entities
{
    public partial class WellSensorMeasurement
    {
        public int PrimaryKey => WellSensorMeasurementID;
        public MeasurementType MeasurementType => MeasurementType.AllLookupDictionary[MeasurementTypeID];

        public static class FieldLengths
        {
            public const int WellRegistrationID = 100;
            public const int SensorName = 100;
        }
    }
}