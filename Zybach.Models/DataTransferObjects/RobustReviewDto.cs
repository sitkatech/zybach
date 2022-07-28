using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public class RobustReviewDto
    {
        public string WellTPID { get; set; }
        public List<MonthlyWaterVolumeDto> MonthlyData { get; set; }
        public List<IrrigatedAcresPerYearDto> IrrigatedAcres { get; set; }
    }
}
