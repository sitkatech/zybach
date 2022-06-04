using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public class WellWaterLevelInspectionDetailedDto
    {
        public WellSimpleDto Well { get; set; }
        public List<WaterLevelInspectionSimpleDto> WaterLevelInspections { get; set; }
    }
}
