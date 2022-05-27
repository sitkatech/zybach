using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class Tillages
    {
        public static IEnumerable<TillageDto> ListAsDto(ZybachDbContext dbContext)
        {
            return dbContext.Tillages
                .AsNoTracking()
                .OrderBy(x => x.TillageName)
                .Select(x => x.AsDto()).ToList();
        }
    }
}