using System;
using System.Collections.Generic;
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

        protected override void RunJobImplementation()
        {
            GetDailyWellFlowMeterData(DefaultStartDate);
        }

        private void GetDailyWellFlowMeterData(DateTime fromDate)
        {
            _dbContext.Database.ExecuteSqlRaw($"EXECUTE dbo.pTruncateWellSensorMeasurementStaging");

            var wellSensorMeasurements = _influxDbService.GetFlowMeterSeries(fromDate).Result;
            _dbContext.WellSensorMeasurementStagings.AddRange(wellSensorMeasurements);
            _dbContext.SaveChanges();

            _dbContext.Database.ExecuteSqlRaw("EXECUTE dbo.pPublishWellSensorMeasurementStaging");
        }
    }
}