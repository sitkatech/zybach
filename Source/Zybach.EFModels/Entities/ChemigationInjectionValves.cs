using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationInjectionValves
    {

        public static IEnumerable<ChemigationInjectionValveDto> ListAsDto(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationInjectionValves.AsNoTracking().Select(x => x.AsDto()).ToList();
        }
    }
}