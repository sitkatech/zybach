using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;
using Zybach.Models.DataTransferObjects.User;

namespace Zybach.EFModels.Entities
{
    public static class SensorAnomalies
    {
        private static IQueryable<SensorAnomaly> GetSensorAnomaliesImpl(ZybachDbContext dbContext)
        {
            return dbContext.SensorAnomalies
                .Include(x => x.Sensor).ThenInclude(x => x.Well)
                .AsNoTracking();
        }

        public static List<SensorAnomalySimpleDto> ListAsSimpleDto(ZybachDbContext dbContext)
        {
            return GetSensorAnomaliesImpl(dbContext)
                .Select(x => x.AsSimpleDto())
                .ToList();
        }

        public static SensorAnomalySimpleDto GetByIDAsSimpleDto(ZybachDbContext dbContext, int sensorAnomalyID)
        {
            var sensorAnomaly = GetSensorAnomaliesImpl(dbContext).SingleOrDefault(x => x.SensorAnomalyID == sensorAnomalyID);
            return sensorAnomaly?.AsSimpleDto();
        }

        public static void CreateNew(ZybachDbContext dbContext, SensorAnomalyUpsertDto sensorAnomalyUpsertDto)
        {
            dbContext.SensorAnomalies.Add(new SensorAnomaly()
            {
                SensorID = sensorAnomalyUpsertDto.SensorID,
                StartDate = sensorAnomalyUpsertDto.StartDate.GetValueOrDefault(),
                EndDate = sensorAnomalyUpsertDto.EndDate.GetValueOrDefault(),
                Notes = sensorAnomalyUpsertDto.Notes
            });

            dbContext.SaveChanges();
        }

        public static void Update(ZybachDbContext dbContext, SensorAnomaly sensorAnomaly, SensorAnomalyUpsertDto sensorAnomalyUpsertDto)
        {
            sensorAnomaly.StartDate = sensorAnomalyUpsertDto.StartDate.GetValueOrDefault();
            sensorAnomaly.EndDate = sensorAnomalyUpsertDto.EndDate.GetValueOrDefault();
            sensorAnomaly.Notes = sensorAnomalyUpsertDto.Notes;

            dbContext.SaveChanges();
        }

        public static void Delete(ZybachDbContext dbContext, SensorAnomaly sensorAnomaly)
        {
            dbContext.SensorAnomalies.Remove(sensorAnomaly);
            dbContext.SaveChanges();
        }
    }
}