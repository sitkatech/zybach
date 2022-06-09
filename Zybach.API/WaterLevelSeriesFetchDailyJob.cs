using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API
{
    public class WaterLevelSeriesFetchDailyJob : ScheduledBackgroundJobBase<WaterLevelSeriesFetchDailyJob>
    {
        private readonly InfluxDBService _influxDbService;
        public const string JobName = "Water Level Series Fetch Daily";

        public WaterLevelSeriesFetchDailyJob(IWebHostEnvironment webHostEnvironment, ILogger<WaterLevelSeriesFetchDailyJob> logger,
            ZybachDbContext zybachDbContext, InfluxDBService influxDbService) : base(
            JobName, logger, webHostEnvironment, zybachDbContext)
        {
            _influxDbService = influxDbService;
        }

        public override List<RunEnvironment> RunEnvironments => new() {RunEnvironment.Production};

        protected override void RunJobImplementation()
        {
            try
            {
                GetDailyWellWaterLevelData(DefaultStartDate);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception($"{JobName} encountered an error", e);
            }
        }

        private void GetDailyWellWaterLevelData(DateTime fromDate)
        {
            _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE dbo.WellSensorMeasurementStaging");

            var wellSensorMeasurements = _influxDbService.GetWaterLevelSeries(fromDate).Result;
            _dbContext.WellSensorMeasurementStagings.AddRange(wellSensorMeasurements);
            _dbContext.SaveChanges();

            _dbContext.Database.ExecuteSqlRaw("EXECUTE dbo.pPublishWellSensorMeasurementStaging");
        }
    }
}