using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class StreamFlowZone
    {
        public static List<StreamFlowZoneDto> List(ZybachDbContext dbContext)
        {
            return dbContext.StreamFlowZones.AsNoTracking().Select(x => x.AsDto()).ToList();
        }

        public static List<StreamFlowZoneWellsDto> ListStreamFlowZonesAndWellsWithinZone(ZybachDbContext dbContext)
        {
            var streamFlowZones = dbContext.StreamFlowZones.AsNoTracking().ToList();
            var aghubWells = dbContext.AgHubWells.Include(x => x.AgHubWellIrrigatedAcres).AsNoTracking().Where(x => x.HasElectricalData).ToList();
            var streamFlowZoneWellsDtos = streamFlowZones.Select(streamFlowZone => new StreamFlowZoneWellsDto
                {
                    StreamFlowZone = streamFlowZone.AsDto(),
                    AgHubWells = aghubWells.Where(x => x.StreamflowZoneID == streamFlowZone.StreamFlowZoneID)
                        .Select(x =>
                        {
                            var agHubWellWithIrrigatedAcresDto = new AgHubWellWithIrrigatedAcresDto
                            {
                                WellRegistrationID = x.WellRegistrationID,
                                IrrigatedAcresPerYear = x.AgHubWellIrrigatedAcres.Select(y =>
                                    new IrrigatedAcresPerYearDto {Year = y.IrrigationYear, Acres = y.Acres}).ToList()
                            };
                            return agHubWellWithIrrigatedAcresDto;
                        })
                        .ToList()
                })
                .ToList();
            return streamFlowZoneWellsDtos;
        }

    }
}