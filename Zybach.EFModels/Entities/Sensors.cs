using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public class Sensors
    {
        public static List<Sensor> SearchBySensorName(ZybachDbContext dbContext, string searchText)
        {
            return dbContext.Sensors.AsNoTracking().Where(x => x.SensorName.Contains(searchText)).ToList();
        }

        private static IQueryable<Sensor> GetSensorsImpl(ZybachDbContext dbContext)
        {
            return dbContext.Sensors
                .Include(x => x.Well)
                .Include(x => x.SensorAnomalies)
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

        public static Sensor GetBySensorName(ZybachDbContext dbContext, string sensorName)
        {
            return GetSensorsImpl(dbContext).SingleOrDefault(x => x.SensorName == sensorName);
        }

        public static List<Sensor> ListActiveByWellID(ZybachDbContext dbContext, int wellID)
        {
            return GetSensorsImpl(dbContext)
                .Where(x => x.IsActive && x.WellID == wellID).ToList();
        }
    }
}