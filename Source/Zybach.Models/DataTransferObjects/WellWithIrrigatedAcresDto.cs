using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public class WellWithIrrigatedAcresDto
    {
        public string WellRegistrationID { get; set; }
        public List<IrrigatedAcresPerYearDto> IrrigatedAcresPerYear { get; set; }
    }
}