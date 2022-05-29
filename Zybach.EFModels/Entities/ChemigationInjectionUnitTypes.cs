using System.Collections.Generic;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationInjectionUnitTypes
    {
        public static List<ChemigationInjectionUnitTypeDto> ListAsDto(ZybachDbContext dbContext)
        {
            return ChemigationInjectionUnitType.AllAsDto;
        }
    }
}