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
            var wells = dbContext.Wells.Include(x => x.WellIrrigatedAcres).AsNoTracking().Where(x => x.HasElectricalData).ToList();
            var streamFlowZoneWellsDtos = streamFlowZones.Select(streamFlowZone => new StreamFlowZoneWellsDto
                {
                    StreamFlowZone = streamFlowZone.AsDto(),
                    Wells = wells.Where(x => x.StreamflowZoneID == streamFlowZone.StreamFlowZoneID)
                        .Select(x =>
                        {
                            var wellWithIrrigatedAcresDto = new WellWithIrrigatedAcresDto
                            {
                                WellRegistrationID = x.WellRegistrationID,
                                IrrigatedAcresPerYear = x.WellIrrigatedAcres.Select(y =>
                                    new IrrigatedAcresPerYearDto {Year = y.IrrigationYear, Acres = y.Acres}).ToList()
                            };
                            return wellWithIrrigatedAcresDto;
                        })
                        .ToList()
                })
                .ToList();
            return streamFlowZoneWellsDtos;
        }

    }
}