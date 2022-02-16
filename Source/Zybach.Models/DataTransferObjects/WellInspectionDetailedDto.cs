using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public class WellInspectionDetailedDto
    {
        public WellSimpleDto Well { get; set; }

        public List<WaterLevelInspectionSimpleDto> WaterLevelInspections { get; set; }
        public List<WaterQualityInspectionSimpleDto> WaterQualityInspections { get; set; }
    }
}
