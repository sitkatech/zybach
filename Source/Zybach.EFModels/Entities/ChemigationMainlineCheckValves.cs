using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationMainlineCheckValves
    {
        public static IEnumerable<ChemigationMainlineCheckValveDto> List(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationMainlineCheckValves.AsNoTracking().Select(x => x.AsDto()).ToList();
        }
    }
}