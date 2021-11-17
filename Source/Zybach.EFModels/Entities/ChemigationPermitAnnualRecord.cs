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

        public static ChemigationPermitAnnualRecordDto CreateAnnualRecord(ZybachDbContext dbContext, ChemigationPermitAnnualRecordDto newChemigationPermitAnnualRecordDto)
        {
            if (newChemigationPermitAnnualRecordDto == null)
            {
                return null;
            }

            var chemigationPermitAnnualRecord = new ChemigationPermitAnnualRecord()
            {
                ChemigationPermitID = newChemigationPermitAnnualRecordDto.ChemigationPermit.ChemigationPermitID,
                // default to pending payment status on new records
                ChemigationPermitAnnualRecordStatusID = (int)ChemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusEnum.PendingPayment,
                ChemigationInjectionUnitTypeID = newChemigationPermitAnnualRecordDto.ChemigationInjectionUnitType.ChemigationInjectionUnitTypeID,
                ApplicantFirstName = newChemigationPermitAnnualRecordDto.ApplicantFirstName,
                ApplicantLastName = newChemigationPermitAnnualRecordDto.ApplicantLastName,
                ApplicantPhone = newChemigationPermitAnnualRecordDto.ApplicantPhone,
                ApplicantMobilePhone = newChemigationPermitAnnualRecordDto.ApplicantMobilePhone,
                ApplicantMailingAddress = newChemigationPermitAnnualRecordDto.ApplicantMailingAddress,
                ApplicantCity = newChemigationPermitAnnualRecordDto.ApplicantCity,
                ApplicantState = newChemigationPermitAnnualRecordDto.ApplicantState,
                ApplicantZipCode = newChemigationPermitAnnualRecordDto.ApplicantZipCode,
                PivotName = newChemigationPermitAnnualRecordDto.PivotName,
                RecordYear = newChemigationPermitAnnualRecordDto.RecordYear
            };

            dbContext.ChemigationPermitAnnualRecords.Add(chemigationPermitAnnualRecord);
            dbContext.SaveChanges();
            dbContext.Entry(chemigationPermitAnnualRecord).Reload();

            return GetChemigationPermitAnnualRecordByID(dbContext,
                chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID);
        }

        public static ChemigationPermitAnnualRecordDto UpdateAnnualRecord(ZybachDbContext dbContext, ChemigationPermitAnnualRecord chemigationPermitAnnualRecord, ChemigationPermitAnnualRecordDto chemigationPermitAnnualRecordUpsertDto)
        {
            chemigationPermitAnnualRecord.ChemigationPermitID = chemigationPermitAnnualRecordUpsertDto.ChemigationPermit.ChemigationPermitID;
            chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordStatusID = chemigationPermitAnnualRecordUpsertDto.ChemigationPermitAnnualRecordStatus.ChemigationPermitAnnualRecordStatusID;
            chemigationPermitAnnualRecord.ChemigationInjectionUnitTypeID = chemigationPermitAnnualRecordUpsertDto.ChemigationInjectionUnitType.ChemigationInjectionUnitTypeID;
            chemigationPermitAnnualRecord.ApplicantFirstName = chemigationPermitAnnualRecordUpsertDto.ApplicantFirstName;
            chemigationPermitAnnualRecord.ApplicantLastName = chemigationPermitAnnualRecordUpsertDto.ApplicantLastName;
            chemigationPermitAnnualRecord.ApplicantPhone = chemigationPermitAnnualRecordUpsertDto.ApplicantPhone;
            chemigationPermitAnnualRecord.ApplicantMobilePhone = chemigationPermitAnnualRecordUpsertDto.ApplicantMobilePhone;
            chemigationPermitAnnualRecord.ApplicantMailingAddress = chemigationPermitAnnualRecordUpsertDto.ApplicantMailingAddress;
            chemigationPermitAnnualRecord.ApplicantCity = chemigationPermitAnnualRecordUpsertDto.ApplicantCity;
            chemigationPermitAnnualRecord.ApplicantState = chemigationPermitAnnualRecordUpsertDto.ApplicantState;
            chemigationPermitAnnualRecord.ApplicantZipCode = chemigationPermitAnnualRecordUpsertDto.ApplicantZipCode;
            chemigationPermitAnnualRecord.PivotName = chemigationPermitAnnualRecordUpsertDto.PivotName;
            chemigationPermitAnnualRecord.RecordYear = chemigationPermitAnnualRecordUpsertDto.RecordYear;

            dbContext.SaveChanges();
            dbContext.Entry(chemigationPermitAnnualRecord).Reload();
            return GetChemigationPermitAnnualRecordByID(dbContext, chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID);
        }
    }
}
