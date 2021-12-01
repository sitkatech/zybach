using System.Collections.Generic;
using System.Linq;
using Zybach.API.Util;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationPermitAnnualRecordWells
    {
        public static void UpdateWells(ZybachDbContext dbContext, int chemigationPermitAnnualRecordID,
            List<ChemigationPermitAnnualRecordWellUpsertDto> chemigationPermitAnnualRecordWellsDto)
        {
            if (chemigationPermitAnnualRecordWellsDto != null && chemigationPermitAnnualRecordWellsDto.Any())
            {
                var wellRegistrationIDsSelected = chemigationPermitAnnualRecordWellsDto.Select(x => x.WellRegistrationID).Distinct().ToList();
                var wellIDsSelected = dbContext.Wells
                    .Where(x => wellRegistrationIDsSelected.Contains(x.WellRegistrationID))
                    .Select(x => x.WellID);

                var newChemigationPermitAnnualRecordWells =
                    wellIDsSelected.Select(x =>
                        new ChemigationPermitAnnualRecordWell
                        {
                            ChemigationPermitAnnualRecordID =
                                chemigationPermitAnnualRecordID,
                            WellID = x
                        }).ToList();
                var existingChemigationPermitAnnualRecordWells = dbContext
                    .ChemigationPermitAnnualRecordWells.Where(x =>
                        x.ChemigationPermitAnnualRecordID ==
                        chemigationPermitAnnualRecordID)
                    .ToList();
                existingChemigationPermitAnnualRecordWells.Merge(
                    newChemigationPermitAnnualRecordWells,
                    dbContext.ChemigationPermitAnnualRecordWells,
                    (x, y) =>
                        x.ChemigationPermitAnnualRecordID == y.ChemigationPermitAnnualRecordID &&
                        x.WellID == y.WellID);
                dbContext.SaveChanges();
            }
        }
    }
}