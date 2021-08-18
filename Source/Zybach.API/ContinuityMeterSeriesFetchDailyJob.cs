using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API
{
    public class ContinuityMeterSeriesFetchDailyJob : ScheduledBackgroundJobBase<ContinuityMeterSeriesFetchDailyJob>, IInfluxDBJob
    {
        private readonly InfluxDBService _influxDbService;
        public const string JobName = "Continuity Meter Series Fetch Daily";

        public ContinuityMeterSeriesFetchDailyJob(IWebHostEnvironment webHostEnvironment, ILogger<ContinuityMeterSeriesFetchDailyJob> logger,
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
                GetDailyWellContinuityMeterData(fromDate);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception($"{JobName} encountered an error", e);
            }
        }

        private void GetDailyWellContinuityMeterData(DateTime fromDate)
        {
            var wellSensorMeasurements = _influxDbService.GetContinuityMeterSeries(fromDate).Result;
            // TODO: Get PumpingRate from Zappa service and multiply it by the value and 1440
            // |> map(fn: (r) => ({r with _value: r._value * float(v: ${gpm * 15})})) \
            // 15 is minutes; should be 1440
            //var zappaRate = new Dictionary<string, double>();
            //wellSensorMeasurements.ForEach(x =>
            //{
            //    var pumpingRate = zappaRate[x.WellRegistrationID];
            //    x.MeasurementValue = x.MeasurementValue * pumpingRate * 1440;
            //});
            _dbContext.WellSensorMeasurements.AddRange(wellSensorMeasurements);
            _dbContext.SaveChanges();
        }
    }
}