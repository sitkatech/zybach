using System.Collections.Generic;
using Zybach.Models.DataTransferObjects;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    public static class Counties
    {
        public static IEnumerable<CountyDto> ListAsDto(ZybachDbContext dbContext)
        {
            return dbContext.Counties
                .AsNoTracking().Select(x => x.AsDto()).ToList();
        }
    }
}