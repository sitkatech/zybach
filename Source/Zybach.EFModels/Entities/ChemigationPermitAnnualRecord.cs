using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class ChemigationPermitAnnualRecord
    {
        public static List<ChemigationInjectionUnitTypeDto> GetChemigationInjectionUnitTypes(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationInjectionUnitTypes
                .Select(x => x.AsDto()).ToList();
        }

        public static List<ChemigationPermitAnnualRecordDto> GetChemigationPermitAnnualRecordsByChemigationPermitID(ZybachDbContext dbContext, int chemigationPermitID)
        {
            return GetChemigationPermitAnnualRecordsImpl(dbContext)
                .Where(x => x.ChemigationPermitID == chemigationPermitID)
                .Select(x => x.AsDto()).ToList();
        }

        public static List<ChemigationPermitAnnualRecordDto> GetChemigationPermitAnnualRecordsByChemigationPermitNumber(ZybachDbContext dbContext, int chemigationPermitNumber)
        {
            return GetChemigationPermitAnnualRecordsImpl(dbContext)
                .Where(x => x.ChemigationPermit.ChemigationPermitNumber == chemigationPermitNumber)
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
                .AsNoTracking();
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

            var chemigationPermitAnnualRecord = new ChemigationPermitAnnualRecord()
            {
                ChemigationPermitID = chemigationPermitID
            };
            MapUpsertDtoToPOCO(chemigationPermitAnnualRecord, chemigationPermitAnnualRecordUpsertDto);
            

            dbContext.ChemigationPermitAnnualRecords.Add(chemigationPermitAnnualRecord);
            dbContext.SaveChanges();
            dbContext.Entry(chemigationPermitAnnualRecord).Reload();

            return GetChemigationPermitAnnualRecordByID(dbContext,
                chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID);
        }

        public static ChemigationPermitAnnualRecordDto UpdateAnnualRecord(ZybachDbContext dbContext, ChemigationPermitAnnualRecord chemigationPermitAnnualRecord, ChemigationPermitAnnualRecordUpsertDto chemigationPermitAnnualRecordUpsertDto)
        {
            //null check in endpoint method
            MapUpsertDtoToPOCO(chemigationPermitAnnualRecord, chemigationPermitAnnualRecordUpsertDto);
            dbContext.SaveChanges();
            dbContext.Entry(chemigationPermitAnnualRecord).Reload();
            return GetChemigationPermitAnnualRecordByID(dbContext, chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID);
        }

        private static void MapUpsertDtoToPOCO(ChemigationPermitAnnualRecord chemigationPermitAnnualRecord,
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
            //TODO: find a better solution to correct date assignment
            chemigationPermitAnnualRecord.DatePaid = chemigationPermitAnnualRecordUpsertDto.DatePaid?.AddHours(8);
            chemigationPermitAnnualRecord.DateReceived = chemigationPermitAnnualRecordUpsertDto.DateReceived?.AddHours(8);
        }

        public static bool DoesChemigationPermitAnnualRecordExistForYear(ZybachDbContext dbContext, int chemigationPermitID, int year)
        {
            return GetChemigationPermitAnnualRecordsImpl(dbContext)
                .Any(x => x.ChemigationPermitID == chemigationPermitID && x.RecordYear == year);
        }

        public static ChemigationPermitAnnualRecordDto GetLatestAnnualRecordByChemigationPermitNumber(ZybachDbContext dbContext, int chemigationPermitNumber)
        {
            var chemigationPermitAnnualRecords =
                GetChemigationPermitAnnualRecordsByChemigationPermitNumber(dbContext, chemigationPermitNumber);

            return chemigationPermitAnnualRecords
                .OrderByDescending(x => x.RecordYear)
                .FirstOrDefault();
        }

        public static ChemigationPermitAnnualRecordDto GetLatestAnnualRecordByChemigationPermitID(ZybachDbContext dbContext, int chemigationPermitID)
        {
            var chemigationPermitAnnualRecords =
                GetChemigationPermitAnnualRecordsByChemigationPermitID(dbContext, chemigationPermitID);

            return chemigationPermitAnnualRecords
                .OrderByDescending(x => x.RecordYear)
                .FirstOrDefault();
        }

        public static ChemigationPermitAnnualRecordDto GetAnnualRecordByPermitNumberAndRecordYear(ZybachDbContext dbContext, int chemigationPermitNumber, int recordYear)
        {
            var chemigationPermitID = dbContext.ChemigationPermits
                .SingleOrDefault(x => x.ChemigationPermitNumber == chemigationPermitNumber)
                .ChemigationPermitID;

            return GetAnnualRecordByPermitIDAndRecordYear(dbContext, chemigationPermitID, recordYear);
        }

        public static ChemigationPermitAnnualRecordDto GetAnnualRecordByPermitIDAndRecordYear(ZybachDbContext dbContext, int chemigationPermitID, int recordYear)
        {
            return GetChemigationPermitAnnualRecordsImpl(dbContext)
                .SingleOrDefault(x => x.ChemigationPermitID == chemigationPermitID && x.RecordYear == recordYear)
                .AsDto();
        }

    }
}
