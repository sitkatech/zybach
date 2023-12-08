//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[OpenETSyncHistory]
namespace Zybach.EFModels.Entities
{
    public partial class OpenETSyncHistory
    {
        public int PrimaryKey => OpenETSyncHistoryID;
        public OpenETSyncResultType OpenETSyncResultType => OpenETSyncResultType.AllLookupDictionary[OpenETSyncResultTypeID];
        public OpenETDataType OpenETDataType => OpenETDataTypeID.HasValue ? OpenETDataType.AllLookupDictionary[OpenETDataTypeID.Value] : null;

        public static class FieldLengths
        {
            public const int GoogleBucketFileRetrievalURL = 200;
        }
    }
}