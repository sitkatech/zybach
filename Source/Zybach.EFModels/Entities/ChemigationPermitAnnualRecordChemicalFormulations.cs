﻿using System.Collections.Generic;
using System.Linq;
using Zybach.API.Util;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationPermitAnnualRecordChemicalFormulations
    {
        public static void UpdateChemicalFormulations(ZybachDbContext dbContext, int chemigationPermitAnnualRecordID,
            List<ChemigationPermitAnnualRecordChemicalFormulationSimpleDto>
                chemigationPermitAnnualRecordChemicalFormulationsDto)
        {
            var newChemigationPermitAnnualRecordChemicalFormulations =
                chemigationPermitAnnualRecordChemicalFormulationsDto.Select(x =>
                    new ChemigationPermitAnnualRecordChemicalFormulation
                    {
                        ChemigationPermitAnnualRecordID =
                            chemigationPermitAnnualRecordID,
                        ChemicalFormulationID = x.ChemicalFormulationID,
                        ChemicalUnitID = x.ChemicalUnitID,
                        TotalApplied = x.TotalApplied
                    }).ToList();
            var existingChemigationPermitAnnualRecordChemicalFormulations = dbContext
                .ChemigationPermitAnnualRecordChemicalFormulations.Where(x =>
                    x.ChemigationPermitAnnualRecordID ==
                    chemigationPermitAnnualRecordID)
                .ToList();
            existingChemigationPermitAnnualRecordChemicalFormulations.Merge(
                newChemigationPermitAnnualRecordChemicalFormulations,
                dbContext.ChemigationPermitAnnualRecordChemicalFormulations,
                (x, y) =>
                    x.ChemigationPermitAnnualRecordID == y.ChemigationPermitAnnualRecordID &&
                    x.ChemicalFormulationID == y.ChemicalFormulationID && x.ChemicalUnitID == y.ChemicalUnitID,
                (x, y) => { x.TotalApplied = y.TotalApplied; });
        }
    }
}