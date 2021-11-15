using System;
using System.Collections.Generic;
using Zybach.Models.DataTransferObjects;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermit
    {
        public static IEnumerable<ChemigationPermitDto> List(ZybachDbContext dbContext)
        {
            return GetChemigationPermitImpl(dbContext)
                .Select(x => x.AsDto()).ToList();
        }

        public static bool IsChemigationPermitNumberUnique(ZybachDbContext dbContext, int chemigationPermitNumber, int? currentID)
        {
            return dbContext.ChemigationPermits
                .Any(x => x.ChemigationPermitNumber == chemigationPermitNumber &&
                          (currentID == null || (
                              currentID != null && x.ChemigationPermitID != currentID)));
        }

        public static ChemigationPermitDto CreateNewChemigationPermit(ZybachDbContext dbContext, ChemigationPermitNewDto chemigationPermitNewDto)
        {
            if (chemigationPermitNewDto == null)
            {
                return null;
            }

            var chemigationPermit = new ChemigationPermit()
            {
                ChemigationPermitNumber = chemigationPermitNewDto.ChemigationPermitNumber,
                ChemigationPermitStatusID = chemigationPermitNewDto.ChemigationPermitStatusID,
                DateCreated = DateTime.Now.Date,
                TownshipRangeSection = chemigationPermitNewDto.TownshipRangeSection,
                ChemigationCounty = chemigationPermitNewDto.ChemigationCountyID
            };

            // when creating new permit, always create a default annual record as well
            var chemigationPermitAnnualRecord = new ChemigationPermitAnnualRecord()
            {
                ChemigationPermit = chemigationPermit,
                // default to pending payment status on new permits
                ChemigationPermitAnnualRecordStatusID = (int)ChemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusEnum.PendingPayment,
                ApplicantFirstName = chemigationPermitNewDto.ApplicantFirstName,
                ApplicantLastName = chemigationPermitNewDto.ApplicantLastName,
                ApplicantMailingAddress = chemigationPermitNewDto.ApplicantMailingAddress,
                ApplicantCity = chemigationPermitNewDto.ApplicantCity,
                ApplicantState = chemigationPermitNewDto.AppplicantState, 
                ApplicantZipCode = chemigationPermitNewDto.ApplicantZipCode,
                PivotName = chemigationPermitNewDto.PivotName,
                // TODO: determine what default behavior should be here: are we creating a record for the year in which the permit was received, or for the upcoming year?
                RecordYear = chemigationPermit.DateCreated.Year,
                DateReceived = chemigationPermit.DateCreated
            };

            dbContext.ChemigationPermits.Add(chemigationPermit);
            dbContext.ChemigationPermitAnnualRecords.Add(chemigationPermitAnnualRecord);
            dbContext.SaveChanges();

            dbContext.Entry(chemigationPermit).Reload();

            return GetChemigationPermitByID(dbContext, chemigationPermit.ChemigationPermitID);
        }

        public static ChemigationPermitDto GetChemigationPermitByID(ZybachDbContext dbContext, int chemigationPermitID)
        {
            var chemigationPermit = GetChemigationPermitImpl(dbContext)
                .SingleOrDefault(x => x.ChemigationPermitID == chemigationPermitID);

            return chemigationPermit?.AsDto();
        }

        public static ChemigationPermitDto GetChemigationPermitByNumber(ZybachDbContext dbContext, int chemigationPermitNumber)
        {
            var chemigationPermit = GetChemigationPermitImpl(dbContext)
                .SingleOrDefault(x => x.ChemigationPermitNumber == chemigationPermitNumber);

            return chemigationPermit?.AsDto();
        }

        private static IQueryable<ChemigationPermit> GetChemigationPermitImpl(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationPermits
                .Include(x => x.ChemigationPermitStatus)
                .AsNoTracking();
        }

        public static ChemigationPermitDto UpdateChemigationPermit(ZybachDbContext dbContext, ChemigationPermit chemigationPermit, ChemigationPermitUpsertDto chemigationPermitUpsertDto)
        {
            // null check occurs in calling endpoint method.
            chemigationPermit.ChemigationPermitNumber = chemigationPermitUpsertDto.ChemigationPermitNumber;
            chemigationPermit.ChemigationPermitStatusID = chemigationPermitUpsertDto.ChemigationPermitStatusID;
            chemigationPermit.TownshipRangeSection = chemigationPermitUpsertDto.TownshipRangeSection;
            chemigationPermit.ChemigationCounty = chemigationPermitUpsertDto.ChemigationCountyID;

            dbContext.SaveChanges();
            dbContext.Entry(chemigationPermit).Reload();
            return GetChemigationPermitByID(dbContext, chemigationPermit.ChemigationPermitID);
        }

        public static void DeleteByChemigationPermitID(ZybachDbContext dbContext, int chemigationPermitID)
        {
            var chemigationPermitToRemove = dbContext.ChemigationPermits.SingleOrDefault(x => x.ChemigationPermitID == chemigationPermitID);

            if (chemigationPermitToRemove != null)
            {
                dbContext.ChemigationPermits.Remove(chemigationPermitToRemove);
                dbContext.SaveChanges();
            }
        }

    }
}