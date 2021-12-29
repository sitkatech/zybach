using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class CropTypes
    {

        public static IEnumerable<CropTypeDto> ListAsDto(ZybachDbContext dbContext)
        {
            return dbContext.CropTypes.AsNoTracking().OrderBy(x => x.CropTypeName).Select(x => x.AsDto()).ToList();
        }
    }
}