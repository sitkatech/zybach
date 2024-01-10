//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[OpenETWaterMeasurement]
namespace Zybach.EFModels.Entities
{
    public partial class OpenETWaterMeasurement
    {
        public int PrimaryKey => OpenETWaterMeasurementID;
        public OpenETDataType OpenETDataType => OpenETDataType.AllLookupDictionary[OpenETDataTypeID];

        public static class FieldLengths
        {
            public const int WellTPID = 100;
        }
    }
}