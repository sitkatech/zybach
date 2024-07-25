//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PrismDailyRecord]
namespace Zybach.EFModels.Entities
{
    public partial class PrismDailyRecord
    {
        public int PrimaryKey => PrismDailyRecordID;
        public PrismSyncStatus PrismSyncStatus => PrismSyncStatus.AllLookupDictionary[PrismSyncStatusID];
        public PrismDataType PrismDataType => PrismDataType.AllLookupDictionary[PrismDataTypeID];

        public static class FieldLengths
        {

        }
    }
}