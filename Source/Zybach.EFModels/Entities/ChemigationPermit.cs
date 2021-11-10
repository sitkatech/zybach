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

        public static bool IsTownshipRangeSectionUnique(ZybachDbContext dbContext, string townshipRangeSection, int? currentID)
        {
            return dbContext.ChemigationPermits
                .Any(x => x.TownshipRangeSection == townshipRangeSection &&
                          (currentID == null || (
                              currentID != null && x.ChemigationPermitID != currentID)));
        }

        public static ChemigationPermitDto CreateNewChemigationPermit(ZybachDbContext dbContext, ChemigationPermitUpsertDto chemigationPermitUpsertDto)
        {
            if (chemigationPermitUpsertDto == null)
            {
                return null;
            }

            var chemigationPermit = new ChemigationPermit()
            {
                ChemigationPermitNumber = chemigationPermitUpsertDto.ChemigationPermitNumber,
                ChemigationPermitStatusID = chemigationPermitUpsertDto.ChemigationPermitStatusID,
                DateReceived = chemigationPermitUpsertDto.DateReceived,
                TownshipRangeSection = chemigationPermitUpsertDto.TownshipRangeSection
            };

            dbContext.ChemigationPermits.Add(chemigationPermit);
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
            chemigationPermit.DateReceived = chemigationPermitUpsertDto.DateReceived;
            chemigationPermit.TownshipRangeSection = chemigationPermitUpsertDto.TownshipRangeSection;

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