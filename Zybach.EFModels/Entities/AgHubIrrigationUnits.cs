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
                IrrigationUnitGeoJSON = irrigationUnit.AgHubIrrigationUnitGeometries.Any() ? 
                    GeoJsonHelpers.GetGeoJsonFromGeometry(irrigationUnit.AgHubIrrigationUnitGeometries.First().IrrigationUnitGeometry) : null,
                WaterYearMonthETAndPrecipData = waterYearMonthETAndPrecipData
            };
            
            return agHubIrrigationUnitDetailDto;
        }

        public static IQueryable<AgHubIrrigationUnit> GetAgHubIrrigationUnitImpl(ZybachDbContext dbContext)
        {
            return dbContext.AgHubIrrigationUnits
                .Include(x => x.AgHubIrrigationUnitGeometries)
                .Include(x => x.AgHubIrrigationUnitWaterYearMonthETData)
                    .ThenInclude(x => x.WaterYearMonth)
                .Include(x => x.AgHubIrrigationUnitWaterYearMonthPrecipitationData)
                    .ThenInclude(x => x.WaterYearMonth)
                .Include(x => x.AgHubWells)
                    .ThenInclude(x => x.Well)
                    .ThenInclude(x => x.Sensors)
                .AsNoTracking();
        }

    }
}
