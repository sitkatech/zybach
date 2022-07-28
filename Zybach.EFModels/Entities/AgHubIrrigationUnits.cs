using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.EFModels.Util;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class AgHubIrrigationUnits
    {
        public static IEnumerable<AgHubIrrigationUnitSimpleDto> ListAsSimpleDto(ZybachDbContext dbContext)
        {
            return dbContext.AgHubIrrigationUnits
                .Include(x => x.AgHubWells)
                    .ThenInclude(x => x.Well)
                    .ThenInclude(x => x.Sensors)
                .OrderBy(x => x.WellTPID)
                .Select(x => AgHubIrrigationUnitAsSimpleDto(x))
                .ToList();
        }

        public static AgHubIrrigationUnitSimpleDto AgHubIrrigationUnitAsSimpleDto(AgHubIrrigationUnit irrigationUnit)
        {
            var irrigationUnitSimpleDto = irrigationUnit.AsSimpleDto();
            var associatedWells = irrigationUnit.AgHubWells.Select(x => x.Well.AsMinimalDto()).ToList();
            irrigationUnitSimpleDto.AssociatedWells = associatedWells;

            return irrigationUnitSimpleDto;
        }

        public static AgHubIrrigationUnitDetailDto AgHubIrrigationUnitAsDetailDto(AgHubIrrigationUnit irrigationUnit)
        {
            var associatedWells = irrigationUnit.AgHubWells.Select(x => x.Well.AsMinimalDto()).ToList();

            var waterYearMonthETAndPrecipData = irrigationUnit.AgHubIrrigationUnitWaterYearMonthPrecipitationData
                .Join(irrigationUnit.AgHubIrrigationUnitWaterYearMonthETData,
                    pr => new { pr.WaterYearMonthID, pr.AgHubIrrigationUnitID },
                    et => new { et.WaterYearMonthID, et.AgHubIrrigationUnitID },
                    (pr, et) => new AgHubIrrigationUnitWaterYearMonthETAndPrecipDatumDto
                    {
                        WaterYearMonth = et.WaterYearMonth.AsDto(),
                        EvapotranspirationInches = et.EvapotranspirationInches,
                        EvapotranspirationAcreInches = et.EvapotranspirationAcreInches,
                        PrecipitationInches = pr.PrecipitationInches,
                        PrecipitationAcreInches = pr.PrecipitationAcreInches
                    })
                .OrderByDescending(x => x.WaterYearMonth.Year)
                .ThenByDescending(x => x.WaterYearMonth.Month)
                .ToList();
            
            var agHubIrrigationUnitDetailDto = new AgHubIrrigationUnitDetailDto
            {
                AgHubIrrigationUnitID = irrigationUnit.AgHubIrrigationUnitID,
                WellTPID = irrigationUnit.WellTPID,
                IrrigationUnitAreaInAcres = irrigationUnit.IrrigationUnitAreaInAcres,
                AssociatedWells = associatedWells,
                IrrigationUnitGeoJSON = irrigationUnit.AgHubIrrigationUnitGeometry != null ? 
                    GeoJsonHelpers.GetGeoJsonFromGeometry(irrigationUnit.AgHubIrrigationUnitGeometry.IrrigationUnitGeometry) : null,
                WaterYearMonthETAndPrecipData = waterYearMonthETAndPrecipData
            };
            
            return agHubIrrigationUnitDetailDto;
        }

        public static IQueryable<AgHubIrrigationUnit> GetAgHubIrrigationUnitImpl(ZybachDbContext dbContext)
        {
            return dbContext.AgHubIrrigationUnits
                .Include(x => x.AgHubIrrigationUnitGeometry)
                .Include(x => x.AgHubIrrigationUnitWaterYearMonthETData)
                    .ThenInclude(x => x.WaterYearMonth)
                .Include(x => x.AgHubIrrigationUnitWaterYearMonthPrecipitationData)
                    .ThenInclude(x => x.WaterYearMonth)
                .Include(x => x.AgHubWells)
                    .ThenInclude(x => x.Well)
                    .ThenInclude(x => x.Sensors)
                .AsNoTracking();
        }

        public static List<RobustReviewDto> GetRobustReviewDtos(ZybachDbContext dbContext)
        {
            var agHubIrrigationUnits = GetAgHubIrrigationUnitImpl(dbContext).ToList();
            var firstReadingDateTimes = WellSensorMeasurements.GetFirstReadingDateTimes(dbContext);
            var robustReviewDtos = agHubIrrigationUnits.Select(x => AgHubIrrigationUnitAsRobustReviewDto(x, firstReadingDateTimes, dbContext)).ToList();
            return robustReviewDtos.Where(x => x != null).ToList();
        }

        public static RobustReviewDto AgHubIrrigationUnitAsRobustReviewDto(AgHubIrrigationUnit irrigationUnit, Dictionary<string, DateTime> firstReadingDateTimes, ZybachDbContext dbContext)
        {
            var associatedWells = irrigationUnit.AgHubWells.Select(x => x.Well).ToList();
            foreach (var well in associatedWells)
            {
                if (!firstReadingDateTimes.ContainsKey(well.WellRegistrationID))
                {
                    return null;
                }
            }

            var monthlyPumpedVolumes = CreateAggregatedMonthlyPumpedVolumeByWells(associatedWells, dbContext);

            var waterYearMonthETAndPrecipData = irrigationUnit.AgHubIrrigationUnitWaterYearMonthPrecipitationData
                .Join(irrigationUnit.AgHubIrrigationUnitWaterYearMonthETData,
                    pr => new { pr.WaterYearMonthID, pr.AgHubIrrigationUnitID },
                    et => new { et.WaterYearMonthID, et.AgHubIrrigationUnitID },
                    (pr, et) => new AgHubIrrigationUnitWaterYearMonthETAndPrecipDatumDto
                    {
                        WaterYearMonth = et.WaterYearMonth.AsDto(),
                        EvapotranspirationInches = et.EvapotranspirationInches,
                        EvapotranspirationAcreInches = et.EvapotranspirationAcreInches,
                        PrecipitationInches = pr.PrecipitationInches,
                        PrecipitationAcreInches = pr.PrecipitationAcreInches
                    })
                .OrderByDescending(x => x.WaterYearMonth.Year)
                .ThenByDescending(x => x.WaterYearMonth.Month)
                .ToList();
            //WIP
            var associatedAgHubWellIDs = irrigationUnit.AgHubWells.Select(x => x.AgHubWellID).ToList();

            var irrigatedAcres = dbContext.AgHubWellIrrigatedAcres
                .Where(x => associatedAgHubWellIDs.Contains(x.AgHubWellID))
                .DistinctBy(x => x.IrrigationYear)
                .ToList()
                .Select(x => x.AsIrrigatedAcresPerYearDto())
                .ToList();

            var robustReviewDto = new RobustReviewDto
            {
                WellTPID = irrigationUnit.WellTPID,
                IrrigatedAcres = irrigatedAcres,
                MonthlyData = null
            };
            return null;
        }

        private static List<MonthlyPumpedVolume> CreateAggregatedMonthlyPumpedVolumeByWells(List<Well> wells, ZybachDbContext dbContext)
        {
            var sensorMeasurementDtos = new List<SensorMeasurementDto>();

            foreach (var well in wells)
            {
                var sensorType = well.AgHubWell.WellConnectedMeter
                    ? (int)SensorTypeEnum.ElectricalUsage
                    : (int)SensorTypeEnum.ContinuityMeter;
                sensorMeasurementDtos.AddRange(WellSensorMeasurements.GetWellSensorMeasurementsForWellAndSensorSimples(dbContext,
                    well.WellRegistrationID,
                    well.Sensors.Where(y => y.SensorTypeID == sensorType).Select(x => x.AsSimpleDto())));
            }
            
            var monthlyPumpedVolumes = sensorMeasurementDtos.GroupBy(x => x.MeasurementDate.ToString("yyyyMM"))
                .Select(x =>
                    new MonthlyPumpedVolume(x.First().MeasurementDate.Year, x.First().MeasurementDate.Month,
                        x.Sum(y => y.MeasurementValue ?? 0))).ToList();

            return monthlyPumpedVolumes;
        }
    }
}
