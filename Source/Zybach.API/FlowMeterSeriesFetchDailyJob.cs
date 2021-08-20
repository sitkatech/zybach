using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API
{
    public class FlowMeterSeriesFetchDailyJob : ScheduledBackgroundJobBase<FlowMeterSeriesFetchDailyJob>
    {
        private readonly InfluxDBService _influxDbService;
        public const string JobName = "Flow Meter Series Fetch Daily";

        public FlowMeterSeriesFetchDailyJob(IWebHostEnvironment webHostEnvironment, ILogger<FlowMeterSeriesFetchDailyJob> logger,
            ZybachDbContext zybachDbContext, InfluxDBService influxDbService) : base(
            JobName, logger, webHostEnvironment, zybachDbContext)
        {
            _influxDbService = influxDbService;
        }

        public override List<RunEnvironment> RunEnvironments => new List<RunEnvironment>
            {RunEnvironment.Development, RunEnvironment.Staging, RunEnvironment.Production};

        protected override void RunJobImplementation()
        {
            // TODO: Needs to switch to current date
            var fromDate = new DateTime(2016, 1, 1);

            try
            {
                GetDailyWellFlowMeterData(fromDate);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception($"{JobName} encountered an error", e);
            }
        }

        private void GetDailyWellFlowMeterData(DateTime fromDate)
        {
            var wellSensorMeasurements = _influxDbService.GetFlowMeterSeries(fromDate).Result;
            _dbContext.WellSensorMeasurements.AddRange(wellSensorMeasurements);
            _dbContext.SaveChanges();
        }
    }
}