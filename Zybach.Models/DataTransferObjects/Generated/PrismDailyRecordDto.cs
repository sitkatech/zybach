//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PrismDailyRecord]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class PrismDailyRecordDto
    {
        public int PrismDailyRecordID { get; set; }
        public PrismMonthlySyncDto PrismMonthlySync { get; set; }
        public PrismSyncStatusDto PrismSyncStatus { get; set; }
        public PrismDataTypeDto PrismDataType { get; set; }
        public BlobResourceDto BlobResource { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string ErrorMessage { get; set; }
    }

    public partial class PrismDailyRecordSimpleDto
    {
        public int PrismDailyRecordID { get; set; }
        public System.Int32 PrismMonthlySyncID { get; set; }
        public System.Int32 PrismSyncStatusID { get; set; }
        public System.Int32 PrismDataTypeID { get; set; }
        public System.Int32? BlobResourceID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string ErrorMessage { get; set; }
    }

}