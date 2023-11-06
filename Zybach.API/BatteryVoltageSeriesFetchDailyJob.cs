using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API;

public class BatteryVoltageSeriesFetchDailyJob : ScheduledBackgroundJobBase<BatteryVoltageSeriesFetchDailyJob>
{
    private readonly InfluxDBService _influxDbService;
    public const string JobName = "Battery Voltage Series Fetch Daily";

    public BatteryVoltageSeriesFetchDailyJob(IWebHostEnvironment webHostEnvironment, ILogger<BatteryVoltageSeriesFetchDailyJob> logger,
        ZybachDbContext zybachDbContext, IOptions<ZybachConfiguration> zybachConfiguration, InfluxDBService influxDbService, SitkaSmtpClientService sitkaSmtpClientService) : base(
        JobName, logger, webHostEnvironment, zybachDbContext, zybachConfiguration, sitkaSmtpClientService)
    {
        _influxDbService = influxDbService;
    }

    public override List<RunEnvironment> RunEnvironments => new() {RunEnvironment.Production};

    protected override void RunJobImplementation()
    {
        GetDailyWellWaterLevelData(DefaultStartDate);
    }

    private void GetDailyWellWaterLevelData(DateTime fromDate)
    {
        _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE dbo.WellSensorMeasurementStaging");

        var wellSensorMeasurements = _influxDbService.GetBatteryVoltageSeries(fromDate).Result;
        _dbContext.WellSensorMeasurementStagings.AddRange(wellSensorMeasurements);
        _dbContext.SaveChanges();

        _dbContext.Database.ExecuteSqlRaw("EXECUTE dbo.pPublishWellSensorMeasurementStaging");
    }
}