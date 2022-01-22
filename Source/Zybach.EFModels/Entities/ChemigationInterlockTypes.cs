using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationInterlockTypes
    {
        public static IEnumerable<ChemigationInterlockTypeDto> ListAsDto(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationInterlockTypes
                .AsNoTracking()
                .OrderBy(x => x.ChemigationInterlockTypeName)
                .Select(x => x.AsDto()).ToList();
        }
    }
}
