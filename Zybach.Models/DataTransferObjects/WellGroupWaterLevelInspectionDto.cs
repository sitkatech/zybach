
using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public class WellGroupWaterLevelInspectionDto
    {
        public WellGroupDto WellGroup { get; set; }
        public List<WaterLevelInspectionSimpleDto> WaterLevelInspections { get; set; }
    }
}
