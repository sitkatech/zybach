using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Zybach.Models.DataTransferObjects;
using Point = GeoJSON.Net.Geometry.Point;
using Position = GeoJSON.Net.Geometry.Position;

namespace Zybach.EFModels.Entities
{
    public class Wells
    {
        public static WellDto GetByIDAsDto(ZybachDbContext dbContext, int wellID)
        {
            var well = GetWellsImpl(dbContext).SingleOrDefault(x => x.WellID == wellID);
            return well?.AsDto();
        }

        public static WellSimpleDto GetByIDAsSimpleDto(ZybachDbContext dbContext, int wellID)
        {
            var well = GetWellsImpl(dbContext).SingleOrDefault(x => x.WellID == wellID);
            return well?.AsSimpleDto();
        }

        public static List<WellWithSensorSimpleDto> ListAsWellWithSensorSimpleDto(ZybachDbContext dbContext)
        {
            var wellsWithWaterLevelInspections = dbContext.WaterLevelInspections
                .Include(x => x.Well)
                .AsNoTracking().ToList()
                .GroupBy(x => x.WellID)
                .ToDictionary(x => x.Key, y => y.Any());
            var wellsWithWaterQualityInspections = dbContext.WaterQualityInspections
                .Include(x => x.Well)
                .AsNoTracking().ToList()
                .GroupBy(x => x.WellID)
                .ToDictionary(x => x.Key, y => y.Any());
            return GetWellsImpl(dbContext)
                .OrderBy(x => x.WellRegistrationID)
                .Select(x => WellWithSensorSimpleDtoFromWell(x,
                    wellsWithWaterLevelInspections.ContainsKey(x.WellID)
                        ? wellsWithWaterLevelInspections[x.WellID]
                        : null,
                    wellsWithWaterQualityInspections.ContainsKey(x.WellID)
                        ? wellsWithWaterQualityInspections[x.WellID]
                        : null))
                .ToList();
        }

        public static WellWithSensorSimpleDto GetByIDAsWellWithSensorSimpleDto(ZybachDbContext dbContext, int wellID)
        {
            var well = GetWellsImpl(dbContext).SingleOrDefault(x => x.WellID == wellID);
            if (well != null)
            {
                return WellWithSensorSimpleDtoFromWell(well, null, null);
            }
            return null;
        }

        public static Well GetByWellRegistrationID(ZybachDbContext dbContext, string wellRegistrationID)
        {
            return dbContext.Wells.AsNoTracking().SingleOrDefault(x => x.WellRegistrationID == wellRegistrationID);
        }

        public static Well GetByWellRegistrationIDWithTracking(ZybachDbContext dbContext, string wellRegistrationID)
        {
            return dbContext.Wells.SingleOrDefault(x => x.WellRegistrationID == wellRegistrationID);
        }

        public static Well GetByID(ZybachDbContext dbContext, int wellID)
        {
            return GetWellsImpl(dbContext).SingleOrDefault(x => x.WellID == wellID);
        }

        public static Well GetByIDWithTracking(ZybachDbContext dbContext, int wellID)
        {
            return dbContext.Wells.SingleOrDefault(x => x.WellID == wellID);
        }

        private static IQueryable<Well> GetWellsImpl(ZybachDbContext dbContext)
        {
            return dbContext.Wells
                .Include(x => x.AgHubWell)
                    .ThenInclude(x => x.AgHubIrrigationUnit)
                .Include(x => x.GeoOptixWell)
                .Include(x => x.AgHubWell.AgHubWellIrrigatedAcres)
                .Include(x => x.Sensors)
                .Include(x => x.WellWaterQualityInspectionTypes)
                .Include(x => x.WellParticipation)
                .AsNoTracking();
        }

        private static WellWithSensorSimpleDto WellWithSensorSimpleDtoFromWell(Well well, bool? hasWaterLevelInspections, bool? hasWaterQualityInspections)
        {
            var wellWithSensorSimpleDto = new WellWithSensorSimpleDto
            {
                WellID = well.WellID,
                WellRegistrationID = well.WellRegistrationID,
                Location = new Feature(new Point(new Position(well.WellGeometry.Coordinate.Y, well.WellGeometry.Coordinate.X))),
                FetchDate = well.LastUpdateDate,
                InGeoOptix = well.GeoOptixWell != null,
                InAgHub = well.AgHubWell != null,
                WellNickname = well.WellNickname,
                OwnerName = well.OwnerName,
                PageNumber = well.PageNumber,
                TownshipRangeSection = well.TownshipRangeSection,
                HasWaterLevelInspections = hasWaterLevelInspections,
                HasWaterQualityInspections = hasWaterQualityInspections
            };

            var sensors = well.Sensors.Select(x => x.AsSimpleDto()).ToList();

            var agHubWell = well.AgHubWell;
            if (agHubWell != null)
            {
                wellWithSensorSimpleDto.WellTPID = agHubWell.AgHubIrrigationUnit?.WellTPID;
                wellWithSensorSimpleDto.HasElectricalData = agHubWell.HasElectricalData;
                wellWithSensorSimpleDto.WellConnectedMeter = agHubWell.WellConnectedMeter;
                wellWithSensorSimpleDto.AgHubRegisteredUser = agHubWell.AgHubRegisteredUser;
                wellWithSensorSimpleDto.FieldName = agHubWell.FieldName;

                if (agHubWell.HasElectricalData)
                {
                    sensors.Add(new SensorSimpleDto()
                    {
                        WellRegistrationID = well.WellRegistrationID,
                        SensorTypeName = SensorType.ElectricalUsage.SensorTypeDisplayName,
                        ChartDataSourceName = SensorType.ElectricalUsage.SensorTypeDisplayName
                    });
                }

                wellWithSensorSimpleDto.IrrigatedAcresPerYear = agHubWell.AgHubWellIrrigatedAcres
                    .Select(x => new IrrigatedAcresPerYearDto { Acres = x.Acres, Year = x.IrrigationYear }).ToList();
            }

            wellWithSensorSimpleDto.Sensors = sensors;

            return wellWithSensorSimpleDto;
        }

        public static List<WellSimpleDto> SearchByWellRegistrationID(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.Wells.AsNoTracking().Where(x => x.WellRegistrationID.Contains(searchText)).Select(x => x.AsSimpleDto()).ToList();
        }

        public static List<string> SearchByWellRegistrationIDHasInspectionType(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.WellWaterQualityInspectionTypes
                .Include(x => x.Well)
                .AsNoTracking()
                .Where(x => x.Well.WellRegistrationID.Contains(searchText))
                .Select(x => x.Well.WellRegistrationID).Distinct()
                .ToList();
        }

        public static List<string> SearchByWellRegistrationIDRequiresChemigation(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.Wells
                .AsNoTracking()
                .Where(x => x.RequiresChemigation && x.WellRegistrationID.Contains(searchText))
                .Select(x => x.WellRegistrationID)
                .Distinct().ToList();
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
            return GetByIDAsDto(dbContext, well.WellID);
        }

        private static Geometry CreateWellGeometryFromLatLong(double latitude, double longitude)
        {
            var point = new NetTopologySuite.Geometries.Point(longitude, latitude)
            {
                SRID = 4326
            };
            return point;
        }

        public static void MapFromContactUpsert(Well well, WellContactInfoDto wellContactInfoDto)
        {
            well.TownshipRangeSection = wellContactInfoDto.TownshipRangeSection;
            well.CountyID = wellContactInfoDto.CountyID;

            well.OwnerName = wellContactInfoDto.OwnerName;
            well.OwnerAddress = wellContactInfoDto.OwnerAddress;
            well.OwnerCity = wellContactInfoDto.OwnerCity;
            well.OwnerState = wellContactInfoDto.OwnerState;
            well.OwnerZipCode = wellContactInfoDto.OwnerZipCode;

            well.AdditionalContactName = wellContactInfoDto.AdditionalContactName;
            well.AdditionalContactAddress = wellContactInfoDto.AdditionalContactAddress;
            well.AdditionalContactCity = wellContactInfoDto.AdditionalContactCity;
            well.AdditionalContactState = wellContactInfoDto.AdditionalContactState;
            well.AdditionalContactZipCode = wellContactInfoDto.AdditionalContactZipCode;

            well.WellNickname = wellContactInfoDto.WellNickname;
            well.Notes = wellContactInfoDto.Notes;
        }

        public static void MapFromParticipationUpsert(Well well, WellParticipationInfoDto wellParticipationInfoDto)
        {
            well.WellParticipationID = wellParticipationInfoDto.WellParticipationID;
            well.WellUseID = wellParticipationInfoDto.WellUseID;
            well.RequiresChemigation = wellParticipationInfoDto.RequiresChemigation;
            well.RequiresWaterLevelInspection = wellParticipationInfoDto.RequiresWaterLevelInspection;
            well.IsReplacement = wellParticipationInfoDto.IsReplacement;
            well.WellDepth = wellParticipationInfoDto.WellDepth;
            well.Clearinghouse = wellParticipationInfoDto.Clearinghouse;
            well.PageNumber = wellParticipationInfoDto.PageNumber;
            well.ScreenDepth = wellParticipationInfoDto.ScreenDepth;
            well.ScreenInterval = wellParticipationInfoDto.ScreenInterval;
            well.SiteName = wellParticipationInfoDto.SiteName;
            well.SiteNumber = wellParticipationInfoDto.SiteNumber;
        }

        public static List<WellInspectionSummaryDto> ListAsWellInspectionSummaryDtos(ZybachDbContext dbContext)
        {
            var latestWellWaterLevelInspection = dbContext.WaterLevelInspections
                .Include(x => x.Well)
                .AsNoTracking().ToList()
                .GroupBy(x => x.WellID).ToDictionary(x => x.Key, x =>
                    x.OrderByDescending(y => y.InspectionDate).FirstOrDefault()?.AsSimpleDto());
            var latestWellWaterQualityInspection = dbContext.WaterQualityInspections
                .Include(x => x.Well)
                .AsNoTracking().ToList()
                .GroupBy(x => x.WellID).ToDictionary(x => x.Key, x =>
                    x.OrderByDescending(y => y.InspectionDate).FirstOrDefault()?.AsSimpleDto());
            var listWithLatestInspectionsAsDto = dbContext.Wells
                .Include(x => x.WellWaterQualityInspectionTypes)
                .Include(x => x.WellParticipation)
                .AsNoTracking()
                .ToList()
                .Select(x =>
                    x.AsWellInspectionSummaryDto(
                        latestWellWaterLevelInspection.ContainsKey(x.WellID) ? latestWellWaterLevelInspection[x.WellID] : null,
                        latestWellWaterQualityInspection.ContainsKey(x.WellID) ? latestWellWaterQualityInspection[x.WellID] : null))
                .Where(x => x.HasWaterLevelInspections || x.HasWaterQualityInspections)
                .ToList();
            return listWithLatestInspectionsAsDto;
        }

        public static IEnumerable<WellWaterLevelInspectionDetailedDto> ListByWellIDsAsWellWaterLevelInspectionDetailedDto(ZybachDbContext dbContext, List<int> wellIDs)
        {
            var wells = dbContext.Wells
                .Include(x => x.WellWaterQualityInspectionTypes)
                .Include(x => x.WellParticipation)
                .AsNoTracking()
                .Where(x => wellIDs.Contains(x.WellID))
                .ToList();
            var waterLevelInspections = dbContext.WaterLevelInspections
                .Include(x => x.Well)
                .AsNoTracking().ToList()
                .GroupBy(x => x.WellID)
                .ToDictionary(x => x.Key, x =>
                    x.OrderByDescending(y => y.InspectionDate).Select(x => x.AsSimpleDto()).ToList());

            var wellWaterLevelInspectionDetailedDtos = wells
                .Select(x => x.AsWellWaterLevelInspectionDetailedDto(waterLevelInspections.ContainsKey(x.WellID) ? waterLevelInspections[x.WellID] : null))
                .ToList();
            return wellWaterLevelInspectionDetailedDtos;
        }

        public static IEnumerable<WellWaterQualityInspectionDetailedDto> ListByWellIDsAsWellWaterQualityInspectionDetailedDto(ZybachDbContext dbContext, List<int> wellIDs)
        {
            var wells = dbContext.Wells
                .Include(x => x.WellWaterQualityInspectionTypes)
                .Include(x => x.WellParticipation)
                .AsNoTracking()
                .Where(x => wellIDs.Contains(x.WellID))
                .ToList();
            var waterQualityInspections = dbContext.WaterQualityInspections
                .Include(x => x.Well)
                .AsNoTracking().ToList()
                .GroupBy(x => x.WellID)
                .ToDictionary(x => x.Key, x =>
                    x.OrderByDescending(y => y.InspectionDate).Select(x => x.AsSimpleDto()).ToList());

            var wellWaterQualityInspectionDetailedDtos = wells
                .Select(x => x.AsWellWaterQualityInspectionDetailedDto(waterQualityInspections.ContainsKey(x.WellID) ? waterQualityInspections[x.WellID] : null))
                .ToList();
            return wellWaterQualityInspectionDetailedDtos;
        }

        public static List<WellWaterLevelMapSummaryDto> ListAsWaterLevelMapSummaryDtos(ZybachDbContext dbContext)
        {
            return GetWellsImpl(dbContext)
                .OrderBy(x => x.WellRegistrationID)
                .Select(x => x.AsWaterLevelMapSummaryDto())
                .ToList();
        }
    }
}