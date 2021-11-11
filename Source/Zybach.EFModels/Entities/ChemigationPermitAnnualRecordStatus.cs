using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermitAnnualRecord
    {
        public enum ChemigationPermitAnnualRecordStatusEnum
        {
            PendingPayment = 1,
            ReadyForReview = 2,
            PendingInspection = 3,
            Approved = 4
        }

        public static IEnumerable<ChemigationPermitAnnualRecordStatusDto> List(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationPermitAnnualRecordStatuses
                .AsNoTracking()
                .Select(x => x.AsDto()).ToList();
        }
    }
}
