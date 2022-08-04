using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API;

public class ContinuityMeterStatusFetchDailyJob : ScheduledBackgroundJobBase<ContinuityMeterStatusFetchDailyJob>
{
    private readonly InfluxDBService _influxDbService;
    public const string JobName = "Continuity Meter Status Fetch Daily";

    public ContinuityMeterStatusFetchDailyJob(IWebHostEnvironment webHostEnvironment, ILogger<ContinuityMeterStatusFetchDailyJob> logger,
        ZybachDbContext zybachDbContext, InfluxDBService influxDbService) : base(
        JobName, logger, webHostEnvironment, zybachDbContext)
    {
        _influxDbService = influxDbService;
    }

    public override List<RunEnvironment> RunEnvironments => new List<RunEnvironment>
        {RunEnvironment.Production};

    protected override void RunJobImplementation()
    {
        try
        {
            GetDailyContinuityMeterStatusData();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw new Exception($"{JobName} encountered an error", e);
        }
    }

    private void GetDailyContinuityMeterStatusData()
    {
        var continuityMeterStatuses = _influxDbService.GetDailyContinuityMeterStatusData().Result;
        
        var currentDateMinusTenDays = DateTime.Today.AddDays(-10);
        var monthsOctoberThroughApril = new [] { 10, 11, 12, 1, 2, 3, 4 };
        var automaticallySnoozeAlwaysOffStatus = monthsOctoberThroughApril.Contains(DateTime.Today.Month);

        var continuityMeters = _dbContext.Sensors.Where(x => x.SensorTypeID == (int)SensorTypeEnum.ContinuityMeter).ToList();
        continuityMeters.ForEach(x =>
        {
            x.ContinuityMeterStatusID = continuityMeterStatuses.ContainsKey(x.SensorName) ? continuityMeterStatuses[x.SensorName] : (int)ContinuityMeterStatusEnum.AlwaysOff;
            x.ContinuityMeterStatusLastUpdated = DateTime.UtcNow;

            if (automaticallySnoozeAlwaysOffStatus && x.ContinuityMeterStatusID == (int)ContinuityMeterStatusEnum.AlwaysOff)
            {
                x.SnoozeStartDate = currentDateMinusTenDays.AddDays(1);
            }
            else if (x.ContinuityMeterStatusID == (int)ContinuityMeterStatusEnum.ReportingNormally || 
                     (x.SnoozeStartDate.HasValue && x.SnoozeStartDate.Value.Date <= currentDateMinusTenDays))
            { 
                x.SnoozeStartDate = null;
            }
        }); 
        
        _dbContext.SaveChanges();
    }
}