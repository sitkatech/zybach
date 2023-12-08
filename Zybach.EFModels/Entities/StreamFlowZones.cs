using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public class StreamFlowZones
    {
        public static List<StreamFlowZoneDto> List(ZybachDbContext dbContext)
        {
            return dbContext.StreamFlowZones.AsNoTracking().Select(x => x.AsDto()).ToList();
        }

        public static List<StreamFlowZoneWellsDto> ListStreamFlowZonesAndWellsWithinZone(ZybachDbContext dbContext)
        {
            var streamFlowZones = dbContext.StreamFlowZones.AsNoTracking().ToList();
            var agHubWells = GetAghubWellsWithElectricalData(dbContext);
            var wellSensorMeasurements = WellSensorMeasurements.GetByMeasurementType(dbContext,
                MeasurementTypeEnum.ElectricalUsage);
            var streamFlowZoneWellsDtos = streamFlowZones.Select(streamFlowZone =>
                {
                    var streamFlowZoneWellsDto = new StreamFlowZoneWellsDto
                    {
                        StreamFlowZone = streamFlowZone.AsDto()
                    };
                    var wellWithIrrigatedAcresDtos = new List<WellWithIrrigatedAcresDto>();
                    // to be included in the list of wells, there are WellSesnsorMeasurement values for this well for MeasurementTypeID = 3 (Electrical Usage)
                    foreach (var agHubWell in agHubWells.Where(x => x.Well.StreamflowZoneID == streamFlowZone.StreamFlowZoneID))
                    {
                        var wellSensorMeasurementsForThisWell = wellSensorMeasurements.Where(y => y.WellRegistrationID == agHubWell.Well.WellRegistrationID).ToList();
                        if (wellSensorMeasurementsForThisWell.Any())
                        {
                            var wellWithIrrigatedAcresDto = new WellWithIrrigatedAcresDto
                            {
                                WellRegistrationID = agHubWell.Well.WellRegistrationID
                            };
                            var irrigatedAcresPerYears = new List<IrrigatedAcresPerYearDto>();

                            foreach (var agHubWellIrrigatedAcre in agHubWell.AgHubWellIrrigatedAcres)
                            {
                                if (wellSensorMeasurementsForThisWell.Any(y => y.ReadingYear == agHubWellIrrigatedAcre.IrrigationYear && (y.IsElectricSource ?? false)))
                                {

                                    irrigatedAcresPerYears.Add(new IrrigatedAcresPerYearDto
                                    {
                                        Acres = agHubWellIrrigatedAcre.Acres,
                                        Year = agHubWellIrrigatedAcre.IrrigationYear
                                    });
                                }
                            }

                            wellWithIrrigatedAcresDto.IrrigatedAcresPerYear = irrigatedAcresPerYears;
                            wellWithIrrigatedAcresDtos.Add(wellWithIrrigatedAcresDto);
                        }
                    }
                    streamFlowZoneWellsDto.Wells = wellWithIrrigatedAcresDtos;
                    return streamFlowZoneWellsDto;
                })
                .ToList();
            return streamFlowZoneWellsDtos;
        }

        private static List<AgHubWell> GetAghubWellsWithElectricalData(ZybachDbContext dbContext)
        {
            return dbContext.AgHubWells.Include(x => x.Well).Include(x => x.AgHubWellIrrigatedAcres).AsNoTracking().ToList();
        }
    }
}