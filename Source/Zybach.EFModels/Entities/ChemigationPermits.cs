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
        public static IEnumerable<ChemigationPermitDto> List(ZybachDbContext dbContext)
        {
            return GetChemigationPermitImpl(dbContext)
                .Select(x => x.AsDto()).ToList();
        }

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

            ChemigationPermitAnnualRecords.CreateAnnualRecord(dbContext, chemigationPermitNewDto.ChemigationPermitAnnualRecord, chemigationPermitID);

            dbContext.Entry(chemigationPermit).Reload();

            return GetChemigationPermitByID(dbContext, chemigationPermitID);
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

        public static IQueryable<ChemigationPermit> GetChemigationPermitImpl(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationPermits
                .Include(x => x.ChemigationPermitStatus)
                .Include(x => x.County)
                .Include(x => x.Well)
                .AsNoTracking();
        }

        public static ChemigationPermitDto UpdateChemigationPermit(ZybachDbContext dbContext, ChemigationPermit chemigationPermit, ChemigationPermitUpsertDto chemigationPermitUpsertDto)
        {
            chemigationPermit.ChemigationPermitStatusID = chemigationPermitUpsertDto.ChemigationPermitStatusID;
            chemigationPermit.CountyID = chemigationPermitUpsertDto.CountyID;

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


        public static int BulkCreateRenewalRecords(ZybachDbContext dbContext, int recordYear)
        {
            var sqlParameter = new SqlParameter("recordYear", recordYear);
            var sqlParameterOutput = new SqlParameter("recordsCreated", SqlDbType.Int);
            sqlParameterOutput.Direction = ParameterDirection.Output;
            dbContext.Database.ExecuteSqlRaw("EXECUTE dbo.pChemigationPermitAnnualRecordBulkCreateForRecordYear @recordYear, @recordsCreated OUTPUT", sqlParameter, sqlParameterOutput);
            return (int)sqlParameterOutput.Value;
        }
    }
}