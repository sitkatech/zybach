using System.Collections.Generic;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class WaterQualityInspectionTypes
    {
        public static IEnumerable<WaterQualityInspectionTypeDto> ListAsDto(ZybachDbContext dbContext)
        {
            return WaterQualityInspectionType.AllAsDto;
        }
    }
}