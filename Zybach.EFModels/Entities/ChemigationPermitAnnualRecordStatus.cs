using System.Collections.Generic;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermitAnnualRecordStatus
    {
        public static IEnumerable<ChemigationPermitAnnualRecordStatusDto> List(ZybachDbContext dbContext)
        {
            return ChemigationPermitAnnualRecordStatus.AllAsDto;
        }

    }
}
