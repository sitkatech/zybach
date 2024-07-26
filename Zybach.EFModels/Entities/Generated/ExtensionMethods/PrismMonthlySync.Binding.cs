//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PrismMonthlySync]
namespace Zybach.EFModels.Entities
{
    public partial class PrismMonthlySync
    {
        public int PrimaryKey => PrismMonthlySyncID;
        public PrismSyncStatus PrismSyncStatus => PrismSyncStatus.AllLookupDictionary[PrismSyncStatusID];
        public RunoffCalculationStatus RunoffCalculationStatus => RunoffCalculationStatus.AllLookupDictionary[RunoffCalculationStatusID];
        public PrismDataType PrismDataType => PrismDataType.AllLookupDictionary[PrismDataTypeID];

        public static class FieldLengths
        {

        }
    }
}