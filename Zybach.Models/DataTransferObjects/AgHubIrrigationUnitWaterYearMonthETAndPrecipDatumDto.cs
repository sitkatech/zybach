namespace Zybach.Models.DataTransferObjects
{
    public class AgHubIrrigationUnitWaterYearMonthETAndPrecipDatumDto
    {
        public WaterYearMonthDto WaterYearMonth { get; set; }
        public decimal? PrecipitationAcreInches { get; set; }
        public decimal? PrecipitationInches { get; set; }
        public decimal? EvapotranspirationAcreInches { get; set; }
        public decimal? EvapotranspirationInches { get; set; }

    }
}
