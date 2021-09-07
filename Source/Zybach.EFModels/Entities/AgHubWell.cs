using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class AgHubWell
    {
        public int PumpingRateGallonsPerMinute => WellAuditPumpRate ?? RegisteredPumpRate ?? WellTPNRDPumpRate ?? 0;

        public static List<AgHubWell> List(ZybachDbContext dbContext)
        {
            return dbContext.AgHubWells.AsNoTracking().ToList();
        }

        public static List<WellWithSensorSummaryDto> GetAgHubWellsAsWellWithSensorSummaryDtos(ZybachDbContext dbContext)
        {
            return dbContext.AgHubWells.AsNoTracking().ToList()
                .Select(WellWithSensorSummaryDtoFromAgHubWell).ToList();
        }

        private static WellWithSensorSummaryDto WellWithSensorSummaryDtoFromAgHubWell(AgHubWell agHubWell)
        {
            var wellWithSensorSummaryDto = new WellWithSensorSummaryDto();
            var sensors = new List<SensorSummaryDto>();
            if (agHubWell.HasElectricalData)
            {
                sensors.Add(new SensorSummaryDto()
                {
                    WellRegistrationID = agHubWell.WellRegistrationID,
                    SensorType = "Electrical Usage"
                });
            }

            wellWithSensorSummaryDto.WellRegistrationID = agHubWell.WellRegistrationID;
            wellWithSensorSummaryDto.WellTPID = agHubWell.WellTPID;
            wellWithSensorSummaryDto.Location = new Feature(new Point(new Position(agHubWell.WellGeometry.Coordinate.Y, agHubWell.WellGeometry.Coordinate.X)));
            wellWithSensorSummaryDto.InGeoOptix = false;
            wellWithSensorSummaryDto.Sensors = sensors;
            wellWithSensorSummaryDto.FetchDate = agHubWell.FetchDate;
            wellWithSensorSummaryDto.HasElectricalData = agHubWell.HasElectricalData;
            wellWithSensorSummaryDto.LandownerName = agHubWell.LandownerName;
            wellWithSensorSummaryDto.FieldName = agHubWell.FieldName;
            return wellWithSensorSummaryDto;
        }

        public static WellWithSensorSummaryDto FindByWellRegistrationIDAsWellWithSensorSummaryDto(ZybachDbContext dbContext, string wellRegistrationID)
        {
            var agHubWell = dbContext.AgHubWells.Include(x => x.AgHubWellIrrigatedAcres).AsNoTracking()
                .SingleOrDefault(x => x.WellRegistrationID == wellRegistrationID);
            if (agHubWell == null)
            {
                return null;
            }

            var wellWithSensorSummaryDto = WellWithSensorSummaryDtoFromAgHubWell(agHubWell);
            wellWithSensorSummaryDto.IrrigatedAcresPerYear = agHubWell.AgHubWellIrrigatedAcres.Select(x =>
                new IrrigatedAcresPerYearDto { Acres = x.Acres, Year = x.IrrigationYear }).ToList();

            return wellWithSensorSummaryDto;
        }

        public static List<AgHubWellDto> SearchByWellRegistrationID(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.AgHubWells.AsNoTracking().Where(x => x.WellRegistrationID.Contains(searchText)).Select(x => x.AsDto()).ToList();
        }

        public static List<AgHubWellDto> SearchByLandowner(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.AgHubWells.AsNoTracking().Where(x => x.LandownerName.Contains(searchText)).Select(x => x.AsDto()).ToList();
        }

        public static List<AgHubWellDto> SearchByField(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.AgHubWells.AsNoTracking().Where(x => x.FieldName.Contains(searchText)).Select(x => x.AsDto()).ToList();
        }
    }
}