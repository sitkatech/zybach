using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermitStatus
    {

        public enum PermitStatusEnum
        {
            Active = 1,
            Inactive = 2
        }

        public static IEnumerable<ChemigationPermitStatusDto> List(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationPermitStatuses
                .AsNoTracking()
                .Select(x => x.AsDto()).ToList();
        }

    }
}
