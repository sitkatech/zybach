//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigationUnitOpenETDatum]
namespace Zybach.EFModels.Entities
{
    public partial class AgHubIrrigationUnitOpenETDatum
    {
        public int PrimaryKey => AgHubIrrigationUnitOpenETDatumID;
        public OpenETDataType OpenETDataType => OpenETDataType.AllLookupDictionary[OpenETDataTypeID];

        public static class FieldLengths
        {

        }
    }
}