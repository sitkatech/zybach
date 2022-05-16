//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigationUnitWaterYearMonthETDatum]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class AgHubIrrigationUnitWaterYearMonthETDatumDto
    {
        public int AgHubIrrigationUnitWaterYearMonthETDatumID { get; set; }
        public AgHubIrrigationUnitDto AgHubIrrigationUnit { get; set; }
        public WaterYearMonthDto WaterYearMonth { get; set; }
        public decimal? EvapotranspirationRateInches { get; set; }
        public decimal? EvapotranspirationRateAcreFeet { get; set; }
    }

    public partial class AgHubIrrigationUnitWaterYearMonthETDatumSimpleDto
    {
        public int AgHubIrrigationUnitWaterYearMonthETDatumID { get; set; }
        public int AgHubIrrigationUnitID { get; set; }
        public int WaterYearMonthID { get; set; }
        public decimal? EvapotranspirationRateInches { get; set; }
        public decimal? EvapotranspirationRateAcreFeet { get; set; }
    }

}