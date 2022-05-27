using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class WellUses
    {
        public static IEnumerable<WellUseDto> ListAsDto(ZybachDbContext dbContext)
        {
            return dbContext.WellUses
                .AsNoTracking()
                .OrderBy(x => x.WellUseDisplayName)
                .Select(x => x.AsDto()).ToList();
        }
    }
}
