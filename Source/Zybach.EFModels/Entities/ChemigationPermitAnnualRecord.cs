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

        public static IQueryable<ChemigationPermitAnnualRecord> GetChemigationPermitAnnualRecordsImpl(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationPermitAnnualRecords
                .Include(x => x.ChemigationPermit)
                    .ThenInclude(x => x.ChemigationPermitStatus)
                .Include(x => x.ChemigationPermit)
                    .ThenInclude(x => x.ChemigationCounty)
                .Include(x => x.ChemigationPermit)
                    .ThenInclude(x => x.Well)
                .Include(x => x.ChemigationPermitAnnualRecordStatus)
                .Include(x => x.ChemigationInjectionUnitType)
                .Include(x => x.ChemigationPermitAnnualRecordChemicalFormulations).ThenInclude(x => x.ChemicalUnit)
                .Include(x => x.ChemigationPermitAnnualRecordChemicalFormulations).ThenInclude(x => x.ChemicalFormulation)
                .Include(x => x.ChemigationPermitAnnualRecordApplicators)
                .AsNoTracking();
        }

        public static List<ChemigationPermitAnnualRecordDto> GetAllChemigationPermitAnnualRecords(ZybachDbContext dbContext)
        {
            return GetChemigationPermitAnnualRecordsImpl(dbContext)
                .Select(x => x.AsDto()).ToList();
        }

        public static ChemigationPermitAnnualRecordDto GetChemigationPermitAnnualRecordByID(ZybachDbContext dbContext, int chemigationPermitAnnualRecordID)
        {
            var chemigationPermitAnnualRecord = GetChemigationPermitAnnualRecordsImpl(dbContext)
                .SingleOrDefault(x => x.ChemigationPermitAnnualRecordID == chemigationPermitAnnualRecordID);
            return chemigationPermitAnnualRecord?.AsDto();
        }

        public static ChemigationPermitAnnualRecord CreateAnnualRecord(ZybachDbContext dbContext, ChemigationPermitAnnualRecordUpsertDto chemigationPermitAnnualRecordUpsertDto, int chemigationPermitID)
        {
            if (chemigationPermitAnnualRecordUpsertDto == null)
            {
                return null;
            }

            var chemigationPermitAnnualRecord = new ChemigationPermitAnnualRecord
            {
                ChemigationPermitID = chemigationPermitID
            };
            dbContext.ChemigationPermitAnnualRecords.Add(chemigationPermitAnnualRecord);

            UpdateAnnualRecord(dbContext, chemigationPermitAnnualRecord, chemigationPermitAnnualRecordUpsertDto);
            dbContext.Entry(chemigationPermitAnnualRecord).Reload();
            return chemigationPermitAnnualRecord;
        }

        public static void UpdateAnnualRecord(ZybachDbContext dbContext, ChemigationPermitAnnualRecord chemigationPermitAnnualRecord, ChemigationPermitAnnualRecordUpsertDto chemigationPermitAnnualRecordUpsertDto)
        {
            MapFromUpsertDto(chemigationPermitAnnualRecord, chemigationPermitAnnualRecordUpsertDto);
            if (chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID <= 0)
            {
                // we need to save the annual record first to get an ID for it
                dbContext.SaveChanges();
            }
            Entities.ChemigationPermitAnnualRecordChemicalFormulations.UpdateChemicalFormulations(dbContext, chemigationPermitAnnualRecord, chemigationPermitAnnualRecordUpsertDto.ChemicalFormulations);
            Entities.ChemigationPermitAnnualRecordApplicators.UpdateApplicators(dbContext, chemigationPermitAnnualRecord, chemigationPermitAnnualRecordUpsertDto.Applicators);
            dbContext.SaveChanges();
        }

        private static void MapFromUpsertDto(ChemigationPermitAnnualRecord chemigationPermitAnnualRecord,
            ChemigationPermitAnnualRecordUpsertDto chemigationPermitAnnualRecordUpsertDto)
        {
            chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordStatusID =
                chemigationPermitAnnualRecordUpsertDto.ChemigationPermitAnnualRecordStatusID;
            chemigationPermitAnnualRecord.ChemigationInjectionUnitTypeID =
                chemigationPermitAnnualRecordUpsertDto.ChemigationInjectionUnitTypeID;
            chemigationPermitAnnualRecord.ApplicantName = chemigationPermitAnnualRecordUpsertDto.ApplicantName;
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
            return GetChemigationPermitAnnualRecordsImpl(dbContext).Where(x => x.ChemigationPermit.Well != null && x.ChemigationPermit.Well.WellRegistrationID == wellRegistrationID).Select(x => x.AsDetailedDto()).ToList();
        }
    }
}
