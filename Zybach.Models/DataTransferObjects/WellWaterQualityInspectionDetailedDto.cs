using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public class WellWaterQualityInspectionDetailedDto
    {
        public WellSimpleDto Well { get; set; }
        public List<WaterQualityInspectionSimpleDto> WaterQualityInspections { get; set; }
    }
}
