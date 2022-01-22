using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class WaterQualityInspectionTypes
    {
        public static IEnumerable<WaterQualityInspectionTypeDto> ListAsDto(ZybachDbContext dbContext)
        {
            return dbContext.WaterQualityInspectionTypes.AsNoTracking().Select(x => x.AsDto()).ToList();
        }
    }
}