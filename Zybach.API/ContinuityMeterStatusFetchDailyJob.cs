using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API;

public class ContinuityMeterStatusFetchDailyJob : ScheduledBackgroundJobBase<ContinuityMeterStatusFetchDailyJob>
{
    private readonly InfluxDBService _influxDbService;
    public const string JobName = "Continuity Meter Status Fetch Daily";
    public static readonly int[] MonthsToAutomaticallySnooze = { 10, 11, 12, 1, 2, 3, 4 };

    public ContinuityMeterStatusFetchDailyJob(IWebHostEnvironment webHostEnvironment, ILogger<ContinuityMeterStatusFetchDailyJob> logger,
        ZybachDbContext zybachDbContext, IOptions<ZybachConfiguration> zybachConfiguration, InfluxDBService influxDbService, SitkaSmtpClientService sitkaSmtpClientService) : base(
        JobName, logger, webHostEnvironment, zybachDbContext, zybachConfiguration, sitkaSmtpClientService)
    {
        _influxDbService = influxDbService;
    }

    public override List<RunEnvironment> RunEnvironments => new List<RunEnvironment>
        {RunEnvironment.Production};

    protected override async void RunJobImplementation()
    {
        await GetDailyContinuityMeterStatusData();
    }

    private async Task GetDailyContinuityMeterStatusData()
    {
        var continuityMeterStatuses = _influxDbService.GetDailyContinuityMeterStatusData().Result;
        
        var currentDateMinusTenDays = DateTime.Today.AddDays(-10);
        var automaticallySnoozeAlwaysOffStatus = MonthsToAutomaticallySnooze.Contains(DateTime.Today.Month);

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
        
        await _dbContext.SaveChangesAsync();
    }
}