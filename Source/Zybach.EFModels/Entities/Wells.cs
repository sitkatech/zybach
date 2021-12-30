using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Zybach.Models.DataTransferObjects;
using Point = GeoJSON.Net.Geometry.Point;

namespace Zybach.EFModels.Entities
{
    public class Wells
    {
        public static List<WellDto> ListAsDtos(ZybachDbContext dbContext)
        {
            return dbContext.Wells.AsNoTracking().Select(x => x.AsDto()).ToList();
        }

        public static WellDto GetByWellIDAsDto(ZybachDbContext dbContext, int wellID)
        {
            var well = GetWellsImpl(dbContext).SingleOrDefault(x => x.WellID == wellID);
            return well?.AsDto();
        }

        public static List<WellWithSensorSummaryDto> ListAsWellWithSensorSummaryDto(ZybachDbContext dbContext)
        {
            return GetWellsImpl(dbContext).OrderBy(x => x.WellRegistrationID).Select(x => WellWithSensorSummaryDtoFromWell(x)).ToList();
        }

        public static WellWithSensorSummaryDto GetAsWellWithSensorSummaryDtoByWellRegistrationID(ZybachDbContext dbContext, string wellRegistrationID)
        {
            var well = GetWellsImpl(dbContext).SingleOrDefault(x => x.WellRegistrationID == wellRegistrationID);
            if (well != null)
            {
                return WellWithSensorSummaryDtoFromWell(well);
            }
            return null;
        }

        public static Well GetByWellRegistrationID(ZybachDbContext dbContext, string wellRegistrationID)
        {
            return GetWellsImpl(dbContext).SingleOrDefault(x => x.WellRegistrationID == wellRegistrationID);
        }

        private static IQueryable<Well> GetWellsImpl(ZybachDbContext dbContext)
        {
            return dbContext.Wells
                .Include(x => x.AgHubWell)
                .Include(x => x.GeoOptixWell)
                .Include(x => x.AgHubWell.AgHubWellIrrigatedAcres)
                .Include(x => x.Sensors)
                .Include(x => x.Sensors).ThenInclude(x => x.SensorType)
                .Include(x => x.WellWaterQualityInspectionTypes).ThenInclude(x => x.WaterQualityInspectionType)
                .Include(x => x.WellParticipation)
                .Include(x => x.WellUse)
                .Include(x => x.County)
                .Include(x => x.WellWaterQualityInspectionTypes).ThenInclude(x => x.WaterQualityInspectionType)
                .AsNoTracking();
        }

        private static WellWithSensorSummaryDto WellWithSensorSummaryDtoFromWell(Well well)
        {
            var wellWithSensorSummaryDto = new WellWithSensorSummaryDto();
            wellWithSensorSummaryDto.WellRegistrationID = well.WellRegistrationID;
            wellWithSensorSummaryDto.Location = new Feature(new Point(new Position(well.WellGeometry.Coordinate.Y, well.WellGeometry.Coordinate.X)));
            wellWithSensorSummaryDto.FetchDate = well.LastUpdateDate;
            wellWithSensorSummaryDto.InGeoOptix = well.GeoOptixWell != null;
            wellWithSensorSummaryDto.InAgHub = well.AgHubWell != null;

            var sensors = well.Sensors.Select(x => new SensorSummaryDto()
            {
                SensorName = x.SensorName,
                SensorType = x.SensorType.SensorTypeDisplayName,
                WellRegistrationID = well.WellRegistrationID,
                IsActive = x.IsActive
            }).ToList();

            var agHubWell = well.AgHubWell;
            if (agHubWell != null)
            {
                wellWithSensorSummaryDto.WellTPID = agHubWell.WellTPID;
                wellWithSensorSummaryDto.HasElectricalData = agHubWell.HasElectricalData;
                wellWithSensorSummaryDto.AgHubRegisteredUser = agHubWell.AgHubRegisteredUser;
                wellWithSensorSummaryDto.FieldName = agHubWell.FieldName;

                if (agHubWell.HasElectricalData)
                {
                    sensors.Add(new SensorSummaryDto()
                    {
                        WellRegistrationID = well.WellRegistrationID,
                        SensorType = "Electrical Usage" // TODO: Use a static enum
                    });
                }

                wellWithSensorSummaryDto.IrrigatedAcresPerYear = agHubWell.AgHubWellIrrigatedAcres
                    .Select(x => new IrrigatedAcresPerYearDto { Acres = x.Acres, Year = x.IrrigationYear }).ToList();
            }

            wellWithSensorSummaryDto.Sensors = sensors;

            return wellWithSensorSummaryDto;
        }

        public static List<WellSimpleDto> SearchByWellRegistrationID(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.Wells.AsNoTracking().Where(x => x.WellRegistrationID.Contains(searchText)).Select(x => x.AsSimpleDto()).ToList();
        }

        public static List<WellSimpleDto> SearchByWellRegistrationIDHasInspectionType(ZybachDbContext dbContext, string searchText)
        {
            var wellIDsWithInspectionTypes = dbContext.WellWaterQualityInspectionTypes.AsNoTracking().Select(x => x.WellID).Distinct();
            return dbContext.Wells.AsNoTracking().Where(x => x.WellRegistrationID.Contains(searchText))
                .Where(x => wellIDsWithInspectionTypes.Contains(x.WellID))
                .Select(x => x.AsSimpleDto()).ToList();
        }

        public static List<WellSimpleDto> SearchByAghubRegisteredUser(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.AgHubWells.Include(x => x.Well).AsNoTracking().Where(x => x.AgHubRegisteredUser.Contains(searchText)).Select(x => x.Well.AsSimpleDto()).ToList();
        }

        public static List<WellSimpleDto> SearchByField(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.AgHubWells.Include(x => x.Well).AsNoTracking().Where(x => x.FieldName.Contains(searchText)).Select(x => x.Well.AsSimpleDto()).ToList();
        }

        public static WellDto CreateNew(ZybachDbContext dbContext, WellNewDto wellNewDto)
        {
            var well = new Well
            {
                CreateDate = DateTime.UtcNow,
                LastUpdateDate = DateTime.UtcNow,
                WellRegistrationID = wellNewDto.WellRegistrationID,
                WellGeometry = CreateWellGeometryFromLatLong(wellNewDto.Latitude, wellNewDto.Longitude)
            };
            well.StreamflowZoneID = dbContext.StreamFlowZones
                .FirstOrDefault(x => x.StreamFlowZoneGeometry.Intersects(well.WellGeometry))?.StreamFlowZoneID;
            dbContext.Wells.Add(well);
            dbContext.SaveChanges();
            dbContext.Entry(well).Reload();
            return GetByWellIDAsDto(dbContext, well.WellID);
        }

        private static Geometry CreateWellGeometryFromLatLong(double latitude, double longitude)
        {
            var point = new NetTopologySuite.Geometries.Point(longitude, latitude)
            {
                SRID = 4326
            };
            return point;
        }
    }
}