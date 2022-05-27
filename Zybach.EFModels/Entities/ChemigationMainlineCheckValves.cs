using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationMainlineCheckValves
    {
        public static IEnumerable<ChemigationMainlineCheckValveDto> ListAsDto(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationMainlineCheckValves.AsNoTracking().OrderBy(x => x.ChemigationMainlineCheckValveName).Select(x => x.AsDto()).ToList();
        }
    }
}