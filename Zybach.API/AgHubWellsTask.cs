using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API;

public static class AgHubWellsTask
{
    public static async Task SyncPumpedVolumeDataForWell(ZybachDbContext dbContext, AgHubService agHubService, DateTime startDate, string wellRegistrationID)
    {
        await dbContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE dbo.WellSensorMeasurementStaging");
        PopulateWellSensorMeasurementsForWell(dbContext, agHubService, wellRegistrationID, startDate);
        await dbContext.SaveChangesAsync();
        await dbContext.Database.ExecuteSqlRawAsync("EXECUTE dbo.pPublishWellSensorMeasurementStaging");
    }

    public static void SyncForAllWells(ZybachDbContext dbContext, AgHubService agHubService, DateTime startDate)
    {
        // first delete all from the tables
        dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE dbo.AgHubWellStaging");
        dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE dbo.AgHubWellIrrigatedAcreStaging");
        dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE dbo.WellSensorMeasurementStaging");

        var agHubWellRaws = agHubService.GetWellCollection().Result;
        if (agHubWellRaws.Any())
        {
            var wellStagings = agHubWellRaws.Select(CreateAgHubWellStaging).ToList();
            foreach (var wellStaging in wellStagings)
            {
                var wellRegistrationID = wellStaging.WellRegistrationID;
                //if (!ProblemWellRegistrationIDs.Contains(wellRegistrationID))
                PopulateIrrigatedAcresPerYearForWell(dbContext, agHubService, wellStaging, wellRegistrationID);
                PopulateWellSensorMeasurementsForWell(dbContext, agHubService, wellRegistrationID, startDate);
                dbContext.AgHubWellStagings.Add(wellStaging);
                dbContext.SaveChanges();
            }
                
            // only publish if we actually got any AgHubWells from Zappa
            dbContext.Database.ExecuteSqlRaw("EXECUTE dbo.pPublishAgHubWells");
        }
    }

    private static void PopulateWellSensorMeasurementsForWell(ZybachDbContext dbContext, AgHubService agHubService, string wellRegistrationID, DateTime startDate)
    {
        var pumpedVolumeResult = agHubService.GetPumpedVolume(wellRegistrationID, startDate).Result;
        if (pumpedVolumeResult is { PumpedVolumeTimeSeries: not null })
        {
            var pumpedVolumeTimePoints = pumpedVolumeResult.PumpedVolumeTimeSeries.Where(x => x.IsElectricSource).ToList();
            if (pumpedVolumeTimePoints.Any())
            {
                var wellSensorMeasurementStagings = pumpedVolumeTimePoints.Select(
                    pumpedVolumeTimeSeries => new WellSensorMeasurementStaging
                    {
                        SensorName = $"E-{wellRegistrationID.ToUpper()}",
                        MeasurementTypeID = (int)MeasurementTypeEnum.ElectricalUsage,
                        ReadingYear = pumpedVolumeTimeSeries.MeasurementDate.Year,
                        ReadingMonth = pumpedVolumeTimeSeries.MeasurementDate.Month,
                        ReadingDay = pumpedVolumeTimeSeries.MeasurementDate.Day,
                        MeasurementValue = pumpedVolumeTimeSeries.PumpedVolumeGallons,
                        WellRegistrationID = wellRegistrationID,
                        IsElectricSource = pumpedVolumeTimeSeries.IsElectricSource

                    }).ToList();
                dbContext.WellSensorMeasurementStagings.AddRange(wellSensorMeasurementStagings);
            }
        }
    }

    private static void PopulateIrrigatedAcresPerYearForWell(ZybachDbContext dbContext, AgHubService agHubService, AgHubWellStaging wellStaging, string wellRegistrationID)
    {
        var agHubWellRawWithAcreYears = agHubService.GetWellIrrigatedAcresPerYear(wellRegistrationID).Result;

        if (agHubWellRawWithAcreYears != null)
        {
            wellStaging.RegisteredUpdated = agHubWellRawWithAcreYears.RegisteredUpdated;
            wellStaging.RegisteredPumpRate = agHubWellRawWithAcreYears.RegisteredPumpRate;
            wellStaging.HasElectricalData = agHubWellRawWithAcreYears.HasElectricalData > 0;
            wellStaging.AgHubRegisteredUser = agHubWellRawWithAcreYears.RegisteredUserDetails.RegisteredUser;
            wellStaging.FieldName = agHubWellRawWithAcreYears.RegisteredUserDetails.RegisteredFieldName;
            wellStaging.IrrigationUnitGeometry = WKTToGeometry(agHubWellRawWithAcreYears.IrrigationUnitGeometry);

            var wellIrrigatedAcreStagings = agHubWellRawWithAcreYears.IrrigUnitDetails
                .Where(x => x.TotalAcres.HasValue).Select(x =>
                {
                    var agHubWellIrrigatedAcreStaging = new AgHubWellIrrigatedAcreStaging()
                    {
                        Acres = x.TotalAcres.Value,
                        WellRegistrationID = wellRegistrationID,
                        IrrigationYear = x.Year
                    };
                    if (x.FarmPractices != null && x.FarmPractices.Any())
                    {
                        var farmPractice = x.FarmPractices.OrderByDescending(y => y.Acres).ThenBy(y => y.Crop)
                            .ThenBy(x => x.Tillage).First();
                        agHubWellIrrigatedAcreStaging.CropType = farmPractice.Crop;
                        agHubWellIrrigatedAcreStaging.Tillage = farmPractice.Tillage;
                    }
                    return agHubWellIrrigatedAcreStaging;
                }).ToList();
            dbContext.AgHubWellIrrigatedAcreStagings.AddRange(wellIrrigatedAcreStagings);
        }
    }

    private static Geometry WKTToGeometry(string wktGeometry)
    {
        return !string.IsNullOrWhiteSpace(wktGeometry) ? new WKTReader().Read(wktGeometry) : null;
    }

    private static AgHubWellStaging CreateAgHubWellStaging(AgHubService.AgHubWellRaw agHubWellRaw)
    {
        var agHubWellStaging = new AgHubWellStaging
        {
            WellRegistrationID = agHubWellRaw.WellRegistrationID,
            AuditPumpRateUpdated = agHubWellRaw.AuditPumpRateUpdated,
            WellAuditPumpRate = agHubWellRaw.WellAuditPumpRate,
            TPNRDPumpRateUpdated = agHubWellRaw.DistrictPumpRateUpdated,
            WellTPNRDPumpRate = agHubWellRaw.WellDistrictPumpRate,
            WellConnectedMeter = agHubWellRaw.WellConnectedMeter ?? false,
            WellGeometry = WKTToGeometry(agHubWellRaw.Location),
            WellTPID = agHubWellRaw.WellIrrigUnitID,
            HasElectricalData = false
        };
        return agHubWellStaging;
    }
}