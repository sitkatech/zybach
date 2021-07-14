using System.Collections.Generic;
using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class AgHubWell
    {
        public static List<AgHubWellDto> List(ZybachDbContext dbContext)
        {
            return dbContext.AgHubWells.Select(x => AgHubWellExtensionMethods.AsDto(x)).ToList();
        }

    }
}