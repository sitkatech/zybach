using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationLowPressureValves
    {
        public static IEnumerable<ChemigationLowPressureValveDto> List(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationLowPressureValves.AsNoTracking().Select(x => x.AsDto()).ToList();
        }
    }
}