using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationInspectionStatuses
    {
        public enum ChemigationInspectionStatusEnum
        {
            Pending = 1,
            Pass = 2,
            Fail = 3
        }

        public static IEnumerable<ChemigationInspectionStatusDto> ListAsDto(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationInspectionStatuses
                .AsNoTracking()
                .OrderBy(x => x.ChemigationInspectionStatusName)
                .Select(x => x.AsDto()).ToList();
        }
    }
}