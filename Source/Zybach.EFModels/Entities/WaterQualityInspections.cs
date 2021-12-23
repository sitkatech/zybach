using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class WaterQualityInspections
    {
        public static IQueryable<WaterQualityInspection> ListImpl(ZybachDbContext dbContext)
        {
            return dbContext.WaterQualityInspections
                .Include(x => x.Well)
                .Include(x => x.CropType)
                .Include(x => x.InspectorUser)
                .ThenInclude(x => x.Role)
                .AsNoTracking();
        }

        public static List<WaterQualityInspectionSimpleDto> ListAsSimpleDto(ZybachDbContext dbContext)
        {
            return ListImpl(dbContext).Select(x => x.AsSimpleDto()).ToList();
        }
    }
}