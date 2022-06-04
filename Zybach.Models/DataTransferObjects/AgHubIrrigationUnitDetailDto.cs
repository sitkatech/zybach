using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public class AgHubIrrigationUnitDetailDto : AgHubIrrigationUnitSimpleDto
    {
        public string IrrigationUnitGeoJSON { get; set; }
        public List<AgHubIrrigationUnitWaterYearMonthETAndPrecipDatumDto> WaterYearMonthETAndPrecipData { get; set; }
    }
}
