using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.EFModels.Entities;
using Zybach.Models.GeoOptix;

namespace Zybach.API
{
    public class GeoOptixSyncDailyJob : ScheduledBackgroundJobBase<GeoOptixSyncDailyJob>
    {
        private readonly GeoOptixService _geoOptixService;
        public const string JobName = "GeoOptix Well and Station Daily Sync";

        public GeoOptixSyncDailyJob(IWebHostEnvironment webHostEnvironment, ILogger<GeoOptixSyncDailyJob> logger,
            ZybachDbContext zybachDbContext, IOptions<ZybachConfiguration> zybachConfiguration, GeoOptixService geoOptixService, SitkaSmtpClientService sitkaSmtpClientService) : base(
            JobName, logger, webHostEnvironment, zybachDbContext, zybachConfiguration, sitkaSmtpClientService)
        {
            _geoOptixService = geoOptixService;
        }

        public override List<RunEnvironment> RunEnvironments => new() {RunEnvironment.Development, RunEnvironment.Staging, RunEnvironment.Production};

        protected override void RunJobImplementation()
        {
            GetDailyWellFlowMeterData();
        }

        private void GetDailyWellFlowMeterData()
        {
            // first delete all from the tables
            _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE dbo.GeoOptixWellStaging");
            _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE dbo.GeoOptixSensorStaging");


            var geoOptixSites = _geoOptixService.GetGeoOptixSites().Result;
            if (geoOptixSites.Any())
            {
                var geoOptixWellStagings = geoOptixSites.Select(CreateGeoOptixWellStaging).ToList();
                _dbContext.GeoOptixWellStagings.AddRange(geoOptixWellStagings);
                _dbContext.SaveChanges();
                _dbContext.Database.ExecuteSqlRaw("EXECUTE dbo.pPublishGeoOptixWells");
            }

            var geoOptixStations = _geoOptixService.GetGeoOptixStations().Result;
            if (geoOptixStations.Any())
            {
                var geoOptixSensorStagings = geoOptixStations.Select(CreateGeoOptixSensorStaging).ToList();
                _dbContext.GeoOptixSensorStagings.AddRange(geoOptixSensorStagings);
                _dbContext.SaveChanges();
                _dbContext.Database.ExecuteSqlRaw("EXECUTE dbo.pPublishGeoOptixSensors");
            }
        }

        private GeoOptixSensorStaging CreateGeoOptixSensorStaging(Station station)
        {
            var geoOptixSensorStaging = new GeoOptixSensorStaging
            {
                WellRegistrationID = station.SiteCanonicalName.ToUpper(),
                SensorName = station.Name,
                SensorType = station.Definition.SensorType
            };
            return geoOptixSensorStaging;
        }

        private static GeoOptixWellStaging CreateGeoOptixWellStaging(Site site)
        {
            var point = ((Point)site.Location.Geometry);
            var geoOptixWellStaging = new GeoOptixWellStaging
            {
                WellRegistrationID = site.CanonicalName.ToUpper(),
                WellGeometry = new NetTopologySuite.Geometries.Point(point.Coordinates.Longitude, point.Coordinates.Latitude)
            };
            return geoOptixWellStaging;
        }
    }
}