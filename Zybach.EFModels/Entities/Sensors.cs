using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public class Sensors
    {
        public enum SensorTypeEnum
        {
            FlowMeter = 1,
            ContinuityMeter = 2,
            WellPressure = 3
        }

        public static List<Sensor> SearchBySensorName(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.Sensors.AsNoTracking().Where(x => x.SensorName.Contains(searchText)).ToList();
        }

        private static IQueryable<Sensor> GetSensorsImpl(ZybachDbContext dbContext)
        {
            return dbContext.Sensors
                .Include(x => x.Well)
                .AsNoTracking();
        }

        public static List<SensorSimpleDto> ListAsSimpleDto(ZybachDbContext dbContext)
        {
            return GetSensorsImpl(dbContext)
                .Select(x => x.AsSimpleDto())
                .ToList();
        }

        public static Sensor GetByID(ZybachDbContext dbContext, int sensorID)
        {
            return GetSensorsImpl(dbContext)
                .SingleOrDefault(x => x.SensorID == sensorID);
        }

        public static List<Sensor> ListByWellID(ZybachDbContext dbContext, int wellID)
        {
            return GetSensorsImpl(dbContext)
                .Where(x => x.WellID == wellID)
                .ToList();
        }
    }
}