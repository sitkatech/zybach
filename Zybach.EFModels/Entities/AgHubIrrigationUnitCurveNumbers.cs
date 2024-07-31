using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public static class AgHubIrrigationUnitCurveNumbers
{
    public static async Task<List<AgHubIrrigationUnitCurveNumberSimpleDto>> ListAsSimpleDtoAsync(ZybachDbContext dbContext)
    {
        var curveNumbers = await dbContext.AgHubIrrigationUnitCurveNumbers
            .AsNoTracking()
            .Include(x => x.AgHubIrrigationUnit)
            .OrderBy(x => x.AgHubIrrigationUnit.WellTPID)
            .Select(x => x.AsSimpleDto())
            .ToListAsync();

        return curveNumbers;
    }

    public static async Task<AgHubIrrigationUnitCurveNumberSimpleDto> GetSimpleByIrrigationUnitIDAsync(ZybachDbContext dbContext, int irrigationUnitID)
    {
        var curveNumber = await dbContext.AgHubIrrigationUnitCurveNumbers
            .AsNoTracking()
            .Include(x => x.AgHubIrrigationUnit)
            .Where(x => x.AgHubIrrigationUnitID == irrigationUnitID)
            .Select(x => x.AsSimpleDto())
            .FirstOrDefaultAsync();

        return curveNumber;
    }

    public static async Task<List<ErrorMessage>> ValidateUpsertAsync(ZybachDbContext dbContext, AgHubIrrigationUnitCurveNumberUpsertDto curveNumberUpsertDto)
    {
        var results = new List<ErrorMessage>();

        if (curveNumberUpsertDto.MTillCurveNumber < 0 || curveNumberUpsertDto.MTillCurveNumber > 100)
        {
            results.Add(new ErrorMessage("MTillCurveNumber", "M Till Curve Number must be between 0 and 100"));
        }

        if (curveNumberUpsertDto.STillCurveNumber < 0 || curveNumberUpsertDto.STillCurveNumber > 100)
        {
            results.Add(new ErrorMessage("STillCurveNumber", "S Till Curve Number must be between 0 and 100"));
        }

        if (curveNumberUpsertDto.NTillCurveNumber < 0 || curveNumberUpsertDto.NTillCurveNumber > 100)
        {
            results.Add(new ErrorMessage("NTillCurveNumber", "N Till Curve Number must be between 0 and 100"));
        }

        if (curveNumberUpsertDto.CTillCurveNumber < 0 || curveNumberUpsertDto.CTillCurveNumber > 100)
        {
            results.Add(new ErrorMessage("CTillCurveNumber", "C Till Curve Number must be between 0 and 100"));
        }

        if (curveNumberUpsertDto.UndefinedTillCurveNumber < 0 || curveNumberUpsertDto.UndefinedTillCurveNumber > 100)
        {
            results.Add(new ErrorMessage("UndefinedTillCurveNumber", "Undefined Till Curve Number must be between 0 and 100"));
        }

        await Task.CompletedTask; // Just to keep the compiler happy, leaving this as async in case we ever need to go to the dbContext to help validate.
        return results;
    }

    public static async Task<AgHubIrrigationUnitCurveNumberSimpleDto> UpsertAsync(ZybachDbContext dbContext, AgHubIrrigationUnitCurveNumberUpsertDto curveNumberUpsertDto, int? existingCurveNumberID = null)
    {
        AgHubIrrigationUnitCurveNumber curveNumber;

        if (existingCurveNumberID.HasValue)
        {
            curveNumber = await dbContext.AgHubIrrigationUnitCurveNumbers
                .FirstOrDefaultAsync(x => x.AgHubIrrigationUnitCurveNumberID == existingCurveNumberID);
        }
        else
        {
            curveNumber = new AgHubIrrigationUnitCurveNumber();
            dbContext.AgHubIrrigationUnitCurveNumbers.Add(curveNumber);
        }

        curveNumber.HydrologicSoilGroup = curveNumberUpsertDto.HydrologicSoilGroup;
        curveNumber.MTillCurveNumber = curveNumberUpsertDto.MTillCurveNumber.GetValueOrDefault(0);
        curveNumber.STillCurveNumber = curveNumberUpsertDto.STillCurveNumber.GetValueOrDefault(0);
        curveNumber.NTillCurveNumber = curveNumberUpsertDto.NTillCurveNumber.GetValueOrDefault(0);
        curveNumber.CTillCurveNumber = curveNumberUpsertDto.CTillCurveNumber.GetValueOrDefault(0);
        curveNumber.UndefinedTillCurveNumber = curveNumberUpsertDto.UndefinedTillCurveNumber.GetValueOrDefault(0);

        await dbContext.SaveChangesAsync();

        return curveNumber.AsSimpleDto();
    }
}

public partial class AgHubIrrigationUnitCurveNumberUpsertDto
{
    [MaxLength(3)]
    public string HydrologicSoilGroup { get; set; }

    [Required]
    public double? MTillCurveNumber { get; set; }

    [Required]
    public double? STillCurveNumber { get; set; }

    [Required]
    public double? NTillCurveNumber { get; set; }

    [Required]
    public double? CTillCurveNumber { get; set; }

    [Required]
    public double? UndefinedTillCurveNumber { get; set; }
}