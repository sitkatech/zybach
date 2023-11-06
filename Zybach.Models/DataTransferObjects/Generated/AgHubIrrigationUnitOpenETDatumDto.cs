//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigationUnitOpenETDatum]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class AgHubIrrigationUnitOpenETDatumDto
    {
        public int AgHubIrrigationUnitOpenETDatumID { get; set; }
        public AgHubIrrigationUnitDto AgHubIrrigationUnit { get; set; }
        public OpenETDataTypeDto OpenETDataType { get; set; }
        public DateTime ReportedDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal? ReportedValueInches { get; set; }
        public decimal? AgHubIrrigationUnitAreaInAcres { get; set; }
    }

    public partial class AgHubIrrigationUnitOpenETDatumSimpleDto
    {
        public int AgHubIrrigationUnitOpenETDatumID { get; set; }
        public int AgHubIrrigationUnitID { get; set; }
        public int OpenETDataTypeID { get; set; }
        public DateTime ReportedDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal? ReportedValueInches { get; set; }
        public decimal? AgHubIrrigationUnitAreaInAcres { get; set; }
    }

}