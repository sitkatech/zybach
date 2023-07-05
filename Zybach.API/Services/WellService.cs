using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Services
{
    public class WellService
    {
        private readonly ZybachDbContext _dbContext;
        private readonly ILogger<WellService> _logger;

        public WellService(ZybachDbContext dbContext, ILogger<WellService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public List<WellWithSensorSimpleDto> GetAghubAndGeoOptixWells()
        {
            var wells = Wells.ListAsWellWithSensorSimpleDto(_dbContext);
            var lastReadingDateTimes = _dbContext.vWellSensorMeasurementFirstAndLatestForSensors.AsNoTracking().ToList().ToLookup(x => x.SensorName);
            wells.AsParallel().ForAll(x =>
            {
                x.LastReadingDate = lastReadingDateTimes.Contains(x.WellRegistrationID)
                    ? lastReadingDateTimes[x.WellRegistrationID].Max(y => y.LastReadingDate)
                    : null;
                x.FirstReadingDate = lastReadingDateTimes.Contains(x.WellRegistrationID)
                    ? lastReadingDateTimes[x.WellRegistrationID].Min(y => y.FirstReadingDate)
                    : null;
            });

            return wells;
        }

        public List<WellWaterLevelMapSummaryDto> GetWellPressureWellsForWaterLevelSummary()
        {
            var wells = Wells.ListAsWaterLevelMapSummaryDtos(_dbContext)
                .Where(x => x.Sensors.Any(y => y.SensorTypeID == (int)SensorTypeEnum.WellPressure))
                .ToList();
            var lastReadingDateTimes = _dbContext.vWellSensorMeasurementFirstAndLatestForSensors.AsNoTracking().ToList().ToLookup(x => x.SensorName);
            wells.AsParallel().ForAll(x =>
            {
                x.LastReadingDate = lastReadingDateTimes.Contains(x.WellRegistrationID)
                    ? lastReadingDateTimes[x.WellRegistrationID].Max(y => y.LastReadingDate)
                    : null;
            });

            return wells;
        }
        
    }
}