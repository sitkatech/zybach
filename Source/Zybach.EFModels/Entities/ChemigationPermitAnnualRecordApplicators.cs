﻿using System.Collections.Generic;
using System.Linq;
using Zybach.API.Util;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationPermitAnnualRecordApplicators
    {
        public static void UpdateApplicators(ZybachDbContext dbContext, int chemigationPermitAnnualRecordID,
            List<ChemigationPermitAnnualRecordApplicatorUpsertDto> chemigationPermitAnnualRecordApplicatorsDto)
        {
            if (chemigationPermitAnnualRecordApplicatorsDto != null && chemigationPermitAnnualRecordApplicatorsDto.Any())
            {
                var newChemigationPermitAnnualRecordApplicators =
                    chemigationPermitAnnualRecordApplicatorsDto.GroupBy(x => new { x.ApplicatorName, x.CertificationNumber }).Select(x =>
                        new ChemigationPermitAnnualRecordApplicator
                        {
                            ChemigationPermitAnnualRecordID =
                                chemigationPermitAnnualRecordID,
                            ApplicatorName = x.Key.ApplicatorName,
                            CertificationNumber = x.Key.CertificationNumber,
                            ExpirationYear = x.First().ExpirationYear,
                            HomePhone = x.First().HomePhone,
                            MobilePhone = x.First().MobilePhone,
                        }).ToList();
                var existingChemigationPermitAnnualRecordApplicators = dbContext
                    .ChemigationPermitAnnualRecordApplicators.Where(x =>
                        x.ChemigationPermitAnnualRecordID ==
                        chemigationPermitAnnualRecordID)
                    .ToList();
                existingChemigationPermitAnnualRecordApplicators.Merge(
                    newChemigationPermitAnnualRecordApplicators,
                    dbContext.ChemigationPermitAnnualRecordApplicators,
                    (x, y) =>
                        x.ChemigationPermitAnnualRecordID == y.ChemigationPermitAnnualRecordID &&
                        x.ApplicatorName == y.ApplicatorName && x.CertificationNumber == y.CertificationNumber,
                    (x, y) =>
                    {
                        x.ExpirationYear = y.ExpirationYear;
                        x.HomePhone = y.HomePhone;
                        x.MobilePhone = y.MobilePhone;
                    });
                dbContext.SaveChanges();
            }
        }
    }
}