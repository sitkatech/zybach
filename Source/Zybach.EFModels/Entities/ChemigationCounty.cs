using System;
using System.Collections.Generic;
using Zybach.Models.DataTransferObjects;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationCounty
    {
        public static IEnumerable<ChemigationCountyDto> List(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationCounties
                .AsNoTracking().Select(x => x.AsDto()).ToList();
        }
    }
}


