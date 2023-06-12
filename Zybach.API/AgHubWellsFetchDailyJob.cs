using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API
{
    public class AgHubWellsFetchDailyJob : ScheduledBackgroundJobBase<AgHubWellsFetchDailyJob>
    {
        private readonly AgHubService _agHubService;
        public const string JobName = "AgHub Well Fetch Daily";
        //        private static readonly List<string> ProblemWellRegistrationIDs = new List<string>{ "G-012886", "G-017908", "G-018992", "G-033855", "G-052662", "G-128363" };
        protected static readonly DateTime AgHubWellPumpedVolumeStartDate = new DateTime(2016, 1, 1);
        public AgHubWellsFetchDailyJob(IWebHostEnvironment webHostEnvironment, ILogger<AgHubWellsFetchDailyJob> logger,
            ZybachDbContext zybachDbContext, IOptions<ZybachConfiguration> zybachConfiguration, AgHubService agHubService, SitkaSmtpClientService sitkaSmtpClientService) : base(
            JobName, logger, webHostEnvironment, zybachDbContext, zybachConfiguration, sitkaSmtpClientService)
        {
            _agHubService = agHubService;
        }

        public override List<RunEnvironment> RunEnvironments => new List<RunEnvironment>
            {RunEnvironment.Production, RunEnvironment.Staging}; 

        protected override void RunJobImplementation()
        {
            GetDailyWellFlowMeterData();
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
                var wellStagings = agHubWellRaws.Select(CreateAgHubWellStaging).ToList();
                foreach (var wellStaging in wellStagings)
                {
                    var wellRegistrationID = wellStaging.WellRegistrationID;
                    //if (!ProblemWellRegistrationIDs.Contains(wellRegistrationID))
                    PopulateIrrigatedAcresPerYearForWell(wellStaging, wellRegistrationID);
                    PopulateWellSensorMeasurementsForWell(wellStaging, wellRegistrationID);
                    _dbContext.AgHubWellStagings.Add(wellStaging);
                    _dbContext.SaveChanges();
                }
                
                // only publish if we actually got any AgHubWells from Zappa
                _dbContext.Database.ExecuteSqlRaw("EXECUTE dbo.pPublishAgHubWells");
            }
        }

        private void PopulateWellSensorMeasurementsForWell(AgHubWellStaging agHubWellStaging, string wellRegistrationID)
        {
            var pumpedVolumeResult =
                _agHubService.GetPumpedVolume(wellRegistrationID, AgHubWellPumpedVolumeStartDate).Result;
            if (pumpedVolumeResult is { PumpedVolumeTimeSeries: { } })
            {
                var pumpedVolumeTimePoints =
                    pumpedVolumeResult.PumpedVolumeTimeSeries.Where(x => x.PumpedVolumeGallons > 0).ToList();
                if (pumpedVolumeTimePoints.Any())
                {
                    var wellSensorMeasurementStagings = pumpedVolumeTimePoints.Select(
                        pumpedVolumeTimeSeries => new WellSensorMeasurementStaging
                        {
                            SensorName = $"E-{wellRegistrationID.ToUpper()}",
                            MeasurementTypeID = (int)MeasurementTypeEnum.ElectricalUsage,
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

        private void PopulateIrrigatedAcresPerYearForWell(AgHubWellStaging wellStaging, string wellRegistrationID)
        {
            var agHubWellRawWithAcreYears =
                _agHubService.GetWellIrrigatedAcresPerYear(wellRegistrationID).Result;

            if (agHubWellRawWithAcreYears != null)
            {
                wellStaging.RegisteredUpdated = agHubWellRawWithAcreYears.RegisteredUpdated;
                wellStaging.RegisteredPumpRate = agHubWellRawWithAcreYears.RegisteredPumpRate;
                wellStaging.HasElectricalData = agHubWellRawWithAcreYears.HasElectricalData > 0;
                wellStaging.AgHubRegisteredUser = agHubWellRawWithAcreYears.RegisteredUserDetails.RegisteredUser;
                wellStaging.FieldName = agHubWellRawWithAcreYears.RegisteredUserDetails.RegisteredFieldName;
                wellStaging.IrrigationUnitGeometry = WKTToGeometry(agHubWellRawWithAcreYears.IrrigationUnitGeometry);

                var wellIrrigatedAcreStagings = agHubWellRawWithAcreYears.IrrigUnitDetails
                    .Where(x => x.TotalAcres.HasValue).Select(x =>
                    {
                        var agHubWellIrrigatedAcreStaging = new AgHubWellIrrigatedAcreStaging()
                        {
                            Acres = x.TotalAcres.Value,
                            WellRegistrationID = wellRegistrationID,
                            IrrigationYear = x.Year
                        };
                        if (x.FarmPractices != null && x.FarmPractices.Any())
                        {
                            var farmPractice = x.FarmPractices.OrderByDescending(x => x.Acres).ThenBy(x => x.Crop)
                                .ThenBy(x => x.Tillage).First();
                            agHubWellIrrigatedAcreStaging.CropType = farmPractice.Crop;
                            agHubWellIrrigatedAcreStaging.Tillage = farmPractice.Tillage;
                        }
                        return agHubWellIrrigatedAcreStaging;
                    }).ToList();
                _dbContext.AgHubWellIrrigatedAcreStagings.AddRange(wellIrrigatedAcreStagings);
            }
        }

        private static Geometry WKTToGeometry(string wktGeometry)
        {
            return !string.IsNullOrWhiteSpace(wktGeometry) ? new WKTReader().Read(wktGeometry) : null;
        }

        private static AgHubWellStaging CreateAgHubWellStaging(AgHubService.AgHubWellRaw agHubWellRaw)
        {
            var agHubWellStaging = new AgHubWellStaging
            {
                WellRegistrationID = agHubWellRaw.WellRegistrationID,
                AuditPumpRateUpdated = agHubWellRaw.AuditPumpRateUpdated,
                WellAuditPumpRate = agHubWellRaw.WellAuditPumpRate,
                TPNRDPumpRateUpdated = agHubWellRaw.DistrictPumpRateUpdated,
                WellTPNRDPumpRate = agHubWellRaw.WellDistrictPumpRate,
                WellConnectedMeter = agHubWellRaw.WellConnectedMeter ?? false,
                WellGeometry = WKTToGeometry(agHubWellRaw.Location),
                WellTPID = agHubWellRaw.WellIrrigUnitID,
                HasElectricalData = false
            };
            return agHubWellStaging;
        }
    }
}