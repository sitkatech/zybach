using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public class StreamFlowZoneWellsDto
    {
        public StreamFlowZoneDto StreamFlowZone { get; set; }
        public List<AgHubWellDto> AgHubWells { get; set; }
    }
}