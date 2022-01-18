using System;
using System.Collections.Generic;
using System.Data;
using Zybach.Models.DataTransferObjects;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    public static class ChemigationPermits
    {
        public static IEnumerable<ChemigationPermitDetailedDto> ListWithLatestAnnualRecordAsDto(ZybachDbContext dbContext)
        {
            var chemigationPermitAnnualRecordDetailedDtos = ChemigationPermitAnnualRecords.GetLatestAsDetailedDto(dbContext).ToDictionary(x => x.ChemigationPermit.ChemigationPermitID);
            var chemigationInspectionSimpleDtos = ChemigationInspections.GetChemigationInspectionsImpl(dbContext).ToList()
                .GroupBy(x => x.ChemigationPermitAnnualRecord.ChemigationPermitID).ToDictionary(x => x.Key, x =>
                    x.OrderByDescending(y => y.InspectionDate).ThenByDescending(y => y.ChemigationInspectionID).FirstOrDefault()?.AsSimpleDto());
            var listWithLatestAnnualRecordAsDto = GetChemigationPermitImpl(dbContext).ToList()
                .Select(x =>
                    x.AsDetailedDto(
                        chemigationPermitAnnualRecordDetailedDtos.ContainsKey(x.ChemigationPermitID) ? chemigationPermitAnnualRecordDetailedDtos[x.ChemigationPermitID] : null,
                        chemigationInspectionSimpleDtos.ContainsKey(x.ChemigationPermitID) ? chemigationInspectionSimpleDtos[x.ChemigationPermitID] : null))
                .OrderBy(x => x.ChemigationPermitNumber).ToList();
            return listWithLatestAnnualRecordAsDto;
        }

        public static IEnumerable<ChemigationPermitDetailedDto> GetDetailedDtosByListOfPermitIDs(ZybachDbContext dbContext, List<int> chemigationPermitIDs)
        {
            var chemigationPermitAnnualRecordDetailedDtos = ChemigationPermitAnnualRecords.GetLatestAsDetailedDto(dbContext).ToDictionary(x => x.ChemigationPermit.ChemigationPermitID);
            var chemigationInspectionSimpleDtos = ChemigationInspections.GetChemigationInspectionsImpl(dbContext).ToList()
                .GroupBy(x => x.ChemigationPermitAnnualRecord.ChemigationPermitID).ToDictionary(x => x.Key, x =>
                    x.OrderByDescending(y => y.InspectionDate).ThenByDescending(y => y.ChemigationInspectionID).FirstOrDefault()?.AsSimpleDto());
            var listWithLatestAnnualRecordAsDto = GetChemigationPermitImpl(dbContext)
                .Where(x => chemigationPermitIDs.Contains(x.ChemigationPermitID))
                .ToList()
                .Select(x =>
                    x.AsDetailedDto(
                        chemigationPermitAnnualRecordDetailedDtos.ContainsKey(x.ChemigationPermitID) ? chemigationPermitAnnualRecordDetailedDtos[x.ChemigationPermitID] : null,
                        chemigationInspectionSimpleDtos.ContainsKey(x.ChemigationPermitID) ? chemigationInspectionSimpleDtos[x.ChemigationPermitID] : null))
                .OrderBy(x => x.ChemigationPermitNumber).ToList();
            return listWithLatestAnnualRecordAsDto;
        }

        public static ChemigationPermitDto CreateNewChemigationPermit(ZybachDbContext dbContext, ChemigationPermitNewDto chemigationPermitNewDto)
        {
            if (chemigationPermitNewDto == null)
            {
                return null;
            }

            var nextPermitNumber = dbContext.ChemigationPermits.Max(x => x.ChemigationPermitNumber) + 1;
            var chemigationPermit = new ChemigationPermit()
            {
                ChemigationPermitNumber = nextPermitNumber,
                ChemigationPermitStatusID = chemigationPermitNewDto.ChemigationPermitStatusID,
                DateCreated = DateTime.Now.Date,
                CountyID = chemigationPermitNewDto.CountyID
            };
            var wellID = dbContext.Wells.SingleOrDefault(x => x.WellRegistrationID == chemigationPermitNewDto.WellRegistrationID)?.WellID;
            chemigationPermit.WellID = wellID;

            dbContext.ChemigationPermits.Add(chemigationPermit);
            dbContext.SaveChanges();

            var chemigationPermitID = chemigationPermit.ChemigationPermitID;

            chemigationPermitNewDto.ChemigationPermitAnnualRecord.NDEEAmount = ChemigationPermitAnnualRecords.NDEEAmounts.New;

            var chemigationPermitAnnualRecord = 
                ChemigationPermitAnnualRecords.CreateAnnualRecord(dbContext, chemigationPermitNewDto.ChemigationPermitAnnualRecord, chemigationPermitID);

            var chemigationPermitAnnualRecordID = chemigationPermitAnnualRecord.ChemigationPermitAnnualRecordID;

            ChemigationInspections.CreateDefaultNewChemigationInspection(dbContext,
                chemigationPermitAnnualRecordID);

            dbContext.Entry(chemigationPermit).Reload();

            return GetByIDAsDto(dbContext, chemigationPermitID);
        }

        public static ChemigationPermit GetByID(ZybachDbContext dbContext, int chemigationPermitID)
        {
            return dbContext.ChemigationPermits.SingleOrDefault(x => x.ChemigationPermitID == chemigationPermitID);
        }

        public static ChemigationPermitDto GetByIDAsDto(ZybachDbContext dbContext, int chemigationPermitID)
        {
            var chemigationPermit = GetChemigationPermitImpl(dbContext)
                .SingleOrDefault(x => x.ChemigationPermitID == chemigationPermitID);

            return chemigationPermit?.AsDto();
        }

        public static ChemigationPermitDto GetByPermitNumberAsDto(ZybachDbContext dbContext, int chemigationPermitNumber)
        {
            var chemigationPermit = GetChemigationPermitImpl(dbContext)
                .SingleOrDefault(x => x.ChemigationPermitNumber == chemigationPermitNumber);

            return chemigationPermit?.AsDto();
        }

        private static IQueryable<ChemigationPermit> GetChemigationPermitImpl(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationPermits
                .Include(x => x.ChemigationPermitStatus)
                .Include(x => x.County)
                .Include(x => x.Well)
                .AsNoTracking();
        }

        public static BulkChemigationPermitAnnualRecordCreationResult BulkCreateRenewalRecords(ZybachDbContext dbContext, int recordYear)
        {
            var sqlParameter = new SqlParameter("recordYear", recordYear);
            var chemigationPermitsRenewed = new SqlParameter("chemigationPermitsRenewed", SqlDbType.Int);
            chemigationPermitsRenewed.Direction = ParameterDirection.Output;
            var chemigationInspectionsCreated = new SqlParameter("chemigationInspectionsCreated", SqlDbType.Int);
            chemigationInspectionsCreated.Direction = ParameterDirection.Output;
            dbContext.Database.ExecuteSqlRaw("EXECUTE dbo.pChemigationPermitAnnualRecordBulkCreateForRecordYear @recordYear, @chemigationPermitsRenewed OUTPUT, @chemigationInspectionsCreated OUTPUT", sqlParameter, chemigationPermitsRenewed, chemigationInspectionsCreated);
            return new BulkChemigationPermitAnnualRecordCreationResult((int) chemigationPermitsRenewed.Value, (int)chemigationInspectionsCreated.Value);
        }

        public static IEnumerable<ChemigationPermitDetailedDto> GetByWellIDAsDetailedDto(ZybachDbContext dbContext, int wellID)
        { 
            var permitIDs = GetChemigationPermitImpl(dbContext)
                .Where(x => x.WellID != null && x.WellID == wellID)
                .Select(x => x.ChemigationPermitID)
                .ToList();

            return GetDetailedDtosByListOfPermitIDs(dbContext, permitIDs);
        }
    }
}