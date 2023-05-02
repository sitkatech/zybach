using System;
using System.Collections.Generic;
using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public class PaigeWirelessPulses
{
    public static PaigeWirelessPulseDto GetLatestBySensorName(ZybachDbContext dbContext, string sensorName)
    {
        return dbContext.PaigeWirelessPulses.Where(x => x.SensorName == sensorName).AsEnumerable()
            .MaxBy(x => x.ReceivedDate)?.AsDto();
    }

    public static IDictionary<string, int> GetLastMessageAgesBySensorName(ZybachDbContext dbContext)
    {
        var currentDate = DateTime.UtcNow;
        return dbContext.PaigeWirelessPulses.AsEnumerable()
            .GroupBy(x => x.SensorName)
            .ToDictionary(x => x.Key, y =>
            {
                var lastReceivedDate = y.MaxBy(z => z.ReceivedDate)!.ReceivedDate;
                var messageAge = currentDate - lastReceivedDate;
                return (int) messageAge.TotalMinutes;
            });
    }

    public static int? GetLastMessageAgeBySensorName(ZybachDbContext dbContext, string sensorName)
    {
        var currentDate = DateTime.UtcNow;

        var lastReceivedDate = dbContext.PaigeWirelessPulses.Where(x => x.SensorName == sensorName).AsEnumerable()
            .MaxBy(x => x.ReceivedDate)?.ReceivedDate;

        if (lastReceivedDate == null) return null;

        var messageAge = currentDate - (DateTime) lastReceivedDate;
        return (int) messageAge.TotalMinutes;
    }

    public static void Create(ZybachDbContext dbContext, SensorPulseDto sensorPulseDto)
    {
        var paigeWirelessPulse = new PaigeWirelessPulse()
        {
            SensorName = sensorPulseDto.SensorName,
            EventMessage = sensorPulseDto.EventMessage,
            ReceivedDate = sensorPulseDto.ReceivedDate
        };

        dbContext.PaigeWirelessPulses.Add(paigeWirelessPulse);
        dbContext.SaveChanges();
    }
}