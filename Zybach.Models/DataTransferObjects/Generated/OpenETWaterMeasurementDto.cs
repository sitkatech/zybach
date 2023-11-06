//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[OpenETWaterMeasurement]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class OpenETWaterMeasurementDto
    {
        public int OpenETWaterMeasurementID { get; set; }
        public string WellTPID { get; set; }
        public OpenETDataTypeDto OpenETDataType { get; set; }
        public DateTime ReportedDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal ReportedValueInches { get; set; }
        public decimal ReportedValueAcreFeet { get; set; }
        public decimal IrrigationUnitArea { get; set; }
    }

    public partial class OpenETWaterMeasurementSimpleDto
    {
        public int OpenETWaterMeasurementID { get; set; }
        public string WellTPID { get; set; }
        public int OpenETDataTypeID { get; set; }
        public DateTime ReportedDate { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal ReportedValueInches { get; set; }
        public decimal ReportedValueAcreFeet { get; set; }
        public decimal IrrigationUnitArea { get; set; }
    }

}