using System.Collections.Generic;
using System.Linq;
using Zybach.API.Util;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationPermitAnnualRecordChemicalFormulations
    {
        public static void UpdateChemicalFormulations(ZybachDbContext dbContext,
            ChemigationPermitAnnualRecord chemigationPermitAnnualRecord,
            List<ChemigationPermitAnnualRecordChemicalFormulationUpsertDto>
                chemigationPermitAnnualRecordChemicalFormulationsDto)
        {
            if (chemigationPermitAnnualRecordChemicalFormulationsDto != null)
            {
                var newChemigationPermitAnnualRecordChemicalFormulations =
                    chemigationPermitAnnualRecordChemicalFormulationsDto.GroupBy(x => new {x.ChemicalFormulationID, x.ChemicalUnitID}).Select(x =>
                    {
                        var formulation = new ChemigationPermitAnnualRecordChemicalFormulation
                            {
                                ChemigationPermitAnnualRecordID = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID,
                                ChemicalFormulationID = x.Key.ChemicalFormulationID.Value,
                                ChemicalUnitID = x.Key.ChemicalUnitID.Value,
                                AcresTreated = x.Sum(y => y.AcresTreated)
                            };

                        var hasTotalAppliedValues = x.Where(y => y.TotalApplied.HasValue).ToList();
                        if (hasTotalAppliedValues.Any())
                        {
                            formulation.TotalApplied = hasTotalAppliedValues.Sum(y => y.TotalApplied.Value);
                        }
                        else
                        {
                            formulation.TotalApplied = null;
                        }
                        return formulation;
                    }).ToList();
                var existingChemigationPermitAnnualRecordChemicalFormulations = dbContext
                    .ChemigationPermitAnnualRecordChemicalFormulations.Where(x =>
                        x.ChemigationPermitAnnualRecordID ==
                        chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID)
                    .ToList();
                existingChemigationPermitAnnualRecordChemicalFormulations.Merge(
                    newChemigationPermitAnnualRecordChemicalFormulations,
                    dbContext.ChemigationPermitAnnualRecordChemicalFormulations,
                    (x, y) =>
                        x.ChemigationPermitAnnualRecordID == y.ChemigationPermitAnnualRecordID &&
                        x.ChemicalFormulationID == y.ChemicalFormulationID && x.ChemicalUnitID == y.ChemicalUnitID,
                    (x, y) =>
                    {
                        x.TotalApplied = y.TotalApplied;
                        x.AcresTreated = y.AcresTreated;
                    });
            }
        }
    }
}