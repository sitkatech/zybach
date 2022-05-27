using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class WaterLevelMeasuringEquipments
    {
        public static IEnumerable<WaterLevelMeasuringEquipmentDto> ListAsDto(ZybachDbContext dbContext)
        {
            return dbContext.WaterLevelMeasuringEquipments
                .AsNoTracking()
                .Select(x => x.AsDto()).ToList();
        }
    }
}
