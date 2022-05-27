namespace Zybach.Models.DataTransferObjects
{
    public class AgHubIrrigationUnitWaterYearMonthETAndPrecipDatumDto
    {
        public WaterYearMonthDto WaterYearMonth { get; set; }
        public decimal? PrecipitationAcreFeet { get; set; }
        public decimal? PrecipitationInches { get; set; }
        public decimal? EvapotranspirationAcreFeet { get; set; }
        public decimal? EvapotranspirationInches { get; set; }

    }
}
