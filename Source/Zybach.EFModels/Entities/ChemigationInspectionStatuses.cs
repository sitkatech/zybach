using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationInspectionStatuses
    {
        public static IEnumerable<ChemigationInspectionStatusDto> List(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationInspectionStatuses
                .AsNoTracking()
                .Select(x => x.AsDto()).ToList();
        }
    }
}