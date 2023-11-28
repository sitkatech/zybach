using System;

namespace Zybach.Models.DataTransferObjects
{
    public class AgHubIrrigationUnitWaterYearMonthETAndPrecipDatumDto
    {
        public DateTime ReportedDate { get; set; }
        public decimal? PrecipitationAcreInches { get; set; }
        public decimal? PrecipitationInches { get; set; }
        public decimal? EvapotranspirationAcreInches { get; set; }
        public decimal? EvapotranspirationInches { get; set; }

    }
}
