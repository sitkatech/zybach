using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public class OpenETSyncs
{
    private static IQueryable<OpenETSync> GetImpl(ZybachDbContext dbContext)
    {
        return dbContext.OpenETSyncs
            .Include(x => x.OpenETSyncHistories);
    }

    public static OpenETSync GetByID(ZybachDbContext dbContext, int openETSyncID)
    {
        return GetImpl(dbContext).SingleOrDefault(x => x.OpenETSyncID == openETSyncID);
    }

    public static List<OpenETSyncDto> List(ZybachDbContext dbContext)
    {
        return GetImpl(dbContext).AsNoTracking()
            .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
            .Select(x => x.AsDto()).ToList();
    }

    public static void FinalizeSyncByID(ZybachDbContext dbContext, int openETSyncID)
    {
        var openETSync = GetByID(dbContext, openETSyncID);

        openETSync.FinalizeDate = DateTime.UtcNow;
        dbContext.SaveChanges();
    }

    public static List<int> ListYears(ZybachDbContext dbContext)
    {
        return dbContext.OpenETSyncs.Select(x => x.Year).Distinct().ToList();
    }
}