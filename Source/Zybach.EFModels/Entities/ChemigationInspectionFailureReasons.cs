using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationInspectionFailureReasons
    {
        public static IEnumerable<ChemigationInspectionFailureReasonDto> List(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationInspectionFailureReasons
                .AsNoTracking()
                .Select(x => x.AsDto()).ToList();
        }
    }
}