using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationInspectionFailureReasons
    {
        public static IEnumerable<ChemigationInspectionFailureReasonDto> ListAsDto(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationInspectionFailureReasons
                .AsNoTracking()
                .OrderBy(x => x.ChemigationInspectionFailureReasonName)
                .Select(x => x.AsDto()).ToList();
        }
    }
}