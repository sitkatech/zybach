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
        private static readonly List<string> ProblemWellRegistrationIDs = new List<string>{ "G-012886", "G-017908", "G-018992", "G-033855", "G-052662", "G-128363" };

        public AgHubWellsFetchDailyJob(IWebHostEnvironment webHostEnvironment, ILogger<AgHubWellsFetchDailyJob> logger,
            ZybachDbContext zybachDbContext, AgHubService agHubService, InfluxDBService influxDbService) : base(
            JobName, logger, webHostEnvironment, zybachDbContext)
        {
            _agHubService = agHubService;
            _influxDbService = influxDbService;
        }

        public override List<RunEnvironment> RunEnvironments => new List<RunEnvironment>
            {RunEnvironment.Development, RunEnvironment.Staging, RunEnvironment.Production};

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
            _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE dbo.AgHubWellStaging");
            _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE dbo.AgHubWellIrrigatedAcreStaging");
            _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE dbo.WellSensorMeasurementStaging");


            var agHubWellRaws = _agHubService.GetWellCollection().Result;
            if (agHubWellRaws.Any())
            {
                var agHubWells = agHubWellRaws.Select(CreateAgHubWell).ToList();
                // TODO: get last reading dates gets pumped volume dates too; original code is only looking at estimated pumped volume;
                var lastReadingDates = _dbContext.WellSensorMeasurements
                    .Where(x => x.MeasurementTypeID == (int) MeasurementTypeEnum.ElectricalUsage).ToList()
                    .GroupBy(x => x.WellRegistrationID).ToDictionary(x => x.Key, x => x.Max(y => y.ReadingDate));
                foreach (var agHubWell in agHubWells)
                {
                    var wellRegistrationID = agHubWell.WellRegistrationID;
                    if (!ProblemWellRegistrationIDs.Contains(wellRegistrationID))
                    {
                        var agHubWellRawWithAcreYears =
                            _agHubService.GetWellIrrigatedAcresPerYear(wellRegistrationID).Result;
                        if (agHubWellRawWithAcreYears != null)
                        {
                            var agHubWellIrrigatedAcreStagings = agHubWellRawWithAcreYears.AcresYear
                                .Where(x => x.Acres.HasValue).Select(x => new AgHubWellIrrigatedAcreStaging()
                                {
                                    Acres = x.Acres.Value,
                                    WellRegistrationID = wellRegistrationID,
                                    IrrigationYear = x.Year
                                }).ToList();
                            _dbContext.AgHubWellIrrigatedAcreStagings.AddRange(agHubWellIrrigatedAcreStagings);
                        }

                        if (agHubWell.WellConnectedMeter ?? false)
                        {
                            var startDate = lastReadingDates.ContainsKey(wellRegistrationID)
                                ? lastReadingDates[wellRegistrationID]
                                : DefaultStartDate;
                            var pumpedVolumeResult =
                                _agHubService.GetPumpedVolume(wellRegistrationID, startDate).Result;
                            if (pumpedVolumeResult != null)
                            {
                                var pumpedVolumeTimePoints = pumpedVolumeResult.PumpedVolumeTimeSeries.Where(x => x.PumpedVolumeGallons > 0).ToList();
                                if (pumpedVolumeTimePoints.Any())
                                {
                                    agHubWell.HasElectricalData = true;
                                    var wellSensorMeasurementStagings = pumpedVolumeTimePoints.Select(
                                        pumpedVolumeTimeSeries => new WellSensorMeasurementStaging()
                                        {
                                            MeasurementTypeID = (int) MeasurementTypeEnum.ElectricalUsage,
                                            ReadingDate = pumpedVolumeTimeSeries.ReadingDate,
                                            MeasurementValue = pumpedVolumeTimeSeries.PumpedVolumeGallons,
                                            WellRegistrationID = wellRegistrationID
                                        }).ToList();
                                    _dbContext.WellSensorMeasurementStagings.AddRange(wellSensorMeasurementStagings);
                                }
                                else
                                {
                                    agHubWell.HasElectricalData = false;
                                }
                            }
                            else
                            {
                                agHubWell.HasElectricalData = false;
                            }
                        }
                        else
                        {
                            agHubWell.HasElectricalData = false;
                        }
                    }
                    _dbContext.AgHubWellStagings.Add(agHubWell);
                    _dbContext.SaveChanges();
                }

            }
        }

        private static AgHubWellStaging CreateAgHubWell(AgHubService.AgHubWellRaw agHubWellRaw)
        {
            var agHubWell = new AgHubWellStaging
            {
                WellRegistrationID = agHubWellRaw.WellRegistrationID,
                AuditPumpRateUpdated = agHubWellRaw.AuditPumpRateUpdated,
                WellAuditPumpRate = agHubWellRaw.WellAuditPumpRate,
                TPNRDPumpRateUpdated = agHubWellRaw.TpnrdPumpRateUpdated,
                TPNRDPumpRate = agHubWellRaw.WellTpnrdPumpRate,
                WellConnectedMeter = agHubWellRaw.WellConnectedMeter,
                WellGeometry = new Point(agHubWellRaw.Location.Coordinates.Latitude,agHubWellRaw.Location.Coordinates.Longitude),
                WellTPID = agHubWellRaw.WellTPID,
                FetchDate = DateTime.UtcNow
            };
            return agHubWell;
        }
    }
}