using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermitAnnualRecord
    {
        public static class NDEEAmountEnum
        {
            public const decimal New = 5.00m;
            public const decimal Renewal = 2.00m;
        }

        public static List<ChemigationInjectionUnitTypeDto> GetChemigationInjectionUnitTypes(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationInjectionUnitTypes
                .Select(x => x.AsDto()).ToList();
        }

        private static IQueryable<ChemigationPermitAnnualRecord> GetChemigationPermitAnnualRecordsImpl(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationPermitAnnualRecords
                .Include(x => x.ChemigationPermit)
                    .ThenInclude(x => x.ChemigationPermitStatus)
                .Include(x => x.ChemigationPermit)
                    .ThenInclude(x => x.ChemigationCounty)
                .Include(x => x.ChemigationPermitAnnualRecordStatus)
                .Include(x => x.ChemigationInjectionUnitType)
                .Include(x => x.ChemigationPermitAnnualRecordChemicalFormulations).ThenInclude(x => x.ChemicalUnit)
                .Include(x => x.ChemigationPermitAnnualRecordChemicalFormulations).ThenInclude(x => x.ChemicalFormulation)
                .Include(x => x.ChemigationPermitAnnualRecordApplicators)
                .Include(x => x.ChemigationPermitAnnualRecordWells).ThenInclude(x => x.Well)
                .AsNoTracking();
        }

        public static List<ChemigationPermitAnnualRecordDto> GetAllChemigationPermitAnnualRecords(ZybachDbContext dbContext)
        {
            return GetChemigationPermitAnnualRecordsImpl(dbContext)
                .Select(x => x.AsDto()).ToList();
        }

        public static int GetYearOfMostRecentChemigationPermitAnnualRecordByPermitID(ZybachDbContext dbContext,
            int chemigationPermitID)
        {
            return GetChemigationPermitAnnualRecordsImpl(dbContext)
                .SingleOrDefault(x => x.ChemigationPermitID == chemigationPermitID)
                .RecordYear;
        }

        public static ChemigationPermitAnnualRecordDto GetChemigationPermitAnnualRecordByID(ZybachDbContext dbContext, int chemigationPermitAnnualRecordID)
        {
            var chemigationPermitAnnualRecord = GetChemigationPermitAnnualRecordsImpl(dbContext)
                .SingleOrDefault(x => x.ChemigationPermitAnnualRecordID == chemigationPermitAnnualRecordID);
            return chemigationPermitAnnualRecord?.AsDto();
        }

        public static ChemigationPermitAnnualRecordDto CreateAnnualRecord(ZybachDbContext dbContext, ChemigationPermitAnnualRecordUpsertDto chemigationPermitAnnualRecordUpsertDto, int chemigationPermitID)
        {
            if (chemigationPermitAnnualRecordUpsertDto == null)
            {
                return null;
            }

            var chemigationPermitAnnualRecord = CreateAnnualRecordImpl(dbContext, chemigationPermitAnnualRecordUpsertDto, chemigationPermitID);

            dbContext.Entry(chemigationPermitAnnualRecord).Reload();
            return GetChemigationPermitAnnualRecordByID(dbContext, chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID);
        }

        public static ChemigationPermitAnnualRecord CreateAnnualRecordImpl(ZybachDbContext dbContext,
            ChemigationPermitAnnualRecordUpsertDto chemigationPermitAnnualRecordUpsertDto, int chemigationPermitID)
        {
            var chemigationPermitAnnualRecord = new ChemigationPermitAnnualRecord
            {
                ChemigationPermitID = chemigationPermitID
            };
            dbContext.ChemigationPermitAnnualRecords.Add(chemigationPermitAnnualRecord);

            UpdateFromDto(dbContext, chemigationPermitAnnualRecord, chemigationPermitAnnualRecordUpsertDto);
            Entities.ChemigationPermitAnnualRecordChemicalFormulations.UpdateChemicalFormulations(dbContext,
                chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID,
                chemigationPermitAnnualRecordUpsertDto.ChemicalFormulations);
            Entities.ChemigationPermitAnnualRecordApplicators.UpdateApplicators(dbContext,
                chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID,
                chemigationPermitAnnualRecordUpsertDto.Applicators);
            Entities.ChemigationPermitAnnualRecordWells.UpdateWells(dbContext,
                chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID, chemigationPermitAnnualRecordUpsertDto.Wells);
            return chemigationPermitAnnualRecord;
        }

        public static ChemigationPermitAnnualRecordDto UpdateAnnualRecord(ZybachDbContext dbContext, ChemigationPermitAnnualRecord chemigationPermitAnnualRecord, ChemigationPermitAnnualRecordUpsertDto chemigationPermitAnnualRecordUpsertDto)
        {
            UpdateFromDto(dbContext, chemigationPermitAnnualRecord, chemigationPermitAnnualRecordUpsertDto);
            Entities.ChemigationPermitAnnualRecordChemicalFormulations.UpdateChemicalFormulations(dbContext, chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID, chemigationPermitAnnualRecordUpsertDto.ChemicalFormulations);
            Entities.ChemigationPermitAnnualRecordApplicators.UpdateApplicators(dbContext, chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID, chemigationPermitAnnualRecordUpsertDto.Applicators);
            Entities.ChemigationPermitAnnualRecordWells.UpdateWells(dbContext, chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID, chemigationPermitAnnualRecordUpsertDto.Wells);

            dbContext.Entry(chemigationPermitAnnualRecord).Reload();
            return GetChemigationPermitAnnualRecordByID(dbContext, chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID);
        }

        private static void UpdateFromDto(ZybachDbContext dbContext,
            ChemigationPermitAnnualRecord chemigationPermitAnnualRecord,
            ChemigationPermitAnnualRecordUpsertDto chemigationPermitAnnualRecordUpsertDto)
        {
            chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordStatusID =
                chemigationPermitAnnualRecordUpsertDto.ChemigationPermitAnnualRecordStatusID;
            chemigationPermitAnnualRecord.ChemigationInjectionUnitTypeID =
                chemigationPermitAnnualRecordUpsertDto.ChemigationInjectionUnitTypeID;
            chemigationPermitAnnualRecord.ApplicantFirstName = chemigationPermitAnnualRecordUpsertDto.ApplicantFirstName;
            chemigationPermitAnnualRecord.ApplicantLastName = chemigationPermitAnnualRecordUpsertDto.ApplicantLastName;
            chemigationPermitAnnualRecord.ApplicantPhone = chemigationPermitAnnualRecordUpsertDto.ApplicantPhone;
            chemigationPermitAnnualRecord.ApplicantMobilePhone = chemigationPermitAnnualRecordUpsertDto.ApplicantMobilePhone;
            chemigationPermitAnnualRecord.ApplicantEmail = chemigationPermitAnnualRecordUpsertDto.ApplicantEmail;
            chemigationPermitAnnualRecord.ApplicantMailingAddress =
                chemigationPermitAnnualRecordUpsertDto.ApplicantMailingAddress;
            chemigationPermitAnnualRecord.ApplicantCity = chemigationPermitAnnualRecordUpsertDto.ApplicantCity;
            chemigationPermitAnnualRecord.ApplicantState = chemigationPermitAnnualRecordUpsertDto.ApplicantState;
            chemigationPermitAnnualRecord.ApplicantZipCode = chemigationPermitAnnualRecordUpsertDto.ApplicantZipCode;
            chemigationPermitAnnualRecord.PivotName = chemigationPermitAnnualRecordUpsertDto.PivotName;
            chemigationPermitAnnualRecord.RecordYear = chemigationPermitAnnualRecordUpsertDto.RecordYear;
            chemigationPermitAnnualRecord.NDEEAmount = chemigationPermitAnnualRecordUpsertDto.NDEEAmount;
            //TODO: find a better solution to correct date assignment
            chemigationPermitAnnualRecord.DatePaid = chemigationPermitAnnualRecordUpsertDto.DatePaid?.AddHours(8);
            chemigationPermitAnnualRecord.DateReceived = chemigationPermitAnnualRecordUpsertDto.DateReceived?.AddHours(8);
            dbContext.SaveChanges();
        }

        public static bool DoesChemigationPermitAnnualRecordExistForYear(ZybachDbContext dbContext, int chemigationPermitID, int year)
        {
            return GetChemigationPermitAnnualRecordsImpl(dbContext)
                .Any(x => x.ChemigationPermitID == chemigationPermitID && x.RecordYear == year);
        }

        public static List<ChemigationPermitAnnualRecordDetailedDto> GetLatestAsDetailedDto(ZybachDbContext dbContext)
        {
            return GetChemigationPermitAnnualRecordsImpl(dbContext).ToList().GroupBy(x => x.ChemigationPermitID).Select(x => x.OrderByDescending(y => y.RecordYear).First().AsDetailedDto()).ToList();
        }

        public static ChemigationPermitAnnualRecordDetailedDto GetLatestByChemigationPermitNumberAsDetailedDto(ZybachDbContext dbContext, int chemigationPermitNumber)
        {
            return ListByChemigationPermitNumber(dbContext, chemigationPermitNumber).OrderByDescending(x => x.RecordYear).FirstOrDefault()?.AsDetailedDto();
        }

        public static ChemigationPermitAnnualRecordDetailedDto GetByPermitNumberAndRecordYearAsDetailedDto(ZybachDbContext dbContext, int chemigationPermitNumber, int recordYear)
        {
            return GetChemigationPermitAnnualRecordsImpl(dbContext)
                .SingleOrDefault(x => x.ChemigationPermit.ChemigationPermitNumber == chemigationPermitNumber && x.RecordYear == recordYear)
                .AsDetailedDto();
        }

        public static IQueryable<ChemigationPermitAnnualRecord> ListByChemigationPermitNumber(ZybachDbContext dbContext, int chemigationPermitNumber)
        {
            return GetChemigationPermitAnnualRecordsImpl(dbContext).Where(x => x.ChemigationPermit.ChemigationPermitNumber == chemigationPermitNumber);
        }

        public static List<ChemigationPermitAnnualRecordDetailedDto> ListByChemigationPermitNumberAsDetailedDto(ZybachDbContext dbContext, int chemigationPermitNumber)
        {
            return ListByChemigationPermitNumber(dbContext, chemigationPermitNumber)
                .Select(x => x.AsDetailedDto()).ToList();
        }

        public static List<ChemigationPermitAnnualRecordDetailedDto> GetByWellRegistrationID(ZybachDbContext dbContext, string wellRegistrationID)
        {
            return GetChemigationPermitAnnualRecordsImpl(dbContext).Where(x => x.ChemigationPermitAnnualRecordWells.Any(y => y.Well.WellRegistrationID == wellRegistrationID)).Select(x => x.AsDetailedDto()).ToList();
        }
    }
}
