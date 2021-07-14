using System.Collections.Generic;
using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class StreamFlowZone
    {
        public static List<StreamFlowZoneDto> List(ZybachDbContext dbContext)
        {
            return dbContext.StreamFlowZones.Select(x => x.AsDto()).ToList();
        }

        public static List<StreamFlowZoneWellsDto> ListStreamFlowZonesAndWellsWithinZone(ZybachDbContext dbContext)
        {
            var streamFlowZones = dbContext.StreamFlowZones.ToList();
            var aghubWells = dbContext.AgHubWells.Where(x => x.HasElectricalData ?? false).ToList();
            var streamFlowZoneWellsDtos = streamFlowZones.Select(streamFlowZone => new StreamFlowZoneWellsDto
                {
                    StreamFlowZone = streamFlowZone.AsDto(),
                    AgHubWells = aghubWells.Where(x => x.WellGeometry.Within(streamFlowZone.StreamFlowZoneGeometry))
                        .Select(x => x.AsDto())
                        .ToList()
                })
                .ToList();
            return streamFlowZoneWellsDtos;
        }

    }
}