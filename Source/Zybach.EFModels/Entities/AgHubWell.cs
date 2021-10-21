using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class Well
    {
        public int PumpingRateGallonsPerMinute => WellAuditPumpRate ?? RegisteredPumpRate ?? WellTPNRDPumpRate ?? 0;

        public static List<Well> List(ZybachDbContext dbContext)
        {
            return dbContext.Wells.AsNoTracking().ToList();
        }

        public static List<WellWithSensorSummaryDto> GetWellsAsWellWithSensorSummaryDtos(ZybachDbContext dbContext)
        {
            return dbContext.Wells.AsNoTracking().ToList()
                .Select(WellWithSensorSummaryDtoFromWell).ToList();
        }

        private static WellWithSensorSummaryDto WellWithSensorSummaryDtoFromWell(Well well)
        {
            var wellWithSensorSummaryDto = new WellWithSensorSummaryDto();
            var sensors = new List<SensorSummaryDto>();
            if (well.HasElectricalData)
            {
                sensors.Add(new SensorSummaryDto()
                {
                    WellRegistrationID = well.WellRegistrationID,
                    SensorType = "Electrical Usage"
                });
            }

            wellWithSensorSummaryDto.WellRegistrationID = well.WellRegistrationID;
            wellWithSensorSummaryDto.WellTPID = well.WellTPID;
            wellWithSensorSummaryDto.Location = new Feature(new Point(new Position(well.WellGeometry.Coordinate.Y, well.WellGeometry.Coordinate.X)));
            wellWithSensorSummaryDto.InGeoOptix = false;
            wellWithSensorSummaryDto.Sensors = sensors;
            wellWithSensorSummaryDto.FetchDate = well.FetchDate;
            wellWithSensorSummaryDto.HasElectricalData = well.HasElectricalData;
            wellWithSensorSummaryDto.LandownerName = well.LandownerName;
            wellWithSensorSummaryDto.FieldName = well.FieldName;
            return wellWithSensorSummaryDto;
        }

        public static WellWithSensorSummaryDto FindByWellRegistrationIDAsWellWithSensorSummaryDto(ZybachDbContext dbContext, string wellRegistrationID)
        {
            var well = dbContext.Wells.Include(x => x.WellIrrigatedAcres).AsNoTracking()
                .SingleOrDefault(x => x.WellRegistrationID == wellRegistrationID);
            if (well == null)
            {
                return null;
            }

            var wellWithSensorSummaryDto = WellWithSensorSummaryDtoFromWell(well);
            wellWithSensorSummaryDto.IrrigatedAcresPerYear = well.WellIrrigatedAcres.Select(x =>
                new IrrigatedAcresPerYearDto { Acres = x.Acres, Year = x.IrrigationYear }).ToList();

            return wellWithSensorSummaryDto;
        }

        public static List<WellDto> SearchByWellRegistrationID(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.Wells.AsNoTracking().Where(x => x.WellRegistrationID.ToUpper().Contains(searchText.ToUpper())).Select(x => x.AsDto()).ToList();
        }

        public static List<WellDto> SearchByLandowner(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.Wells.AsNoTracking().Where(x => x.LandownerName.ToUpper().Contains(searchText.ToUpper())).Select(x => x.AsDto()).ToList();
        }

        public static List<WellDto> SearchByField(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.Wells.AsNoTracking().Where(x => x.FieldName.ToUpper().Contains(searchText.ToUpper())).Select(x => x.AsDto()).ToList();
        }
    }
}