﻿using System.Collections.Generic;
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
            var agHubWells = GetAghubWellsWithElectricalData(dbContext);
            var streamFlowZoneWellsDtos = streamFlowZones.Select(streamFlowZone => new StreamFlowZoneWellsDto
                {
                    StreamFlowZone = streamFlowZone.AsDto(),
                    Wells = agHubWells.Where(x => x.Well.StreamflowZoneID == streamFlowZone.StreamFlowZoneID)
                        .Select(x =>
                        {
                            var wellWithIrrigatedAcresDto = new WellWithIrrigatedAcresDto
                            {
                                WellRegistrationID = x.Well.WellRegistrationID,
                                IrrigatedAcresPerYear = x.AgHubWellIrrigatedAcres.Select(y =>
                                    new IrrigatedAcresPerYearDto {Year = y.IrrigationYear, Acres = y.Acres}).ToList()
                            };
                            return wellWithIrrigatedAcresDto;
                        })
                        .ToList()
                })
                .ToList();
            return streamFlowZoneWellsDtos;
        }

        private static List<AgHubWell> GetAghubWellsWithElectricalData(ZybachDbContext dbContext)
        {
            return dbContext.AgHubWells.Include(x => x.Well).Include(x => x.AgHubWellIrrigatedAcres).AsNoTracking()
                .Where(x => x.HasElectricalData).ToList();
        }
    }
}