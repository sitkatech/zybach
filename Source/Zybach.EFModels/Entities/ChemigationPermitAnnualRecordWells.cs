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
                var wellsSelectedDictionary = dbContext.Wells
                    .Where(x => wellRegistrationIDsSelected.Contains(x.WellRegistrationID))
                    .ToDictionary(x => x.WellRegistrationID, x => x.WellID);

                var newChemigationPermitAnnualRecordWells =
                    chemigationPermitAnnualRecordWellsDto.Select(x =>
                        new ChemigationPermitAnnualRecordWell
                        {
                            ChemigationPermitAnnualRecordID =
                                chemigationPermitAnnualRecordID,
                            WellID = wellsSelectedDictionary[x.WellRegistrationID]
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