//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[OpenETWaterMeasurement]
namespace Zybach.EFModels.Entities
{
    public partial class OpenETWaterMeasurement
    {
        public OpenETDataType OpenETDataType => OpenETDataType.AllLookupDictionary[OpenETDataTypeID];
    }
}