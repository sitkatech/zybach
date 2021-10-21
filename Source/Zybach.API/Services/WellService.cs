using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Services
{
    public class WellService
    {
        private readonly ZybachDbContext _dbContext;
        private readonly ILogger<WellService> _logger;
        private readonly GeoOptixService _geoOptixService;

        public WellService(ZybachDbContext dbContext, ILogger<WellService> logger, GeoOptixService geoOptixService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _geoOptixService = geoOptixService;
        }

        public async Task<List<WellWithSensorSummaryDto>> GetAghubAndGeoOptixWells()
        {
            var wells = Well.GetWellsAsWellWithSensorSummaryDtos(_dbContext);
            var wellsWithSensorSummaryFromGeoOptix = await _geoOptixService.GetWellsWithSensors();
            foreach (var geoOptixWell in wellsWithSensorSummaryFromGeoOptix)
            {
                var wellWithSensorSummaryDto =
                    wells.SingleOrDefault(x => x.WellRegistrationID == geoOptixWell.WellRegistrationID);
                if (wellWithSensorSummaryDto == null)
                {
                    wells.Add(geoOptixWell);
                }
                else
                {
                    wellWithSensorSummaryDto.InGeoOptix = true;
                    wellWithSensorSummaryDto.Sensors.AddRange(geoOptixWell.Sensors);
                }
            }

            var lastReadingDateTimes = WellSensorMeasurement.GetLastReadingDateTimes(_dbContext);
            var firstReadingDateTimes = WellSensorMeasurement.GetFirstReadingDateTimes(_dbContext);
            wells.ForEach(x =>
            {
                x.LastReadingDate = lastReadingDateTimes.ContainsKey(x.WellRegistrationID)
                    ? lastReadingDateTimes[x.WellRegistrationID]
                    : (DateTime?)null;
                x.FirstReadingDate = firstReadingDateTimes.ContainsKey(x.WellRegistrationID)
                    ? firstReadingDateTimes[x.WellRegistrationID]
                    : (DateTime?)null;
            });

            return wells;
        }
    }
}