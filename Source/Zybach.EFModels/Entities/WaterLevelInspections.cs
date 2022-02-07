using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class WaterLevelInspections
    {
        public static IQueryable<WaterLevelInspection> ListImpl(ZybachDbContext dbContext)
        {
            return dbContext.WaterLevelInspections
                .Include(x => x.Well).ThenInclude(x => x.WellParticipation)
                .Include(x => x.CropType)
                .Include(x => x.InspectorUser)
                .ThenInclude(x => x.Role)
                .AsNoTracking();
        }

        public static List<WaterLevelInspectionSimpleDto> ListAsSimpleDto(ZybachDbContext dbContext)
        {
            return ListImpl(dbContext).OrderByDescending(x => x.InspectionDate).ThenBy(x => x.Well.WellRegistrationID).Select(x => x.AsSimpleDto()).ToList();
        }

        public static WaterLevelInspectionSimpleDto GetByIDAsSimpleDto(ZybachDbContext dbContext, int waterLevelInspectionID)
        {
            return ListImpl(dbContext).SingleOrDefault(x => x.WaterLevelInspectionID == waterLevelInspectionID)?.AsSimpleDto();
        }

        public static WaterLevelInspection GetByID(ZybachDbContext dbContext, int waterLevelInspectionID)
        {
            return dbContext.WaterLevelInspections.SingleOrDefault(x => x.WaterLevelInspectionID == waterLevelInspectionID);
        }

        public static List<WaterLevelInspectionSummaryDto> ListByWellIDAsSummaryDto(ZybachDbContext dbContext, int wellID)
        {
            return dbContext.WaterLevelInspections
                .AsNoTracking()
                .OrderByDescending(x => x.InspectionDate)
                .Where(x => x.WellID == wellID)
                .Select(x => x.AsSummaryDto())
                .ToList();
        }

        public static WaterLevelInspectionSimpleDto Create(ZybachDbContext dbContext, WaterLevelInspectionUpsertDto waterLevelInspectionUpsertDto, int wellID)
        {
            var waterLevelInspection = new WaterLevelInspection()
            {
                WellID = wellID,
                InspectionDate = waterLevelInspectionUpsertDto.InspectionDate.AddHours(8),
                InspectorUserID = waterLevelInspectionUpsertDto.InspectorUserID,
                Measurement = waterLevelInspectionUpsertDto.Measurement,
                HasOil = waterLevelInspectionUpsertDto.HasOil,
                HasBrokenTape = waterLevelInspectionUpsertDto.HasBrokenTape,
                InspectionNotes = waterLevelInspectionUpsertDto.InspectionNotes
            };

            dbContext.WaterLevelInspections.Add(waterLevelInspection);
            dbContext.SaveChanges();
            dbContext.Entry(waterLevelInspection).Reload();
            return GetByIDAsSimpleDto(dbContext, waterLevelInspection.WaterLevelInspectionID);
        }

        public static void Update(ZybachDbContext dbContext, WaterLevelInspection waterLevelInspection, WaterLevelInspectionUpsertDto waterLevelInspectionUpsertDto, int wellID)
        {
            waterLevelInspection.WellID = wellID;
            waterLevelInspection.InspectionDate = waterLevelInspectionUpsertDto.InspectionDate;
            waterLevelInspection.InspectorUserID = waterLevelInspectionUpsertDto.InspectorUserID;
            waterLevelInspection.Measurement = waterLevelInspectionUpsertDto.Measurement;
            waterLevelInspection.HasOil = waterLevelInspectionUpsertDto.HasOil;
            waterLevelInspection.HasBrokenTape = waterLevelInspectionUpsertDto.HasBrokenTape;
            waterLevelInspection.InspectionNotes = waterLevelInspectionUpsertDto.InspectionNotes;

            dbContext.SaveChanges();
        }
    }
}