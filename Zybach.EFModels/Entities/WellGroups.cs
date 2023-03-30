using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Zybach.API.Util;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public class WellGroups
{
    private static IQueryable<WellGroup> GetImpl(ZybachDbContext dbContext)
    {
        return dbContext.WellGroups
            .Include(x => x.WellGroupWells).ThenInclude(x => x.Well).ThenInclude(x => x.Sensors)
            .Include(x => x.WellGroupWells).ThenInclude(x => x.Well).ThenInclude(x => x.WaterLevelInspections);
    }
    public static List<WellGroupDto> ListAsDto(ZybachDbContext dbContext)
    {
        return GetImpl(dbContext).AsNoTracking().Select(x => x.AsDto()).ToList();
    }

    public static List<WellGroupWaterLevelInspectionDto> ListByIDsAsWellGroupWaterLevelInspectionDto(ZybachDbContext dbContext, List<int> wellGroupIDs)
    {
        return GetImpl(dbContext).Where(x => wellGroupIDs.Contains(x.WellGroupID))
            .Select(x => x.AsWellGroupWaterLevelInspectionDto()).ToList();
    }

    public static WellGroup GetByID(ZybachDbContext dbContext, int wellGroupID)
    {
        return GetImpl(dbContext).SingleOrDefault(x => x.WellGroupID == wellGroupID);
    }

    public static int Create(ZybachDbContext dbContext, WellGroupDto wellGroupDto)
    {
        var wellGroup = new WellGroup()
        {
            WellGroupName = wellGroupDto.WellGroupName
        };

        dbContext.WellGroups.Add(wellGroup);
        dbContext.SaveChanges();

        MergeWellGroupWells(dbContext, wellGroup.WellGroupID, wellGroupDto.WellGroupWells);

        return wellGroup.WellGroupID;
    }

    public static void Update(ZybachDbContext dbContext, WellGroup wellGroup, WellGroupDto wellGroupDto)
    {
        wellGroup.WellGroupName = wellGroupDto.WellGroupName;
        dbContext.SaveChanges();

        MergeWellGroupWells(dbContext, wellGroup.WellGroupID, wellGroupDto.WellGroupWells);
    }

    private static void MergeWellGroupWells(ZybachDbContext dbContext, int wellGroupID, List<WellGroupWellSimpleDto> wellGroupWellDtos)
    {
        var updatedWellGroupWells = wellGroupWellDtos.Select(x => new WellGroupWell
        {
            WellGroupID = wellGroupID,
            WellID = x.WellID,
            IsPrimary = x.IsPrimary
        }).ToList();

        var allWellGroupWells = dbContext.WellGroupWells;
        var existingWellGroupWells = allWellGroupWells.Where(x => x.WellGroupID == wellGroupID).ToList();

        existingWellGroupWells.Merge(updatedWellGroupWells, allWellGroupWells, 
            (x, y) => (x.WellGroupID == y.WellGroupID && x.WellID == y.WellID),
            (x, y) =>
            {
                x.IsPrimary = y.IsPrimary;
            });

        dbContext.SaveChanges();
    }

    public static void Delete(ZybachDbContext dbContext, WellGroup wellGroup)
    {
        var wellGroupWells = dbContext.WellGroupWells
            .Where(x => x.WellGroupID == wellGroup.WellGroupID);
        
        dbContext.WellGroupWells.RemoveRange(wellGroupWells);
        dbContext.WellGroups.Remove(wellGroup);

        dbContext.SaveChanges();
    }
}