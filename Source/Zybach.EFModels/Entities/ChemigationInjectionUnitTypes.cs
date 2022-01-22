using System.Collections.Generic;
using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationInjectionUnitTypes
    {
        public static List<ChemigationInjectionUnitTypeDto> ListAsDto(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationInjectionUnitTypes
                .Select(x => x.AsDto()).ToList();
        }
    }
}