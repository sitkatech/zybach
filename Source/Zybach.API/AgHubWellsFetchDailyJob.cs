using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API
{
    public class AgHubWellsFetchDailyJob : ScheduledBackgroundJobBase<AgHubWellsFetchDailyJob>
    {
        private readonly InfluxDBService _influxDbService;
        private readonly AgHubService _agHubService;
        public const string JobName = "AgHub Well Fetch Daily";
//        private static readonly List<string> ProblemWellRegistrationIDs = new List<string>{ "G-012886", "G-017908", "G-018992", "G-033855", "G-052662", "G-128363" };

        public AgHubWellsFetchDailyJob(IWebHostEnvironment webHostEnvironment, ILogger<AgHubWellsFetchDailyJob> logger,
            ZybachDbContext zybachDbContext, AgHubService agHubService, InfluxDBService influxDbService) : base(
            JobName, logger, webHostEnvironment, zybachDbContext)
        {
            _agHubService = agHubService;
            _influxDbService = influxDbService;
        }

        public override List<RunEnvironment> RunEnvironments => new List<RunEnvironment>
            {RunEnvironment.Production};

        protected override void RunJobImplementation()
        {
            try
            {
                GetDailyWellFlowMeterData();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception($"{JobName} encountered an error", e);
            }
        }

        private void GetDailyWellFlowMeterData()
        {
            // first delete all from the tables
            _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE dbo.WellStaging");
            _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE dbo.WellIrrigatedAcreStaging");
            _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE dbo.WellSensorMeasurementStaging");


            var agHubWellRaws = _agHubService.GetWellCollection().Result;
            if (agHubWellRaws.Any())
            {
                var wellStagings = agHubWellRaws.Select(CreateWellStaging).ToList();
                // TODO: get last reading dates gets pumped volume dates too; original code is only looking at estimated pumped volume;
                var lastReadingDates = _dbContext.WellSensorMeasurements
                    .Where(x => x.MeasurementTypeID == (int) MeasurementTypeEnum.ElectricalUsage).ToList()
                    .GroupBy(x => x.WellRegistrationID).ToDictionary(x => x.Key, x => x.Max(y => y.MeasurementDate));
                foreach (var wellStaging in wellStagings)
                {
                    var wellRegistrationID = wellStaging.WellRegistrationID;
                    //if (!ProblemWellRegistrationIDs.Contains(wellRegistrationID))
                    PopulateIrrigatedAcresPerYearForWell(wellStaging, wellRegistrationID);
                    PopulateWellSensorMeasurementsForWell(wellStaging, lastReadingDates, wellRegistrationID);
                    _dbContext.WellStagings.Add(wellStaging);
                    _dbContext.SaveChanges();
                }

                // only publish if we actually got any AgHubWells from Zappa
                _dbContext.Database.ExecuteSqlRaw("EXECUTE dbo.pPublishAgHubWells");
            }
        }

        private void PopulateWellSensorMeasurementsForWell(WellStaging wellStaging, Dictionary<string, DateTime> lastReadingDates,
            string wellRegistrationID)
        {
            if (wellStaging.WellConnectedMeter)
            {
                var startDate = lastReadingDates.ContainsKey(wellRegistrationID) ? lastReadingDates[wellRegistrationID] : DefaultStartDate;
                var pumpedVolumeResult =
                    _agHubService.GetPumpedVolume(wellRegistrationID, startDate).Result;
                if (pumpedVolumeResult != null)
                {
                    var pumpedVolumeTimePoints =
                        pumpedVolumeResult.PumpedVolumeTimeSeries.Where(x => x.PumpedVolumeGallons > 0).ToList();
                    if (pumpedVolumeTimePoints.Any())
                    {
                        var wellSensorMeasurementStagings = pumpedVolumeTimePoints.Select(
                            pumpedVolumeTimeSeries => new WellSensorMeasurementStaging
                            {
                                MeasurementTypeID = (int) MeasurementTypeEnum.ElectricalUsage,
                                ReadingYear = pumpedVolumeTimeSeries.MeasurementDate.Year, 
                                ReadingMonth = pumpedVolumeTimeSeries.MeasurementDate.Month, 
                                ReadingDay = pumpedVolumeTimeSeries.MeasurementDate.Day,
                                MeasurementValue = pumpedVolumeTimeSeries.PumpedVolumeGallons,
                                WellRegistrationID = wellRegistrationID
                            }).ToList();
                        _dbContext.WellSensorMeasurementStagings.AddRange(wellSensorMeasurementStagings);
                    }
                }
            }
        }

        private void PopulateIrrigatedAcresPerYearForWell(WellStaging wellStaging, string wellRegistrationID)
        {
            var agHubWellRawWithAcreYears =
                _agHubService.GetWellIrrigatedAcresPerYear(wellRegistrationID).Result;
            if (agHubWellRawWithAcreYears != null)
            {
                wellStaging.RegisteredUpdated = agHubWellRawWithAcreYears.RegisteredUpdated;
                wellStaging.RegisteredPumpRate = agHubWellRawWithAcreYears.RegisteredPumpRate;
                wellStaging.HasElectricalData = agHubWellRawWithAcreYears.HasElectricalData;
                wellStaging.AgHubRegisteredUser = agHubWellRawWithAcreYears.RegisteredUserDetails.RegisteredUser;
                wellStaging.FieldName = agHubWellRawWithAcreYears.RegisteredUserDetails.RegisteredFieldName;

                var wellIrrigatedAcreStagings = agHubWellRawWithAcreYears.AcresYear
                    .Where(x => x.Acres.HasValue).Select(x => new WellIrrigatedAcreStaging()
                    {
                        Acres = x.Acres.Value,
                        WellRegistrationID = wellRegistrationID,
                        IrrigationYear = x.Year
                    }).ToList();
                _dbContext.WellIrrigatedAcreStagings.AddRange(wellIrrigatedAcreStagings);
            }
        }

        private static WellStaging CreateWellStaging(AgHubService.AgHubWellRaw agHubWellRaw)
        {
            var wellStaging = new WellStaging
            {
                WellRegistrationID = agHubWellRaw.WellRegistrationID,
                AuditPumpRateUpdated = agHubWellRaw.AuditPumpRateUpdated,
                WellAuditPumpRate = agHubWellRaw.WellAuditPumpRate,
                TPNRDPumpRateUpdated = agHubWellRaw.TpnrdPumpRateUpdated,
                WellTPNRDPumpRate = agHubWellRaw.WellTpnrdPumpRate,
                WellConnectedMeter = agHubWellRaw.WellConnectedMeter ?? false,
                WellGeometry = new Point(agHubWellRaw.Location.Coordinates.Longitude,agHubWellRaw.Location.Coordinates.Latitude),
                WellTPID = agHubWellRaw.WellTPID,
                HasElectricalData = false
            };
            return wellStaging;
        }
    }
}