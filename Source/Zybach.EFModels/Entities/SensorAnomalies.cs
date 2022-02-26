using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static class SensorAnomalies
    {
        public static List<SensorAnomalySimpleDto> ListAsSimpleDto(ZybachDbContext dbContext)
        {
            return dbContext.SensorAnomalies
                .Include(x => x.Sensor).ThenInclude(x => x.Well)
                .AsNoTracking()
                .Select(x => x.AsSimpleDto())
                .ToList();
        }

        public static void CreateNew(ZybachDbContext dbContext, SensorAnomalySimpleDto sensorAnomalySimpleDto)
        {
            dbContext.SensorAnomalies.Add(new SensorAnomaly()
            {
                SensorID = sensorAnomalySimpleDto.SensorID,
                StartDate = sensorAnomalySimpleDto.StartDate,
                EndDate = sensorAnomalySimpleDto.EndDate,
                Notes = sensorAnomalySimpleDto.Notes
            });

            dbContext.SaveChanges();
        }

        public static void Update(ZybachDbContext dbContext, SensorAnomaly sensorAnomaly, SensorAnomalySimpleDto sensorAnomalySimpleDto)
        {
            sensorAnomaly.StartDate = sensorAnomalySimpleDto.StartDate;
            sensorAnomaly.EndDate = sensorAnomalySimpleDto.EndDate;
            sensorAnomalySimpleDto.Notes = sensorAnomalySimpleDto.Notes;

            dbContext.SaveChanges();
        }

        public static void Delete(ZybachDbContext dbContext, SensorAnomaly sensorAnomaly)
        {
            dbContext.SensorAnomalies.Remove(sensorAnomaly);
            dbContext.SaveChanges();
        }
    }
}