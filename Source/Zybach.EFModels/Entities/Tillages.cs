using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class Tillages
    {
        public static IEnumerable<TillageDto> List(ZybachDbContext dbContext)
        {
            return dbContext.Tillages
                .AsNoTracking()
                .Select(x => x.AsDto()).ToList();
        }
    }
}