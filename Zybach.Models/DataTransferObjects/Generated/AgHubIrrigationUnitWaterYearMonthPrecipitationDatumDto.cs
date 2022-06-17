//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigationUnitWaterYearMonthPrecipitationDatum]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class AgHubIrrigationUnitWaterYearMonthPrecipitationDatumDto
    {
        public int AgHubIrrigationUnitWaterYearMonthPrecipitationDatumID { get; set; }
        public AgHubIrrigationUnitDto AgHubIrrigationUnit { get; set; }
        public WaterYearMonthDto WaterYearMonth { get; set; }
        public decimal? PrecipitationInches { get; set; }
        public decimal? PrecipitationAcreInches { get; set; }
    }

    public partial class AgHubIrrigationUnitWaterYearMonthPrecipitationDatumSimpleDto
    {
        public int AgHubIrrigationUnitWaterYearMonthPrecipitationDatumID { get; set; }
        public int AgHubIrrigationUnitID { get; set; }
        public int WaterYearMonthID { get; set; }
        public decimal? PrecipitationInches { get; set; }
        public decimal? PrecipitationAcreInches { get; set; }
    }

}