//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[OpenETSyncHistory]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class OpenETSyncHistoryDto
    {
        public int OpenETSyncHistoryID { get; set; }
        public WaterYearMonthDto WaterYearMonth { get; set; }
        public OpenETSyncResultTypeDto OpenETSyncResultType { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string GoogleBucketFileRetrievalURL { get; set; }
        public string ErrorMessage { get; set; }
        public OpenETDataTypeDto OpenETDataType { get; set; }
        public OpenETSyncDto OpenETSync { get; set; }
    }

    public partial class OpenETSyncHistorySimpleDto
    {
        public int OpenETSyncHistoryID { get; set; }
        public int WaterYearMonthID { get; set; }
        public int OpenETSyncResultTypeID { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string GoogleBucketFileRetrievalURL { get; set; }
        public string ErrorMessage { get; set; }
        public int? OpenETDataTypeID { get; set; }
        public int? OpenETSyncID { get; set; }
    }

}