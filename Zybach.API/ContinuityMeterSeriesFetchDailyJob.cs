using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API
{
    public class ContinuityMeterSeriesFetchDailyJob : ScheduledBackgroundJobBase<ContinuityMeterSeriesFetchDailyJob>
    {
        private readonly InfluxDBService _influxDbService;
        public const string JobName = "Continuity Meter Series Fetch Daily";

        public ContinuityMeterSeriesFetchDailyJob(IWebHostEnvironment webHostEnvironment, ILogger<ContinuityMeterSeriesFetchDailyJob> logger,
            ZybachDbContext zybachDbContext, IOptions<ZybachConfiguration> zybachConfiguration, InfluxDBService influxDbService, SitkaSmtpClientService sitkaSmtpClientService) : base(
            JobName, logger, webHostEnvironment, zybachDbContext, zybachConfiguration, sitkaSmtpClientService)
        {
            _influxDbService = influxDbService;
        }

        public override List<RunEnvironment> RunEnvironments => new List<RunEnvironment>
            {RunEnvironment.Production, RunEnvironment.Staging};

        protected override void RunJobImplementation()
        {
            GetDailyWellContinuityMeterData(DefaultStartDate);
        }

        private void GetDailyWellContinuityMeterData(DateTime fromDate)
        {
            _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE dbo.WellSensorMeasurementStaging");

            var wellSensorMeasurementStagings = _influxDbService.GetContinuityMeterSeries(fromDate).Result;
            var pumpingRates = _dbContext.AgHubWells.Include(x => x.Well).ToList().ToDictionary(x => x.Well.WellRegistrationID, x =>
                x.PumpingRateGallonsPerMinute, StringComparer.InvariantCultureIgnoreCase);

            wellSensorMeasurementStagings.ForEach(x =>
            {
                var pumpingRate = pumpingRates.ContainsKey(x.WellRegistrationID) ? pumpingRates[x.WellRegistrationID] : 0;
                x.MeasurementValue *= Convert.ToDouble(pumpingRate);
            });
            _dbContext.WellSensorMeasurementStagings.AddRange(wellSensorMeasurementStagings);
            _dbContext.SaveChanges();

            _dbContext.Database.ExecuteSqlRaw("EXECUTE dbo.pPublishWellSensorMeasurementStaging");
        }
    }
}