using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class WaterYearMonths
    {
        public static IQueryable<WaterYearMonth> GetWaterYearMonthImpl(ZybachDbContext dbContext)
        {
            return dbContext.WaterYearMonths
                .AsNoTracking();
        }
        public static List<WaterYearMonthDto> List(ZybachDbContext dbContext)
        {
            return GetWaterYearMonthImpl(dbContext)
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .Select(x => x.AsDto())
                .ToList();
        }

        public static List<int> ListWaterYears(ZybachDbContext dbContext)
        {
            return GetWaterYearMonthImpl(dbContext).Select(x => x.Year).Distinct().ToList();
        }

        public static WaterYearMonthDto GetByWaterYearMonthID(ZybachDbContext dbContext, int waterYearMonthID)
        {
            return GetWaterYearMonthImpl(dbContext).SingleOrDefault(x => x.WaterYearMonthID == waterYearMonthID).AsDto();
        }

        public static WaterYearMonthDto Finalize(ZybachDbContext dbContext, int waterYearMonthID)
        {
            var waterYear = dbContext.WaterYearMonths.Single(x => x.WaterYearMonthID == waterYearMonthID);

            waterYear.FinalizeDate = DateTime.UtcNow;

            dbContext.SaveChanges();
            dbContext.Entry(waterYear).Reload();
            return GetByWaterYearMonthID(dbContext, waterYearMonthID);
        }

        public static List<WaterYearMonthDto> ListNonFinalized(ZybachDbContext dbContext)
        {
            return GetWaterYearMonthImpl(dbContext)
                .Where(x => x.FinalizeDate == null)
                .OrderByDescending(x => x.Year).ThenByDescending(x => x.Month)
                .Select(x => x.AsDto())
                .ToList();
        }

        public static WaterYearMonthDto GetByYearAndMonth(ZybachDbContext dbContext, int waterYear, int waterMonth)
        {
            return GetWaterYearMonthImpl(dbContext).SingleOrDefault(x => x.Year == waterYear && x.Month == waterMonth).AsDto();
        }

        public static object ListForCurrentDateOrEarlier(ZybachDbContext dbContext)
        {
            var currentDate = DateTime.UtcNow;
            return GetWaterYearMonthImpl(dbContext)
                .Where(x => x.Year < currentDate.Year || (x.Year == currentDate.Year && x.Month <= currentDate.Month ))
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .Select(x => x.AsDto())
                .ToList();

        }
    }
}