using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API
{
    public class FlowMeterSeriesFetchDailyJob : ScheduledBackgroundJobBase<FlowMeterSeriesFetchDailyJob>
    {
        private readonly InfluxDBService _influxDbService;
        public const string JobName = "Flow Meter Series Fetch Daily";

        public FlowMeterSeriesFetchDailyJob(IWebHostEnvironment webHostEnvironment, ILogger<FlowMeterSeriesFetchDailyJob> logger,
            ZybachDbContext zybachDbContext, IOptions<ZybachConfiguration> zybachConfiguration, InfluxDBService influxDbService, SitkaSmtpClientService sitkaSmtpClientService) : base(
            JobName, logger, webHostEnvironment, zybachDbContext, zybachConfiguration, sitkaSmtpClientService)
        {
            _influxDbService = influxDbService;
        }

        public override List<RunEnvironment> RunEnvironments => new List<RunEnvironment>
            {RunEnvironment.Production};

        protected override async void RunJobImplementation()
        {
            await GetDailyWellFlowMeterData(DefaultStartDate);
        }

        private async Task GetDailyWellFlowMeterData(DateTime fromDate)
        {
            await _dbContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE dbo.WellSensorMeasurementStaging");

            var wellSensorMeasurements = _influxDbService.GetFlowMeterSeries(fromDate).Result;
            _dbContext.WellSensorMeasurementStagings.AddRange(wellSensorMeasurements);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Database.ExecuteSqlRawAsync("EXECUTE dbo.pPublishWellSensorMeasurementStaging");
        }
    }
}