using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class WellParticipations
    {
        public static IEnumerable<WellParticipationDto> ListAsDto(ZybachDbContext dbContext)
        {
            return dbContext.WellParticipations
                .AsNoTracking()
                .OrderBy(x => x.WellParticipationDisplayName)
                .Select(x => x.AsDto()).ToList();
        }
    }
}
