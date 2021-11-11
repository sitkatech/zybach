using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    public class Sensors
    {
        public static Sensor GetBySensorName(ZybachDbContext dbContext, string sensorName)
        {
            return GetSensorsImpl(dbContext).SingleOrDefault(x => x.SensorName.Equals(sensorName));
        }

        private static IQueryable<Sensor> GetSensorsImpl(ZybachDbContext dbContext)
        {
            return dbContext.Sensors
                .Include(x => x.SensorType)
                .Include(x => x.Well)
                .AsNoTracking();
        }
    }
}