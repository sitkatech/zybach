﻿using System;
using System.Collections.Generic;
using Zybach.Models.DataTransferObjects;
using System.Linq;
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
            var chemigationPermitAnnualRecordDetailedDtos = ChemigationPermitAnnualRecord.GetLatestAsDetailedDto(dbContext).ToDictionary(x => x.ChemigationPermit.ChemigationPermitID);
            var listWithLatestAnnualRecordAsDto = GetChemigationPermitImpl(dbContext)
                .Select(x =>
                    x.AsDetailedDto(chemigationPermitAnnualRecordDetailedDtos.ContainsKey(x.ChemigationPermitID)
                        ? chemigationPermitAnnualRecordDetailedDtos[x.ChemigationPermitID]
                        : null)).ToList().OrderBy(x => x.ChemigationPermitNumber).ToList();
            return listWithLatestAnnualRecordAsDto;
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
                ChemigationCountyID = chemigationPermitNewDto.ChemigationCountyID
            };

            dbContext.ChemigationPermits.Add(chemigationPermit);
            dbContext.SaveChanges();

            var chemigationPermitID = chemigationPermit.ChemigationPermitID;
            ChemigationPermitAnnualRecord.CreateAnnualRecord(dbContext, chemigationPermitNewDto.ChemigationPermitAnnualRecord, chemigationPermitID);

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

        private static IQueryable<ChemigationPermit> GetChemigationPermitImpl(ZybachDbContext dbContext)
        {
            return dbContext.ChemigationPermits
                .Include(x => x.ChemigationPermitStatus)
                .Include(x => x.ChemigationCounty)
                .AsNoTracking();
        }

        public static ChemigationPermitDto UpdateChemigationPermit(ZybachDbContext dbContext, ChemigationPermit chemigationPermit, ChemigationPermitUpsertDto chemigationPermitUpsertDto)
        {
            // null check occurs in calling endpoint method.
            chemigationPermit.ChemigationPermitNumber = chemigationPermitUpsertDto.ChemigationPermitNumber;
            chemigationPermit.ChemigationPermitStatusID = chemigationPermitUpsertDto.ChemigationPermitStatusID;
            chemigationPermit.TownshipRangeSection = chemigationPermitUpsertDto.TownshipRangeSection;
            chemigationPermit.ChemigationCountyID = chemigationPermitUpsertDto.ChemigationCountyID;

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