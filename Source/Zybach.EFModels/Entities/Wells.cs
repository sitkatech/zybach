﻿using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public class Wells
    {
        public static List<WellDto> ListAsDtos(ZybachDbContext dbContext)
        {
            return dbContext.Wells.AsNoTracking().Select(x => x.AsDto()).ToList();
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
                .AsNoTracking();
        }

        private static WellWithSensorSummaryDto WellWithSensorSummaryDtoFromWell(Well well)
        {
            var wellWithSensorSummaryDto = new WellWithSensorSummaryDto();
            wellWithSensorSummaryDto.WellRegistrationID = well.WellRegistrationID;
            wellWithSensorSummaryDto.Location = new Feature(new Point(new Position(well.WellGeometry.Coordinate.Y, well.WellGeometry.Coordinate.X)));
            wellWithSensorSummaryDto.FetchDate = well.LastUpdateDate;
            wellWithSensorSummaryDto.InGeoOptix = well.GeoOptixWell != null;

            var sensors = well.Sensors.Select(x => new SensorSummaryDto()
            {
                SensorName = x.SensorName,
                SensorType = x.SensorType.SensorTypeDisplayName,
                WellRegistrationID = well.WellRegistrationID
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