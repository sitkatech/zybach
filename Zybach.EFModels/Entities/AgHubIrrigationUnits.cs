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

            var waterYearMonthETAndPrecipData = irrigationUnit.AgHubIrrigationUnitOpenETData
                .GroupBy(x => x.ReportedDate)
                .OrderByDescending(x => x.Key)
                .Select(x =>
                {
                    var et = x.SingleOrDefault(x => x.OpenETDataTypeID == (int)OpenETDataTypeEnum.Evapotranspiration);
                    var pr = x.SingleOrDefault(x => x.OpenETDataTypeID == (int)OpenETDataTypeEnum.Precipitation);

                    return new AgHubIrrigationUnitWaterYearMonthETAndPrecipDatumDto
                    {
                        ReportedDate = x.Key,
                        EvapotranspirationInches = et?.ReportedValueInches,
                        EvapotranspirationAcreInches = et?.ReportedValueAcreInches,
                        PrecipitationInches = pr?.ReportedValueInches,
                        PrecipitationAcreInches = pr?.ReportedValueAcreInches
                    };
                }).ToList();
            
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
                .Include(x => x.AgHubIrrigationUnitOpenETData)
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
            var monthlyWaterVolumeSummaries = AgHubIrrigationUnitMonthlyWaterVolumeSummary.List(dbContext);
            var irrigatedAcres = dbContext.AgHubWellIrrigatedAcres
                .ToList();
            var robustReviewDtos = agHubIrrigationUnits.Select(x => AgHubIrrigationUnitAsRobustReviewDto(x, irrigatedAcres, monthlyWaterVolumeSummaries)).ToList();
            return robustReviewDtos.Where(x => x != null).ToList();
        }

        public static RobustReviewDto AgHubIrrigationUnitAsRobustReviewDto(AgHubIrrigationUnit irrigationUnit, List<AgHubWellIrrigatedAcre> irrigatedAcres, IEnumerable<AgHubIrrigationUnitMonthlyWaterVolumeSummary> monthlyWaterVolumeSummaries)
        {
            var associatedAgHubWellIDs = irrigationUnit.AgHubWells.Select(x => x.AgHubWellID).ToList();

            var unitIrrigatedAcres = irrigatedAcres
                .Where(x => associatedAgHubWellIDs.Contains(x.AgHubWellID))
                .GroupBy(x => x.IrrigationYear).Select(x => x.First())
                .Select(x => x.AsIrrigatedAcresPerYearDto())
                .OrderByDescending(x => x.Year)
                .ToList();

            var robustReviewDto = new RobustReviewDto
            {
                WellTPID = irrigationUnit.WellTPID,
                IrrigatedAcres = unitIrrigatedAcres,
                MonthlyData = monthlyWaterVolumeSummaries
                    .Where(x => x.AgHubIrrigationUnitID == irrigationUnit.AgHubIrrigationUnitID)
                    .OrderByDescending(x => x.Year)
                    .ThenByDescending(x => x.Month)
                    .Select(x => new MonthlyWaterVolumeDto
                    {
                        Month = x.Month,
                        Year = x.Year,
                        OpenET = x.EvapotranspirationAcreFeet,
                        Precip = x.PrecipitationAcreFeet,
                        VolumePumped = x.PumpedVolumeAcreFeet
                    })
                    .ToList()
            };
            return robustReviewDto;
        }

        public static List<AgHubIrrigationUnitFarmingPracticeDto> ListAsAgHubIrrigationUnitWellIrrigatedAcreDtos(ZybachDbContext dbContext)
        {
            var wellsByIrrigationUnitID = dbContext.AgHubIrrigationUnits.AsNoTracking()
                .Include(x => x.AgHubWells).ThenInclude(x => x.Well)
                .ToDictionary(x => x.AgHubIrrigationUnitID, x => x.AgHubWells);

            var irrigationUnitFarmingPracticeDtos =  dbContext.vGeoServerAgHubIrrigationUnits.AsNoTracking()
                .Select(x => new AgHubIrrigationUnitFarmingPracticeDto()
                {
                    AgHubIrrigationUnitID = x.AgHubIrrigationUnitID,
                    WellTPID = x.WellTPID,
                    IrrigationYear = x.IrrigationYear,
                    Acres = x.Acres,
                    CropType = x.CropType,
                    CropTypeLegendDisplayName = x.CropTypeLegendDisplayName,
                    CropTypeMapColor = x.CropTypeMapColor,
                    Tillage = x.Tillage,
                    TillageTypeLegendDisplayName = x.TillageTypeLegendDisplayName,
                    TillageTypeMapColor = x.TillageTypeMapColor
                }).ToList();

            irrigationUnitFarmingPracticeDtos.ForEach(x =>
            {
                if (wellsByIrrigationUnitID.ContainsKey(x.AgHubIrrigationUnitID))
                    x.Wells = wellsByIrrigationUnitID[x.AgHubIrrigationUnitID]
                        .Select(y => new WellLinkDto() { WellID = y.WellID, WellRegistrationID = y.Well.WellRegistrationID}).ToList();
            });

            return irrigationUnitFarmingPracticeDtos;
        }
    }
}
