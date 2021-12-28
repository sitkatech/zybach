using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class CropTypes
    {

        public static IEnumerable<CropTypeDto> List(ZybachDbContext dbContext)
        {
            return dbContext.CropTypes.AsNoTracking().Select(x => x.AsDto()).ToList();
        }
    }
}