using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermitAnnualRecord
    {
        public static List<ChemigationPermitAnnualRecordDto> GetChemigationPermitAnnualRecordsByID(ZybachDbContext dbContext, int chemigationPermitID)
        {
            return dbContext.ChemigationPermitAnnualRecords
                .Include(x => x.ChemigationPermitAnnualRecordStatus)
                .AsNoTracking()
                .Where(x => x.ChemigationPermitID == chemigationPermitID)
                .Select(x => x.AsDto()).ToList();
        }

        public static ChemigationPermitAnnualRecordDto CreateAnnualRecord(ChemigationPermitAnnualRecordUpsertDto chemigationPermitAnnualRecordUpsertDto)
        {
            throw new System.NotImplementedException();
        }
    }
}
