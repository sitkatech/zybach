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

        public static List<Well> List(ZybachDbContext dbContext)
        {
            return dbContext.Wells.AsNoTracking().ToList();
        }

        public static List<WellWithSensorSummaryDto> GetWellsAsWellWithSensorSummaryDtos(ZybachDbContext dbContext)
        {
            return dbContext.AgHubWells.Include(x => x.Well).AsNoTracking().ToList()
                .Select(WellWithSensorSummaryDtoFromWell).ToList();
        }

        private static WellWithSensorSummaryDto WellWithSensorSummaryDtoFromWell(AgHubWell agHubWell)
        {
            var wellWithSensorSummaryDto = new WellWithSensorSummaryDto();
            var sensors = new List<SensorSummaryDto>();
            if (agHubWell.HasElectricalData)
            {
                sensors.Add(new SensorSummaryDto()
                {
                    WellRegistrationID = agHubWell.Well.WellRegistrationID,
                    SensorType = "Electrical Usage"
                });
            }

            wellWithSensorSummaryDto.WellRegistrationID = agHubWell.Well.WellRegistrationID;
            wellWithSensorSummaryDto.WellTPID = agHubWell.WellTPID;
            wellWithSensorSummaryDto.Location = new Feature(new Point(new Position(agHubWell.Well.WellGeometry.Coordinate.Y, agHubWell.Well.WellGeometry.Coordinate.X)));
            wellWithSensorSummaryDto.InGeoOptix = false;
            wellWithSensorSummaryDto.Sensors = sensors;
            wellWithSensorSummaryDto.FetchDate = agHubWell.Well.LastUpdateDate;
            wellWithSensorSummaryDto.HasElectricalData = agHubWell.HasElectricalData;
            wellWithSensorSummaryDto.AgHubRegisteredUser = agHubWell.AgHubRegisteredUser;
            wellWithSensorSummaryDto.FieldName = agHubWell.FieldName;
            return wellWithSensorSummaryDto;
        }

        public static WellWithSensorSummaryDto FindByWellRegistrationIDAsWellWithSensorSummaryDto(ZybachDbContext dbContext, string wellRegistrationID)
        {
            var well = dbContext.AgHubWells.Include(x => x.Well).Include(x => x.AgHubWellIrrigatedAcres).AsNoTracking()
                .SingleOrDefault(x => x.Well.WellRegistrationID == wellRegistrationID);
            if (well == null)
            {
                return null;
            }

            var wellWithSensorSummaryDto = WellWithSensorSummaryDtoFromWell(well);
            wellWithSensorSummaryDto.IrrigatedAcresPerYear = well.AgHubWellIrrigatedAcres.Select(x =>
                new IrrigatedAcresPerYearDto { Acres = x.Acres, Year = x.IrrigationYear }).ToList();

            return wellWithSensorSummaryDto;
        }

        public static List<WellDto> SearchByWellRegistrationID(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.Wells.AsNoTracking().Where(x => x.WellRegistrationID.ToUpper().Contains(searchText.ToUpper())).Select(x => x.AsDto()).ToList();
        }

        public static List<WellDto> SearchByLandowner(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.AgHubWells.Include(x => x.Well).AsNoTracking().Where(x => x.AgHubRegisteredUser.ToUpper().Contains(searchText.ToUpper())).Select(x => x.Well.AsDto()).ToList();
        }

        public static List<WellDto> SearchByField(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.AgHubWells.Include(x => x.Well).AsNoTracking().Where(x => x.FieldName.ToUpper().Contains(searchText.ToUpper())).Select(x => x.Well.AsDto()).ToList();
        }
    }
}