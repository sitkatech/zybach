//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PrismMonthlySync]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class PrismMonthlySyncDto
    {
        public int PrismMonthlySyncID { get; set; }
        public PrismSyncStatusDto PrismSyncStatus { get; set; }
        public RunoffCalculationStatusDto RunoffCalculationStatus { get; set; }
        public PrismDataTypeDto PrismDataType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime? LastSynchronizedDate { get; set; }
        public UserDto LastSynchronizedByUser { get; set; }
        public DateTime? LastRunoffCalculationDate { get; set; }
        public UserDto LastRunoffCalculatedByUser { get; set; }
        public string LastRunoffCalculationError { get; set; }
        public DateTime? FinalizeDate { get; set; }
        public UserDto FinalizeByUser { get; set; }
    }

    public partial class PrismMonthlySyncSimpleDto
    {
        public int PrismMonthlySyncID { get; set; }
        public System.Int32 PrismSyncStatusID { get; set; }
        public System.Int32 RunoffCalculationStatusID { get; set; }
        public System.Int32 PrismDataTypeID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public DateTime? LastSynchronizedDate { get; set; }
        public System.Int32? LastSynchronizedByUserID { get; set; }
        public DateTime? LastRunoffCalculationDate { get; set; }
        public System.Int32? LastRunoffCalculatedByUserID { get; set; }
        public string LastRunoffCalculationError { get; set; }
        public DateTime? FinalizeDate { get; set; }
        public System.Int32? FinalizeByUserID { get; set; }
    }

}