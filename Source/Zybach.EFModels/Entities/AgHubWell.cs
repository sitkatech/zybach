using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class AgHubWell
    {
        public static List<AgHubWellDto> List(ZybachDbContext dbContext)
        {
            return dbContext.AgHubWells.AsNoTracking().Select(x => x.AsDto()).ToList();
        }

    }
}