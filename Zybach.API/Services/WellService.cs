using System;
using System.Collections.Generic;
using System.Linq;
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
            var lastReadingDateTimes = WellSensorMeasurements.GetLastReadingDateTimes(_dbContext);
            var firstReadingDateTimes = WellSensorMeasurements.GetFirstReadingDateTimes(_dbContext);
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

        public List<WellWaterLevelMapSummaryDto> GetWellPressureWellsForWaterLevelSummary()
        {
            var wells = Wells.ListAsWaterLevelMapSummaryDtos(_dbContext)
                .Where(x => x.Sensors.Any(y => y.SensorTypeID == (int)SensorTypeEnum.WellPressure))
                .ToList();
            var lastReadingDateTimes = WellSensorMeasurements.GetLastReadingDateTimes(_dbContext);
            wells.ForEach(x =>
            {
                x.LastReadingDate = lastReadingDateTimes.ContainsKey(x.WellRegistrationID)
                    ? lastReadingDateTimes[x.WellRegistrationID]
                    : (DateTime?)null;
            });

            return wells;
        }
        
    }
}