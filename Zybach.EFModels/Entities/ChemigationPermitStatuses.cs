using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationPermitStatuses
    {
        public static IEnumerable<ChemigationPermitStatusDto> ListAsDto(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationPermitStatuses
                .AsNoTracking()
                .Select(x => x.AsDto()).ToList();
        }

    }
}
