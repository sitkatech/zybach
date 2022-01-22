using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationPermitStatuses
    {

        public enum ChemigationPermitStatusEnum
        {
            Active = 1,
            Inactive = 2,
            PermanentlyInactive = 3
        }

        public static IEnumerable<ChemigationPermitStatusDto> ListAsDto(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationPermitStatuses
                .AsNoTracking()
                .Select(x => x.AsDto()).ToList();
        }

    }
}
