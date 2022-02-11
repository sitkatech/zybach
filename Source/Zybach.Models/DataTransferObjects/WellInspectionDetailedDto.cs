using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zybach.Models.DataTransferObjects
{
    public class WellInspectionDetailedDto
    {
        public WellSimpleDto Well { get; set; }

        public List<WaterLevelInspectionSimpleDto> WaterLevelInspections { get; set; }
        public List<WaterQualityInspectionSimpleDto> WaterQualityInspections { get; set; }
    }
}
