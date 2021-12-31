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
    }
}