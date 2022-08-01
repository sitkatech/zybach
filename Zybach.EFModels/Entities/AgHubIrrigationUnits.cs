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
                .AsNoTracking()
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
            var agHubIrrigationUnits = dbContext.AgHubIrrigationUnits
                .AsNoTracking()
                .OrderBy(x => x.WellTPID)
                .Include(x => x.AgHubWells)
                .ToList();
            var monthlyWaterVolumeSummaries = MonthlyWaterVolumeSummary.AggregateMonthlyWaterVolumesByIrrigationUnit(dbContext);
            var irrigatedAcres = dbContext.AgHubWellIrrigatedAcres
                .ToList();
            var robustReviewDtos = agHubIrrigationUnits.Select(x => AgHubIrrigationUnitAsRobustReviewDto(x, irrigatedAcres, monthlyWaterVolumeSummaries)).ToList();
            return robustReviewDtos.Where(x => x != null).ToList();
        }

        public static RobustReviewDto AgHubIrrigationUnitAsRobustReviewDto(AgHubIrrigationUnit irrigationUnit, List<AgHubWellIrrigatedAcre> irrigatedAcres, IEnumerable<MonthlyWaterVolumeSummary> monthlyWaterVolumeSummaries)
        {
            var associatedAgHubWellIDs = irrigationUnit.AgHubWells.Select(x => x.AgHubWellID).ToList();

            var unitIrrigatedAcres = irrigatedAcres
                .Where(x => associatedAgHubWellIDs.Contains(x.AgHubWellID))
                .GroupBy(x => x.IrrigationYear).Select(x => x.First())
                .Select(x => x.AsIrrigatedAcresPerYearDto())
                .OrderBy(x => x.Year)
                .ToList();

            var robustReviewDto = new RobustReviewDto
            {
                WellTPID = irrigationUnit.WellTPID,
                IrrigatedAcres = unitIrrigatedAcres,
                MonthlyData = monthlyWaterVolumeSummaries
                    .Where(x => x.AgHubIrrigationUnitID == irrigationUnit.AgHubIrrigationUnitID)
                    .OrderBy(x => x.Year)
                    .ThenBy(x => x.Month)
                    .Select(x => new MonthlyWaterVolumeDto
                    {
                        Month = x.Month,
                        Year = x.Year,
                        OpenET = x.EvapotranspirationAcreFeet,
                        Precip = x.PrecipitationAcreFeet,
                        VolumePumped = x.ElectricalUsagePumpedVolumeAcreFeet
                    })
                    .ToList()
            };
            return robustReviewDto;
        }

    }
}
