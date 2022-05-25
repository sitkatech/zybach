//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatum]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumDto
    {
        public int AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumID { get; set; }
        public AgHubIrrigationUnitDto AgHubIrrigationUnit { get; set; }
        public WaterYearMonthDto WaterYearMonth { get; set; }
        public decimal? EvapotranspirationAcreFeet { get; set; }
        public decimal? EvapotranspirationInches { get; set; }
        public decimal? PrecipitationAcreFeet { get; set; }
        public decimal? PrecipitationInches { get; set; }
    }

    public partial class AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumSimpleDto
    {
        public int AgHubIrrigationUnitWaterYearMonthETAndPrecipitationDatumID { get; set; }
        public int AgHubIrrigationUnitID { get; set; }
        public int WaterYearMonthID { get; set; }
        public decimal? EvapotranspirationAcreFeet { get; set; }
        public decimal? EvapotranspirationInches { get; set; }
        public decimal? PrecipitationAcreFeet { get; set; }
        public decimal? PrecipitationInches { get; set; }
    }

}