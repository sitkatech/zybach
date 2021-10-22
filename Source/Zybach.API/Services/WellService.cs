using System;
using System.Collections.Generic;
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

        public List<WellWithSensorSummaryDto> GetAghubAndGeoOptixWells()
        {
            var wells = Wells.ListAsWellWithSensorSummaryDto(_dbContext);
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