using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationInspectionTypes
    {
        public static IEnumerable<ChemigationInspectionTypeDto> ListAsDto(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationInspectionTypes
                .AsNoTracking()
                .OrderBy(x => x.ChemigationInspectionTypeName)
                .Select(x => x.AsDto()).ToList();
        }

    }
}
