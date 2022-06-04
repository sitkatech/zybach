using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public partial class AgHubIrrigationUnitSimpleDto
    {
        public List<WellMinimalDto> AssociatedWells { get; set; }
    }
}
