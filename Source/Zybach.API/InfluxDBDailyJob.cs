using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API
{
    public class InfluxDBDailyJob : ScheduledBackgroundJobBase<InfluxDBDailyJob>, IInfluxDBJob
    {
        private readonly InfluxDBService _influxDbService;
        public const string JobName = "Influx DB Daily Sync";

        public InfluxDBDailyJob(IWebHostEnvironment webHostEnvironment, ILogger<InfluxDBDailyJob> logger,
            ZybachDbContext zybachDbContext, InfluxDBService influxDbService) : base(
            "Influx DB Sync", logger, webHostEnvironment, zybachDbContext)
        {
            _influxDbService = influxDbService;
        }

        public override List<RunEnvironment> RunEnvironments => new List<RunEnvironment>
            {RunEnvironment.Development, RunEnvironment.Staging, RunEnvironment.Production};

        protected override void RunJobImplementation()
        {
            // TODO: Needs to switch to current date
            var fromDate = new DateTime(2019, 1, 1);

            try
            {
                GetDailyWellFlowMeterData(fromDate);
                //GetDailyWellContinuityMeterData(fromDate);
                // TODO: Get electrical usage data
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception("Influx DB Daily Sync encountered an error", e);
            }
        }

        private void GetDailyWellContinuityMeterData(DateTime fromDate)
        {
            var wellSensorMeasurements = _influxDbService.GetContinuityMeterSeries(fromDate).Result;
            // TODO: Get PumpingRate from Zappa service and multiply it by the value
            // |> map(fn: (r) => ({r with _value: r._value * float(v: ${gpm * 15})})) \
            _dbContext.WellSensorMeasurements.AddRange(wellSensorMeasurements);
            _dbContext.SaveChanges();
        }

        private void GetDailyWellFlowMeterData(DateTime fromDate)
        {
            var wellSensorMeasurements = _influxDbService.GetFlowMeterSeries(fromDate).Result;
            _dbContext.WellSensorMeasurements.AddRange(wellSensorMeasurements);
            _dbContext.SaveChanges();
        }
    }
}