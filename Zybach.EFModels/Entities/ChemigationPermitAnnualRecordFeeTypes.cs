using System.Collections.Generic;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public class ChemigationPermitAnnualRecordFeeTypes
    {
        public enum ChemigationPermitAnnualRecordFeeTypeEnum
        {
            New = 1,
            Renewal = 2,
            Emergency = 3
        }

        public static IEnumerable<ChemigationPermitAnnualRecordFeeTypeDto> List(ZybachDbContext dbContext)
        {
            return ChemigationPermitAnnualRecordFeeType.AllAsDto;
        }
    }
}
